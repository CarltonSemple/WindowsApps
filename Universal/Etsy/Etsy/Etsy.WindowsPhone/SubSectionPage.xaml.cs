using Etsy.Common;
using Etsy.DataTransfer;
using Etsy.Model.Browse;
using Etsy.Model.User;
using Etsy.UI_Extras;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace Etsy
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SubSectionPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private static bool hideEverything = false;
        private static BrowseSegment parentSegment;
        private static ObservableCollection<BrowseSegment> subSegments;
        private static bool started = false, started2 = false;
        private User user;

        public SubSectionPage()
        {
            this.InitializeComponent();

            // disable navigation animation
            Frame mainFrame = Window.Current.Content as Frame;
            mainFrame.ContentTransitions = null;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;

            Window.Current.SizeChanged += Current_SizeChanged;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            Window.Current.SizeChanged += Current_SizeChanged;
            parentSegment = (BrowseSegment)e.NavigationParameter;

            // null parameter error
            if (Loading.Null_Error<BrowseSegment>(parentSegment, loadErrorBlock))
            {
                LoadErrorPanel.Visibility = Visibility.Visible;     // Show the error
                errorVisibilityBlocker.Visibility = Visibility.Visible;
                return;
            }

            Loading.ControlProgressRing<BrowseSegment>(subSegments, pRing);     // progress ring

            // start loading the browse segments
            Task<ObservableCollection<BrowseSegment>> tbrowse = DataGET.findBrowseSegments(parentSegment.path);

            this.DefaultViewModel["title"] = parentSegment.name;

            // Finish loading
            subSegments = await tbrowse;
            //subSegments = parentSegment.sub_sections;
            this.DefaultViewModel["segments"] = subSegments;        // data binding
            
            Loading.ControlProgressRing<BrowseSegment>(subSegments, pRing);     // progress ring
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            //subSegments = null;
            //liView.ItemsSource = null;
            //griView.ItemsSource = null;
            //GC.Collect();
        }

        /// <summary>
        /// Get size and orientation of screen upon orientation change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            // Get the new view state
            var CurrentViewState = ApplicationView.GetForCurrentView().Orientation;

            double AppWidth = e.Size.Width;
            double AppHeight = e.Size.Height;

            try
            {
                if (AppHeight / AppWidth > 1)
                {
                    liView.Visibility = Visibility.Visible;
                    griView.Visibility = Visibility.Collapsed;
                }
                else
                {
                    liView.Visibility = Visibility.Collapsed;
                    griView.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ee)
            {
                string message = ee.Message;
            }
            
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);

            // Hide the ads if the user paid to remove them
            if (App.purchased)
                if(adBottom != null)
                    adBottom.Visibility = Visibility.Collapsed;

            if (App.logged_in)   // change the user options
            {
                user = App.user;
                if(user != null)
                    if(user.Profile != null)
                        this.DefaultViewModel["image_url_75x75"] = user.Profile.image_url_75x75;    // data binding for user image
                try
                {
                    if (avatar != null)
                        if(avatar.Visibility != Visibility.Visible)
                            avatar.Visibility = Visibility.Visible;
                }
                catch(Exception ee)
                {
                    string message = ee.Message;
                }
            }
            else
            {
                if(avatar != null)
                    avatar.Visibility = Visibility.Collapsed;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);

            //liView = null;
            //griView = null;

            Window.Current.SizeChanged -= Current_SizeChanged;
        }

        #endregion


        private void liView_Loaded(object sender, RoutedEventArgs e)
        {
            ListView lview = (ListView)sender;
            double height = this.Frame.ActualHeight, width = this.Frame.ActualWidth;

            if (height / width < 1)
                lview.Visibility = Visibility.Collapsed;
        }

        private void griView_Loaded(object sender, RoutedEventArgs e)
        {
            GridView gview = (GridView)sender;
            double height = this.Frame.ActualHeight, width = this.Frame.ActualWidth;

            if (height / width > 1)
                gview.Visibility = Visibility.Collapsed;
        }


        /// <summary>
        /// Navigate to the cart button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cartButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CartPage));
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
        /// Portrait list selection-changed event
        /// Navigate to the browse page for the selected path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void liView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lview = (ListView)sender;
            if (!started)
            {
                started = true;
                return;
            }

            //this.Frame.Navigate(typeof(SegmentBrowsePage), liView.SelectedItem);
        }

        /// <summary>
        /// Landscape list selection-changed event.
        /// Navigate to the browse page for the selected path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void griView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GridView gview = (GridView)sender;
            if (!started2)
            {
                started2 = true;
                return;
            }

            //this.Frame.Navigate(typeof(SegmentBrowsePage), gview.SelectedItem);
        }

        private void liView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(SegmentBrowsePage), e.ClickedItem);
        }

        private void griView_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(SegmentBrowsePage), e.ClickedItem);
        }

        /// <summary>
        /// Redo the search with whatever is in the search box. Keep the same filter & sort preferences
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search(object sender, RoutedEventArgs e)
        {
            searchButton.SetValue(Grid.RowProperty, 1);
            searchButton.SetValue(Grid.ColumnProperty, 0);

            if (searchBox.Visibility == Visibility.Visible)
            {
                if (searchBox.Text != "")
                    this.Frame.Navigate(typeof(SearchPage), searchBox.Text);
                else
                { // return to original positions/states
                    searchButton.SetValue(Grid.RowProperty, 0);
                    searchButton.SetValue(Grid.ColumnProperty, 2);
                    searchBox.Visibility = Visibility.Collapsed;
                    visibilityBlocker.Visibility = Visibility.Collapsed;
                    return;
                }
            }

            searchBox.Visibility = Visibility.Visible;
            searchBox.Focus(FocusState.Programmatic);
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
        /// Navigate to main page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        /// <summary>
        /// Navigate to the profile page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void profileButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ProfilePage));
        }

        /// <summary>
        /// Hide the search box and move the search button back to its original position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void visibilityBlocker_Tapped(object sender, TappedRoutedEventArgs e)
        {
            searchBox.Visibility = Visibility.Collapsed;
            searchButton.SetValue(Grid.RowProperty, 0);
            searchButton.SetValue(Grid.ColumnProperty, 2);
            visibilityBlocker.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Return to the previous page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void error_goBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (navigationHelper.CanGoBack())
                this.Frame.GoBack();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
        }
    }
}
