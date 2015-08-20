using Etsy.Common;
using Etsy.DataBinding;
using Etsy.DataTransfer;
using Etsy.Model;
using Etsy.Model.List;
using Etsy.Model.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Etsy
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class SearchPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        // Page variables
        IncrementalSource<ListGetter, Listing> products;    // = new IncrementalSource<ListGetter, Listing>("trending", "");
        private static double scrollPosition = 0.0;                                    // position of ListView
        private static double current_offset = 0.0;
        private static bool scrollDown = false;
        private static bool refresh = false;
        //private static ObservableCollection<KeyEnumPair<string, SortOptions, string>> sortingOptions = SearchRefinery.SortingOptions();

        private List<Parameter> searchParameters = new List<Parameter>();
        private List<Parameter> sortParameters = new List<Parameter>();
        private List<Parameter> filterParameters = new List<Parameter>();

        private static ProductList prodList = new ProductList("active");
        private static User user = App.user;
        private string searchTerm = "";


        DispatcherTimer timer;      // used in tracking the direction of the listview scroll action

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public SearchPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            

            // timer
            timer = new DispatcherTimer();
                                           // tick handler (time)
            timer.Interval = new TimeSpan(0, 0, 0, 0, 2);        // time between each tick

            timer.Start();
        }

        /// <summary>
        /// Handle things according to the timer's interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, object e)
        {
            var scroller = FindVisualChild<ScrollViewer>(productList);
            if (scroller == null)
                return;

            current_offset = scroller.VerticalOffset;

            if (current_offset == scrollPosition && scrollDown == true) // hide after scrolling down
            {
                bottomMenuPanel.Visibility = Visibility.Collapsed;
                filtersPanel.Visibility = Visibility.Collapsed;
                sortOptionsView.Visibility = Visibility.Collapsed;
                headerGrid.Visibility = Visibility.Collapsed;           // hide header
            }
            else if (current_offset < scrollPosition)                   // show when scrolling up
            {
                bottomMenuPanel.Visibility = Visibility.Visible;
                headerGrid.Visibility = Visibility.Visible;
                scrollDown = false;
            }
            else if (current_offset == 2.0)
            {
                bottomMenuPanel.Visibility = Visibility.Visible;    // show menu when at the starting position
                headerGrid.Visibility = Visibility.Visible;
            }
            else if (current_offset == scrollPosition && scrollDown == false)
            {
                bottomMenuPanel.Visibility = Visibility.Visible;    // Show menu when staying in place after scrolling up
                headerGrid.Visibility = Visibility.Visible;
            }
            else
            {
                bottomMenuPanel.Visibility = Visibility.Collapsed;  // hide when scrolling down
                filtersPanel.Visibility = Visibility.Collapsed;
                sortOptionsView.Visibility = Visibility.Collapsed;
                headerGrid.Visibility = Visibility.Collapsed;
                scrollDown = true;
            }


            scrollPosition = scroller.VerticalOffset;   // update position variable

        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            timer.Start();
            timer.Tick += timer_Tick;
            initial_orientation();
            Window.Current.SizeChanged += Current_SizeChanged;

            if (App.logged_in)
            {
                this.DefaultViewModel["login_name"] = user.login_name;                      // data bind to the user's login name
                this.DefaultViewModel["image_url_75x75"] = user.Profile.image_url_75x75;    // data binding for user image
            }

            searchTerm = (string)e.NavigationParameter; // get search term from main page

            // Search refinements
            sortOptionsView.ItemsSource = SearchRefinery.SortingOptions();
            sortOptionsView.SelectedIndex = 1;

            if (App.started2 == false)  // Default search parameters
            {
                searchParameters.AddRange(SearchRefinery.sortingParameters(SortOptions.Relevancy));     // default sort
            }

            try
            {
                if (App.started2 == false || refresh == true)                                 // start anew
                    products = new IncrementalSource<ListGetter, Listing>("active", searchTerm, new List<Parameter>(), App.defaultAddress.country_id);
                else
                    await loadSavedListState();                                             // Load the previous list and scroll position
            }
            catch // no list was in existence prior                          
            {
                products = new IncrementalSource<ListGetter, Listing>("active", searchTerm, new List<Parameter>(), App.defaultAddress.country_id);      // create a new list
            }
            finally
            {
                productList.ItemsSource = products;                                         // set the source of the xaml list
            }

            this.DefaultViewModel["searchTerm"] = searchTerm;
            App.started2 = true;
        }

        /// <summary>
        /// Load the list and get the previous position of the ListView
        /// </summary>
        /// <returns></returns>
        private async Task loadSavedListState()
        {
            products = new IncrementalSource<ListGetter, Listing>("active", "", new List<Parameter>(), App.defaultAddress.country_id);
            products = await FileIO.DeserializeFromFile<IncrementalSource<ListGetter, Listing>>("searchProductsXAML");

            try
            {
                await products.loadAttributes("searchProductsXAML_attributes");
            }
            catch(Exception e)
            {
                string message = e.Message;
            }

            productList.ItemsSource = products;

            scrollPosition = Convert.ToDouble(await FileIO.ReadEncryptedFileAsString("searchProducts_scrollPosition"));// set the source of the xaml list
            current_offset = scrollPosition;

            var scroller = FindVisualChild<ScrollViewer>(productList);                  // scroll to the previously saved ListView position
            scroller.ChangeView(0, 0, null);
            scroller.ChangeView(0, scrollPosition, null);                               // only after the list has items in it

            return;
        }

        /// <summary>
        /// Save the state of the search list, as well as the parameters used
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private async void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            try
            {
                await saveListState();
            }
            catch (Exception ee)
            {
                string message = ee.Message;
            }
        }

        /// <summary>
        /// Save the list and listview position
        /// </summary>
        /// <returns></returns>
        private async Task saveListState()
        {
            await FileIO.SerializeAndSave<IncrementalSource<ListGetter, Listing>>(products, "searchProductsXAML");
            await products.saveAttributes("searchProductsXAML_attributes");

            var scroller = FindVisualChild<ScrollViewer>(productList);
            scrollPosition = scroller.VerticalOffset;                           // save listView position
            await FileIO.EncryptAndSave(Convert.ToString(scrollPosition), "searchProducts_scrollPosition");

            return;
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Hide the ads if the user paid to remove them
            if (App.purchased)
                adBottom.Visibility = Visibility.Collapsed;

            navigationHelper.OnNavigatedTo(e);

            if (App.logged_in)   // change the user options
            {
                user = App.user;
                this.DefaultViewModel["image_url_75x75"] = user.Profile.image_url_75x75;    // data binding for user image
                avatar.Visibility = Visibility.Visible;
            }
            else
            {
                avatar.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);

            // save the incremental loading list
            prodList.results = products;

            timer.Stop();
            timer.Tick -= timer_Tick;
            Window.Current.SizeChanged -= Current_SizeChanged;
        }

        #endregion

        /// <summary>
        /// Set up the UI according to the device's orientation when the page is loaded
        /// </summary>
        private void initial_orientation()
        {
            var CurrentViewState = ApplicationView.GetForCurrentView().Orientation;

            if (CurrentViewState.ToString() == "Portrait")
            {
            }
            else  // landscape
            {
            }
        }

        /// <summary>
        /// Orientation Change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            // Get the new view state
            string CurrentViewState = ApplicationView.GetForCurrentView().Orientation.ToString();

            // Adapt to the view port size rather than the device orientation
            double AppWidth = e.Size.Width;
            double AppHeight = e.Size.Height;

            if (AppHeight / AppWidth > 1)  // portrait
            {
            }
            else  // landscape
            {
            }
        }

        /// <summary>
        /// Find specified child element in a XAML object
        /// </summary>
        /// <typeparam name="childItem"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        private childItem FindVisualChild<childItem>(DependencyObject obj)
        where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        /// <summary>
        /// Product selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void productList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = (ListView)sender;

            this.Frame.Navigate(typeof(ItemDetailPage), list.SelectedItem);
        }

        /// <summary>
        /// Lower the visibility of the rest of the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            visibilityBlocker.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Return visibility to the rest of the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchBox_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Only show the avatar if the user is logged in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void profileButton_Loaded(object sender, RoutedEventArgs e)
        {
            Button _button = (Button)sender;

            if (App.logged_in == false)
                _button.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Redo the search with whatever is in the search box. Keep the same filter & sort preferences
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search(object sender, RoutedEventArgs e)
        {
            searchTerm = searchBox.Text;            // set the search term

            redo_FilteredSorted_Search();
        }

        /// <summary>
        /// Return to the previous page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if (navigationHelper.CanGoBack())
                this.Frame.GoBack();
        }

        /// <summary>
        /// Redo the search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sortOptionsView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox lview = (ListBox)sender;
            if (lview.SelectedIndex == -1)
                return;     // do nothing if the selected index was set to 1, or else it will reset the whole list

            var selectedItem = lview.SelectedItem as KeyEnumPair<string, SortOptions, string>;

            sortParameters = SearchRefinery.sortingParameters(selectedItem.enumTemplate);   // update the parameters for sorting

            redo_FilteredSorted_Search();                                                   // Redo the search
        }

        private void sortOptionsView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        /// <summary>
        /// Navigate to the cart page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cartButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CartPage));
        }

        /// <summary>
        /// Navigate to main page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Create a new version of the existing search, one that has a different filter or sort.
        /// Activated upon a change in the sort selection or the filter selection
        /// </summary>
        private void redo_FilteredSorted_Search()
        {
            searchParameters.Clear();                           // clear previous search parameters

            searchParameters.AddRange(sortParameters);          // Add the current sorting parameters
            searchParameters.AddRange(filterParameters);        // Add the current filter parameters

            searchBox.Text = searchTerm;                        // Remind the user of the current search, in case they had started to change the searchBox text

            products = new IncrementalSource<ListGetter, Listing>("active", searchTerm, searchParameters);

            productList.ItemsSource = products;                 // data bind
        }

        /// <summary>
        /// Redo the search with the new filter values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minPriceSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Slider slider = (Slider)sender;

            if (slider.Value > maxPriceSlider.Value)
                maxPriceSlider.Value = slider.Value;                        // Make sure the minimum price slider value is less than the maximum price slider value

            this.DefaultViewModel["minPrice"] = minPriceSlider.Value;
            this.DefaultViewModel["maxPrice"] = maxPriceSlider.Value;

            filterParameters = SearchRefinery.filterParameters(slider.Value, maxPriceSlider.Value);     // Set the filter parameters

            redo_FilteredSorted_Search();                                   // Redo the search
        }

        /// <summary>
        /// Redo the search with the new filter values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void maxPriceSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Slider slider = (Slider)sender;

            if (slider.Value < minPriceSlider.Value)
                minPriceSlider.Value = slider.Value;                        // Make sure the minimum price slider value is less than the maximum price slider value

            this.DefaultViewModel["minPrice"] = minPriceSlider.Value;
            this.DefaultViewModel["maxPrice"] = maxPriceSlider.Value;

            filterParameters = SearchRefinery.filterParameters(minPriceSlider.Value, slider.Value);     // Set the filter parameters

            redo_FilteredSorted_Search();                                   // Redo the search
        }

        /// <summary>
        /// Bind the slider's value to the view model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void minPriceSlider_Loaded(object sender, RoutedEventArgs e)
        {
            Slider slider = (Slider)sender;

            this.DefaultViewModel["minPrice"] = slider.Value;
        }

        /// <summary>
        /// Start the max price slider at its maximum value
        /// Bind the value to the view model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void maxPriceSlider_Loaded(object sender, RoutedEventArgs e)
        {
            Slider slider = (Slider)sender;

            slider.Value = slider.Maximum;

            this.DefaultViewModel["maxPrice"] = slider.Value;
        }

        /// <summary>
        /// Show the sort menu and hide the filter menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sortButton_Click(object sender, RoutedEventArgs e)
        {
            if (sortOptionsView.Visibility == Visibility.Collapsed)
            {
                filtersPanel.Visibility = Visibility.Collapsed;
                sortOptionsView.Visibility = Visibility.Visible;
            }
            else
            {
                sortOptionsView.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Show the filter menu and hide the sort menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void filterButton_Click(object sender, RoutedEventArgs e)
        {

            if (filtersPanel.Visibility == Visibility.Collapsed)
            {
                sortOptionsView.Visibility = Visibility.Collapsed;
                filtersPanel.Visibility = Visibility.Visible;
            }
            else
            {
                filtersPanel.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Reset the filter and sorting options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            sortOptionsView.SelectedIndex = 1;
            minPriceSlider.Value = minPriceSlider.Minimum;
            maxPriceSlider.Value = maxPriceSlider.Maximum;

            filterParameters = SearchRefinery.filterParameters(minPriceSlider.Value, maxPriceSlider.Value);     // Set the filter parameters

            sortParameters = SearchRefinery.sortingParameters(SortOptions.Relevancy);   // update the parameters for sorting

            sortOptionsView.Visibility = Visibility.Collapsed;
            filtersPanel.Visibility = Visibility.Collapsed;             // hide options panels

            redo_FilteredSorted_Search();
        }

        private void profileButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ProfilePage));
        }

        /// <summary>
        /// Hide itself
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void visibilityBlocker_Tapped(object sender, TappedRoutedEventArgs e)
        {
            visibilityBlocker.Visibility = Visibility.Collapsed;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            timer.Tick -= timer_Tick;
            Window.Current.SizeChanged -= Current_SizeChanged;
        }





    }
}
