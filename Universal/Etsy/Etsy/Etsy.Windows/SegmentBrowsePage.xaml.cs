using Etsy.Common;
using Etsy.DataBinding;
using Etsy.DataTransfer;
using Etsy.Model;
using Etsy.Model.List;
using Etsy.Model.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Etsy.Model.Browse;
using Etsy.UI_Extras;

namespace Etsy
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class SegmentBrowsePage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        // Page variables
        IncrementalSource<ListGetter, Listing> products;    // = new IncrementalSource<ListGetter, Listing>("trending", "");
        private static double scrollPosition = 0.0;                                    // position of ListView
        private static bool scrollDown = false;
        private static bool refresh = false;

        private List<Parameter> searchParameters = new List<Parameter>();
        private List<Parameter> sortParameters = new List<Parameter>();
        private List<Parameter> filterParameters = new List<Parameter>();
        private List<Parameter> pathParameter = new List<Parameter>();      // segment path

        private static ProductList prodList = new ProductList("active");
        private static User user = App.user;
        private static BrowseSegment segment;

        DispatcherTimer timer;      // used in tracking the direction of the listview scroll action
        private static int loadCount = 0;   // used to determine if the load time is taking too long

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
        

        public SegmentBrowsePage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            initial_orientation();
            Window.Current.SizeChanged += Current_SizeChanged;

            // timer
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;                               // tick handler (time)
            timer.Interval = new TimeSpan(0, 0, 0, 0, 2);        // time between each tick

            timer.Start();
        }

        /// <summary>
        /// Check the loading progress.  Ask the user if they want to reload if there's an issue.
        /// Control the state of the options menu; it disappears when the user is scrolling down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, object e)
        {
            // Show the progress ring while loading
            if (!Loading.ControlProgressRing(products, pRing))     // progress ring
                loadCount++;
            else
            {
                quality_panel.Visibility = Visibility.Collapsed;
                loadCount = 0;
            }

            // Give the user the option to try a lower image quality if it's taking too long to load
            if (loadCount > 70 && Loading._imageQuality == Loading.ImageQuality.high)
                quality_panel.Visibility = Visibility.Visible;

            // Notify the user of a loading issue only when no items are present
            if(products.Count == 0)
            {
                if (Loading.Notify_loadingError(loadErrorBlock))
                {
                    LoadErrorPanel.Visibility = Visibility.Visible;     // Show the error
                    refreshButton.Visibility = Visibility.Visible;
                    errorVisibilityBlocker.Visibility = Visibility.Visible;
                }
            }
            else // hide the error panel
            {
                LoadErrorPanel.Visibility = Visibility.Collapsed;
                errorVisibilityBlocker.Visibility = Visibility.Collapsed;
            }

            var scroller = FindVisualChild<ScrollViewer>(productList);
            if (scroller == null)
                return;

            double current_offset = scroller.VerticalOffset;

            if (current_offset == scrollPosition && scrollDown == true) // hide after scrolling down
            {
                bottomMenuPanel.Visibility = Visibility.Collapsed;
                filtersPanel.Visibility = Visibility.Collapsed;
                sortOptionsView.Visibility = Visibility.Collapsed;
            }
            else if (current_offset < scrollPosition)                   // show when scrolling up
            {
                bottomMenuPanel.Visibility = Visibility.Visible;
                scrollDown = false;
            }
            else if (current_offset == 2.0)
                bottomMenuPanel.Visibility = Visibility.Visible;    // show menu when at the starting position
            else if (current_offset == scrollPosition && scrollDown == false)
            {
                bottomMenuPanel.Visibility = Visibility.Visible;    // Show menu when staying in place after scrolling up
            }
            else
            {
                bottomMenuPanel.Visibility = Visibility.Collapsed;  // hide when scrolling down
                filtersPanel.Visibility = Visibility.Collapsed;
                sortOptionsView.Visibility = Visibility.Collapsed;
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
            timer.Tick += timer_Tick;
            timer.Start();

            if (App.logged_in)
            {
                this.DefaultViewModel["login_name"] = user.login_name;                      // data bind to the user's login name
                this.DefaultViewModel["image_url_75x75"] = user.Profile.image_url_75x75;    // data binding for user image
            }

            segment = (BrowseSegment)e.NavigationParameter; // get search term from main page

            // null parameter error
            if (Loading.Null_Error<BrowseSegment>(segment, loadErrorBlock))
            {
                LoadErrorPanel.Visibility = Visibility.Visible;     
                error_goBackButton.Visibility = Visibility.Visible; // show the button for going back
                errorVisibilityBlocker.Visibility = Visibility.Visible;
                return;
            }

            if (segment != null)
            {
                pathParameter.Add(new Parameter("path", segment.path));
                searchParameters.Clear();
                searchParameters.AddRange(SearchRefinery.sortingParameters(SortOptions.Relevancy));     // default sort
                searchParameters.AddRange(pathParameter);
            }

            // Search refinements
            sortOptionsView.ItemsSource = SearchRefinery.SortingOptions();
            sortOptionsView.SelectedIndex = 1;

            if (App.started2 == false)  // Default search parameters
            {
                
            }

            try
            {
                //if (App.started2 == false || refresh == true)                                 // start anew
                    products = new IncrementalSource<ListGetter, Listing>(-1, 1, searchParameters);
                //else
                    //await loadSavedListState();                                             // Load the previous list and scroll position
            }
            catch // no list was in existence prior                          
            {
                products = new IncrementalSource<ListGetter, Listing>(-1, 1, searchParameters);      // create a new list
            }
            finally
            {
                productList.ItemsSource = products;                                         // set the source of the xaml list
            }

            this.DefaultViewModel["title"] = segment.name;

            this.DefaultViewModel["productList"] = prodList.results;

            App.started2 = true;
        }

        /// <summary>
        /// Load the list and get the previous position of the ListView
        /// </summary>
        /// <returns></returns>
        private async Task loadSavedListState()
        {
            products = new IncrementalSource<ListGetter, Listing>(-1, 1, pathParameter);
            products = await FileIO.DeserializeFromFile<IncrementalSource<ListGetter, Listing>>("browseProductsXAML");
            await products.loadAttributes("browseProductsXAML_attributes");

            productList.ItemsSource = products;                                         // set the source of the xaml list

            var scroller = FindVisualChild<ScrollViewer>(productList);                  // scroll to the previously saved ListView position
            scroller.ChangeView(0, scrollPosition, null);                               // only after the list has items in it

            return;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
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
            await FileIO.SerializeAndSave<IncrementalSource<ListGetter, Listing>>(products, "browseProductsXAML");
            await products.saveAttributes("browseProductsXAML_attributes");

            var scroller = FindVisualChild<ScrollViewer>(productList);
            scrollPosition = scroller.VerticalOffset;                           // save listView position

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
                pageTitle.Text = "A";
            }
            else  // landscape
            {
                pageTitle.Text = "Artsy";
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
                pageTitle.Text = "A";
            }
            else  // landscape
            {
                pageTitle.Text = "Artsy";
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

            this.Frame.Navigate(typeof(ItemDetailPage1), list.SelectedItem);
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
            searchBox.Visibility = Visibility.Collapsed;
            searchButton.SetValue(Grid.RowProperty, 0);
            visibilityBlocker.Visibility = Visibility.Collapsed;
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
            searchButton.SetValue(Grid.RowProperty, 1);

            if (searchBox.Visibility == Visibility.Visible)
            {
                if (searchBox.Text != "")
                    this.Frame.Navigate(typeof(SearchPage), searchBox.Text);
            }

            searchBox.Visibility = Visibility.Visible;
            searchBox.Focus(FocusState.Programmatic);
            if (searchBox.Text != "")
                this.Frame.Navigate(typeof(SearchPage), searchBox.Text);
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
            ListView lview = (ListView)sender;
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

            searchParameters.AddRange(pathParameter);           // Add path

            products = new IncrementalSource<ListGetter, Listing>(-1, 1, searchParameters);

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
        /// Navigate to the settings page, where the user can change the image quality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imageQualityButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }

        /// <summary>
        /// Go to the previous page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void error_goBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (navigationHelper.CanGoBack())
                this.Frame.GoBack();
        }

        /// <summary>
        /// Try to reload the items for the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            redo_FilteredSorted_Search();
        }
           
    }
}
