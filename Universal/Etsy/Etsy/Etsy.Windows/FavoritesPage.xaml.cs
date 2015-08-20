using Etsy.Common;
using Etsy.DataBinding;
using Etsy.DataTransfer;
using Etsy.Model;
using Etsy.Model.Shop;
using Etsy.Model.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Etsy
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class FavoritesPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

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

        // variables 

        IncrementalSource<ListGetter, Listing> products; 

        public FavoritesPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            initial_Orientation();
            Window.Current.SizeChanged += Current_SizeChanged;
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
            // favorite listings
            FavoritesAccess.favoriteListings = new IncrementalSource<ListGetter, Listing>(1, -1, new List<Parameter>());
            productList.ItemsSource = FavoritesAccess.favoriteListings; // bind the listview to the remote collection so that the ItemInFavoritesList controls can access it
        
            // favorite users
            FavoritesAccess.favoriteUsers = new IncrementalSource<UserGetter, User>();
            shopList.ItemsSource = FavoritesAccess.favoriteUsers;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
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
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        void initial_Orientation()
        {
            var CurrentViewState = ApplicationView.GetForCurrentView().Orientation;

            if (CurrentViewState.ToString() == "Portrait")
            {
                itemsHeader.SetValue(Grid.ColumnSpanProperty, 2);
                productList.SetValue(Grid.RowSpanProperty, 1);
                productList.SetValue(Grid.ColumnSpanProperty, 2);

                shopsHeader.SetValue(Grid.RowProperty, 3);
                shopsHeader.SetValue(Grid.ColumnProperty, 0);
                shopList.SetValue(Grid.RowProperty, 4);
                shopList.SetValue(Grid.RowSpanProperty, 1);
                shopList.SetValue(Grid.ColumnProperty, 0);
                shopList.SetValue(Grid.ColumnSpanProperty, 2);
            }
            else
            {
                itemsHeader.SetValue(Grid.ColumnSpanProperty, 1);
                productList.SetValue(Grid.RowSpanProperty, 3);
                productList.SetValue(Grid.ColumnSpanProperty, 1);

                shopsHeader.SetValue(Grid.RowProperty, 1);
                shopsHeader.SetValue(Grid.ColumnProperty, 1);
                shopList.SetValue(Grid.RowProperty, 2);
                shopList.SetValue(Grid.RowSpanProperty, 3);
                shopList.SetValue(Grid.ColumnProperty, 1);
                shopList.SetValue(Grid.ColumnSpanProperty, 1);
            }
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

            string orientation = "";

            double AppWidth = e.Size.Width;
            double AppHeight = e.Size.Height;

            if (AppHeight / AppWidth > 1)
            {
                itemsHeader.SetValue(Grid.ColumnSpanProperty, 2);
                productList.SetValue(Grid.RowSpanProperty, 1);
                productList.SetValue(Grid.ColumnSpanProperty, 2);

                shopsHeader.SetValue(Grid.RowProperty, 3);
                shopsHeader.SetValue(Grid.ColumnProperty, 0);
                shopList.SetValue(Grid.RowProperty, 4);
                shopList.SetValue(Grid.RowSpanProperty, 1);
                shopList.SetValue(Grid.ColumnProperty, 0);
                shopList.SetValue(Grid.ColumnSpanProperty, 2);
            }
            else
            {
                itemsHeader.SetValue(Grid.ColumnSpanProperty, 1);
                productList.SetValue(Grid.RowSpanProperty, 3);
                productList.SetValue(Grid.ColumnSpanProperty, 1);

                shopsHeader.SetValue(Grid.RowProperty, 1);
                shopsHeader.SetValue(Grid.ColumnProperty, 1);
                shopList.SetValue(Grid.RowProperty, 2);
                shopList.SetValue(Grid.RowSpanProperty, 3);
                shopList.SetValue(Grid.ColumnProperty, 1);
                shopList.SetValue(Grid.ColumnSpanProperty, 1);
            }
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

        private void shopList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var list = (GridView)sender;

            User user = (User)list.SelectedItem;

            this.Frame.Navigate(typeof(ShopPage), user.shop);
        }

    }
}
