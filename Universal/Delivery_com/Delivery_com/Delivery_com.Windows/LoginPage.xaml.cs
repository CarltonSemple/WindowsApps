using Delivery_com.Common;
using Delivery_com.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Delivery_com
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class LoginPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        // client id from staging.delivery.com. with the following client_id, use sandbox.delivery.com instead of api.delivery.com
        // oauth 2.0 credentials
        //private string client_id = "MjM2NDJkNDk0NjMxZmFlZjNjZTZhZWY2YWEwYTJhNGJh";
        //private string client_secret = "8B6hSXUNCq0Oudeu2SDHMalk9QzjWGqrohV9bqR6";

        // oauth 2.0 clientID: 
        private string client_id = "NzdhOWE3ZDgzYmZjN2I1OTdlMDM1NjRmNzFhMTlhN2Iz";
        // oauth 2.0:
        private string client_secret = "05tCh5gI4PvfpntkUB17P1ZWWYFJER9nUBp9ds6U";

        // retrieved first, used to get access_token
        string authorizationCode = "";

        // used when getting access_token. it will either be "authorization_code" or "refresh_token"
        string grant_type;

        // the access_token is received in Get_Access_Token()...
        // This is the token you submit with any API requests that require it. 
        // You must include it in the Authorization HTTP header.
        string access_token;

        /************** other items received at the same time as the access_token **************/
        string token_type;

        // How long until this token expires, in seconds.
        int expires_in; 

        // The point in time that this token expires, in Unix Epoch Time.
        Int64 expires;  // 64-bit just to be safe; these numbers are pretty large.. 1391014905
        
        // use refresh_token instead of authorization_code after the first time
        string refresh_token = "";

        

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


        public LoginPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            // retrieve authorization code by allowing the user to log in
            Get_Authorization_Code();   // calls Get_Access_Token
            
        }

        /// <summary>
        /// Get the authorization code by directing the user to sign in.  Catch any errors and try again until there is a successful response.
        /// </summary>
        private async void Get_Authorization_Code()
        {
            String authenticationurl = String.Format("https://api.delivery.com/third_party/authorize?" + "client_id={0}" + "&redirect_uri=http://localhost&response_type=code&scope=global&state=hmm", client_id);
            Uri StartUri = new Uri(authenticationurl);
            Uri EndUri = new Uri("http://localhost");
            
            // prepare to split up the result string so as to obtain just the authorizationCode
                List<string> separators = new List<string>();
                separators.Add("code="); separators.Add("&state=");
            string[] separator = separators.ToArray();

            HttpClient client = new HttpClient();

            // split the results. the authorizationCode will be the [1], state will be [2]
            string[] results;
            try
            {
                WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(
                                                        WebAuthenticationOptions.None,
                                                        StartUri,
                                                        EndUri);

                if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    //authorizationCode = WebAuthenticationResult.ResponseData.Substring(23, WebAuthenticationResult.ResponseData.Length - 23);
                    results = WebAuthenticationResult.ResponseData.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    authorizationCode = results[1];
                    Get_Access_Token();
                }
                else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                {
                   // OutputToken("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                }
                else
                {
                   // OutputToken("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                }


            }
            catch
            {

            }
            
        }

        /// <summary>
        /// Use the authorization code received from a successful login, to then retrieve the access_code and accompanying refresh_code, along with expire times
        /// </summary>
        private async void Get_Access_Token()
        {
            string url = "https://api.delivery.com/third_party/access_token?";
            // it's a POST request, so we need the HttpClient()
            HttpClient httpClient = new HttpClient();

            // change grant_type to "refresh_token" if using a refresh_token
            grant_type = "authorization_code";
            //grant_type = "refresh_token";

            // Create the Request content
            var values = new List<KeyValuePair<string, string>>
            { 
                // add the information pairs to the values list
                new KeyValuePair<string, string>("client_id", client_id),
                new KeyValuePair<string, string>("redirect_uri", "http://localhost"),
                new KeyValuePair<string, string>("grant_type", grant_type),
                new KeyValuePair<string, string>("client_secret", client_secret),
                new KeyValuePair<string, string>("code", authorizationCode)
            };

            // Receive the response
            HttpResponseMessage response = await httpClient.PostAsync(url, new FormUrlEncodedContent(values));
 
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // translate the response into a JsonObject
            JsonObject jsonObject = JsonObject.Parse(responseString);

            // extract the response values
            access_token = jsonObject["access_token"].GetString();
            token_type = jsonObject["token_type"].GetString();
            expires = Convert.ToInt64(jsonObject["expires"].GetNumber());
            expires_in = Convert.ToInt32(jsonObject["expires_in"].GetNumber());
            refresh_token = jsonObject["refresh_token"].GetString();
            
            // set global variables
            (App.Current as App).access_token = access_token;
            (App.Current as App).refresh_token = refresh_token;

            // Get the customer addresses
            await (App.Current as App).customer.GetLocations();


            
            // test the delete function
            //await (App.Current as App).customer.DeleteLocation((App.Current as App).customer.customer_Addresses[2].location_id);

            // test the update function
                //string location_id = "1741106";
                //string street = "526 Warwick St";
                //string unit_number = "";
                //string city = "Brooklyn";
                //string state = "NY";
                //string phone = "315-350-6017";
                //string zip_code = "11207";
                //string company = "";
                //string cross_streets = "";
                //await (App.Current as App).customer.UpdateLocation((App.Current as App).customer.customer_Addresses[1].location_id, 
                //                                                    street, unit_number, city, state, phone, zip_code, company, cross_streets);
            
            // test the addition of a customer location
                //await (App.Current as App).customer.CreateLocation("526 Warrick St", "", "New York",
                    //                                            "NY", "347-383-9913", "11207", "", "");
        
        
        
        
            // Navigate to Hub / Search Page
            this.Frame.Navigate(typeof(HubPage));
        
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
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
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
    }
}
