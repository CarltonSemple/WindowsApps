using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Delivery_com.Data;
using Delivery_com.Common;
using Delivery_com.DataModel;
using System.Collections.ObjectModel;
using Windows.UI;

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace Delivery_com
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        // test to see if an address has been chosen before allowing the search to commence ;; avoid error!
        bool addressSelected = false;

        bool merchantSelected = false;

        // strings used for api requests
        private string client_id = "NzdhOWE3ZDgzYmZjN2I1OTdlMDM1NjRmNzFhMTlhN2Iz";
        private string secret = "05tCh5gI4PvfpntkUB17P1ZWWYFJER9nUBp9ds6U";
        private string address;

        // client id from staging.delivery.com. with the following client_id, use sandbox.delivery.com instead of api.delivery.com
        // oauth 2.0 credentials
        //private string client_id = "MjM2NDJkNDk0NjMxZmFlZjNjZTZhZWY2YWEwYTJhNGJh";
        //private string secret = "8B6hSXUNCq0Oudeu2SDHMalk9QzjWGqrohV9bqR6";

        // temporary
        string deliveryResult;

        // oauth 2.0 clientID: NzdhOWE3ZDgzYmZjN2I1OTdlMDM1NjRmNzFhMTlhN2Iz
        // oauth 2.0 secret: 05tCh5gI4PvfpntkUB17P1ZWWYFJER9nUBp9ds6U

        // test search url with json response: https://api.delivery.com/merchant/search/delivery?client_id=NzdhOWE3ZDgzYmZjN2I1OTdlMDM1NjRmNzFhMTlhN2Iz&address=130NottinghamRoad13210

        //String authenticationurl = "https://sandbox.delivery.com/third_party/authorize?client_id=MjM2NDJkNDk0NjMxZmFlZjNjZTZhZWY2YWEwYTJhNGJh&redirect_uri=http://localhost&response_type=code&scope=global&state=hmm";
        
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

        public HubPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;

            merchantListSection.Width = 0;
            merchantPreviewSection.Width = 0;
            orderButton.Opacity = 0;
            orderButton.IsEnabled = false;
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
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            //var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-4");
            //this.DefaultViewModel["Section3Items"] = sampleDataGroup;

        }

        /// <summary>
        /// Invoked when a HubSection header is clicked.
        /// </summary>
        /// <param name="sender">The Hub that contains the HubSection whose header was clicked.</param>
        /// <param name="e">Event data that describes how the click was initiated.</param>
        void Hub_SectionHeaderClick(object sender, HubSectionHeaderClickEventArgs e)
        {
            HubSection section = e.Section;
            var group = section.DataContext;
            this.Frame.Navigate(typeof(SectionPage), ((SampleDataGroup)group).UniqueId);
        }


        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        /// <param name="sender">The GridView or ListView
        /// displaying the item clicked.</param>
        /// <param name="e">Event data that describes the item clicked.</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemPage), itemId);
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
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


              
        /// <summary>
        /// // begin to search, and hide the addresses, after an address is selected and the button is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void search_checked(object sender, RoutedEventArgs e)
        {
            if (initial.Width > 0)
            {
                // When making the panel invisible, search for merchants at the chosen address

                // only proceed if an address has been selecteed
                if (addressSelected == true)
                {
                    // return the unfiltered list of merchants
                    var Merchants = await SearchSource.GetMerchantsAsync();
                    (App.Current as App).Merchants = (ObservableCollection<Merchant>)Merchants; // a conversion is technically necessary

                    // keep the local Merchants as the binding for the view model
                    this.DefaultViewModel["Merchants"] = Merchants;
                    initial.Width = 0;
                    merchantListSection.Width = 520;
                    merchantPreviewSection.Width = 520;
                    merchantListSection.Visibility = Visibility.Visible;
                    orderButton.Opacity = 1;
                }
                else
                {   // reset the toggle to how it was if no address has been selected yet
                    var togglebutton = (ToggleButton)sender;
                    togglebutton.IsChecked = false;
                }
            }
        }

        /// <summary>
        /// restore the addresses once the button is unchecked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search_unchecked(object sender, RoutedEventArgs e)
        {
            if (initial.Width <= 0)
            {
                initial.Width = 900;
                merchantListSection.Width = 0;
                merchantListSection.Visibility = Visibility.Collapsed;
                merchantPreviewSection.Width = 0;
                orderButton.Opacity = 0;
                orderButton.IsEnabled = false;
            }            
        }


        /// <summary>
        /// Customer Addresses ListBox, load the items and set selectedItem to 0
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddressList_Loaded(object sender, RoutedEventArgs e)
        {
            var listbox = (ListBox)sender;
            listbox.ItemsSource = (App.Current as App).customer.customer_Addresses;
            listbox.SelectedIndex = 0; // initialize with an item selected so as to avoid error
        }

        
        /// <summary>
        /// Set the selectedAddress Location variable in the customer to the selected address from the listbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddressList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            addressSelected = true;
            var listBox = (ListBox)sender;
            (App.Current as App).customer.selectedAddress = (LocationCustomer)listBox.SelectedItem;
            (App.Current as App).searchURL = String.Format("https://api.delivery.com/merchant/search/delivery" +
                                                                                                    "?client_id={0}" +
                                                                                                    "&address={1}", 
                                                                                                    client_id, 
                                                                                                    (App.Current as App).customer.selectedAddress.searchAddress);
        }


        /// <summary>
        /// Change the preview section whenever the selected merchant changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void merchantList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {           
            merchantSelected = true;
            var list = (ListBox)sender;
            Merchant MerchantSelected = (Merchant)list.SelectedItem;

            if(list.SelectedIndex != -1)
                orderButton.IsEnabled = true;

            // set the global merchant
            (App.Current as App).selectedMerchant = MerchantSelected;

            // set the key in the view model to apply to this one merchant. the preview panel will use this
            this.DefaultViewModel["Merchant"] = MerchantSelected;
        }
        
        /// <summary>
        /// When the merchant List loads, initialize with the selectedIndex = 0 so that the preview pane isn't empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void merchantList_Loaded(object sender, RoutedEventArgs e)
        {
            var list = (ListBox)sender; // cast the sender as the correct object type
            //list.SelectedIndex = 0;

            // doesn't work because it has to wait for the list to actually be created
        }

        private void orderButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SectionPage));
        }

        private void merchantList_GotFocus(object sender, RoutedEventArgs e)
        {
        
        }

        private void merchantList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
        }

        private void locationManagementClick(object sender, RoutedEventArgs e)
        {
            // Navigate to Location Management Page
            this.Frame.Navigate(typeof(ManageLocations));
        }

    }
}
