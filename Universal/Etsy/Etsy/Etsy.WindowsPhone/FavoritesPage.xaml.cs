using Etsy.Common;
using Etsy.DataBinding;
using Etsy.DataTransfer;
using Etsy.Model;
using Etsy.Model.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class FavoritesPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public FavoritesPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;        
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
            orientation_Adjustments();
            Window.Current.SizeChanged += Current_SizeChanged;

            // favorite listings
            FavoritesAccess.favoriteListings = new IncrementalSource<ListGetter, Listing>(1, -1, new List<Parameter>());
            this.DefaultViewModel["items"] = FavoritesAccess.favoriteListings; // bind the listview to the remote collection so that the ItemInFavoritesList controls can access it

            // favorite users
            FavoritesAccess.favoriteUsers = new IncrementalSource<UserGetter, User>();
            this.DefaultViewModel["shops"] = FavoritesAccess.favoriteUsers;
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
                adBottom.Visibility = Visibility.Collapsed;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);

            Window.Current.SizeChanged -= Current_SizeChanged;
        }

        #endregion
        
        /// <summary>
        /// Fix the UI according to the orientation
        /// </summary>
        private void orientation_Adjustments()
        {
            var CurrentViewState = ApplicationView.GetForCurrentView().Orientation;

            if (CurrentViewState.ToString() == "Portrait")
            {
                titleGrid.Visibility = Visibility.Visible;
                landscapeTitleGrid.Visibility = Visibility.Collapsed;
            }
            else  // landscape
            {
                titleGrid.Visibility = Visibility.Collapsed;
                landscapeTitleGrid.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Fix the UI according to the orientation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            orientation_Adjustments();
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

            this.Frame.Navigate(typeof(ItemDetailPage), list.SelectedItem);
        }

        private void shopList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = (ListView)sender;

            User user = (User)list.SelectedItem;

            this.Frame.Navigate(typeof(ShopPage), user.shop);
        }
    }
}
