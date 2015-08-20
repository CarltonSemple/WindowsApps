using Etsy.Common;
using Etsy.Model;
using Etsy.Model.List;
using Etsy.Model.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Etsy.DataBinding;
using Etsy.DataTransfer;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Etsy.Model.Browse;
using Windows.ApplicationModel.Store;
using Etsy.UI_Extras;

namespace Etsy
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;

        // Aid in navigation
        private NavigationHelper navigationHelper;

        // Observable Dictionary makes data binding easier
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        // Page variables
        IncrementalSource<ListGetter, Listing> products;    // = new IncrementalSource<ListGetter, Listing>("trending", "");
        private static double scrollPosition = 0.0;                                    // position of ListView
        private static bool refresh = false;
        private static ProductList prodList = new ProductList("trending");
        private static User user = App.user;
        private static ObservableCollection<BrowseSegment> browseSections;      // browse sections, initialized upon browseButton_Click

        private static string currentView = "Browse";                           // control which shows up, browse or trending list
        private bool hideEverything = false;
        private static string currentOrientation;

        // xaml pointers
        WrapGrid wGrid;

        DispatcherTimer timer;      // used in tracking the direction of the listview scroll action
        private static int loadCount = 0;   // used to determine if the load time is taking too long

        
        private double searchButton_startPercentage = 0.3;
        private double searchButton_positionPercentage = 0.3;   // keep track of the percentage of the page's width that the search button is at, in case the device is rotated
        private double searchButton_goalPercentage = 0.6;

        public MainPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            initial_orientation();
            Window.Current.SizeChanged += Current_SizeChanged;

            // timer
            timer = new DispatcherTimer();
            // tick handler (time)
            timer.Interval = new TimeSpan(0, 0, 0, 0, 2);        // time between each tick

            timer.Start();
        }






        /***************** Navigation & Dictionary ****************/

        /// <summary>
        /// Gets the NavigationHelper used to aid in navigation and process lifetime management.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the DefaultViewModel. This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Handle things according to the timer's interval
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, object e)
        {
            if (!App.logged_in)
            {
                pRing.Visibility = Visibility.Collapsed;
                return;
            }

            if (currentView == "Trending")
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
                if (products.Count == 0)
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
            }

            // The browse list functions handle the progress ring separately
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            timer.Start();
            timer.Tick += timer_Tick;
            Window.Current.SizeChanged += Current_SizeChanged;
            this.DefaultViewModel["productList"] = prodList.results;

            if (App.logged_in)
            {
                user = App.user;
                this.DefaultViewModel["login_name"] = user.login_name;                      // data bind to the user's login name
                this.DefaultViewModel["image_url_75x75"] = user.Profile.image_url_75x75;    // data binding for user image
            }

            try
            {
                if (App.started == false || refresh == true)
                {// start anew
                    products = new IncrementalSource<ListGetter, Listing>("trending", "", new List<Parameter>(), App.defaultAddress.country_id);
                }
                else
                    await loadSavedListState();                                             // Load the previous list and scroll position
            }
            catch // no list was in existence prior                          
            {
                products = new IncrementalSource<ListGetter, Listing>("trending", "", new List<Parameter>(), App.defaultAddress.country_id);      // create a new list
            }
            finally
            {
                productList.ItemsSource = products;                                         // set the source of the xaml list
            }

            App.started = true;
        }

        /// <summary>
        /// Load the list and get the previous position of the ListView
        /// </summary>
        /// <returns></returns>
        private async Task loadSavedListState()
        {
            // load browse segments
            if(App.browseLoadedOnce)
                browseSections = await FileIO.DeserializeFromFile<ObservableCollection<BrowseSegment>>("mainBrowseSegments");

            products = new IncrementalSource<ListGetter, Listing>("trending", "", new List<Parameter>(), App.defaultAddress.country_id);
            products = await FileIO.DeserializeFromFile<IncrementalSource<ListGetter, Listing>>("productsXAML");
            await products.loadAttributes("productsXAML_attributes");

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

                await FileIO.EncryptAndSave(currentView, "mainPageView");       // save which view will show up when loading the page next time

                // Save the browse list
                await FileIO.SerializeAndSave<ObservableCollection<BrowseSegment>>(browseSections, "mainBrowseSegments");
            }
            catch(Exception ee)
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
            App.browseLoadedOnce = true;    // load the saved version of the browse segments upon returning to this page from inside the app

            await FileIO.SerializeAndSave<IncrementalSource<ListGetter, Listing>>(products, "productsXAML");
            await products.saveAttributes("productsXAML_attributes");

            var scroller = FindVisualChild<ScrollViewer>(productList);
            scrollPosition = scroller.VerticalOffset;                           // save listView position

            return;
        }


        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// and <see cref="Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </summary>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);

            Window.Current.SizeChanged += Current_SizeChanged;

            if (App.logged_in)
                loginEnforcer.Visibility = Visibility.Collapsed;

            try
            {
                currentView = await FileIO.ReadEncryptedFileAsString("mainPageView");      // get previously saved view
            }
            catch
            {
                currentView = "Browse";
            }

            if(currentView == "Browse")     // make the browse list visible
            {
                productList.Visibility = Visibility.Collapsed;
                browseListView.Visibility = Visibility.Visible;
                this.DefaultViewModel["currentView"] = "Browse";
            }
            else                            // make the trending list visible
            {
                productList.Visibility = Visibility.Visible;
                browseListView.Visibility = Visibility.Collapsed;
                this.DefaultViewModel["currentView"] = "Trending";
            }

            if(App.logged_in)   // change the user options
            {
                user = App.user;
                this.DefaultViewModel["login_name"] = user.login_name;                      // data bind to the user's login name
                this.DefaultViewModel["image_url_75x75"] = user.Profile.image_url_75x75;    // data binding for user image
                loginButton.Visibility = Visibility.Collapsed;
                NameAndPhoto.Visibility = Visibility.Visible;
            }
            else
            {
                loginButton.Visibility = Visibility.Visible;
                NameAndPhoto.Visibility = Visibility.Collapsed;
            }

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);

            // save the incremental loading list
            prodList.results = products;

            Window.Current.SizeChanged -= Current_SizeChanged;
            timer.Stop();
            timer.Tick -= timer_Tick;
        }

        #endregion

        private void initial_orientation()
        {
            // Get the new view state
            var CurrentViewState = ApplicationView.GetForCurrentView().Orientation;
            currentOrientation = CurrentViewState.ToString();

            if (currentOrientation == "Portrait")  // portrait
            {
                statusBlock.SetValue(Grid.ColumnSpanProperty, 2);

                if (searchBoxLandscape.Text != "") // put the search button & box into position if the search box already has text in it
                {
                    searchButton.SetValue(Grid.RowProperty, 1);
                    searchBoxLandscape.Visibility = Visibility.Visible;
                }

                // search box
                searchBoxLandscape.SetValue(Grid.RowProperty, 1);
                searchBoxLandscape.SetValue(Grid.ColumnProperty, 1);
                searchBoxLandscape.SetValue(Grid.ColumnSpanProperty, 2);
                if (wGrid != null)
                    wGrid.ItemWidth = 342;
            }
            else
            {
                statusBlock.SetValue(Grid.ColumnSpanProperty, 1);

                searchButton.SetValue(Grid.RowProperty, 0);
                if (searchBoxLandscape.Text != "") // put the search button & box into position if the search box already has text in it
                    searchBoxLandscape.Visibility = Visibility.Visible;

                // search box
                searchBoxLandscape.SetValue(Grid.RowProperty, 0);
                searchBoxLandscape.SetValue(Grid.ColumnProperty, 2);
                searchBoxLandscape.SetValue(Grid.ColumnSpanProperty, 1);

                if (wGrid != null)
                    wGrid.ItemWidth = 400;
            }
        }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            
            // Get the new view state
            var CurrentViewState = ApplicationView.GetForCurrentView().Orientation;
            currentOrientation = CurrentViewState.ToString();

            // Adapt to the view port size rather than the device orientation
            double AppWidth = e.Size.Width;
            double AppHeight = e.Size.Height;

            double ratio = AppHeight / AppWidth;

            if(ratio > 1)  // portrait
            {
                statusBlock.SetValue(Grid.ColumnSpanProperty, 2);

                if (searchBoxLandscape.Text != "") // put the search button & box into position if the search box already has text in it
                {
                    searchButton.SetValue(Grid.RowProperty, 1);
                    searchBoxLandscape.Visibility = Visibility.Visible;
                }

                // search box
                searchBoxLandscape.SetValue(Grid.RowProperty, 1);
                searchBoxLandscape.SetValue(Grid.ColumnProperty, 1);
                searchBoxLandscape.SetValue(Grid.ColumnSpanProperty, 2);
                if (wGrid != null)
                    wGrid.ItemWidth = 342;
            }
            else
            {
                statusBlock.SetValue(Grid.ColumnSpanProperty, 1);

                searchButton.SetValue(Grid.RowProperty, 0);
                if (searchBoxLandscape.Text != "") // put the search button & box into position if the search box already has text in it
                    searchBoxLandscape.Visibility = Visibility.Visible;

                // search box
                searchBoxLandscape.SetValue(Grid.RowProperty, 0);
                searchBoxLandscape.SetValue(Grid.ColumnProperty, 2);
                searchBoxLandscape.SetValue(Grid.ColumnSpanProperty, 1);

                if(wGrid != null)
                    wGrid.ItemWidth = 400;
            }

           
                
        
        }


        /// <summary>
        /// Change the UI according to the orientation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OrientationChanged(object sender, SimpleOrientationSensorOrientationChangedEventArgs e)
        {


            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                SimpleOrientation orientation = e.Orientation;
                switch (orientation)
                {
                    //case SimpleOrientation.NotRotated:
                    //    ;
                    //    break;
                    //case SimpleOrientation.Rotated90DegreesCounterclockwise:
                    //    txtOrientation.Text = "Rotated 90 Degrees Counterclockwise";
                    //    break;
                    //case SimpleOrientation.Rotated180DegreesCounterclockwise:
                    //    txtOrientation.Text = "Rotated 180 Degrees Counterclockwise";
                    //    break;
                    //case SimpleOrientation.Rotated270DegreesCounterclockwise:
                    //    txtOrientation.Text = "Rotated 270 Degrees Counterclockwise";
                    //    break;
                    //case SimpleOrientation.Faceup:
                    //    txtOrientation.Text = "Faceup";
                    //    break;
                    //case SimpleOrientation.Facedown:
                    //    txtOrientation.Text = "Facedown";
                    //    break;
                    default:
                        //txtOrientation.Text = "Unknown orientation";
                        break;
                }
            });
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
        /// Navigate to the selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void productList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = (ListView)sender;

            //this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;

            this.Frame.Navigate(typeof(ItemDetailPage1), list.SelectedItem);
        }

        /// <summary>
        /// Navigate to the user's profile page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void profileButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ProfilePage));
        }

        /// <summary>
        /// Only show the username and photo if the user is logged in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameAndPhoto_Loaded(object sender, RoutedEventArgs e)
        {
            Grid grid = (Grid)sender;

            if (App.logged_in == false)
                grid.Visibility = Visibility.Collapsed;             // hide if the user isn't logged in
        }

        /// <summary>
        /// Only show the log in button if the user isn't logged in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginButton_Loaded(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            if (App.logged_in == true)
                button.Visibility = Visibility.Collapsed;           // hide if the user is logged in
        }

        /// <summary>
        /// Go to the log in page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LoginPage));                 // navigate to LoginPage
        }

        public void changeLoginButtonStatus()
        {
            
        }

        /// <summary>
        /// Move the search button to the left, then enable the search textbox.
        /// The horizontal move distance is calculated based off of the percentages used by the main grid columns
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentOrientation == "Portrait")
            {
                searchButton.SetValue(Grid.RowProperty, 1);
                if (searchBoxLandscape.Visibility == Visibility.Visible)
                {
                    if (searchBoxLandscape.Text != "")
                        this.Frame.Navigate(typeof(SearchPage), searchBoxLandscape.Text);
                }
                else
                    searchBoxLandscape.Visibility = Visibility.Visible;
            }
            //searchButton.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;   // Move the search button

            searchBoxLandscape.Visibility = Visibility.Visible;
            searchBoxLandscape.Focus(FocusState.Programmatic);                                                     // focus on the search box
                        
            searchLine.Visibility = Visibility.Visible;

            if (searchBoxLandscape.Text != "")
                this.Frame.Navigate(typeof(SearchPage), searchBoxLandscape.Text);
        }


        /// <summary>
        /// Start off at a percentage of the page's width
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchButton_Loaded(object sender, RoutedEventArgs e)
        {
            //double pageWidth = mainGrid.ActualWidth;
            //double displacement = searchButton_startPercentage * pageWidth;

            //Button button = (Button)sender;

            //button.Margin = new Thickness(  0,              // left 
            //                                0,              // top
            //                                displacement,   // right is a percentage of the page's width
            //                                0);             // bottom

            //searchButton_positionPercentage = displacement / pageWidth;

        }

        /// <summary>
        /// Lower the visibility of the rest of the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            hideEverything = true;
            visibilityBlocker.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Return visibility to the rest of the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (currentOrientation == "Portrait")
            {
                searchBoxLandscape.Visibility = Visibility.Collapsed;
                searchButton.SetValue(Grid.RowProperty, 0);
            }
            hideEverything = false;
            visibilityBlocker.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Make the browse list items smaller if the app is in portrait mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wGrid2_Loaded(object sender, RoutedEventArgs e)
        {
            wGrid = (WrapGrid)sender;

            var CurrentViewState = ApplicationView.GetForCurrentView().Orientation;

            if (CurrentViewState.ToString() == "Portrait")
                wGrid.ItemWidth = 342;
            else
                wGrid.ItemWidth = 400;
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
        /// Show/hide the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuButton_Click(object sender, RoutedEventArgs e)
        {
            if (menuPanel.Margin.Top == -775)
            {
                menuPanel.Margin = new Thickness(0, 0, 0, 0);           // Show menu
                visibilityBlocker2.Visibility = Visibility.Visible;     // hide everything else
            }
            else
            {
                menuPanel.Margin = new Thickness(0, -775, 0, 0);        // Hide menu
                visibilityBlocker2.Visibility = Visibility.Collapsed;   // show everything else
            }

        }

        private void profileButton2_Loaded(object sender, RoutedEventArgs e)
        {
            Button pBut = (Button)sender;
            if (App.logged_in == false)
                pBut.Visibility = Visibility.Collapsed;
        }

        private void avatar2_Loaded(object sender, RoutedEventArgs e)
        {
            Image av = (Image)sender;
            if (App.logged_in == false)
                av.Visibility = Visibility.Collapsed;
        }


        /// <summary>
        /// Show the trending list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void trendingButton_Click(object sender, RoutedEventArgs e)
        {
            productList.Visibility = Visibility.Visible;            // show trending list
            browseListView.Visibility = Visibility.Collapsed;       // hide browse sections

            // Change statusBlock to say Trending
            currentView = "Trending";
            this.DefaultViewModel["currentView"] = "Trending";

            menuPanel.Margin = new Thickness(0, -775, 0, 0);        // hide menu
            visibilityBlocker.Visibility = Visibility.Collapsed;   // show everything
            visibilityBlocker2.Visibility = Visibility.Collapsed;   // show everything

            await FileIO.EncryptAndSave(currentView, "mainPageView");       // save which view will show up when loading the page next time
        }

        /// <summary>
        /// Load the latest browse sections and make it all visible, hiding the menu as well
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void browseButton_Click(object sender, RoutedEventArgs e)
        {
            // Change statusBlock to say Browse
            currentView = "Browse";
            this.DefaultViewModel["currentView"] = "Browse";

            Loading.ControlProgressRing<BrowseSegment>(browseSections, pRing);     // progress ring
            Task<ObservableCollection<BrowseSegment>> sBrowse;
            if (browseSections != null)
            {
                if (browseSections.Count == 0)
                {
                    sBrowse = DataGET.findBrowseSegments("");
                    browseSections = await sBrowse;
                }
            }
            else
            {
                sBrowse = DataGET.findBrowseSegments("");
                browseSections = await sBrowse;
            }

            Loading.ControlProgressRing<BrowseSegment>(browseSections, pRing);     // progress ring

            browseListView.ItemsSource = browseSections;

            App.browseLoadedOnce = true;

            productList.Visibility = Visibility.Collapsed;          // hide trending list
            browseListView.Visibility = Visibility.Visible;         // Show browse sections


            menuPanel.Margin = new Thickness(0, -775, 0, 0);        // Hide menu
            visibilityBlocker.Visibility = Visibility.Collapsed;   // show everything
            visibilityBlocker2.Visibility = Visibility.Collapsed;   // show everything else

            await FileIO.EncryptAndSave(currentView, "mainPageView");       // save which view will show up when loading the page next time
        }

        /// <summary>
        /// Go to the favorites page to view the logged in user's favorites
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void favoritesButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FavoritesPage));
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }

        /// <summary>
        /// Load the latest browse sections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void browseListView_Loaded(object sender, RoutedEventArgs e)
        {
            Loading.ControlProgressRing<BrowseSegment>(browseSections, pRing);     // progress ring
            ListView lview = (ListView)sender;
            Task<ObservableCollection<BrowseSegment>> sBrowse;
            if (browseSections != null)
            {
                if (browseSections.Count == 0)
                {
                    sBrowse = DataGET.findBrowseSegments("");
                    browseSections = await sBrowse;
                }
            }
            else
            {
                sBrowse = DataGET.findBrowseSegments("");
                browseSections = await sBrowse;
            }
            App.browseLoadedOnce = true;

            Loading.ControlProgressRing<BrowseSegment>(browseSections, pRing);     // progress ring
            browseListView.ItemsSource = browseSections;
        }

        /// <summary>
        /// Go to that browsing section's sub section page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // http://www.c-sharpcorner.com/UploadFile/0f68f2/color-detecting-in-an-image-in-C-Sharp/
            ListView lview = (ListView)sender;
            BrowseSegment selectedSection = lview.SelectedItem as BrowseSegment;

            this.Frame.Navigate(typeof(SubSectionPage), selectedSection);       // Send the section to the next page
        }

        /// <summary>
        /// Show ads or hide them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void adsPanel_Loaded(object sender, RoutedEventArgs e)
        {
            StackPanel panel = (StackPanel)sender;
            if (App.advertisements)
                panel.Visibility = Visibility.Visible;
            else
                panel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Remove advertisements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void purchaseButton_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                await CurrentApp.RequestProductPurchaseAsync("noAdvertisements");

                // check for success
                App.checkLicenseState();
                if (App.advertisements == false)
                    adsPanel.Visibility = Visibility.Collapsed;
            }
            catch
            {
                // error occurred, so purchase was not completed
                int i = 0;
            }
            
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage));
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
            timer.Tick -= timer_Tick;
        }

        private void imageQualityButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }

        /// <summary>
        /// Try to reload the items for the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            products = new IncrementalSource<ListGetter, Listing>("trending", "", new List<Parameter>(), App.defaultAddress.country_id);

            productList.ItemsSource = products;                 // data bind
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

        private void visibilityBlocker2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            visibilityBlocker2.Visibility = Visibility.Collapsed;
            menuPanel.Margin = new Thickness(0, -775, 0, 0);        // Hide menu
        }

    }
}
