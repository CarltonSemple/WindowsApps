using Etsy.Common;
using Etsy.DataTransfer;
using Etsy.Model.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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


// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Etsy
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class LoginPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private static string login_url;
        private static HttpClient client = new HttpClient();
        private static bool accessTokenCalled = false;

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
        }

        /// <summary>
        /// Get temporary credentials and send the user to the web page for allowing the app access to their profile
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
            string status = await App.getData.getTemporaryCredentials(true);                    // Get the oauth_token and oauth_token_secret
            
            if(status.Contains("ERROR:"))                                                       // Return if there's an error
            {
                NotifyUser(status);                                                             // Notify the User of an error before returning to previous page
            }
            else
            {
                // navigate to the log in URL, using the oauth_token and api_token (latter not really necessary)
                login_url = status + "?oauth_token=" + App.oauth_token + "&oauth_consumer_key=" + App.key;

                webView.Navigate(new Uri(login_url));                                           // Navigate to login URL ***************************************
            }      
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

        /// <summary>
        /// Used to display messages to the user
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="type"></param>
        public void NotifyUser(string strMessage)
        {
            StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            if (StatusBlock.Text != String.Empty)
            {
                StatusBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                StatusBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Take the callback URL and parse it to get the oauth_verifier code, then get the access token
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void webView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (accessTokenCalled == true)
                return;                     // prevent the function for getting the access token from being called multiple times

            string url = "", oauth_verifier = "";
            try
            {
                url = args.Uri.ToString();
                if(url.Contains("localhost"))
                {
                    string[] r = url.Split('?');   // get rid of the localhost/?
                    string[] parameters = r[1].Split('&');    // split up the parameters

                    foreach(string p in parameters)
                    {
                        if(p.Contains("oauth_verifier"))
                        {
                            oauth_verifier = p.Replace("oauth_verifier=", "");
                            if (oauth_verifier.Contains("#_=_"))
                                oauth_verifier = oauth_verifier.Replace("#_=_", "");            // get the oauth_verifier string from within the url

                            await App.getData.getAccessToken(oauth_verifier);                   // Get the access token here. Save for future use
                                                    
                            accessTokenCalled = true;
                            App.logged_in = true;

                            App.user = await UserAccess.getUserFull("__SELF__");                // Get the logged in user's info

                            await FileIO.SerializeAndSave(App.user, "user");                    // Save the user

                            // get default address
                            foreach (var add in App.user.Addresses)
                                if (add.is_default_shipping == true)
                                    App.defaultAddress = add;
                            if (App.defaultAddress == null)
                            {
                                App.defaultAddress = new Address();
                                App.defaultAddress.country_id = 209;    // default to US
                            }

                            if (navigationHelper.CanGoBack())
                                navigationHelper.GoBack();      // Go to previous page, if possible
                            break;
                        }
                    }
                }
            }
            catch(Exception e)
            {
                NotifyUser("Error getting verification code");
            }
            finally
            {
                ;
            }
        }

        /// <summary>
        /// Take the callback URL and parse it to get the oauth_verifier code, then get the access token
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void webView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            //if (accessTokenCalled == true)
            //    return;                     // prevent the function for getting the access token from being called multiple times

            //string url = "", oauth_verifier = "";
            //try
            //{
            //    url = args.Uri.ToString();
            //    if (url.Contains("localhost"))
            //    {
            //        string[] r = url.Split('?');   // get rid of the localhost/?
            //        string[] parameters = r[1].Split('&');    // split up the parameters

            //        foreach (string p in parameters)
            //        {
            //            if (p.Contains("oauth_verifier"))
            //            {
            //                oauth_verifier = p.Replace("oauth_verifier=", "");
            //                if (oauth_verifier.Contains("#_=_"))
            //                    oauth_verifier = oauth_verifier.Replace("#_=_", "");    // get the oauth_verifier string from within the url

            //                await App.getData.getAccessToken(oauth_verifier);         // Get the access token here. 
            //                                            // Only set the App's oauth_verifier variable once this has been completedly successfully
            //                accessTokenCalled = true;
            //                App.logged_in = true;

            //                if(navigationHelper.CanGoBack())
            //                    navigationHelper.GoBack();      // Go to previous page, if possible
            //                return;
            //            }
            //        }
            //    }
            //}
            //catch
            //{
            //    NotifyUser("Error getting verification code");
            //}
            //finally
            //{
            //    ;
            //}
        }

    }
}
