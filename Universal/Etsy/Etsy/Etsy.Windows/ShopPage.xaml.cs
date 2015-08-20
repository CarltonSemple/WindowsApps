using Etsy.Common;
using Etsy.DataBinding;
using Etsy.DataTransfer;
using Etsy.Model;
using Etsy.Model.List;
using Etsy.Model.Shop;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Etsy
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShopPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        // Page variables
        IncrementalSource<ListGetter, Listing> products;    // = new IncrementalSource<ListGetter, Listing>("trending", "");
        private static double scrollPosition = 0.0;                                    // position of ListView
        private static bool scrollDown = false;
        private static bool refresh = true;
        //private static ObservableCollection<KeyEnumPair<string, SortOptions, string>> sortingOptions = SearchRefinery.SortingOptions();

        private List<Parameter> searchParameters = new List<Parameter>();
        private List<Parameter> sortParameters = new List<Parameter>();
        private List<Parameter> filterParameters = new List<Parameter>();

        private static ProductList prodList = new ProductList("active");
        private static User user = App.user;
        private string searchTerm = "";
        private GeneralInfo shop;

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


        public ShopPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            Window.Current.SizeChanged += Current_SizeChanged;

            // timer
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;                               // tick handler (time)
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
            if (App.logged_in)
            {
                this.DefaultViewModel["login_name"] = user.login_name;                      // data bind to the user's login name
                this.DefaultViewModel["image_url_75x75"] = user.Profile.image_url_75x75;    // data binding for user image
            }

            shop = (GeneralInfo)e.NavigationParameter; 


            // Set up data binding for shop info
            shopName.Text = shop.shop_name;
            shopTitle.Text = shop.title;

            shop.about = await ShopAccess.getShopAbout(shop);

            if (shop.about.Images.Count > 0)
                shopAvatar.Source = new BitmapImage(new Uri(shop.about.Images[0].url_545xN));

            // Search refinements
            sortOptionsView.ItemsSource = SearchRefinery.SortingOptions();
            sortOptionsView.SelectedIndex = 1;


            searchParameters.AddRange(SearchRefinery.sortingParameters(SortOptions.Relevancy));     // default sort            

            try
            {
                if (refresh == true)                                 // start anew
                    products = new IncrementalSource<ListGetter, Listing>(shop.shop_id, "active", searchTerm, new List<Parameter>(), App.defaultAddress.country_id);
                else
                    await loadSavedListState();                                             // Load the previous list and scroll position
            }
            catch // no list was in existence prior                          
            {
                products = new IncrementalSource<ListGetter, Listing>(shop.shop_id, "active", searchTerm, new List<Parameter>(), App.defaultAddress.country_id);      // create a new list
            }
            finally
            {
                productList.ItemsSource = products;                                         // set the source of the xaml list
            }

            this.DefaultViewModel["searchTerm"] = searchTerm;

            this.DefaultViewModel["productList"] = prodList.results;

            refresh = false;
        }

        /// <summary>
        /// Load the list and get the previous position of the ListView
        /// </summary>
        /// <returns></returns>
        private async Task loadSavedListState()
        {
            products = new IncrementalSource<ListGetter, Listing>(shop.shop_id, "active", "", new List<Parameter>(), App.defaultAddress.country_id);
            products = await FileIO.DeserializeFromFile<IncrementalSource<ListGetter, Listing>>(shop.shop_name + "ProductsXAML");
            await products.loadAttributes(shop.shop_name + "ProductsXAML_attributes");

            productList.ItemsSource = products;                                         // set the source of the xaml list

            var scroller = FindVisualChild<ScrollViewer>(productList);                  // scroll to the previously saved ListView position
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
            await FileIO.SerializeAndSave<IncrementalSource<ListGetter, Listing>>(products, shop.shop_name + "ProductsXAML");
            await products.saveAttributes(shop.shop_name + "ProductsXAML_attributes");

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
        }

        #endregion


        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            // Get the new view state
            string CurrentViewState = ApplicationView.GetForCurrentView().Orientation.ToString();

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

        private void search(object sender, RoutedEventArgs e)
        {
            if(searchBox.Text != "")
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
        /// Create a new version of the existing search, one that has a different filter or sort.
        /// Activated upon a change in the sort selection or the filter selection
        /// </summary>
        private void redo_FilteredSorted_Search()
        {
            searchParameters.Clear();                           // clear previous search parameters

            searchParameters.AddRange(sortParameters);          // Add the current sorting parameters
            searchParameters.AddRange(filterParameters);        // Add the current filter parameters

            products = new IncrementalSource<ListGetter, Listing>(shop.shop_id, "active", searchTerm, searchParameters, App.defaultAddress.country_id);

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

        private async void favoriteButton_Click(object sender, RoutedEventArgs e)
        {
            if (shop.isFavorite == null)
            {
                await FavoritesAccess.AddFavoriteUserShop(shop, App.userID);    // add favorite
                favoriteBlock.Text = "Favorite Added";
            }
            else if (shop.isFavorite == false)
            {
                await FavoritesAccess.AddFavoriteUserShop(shop, App.userID);    // add favorite
                favoriteBlock.Text = "Favorite Added";
            }
            else
            {
                await FavoritesAccess.RemoveFavoriteUser(shop);     // remove favorite
                favoriteBlock.Text = "Favorite Removed";
            }
        }

        private void heartFilled_Loaded(object sender, RoutedEventArgs e)
        {
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

        private void reviewsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ReviewsPage), shop);
        }

        private void cartButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CartPage));
        }
    }
}
