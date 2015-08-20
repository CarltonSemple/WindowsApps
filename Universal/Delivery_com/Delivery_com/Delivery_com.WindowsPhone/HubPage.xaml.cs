using Delivery_com.Common;
using Delivery_com.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

using Delivery_com.DataModel; // used to access the search url 

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace Delivery_com
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");


        // strings used for api requests
        //private string client_id = "NzdhOWE3ZDgzYmZjN2I1OTdlMDM1NjRmNzFhMTlhN2Iz";
        private string address;

        // client id from staging.delivery.com. with the following client_id, use sandbox.delivery.com instead of api.delivery.com
        // oauth 2.0 credentials
        private string client_id = "MjM2NDJkNDk0NjMxZmFlZjNjZTZhZWY2YWEwYTJhNGJh";
        private string secret = "8B6hSXUNCq0Oudeu2SDHMalk9QzjWGqrohV9bqR6";

        // temporary
        string deliveryResult;

        // oauth 2.0 clientID: NzdhOWE3ZDgzYmZjN2I1OTdlMDM1NjRmNzFhMTlhN2Iz
        // oauth 2.0 secret: 05tCh5gI4PvfpntkUB17P1ZWWYFJER9nUBp9ds6U



        // test search url with json response: https://api.delivery.com/merchant/search/delivery?client_id=NzdhOWE3ZDgzYmZjN2I1OTdlMDM1NjRmNzFhMTlhN2Iz&address=130NottinghamRoad13210




        //String authenticationurl = "https://sandbox.delivery.com/third_party/authorize?client_id=MjM2NDJkNDk0NjMxZmFlZjNjZTZhZWY2YWEwYTJhNGJh&redirect_uri=http://localhost&response_type=code&scope=global&state=hmm";
        
        public HubPage()
        {
            this.InitializeComponent();

            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;



            // API 
            // https://api.delivery.com/merchant/search/delivery?client_id=abc123&address=199WaterSt10038


            HttpClient client = new HttpClient();

            

            // temporary for now...
            address = "130NottinghamRd13210";

            string url = "https://api.deliver.com/merchant/search/delivery" +
                "?client_id={0}" +
                "&address={1}";

            var baseurl = string.Format(url, client_id, address);

            //string result = await client.GetStringAsync(baseurl);
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
        /// <see cref="Frame.Navigate(Type, object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            //var sampleDataGroups = await SampleDataSource.GetGroupsAsync();
            //this.DefaultViewModel["Groups"] = sampleDataGroups;

            // temporary for now...
            //address = "130NottinghamRoad13210";
            address = "322West45Street10036";

            (App.Current as App).searchURL = String.Format("https://sandbox.delivery.com/merchant/search/delivery" +
                                                                                                    "?client_id={0}" +
                                                                                                    "&address={1}",client_id,address);

            var Merchants = await SearchSource.GetMerchantsAsync();
            this.DefaultViewModel["Merchants"] = Merchants;
           

            int i = 0;
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
            // TODO: Save the unique state of the page here.
        }

        /// <summary>
        /// Shows the details of a clicked group in the <see cref="SectionPage"/>.
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Details about the click event.</param>
        private void GroupSection_ItemClick(object sender, ItemClickEventArgs e)
        {
            var groupId = ((Merchant)e.ClickedItem).uniqueID;
            

            // either display the selected item in the neighboring hub, or go to a page that shows the details... probably the latter
            hubsection2.Header = ((Merchant)e.ClickedItem).name;



            //if (!Frame.Navigate(typeof(SectionPage), groupId))
            //{
            //    throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            //}
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Defaults about the click event.</param>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(ItemPage), itemId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
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
        /// <param name="e">Event data that describes how this page was reached.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}