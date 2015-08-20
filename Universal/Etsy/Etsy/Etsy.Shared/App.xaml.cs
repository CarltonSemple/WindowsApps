using Etsy.DataTransfer;
using Etsy.Model;
using Etsy.Model.List;
using Etsy.Model.User;
using Etsy.UI_Extras;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Etsy
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            this.UnhandledException += this.Application_UnhandledException;
        }

        private void Application_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(e.Message);
        }
                
        public static string baseURL = "https://openapi.etsy.com/v2";
        public static string key = "1v5aqfc23d72ufjaiil7aufr";
        public static string sharedSecret = "fdvvyh75xp";

        // these will remain valid until the member revokes access to the application, 
        // and they can be reused for this member (store them to after they're obtained)
        public static string oauth_token = "";
        public static string oauth_token_secret = "";
        public static bool oauth_callback_confirmed = false;    // ...

        public static string oauth_verifier = "";
        public static string access_token = "";
        public static string access_token_secret = "";

        public static string login_url = "";        // obtained with temporary credentials
        public static bool logged_in = false;       // will change upon start up
        public static string userID = "__SELF__";   // user ID for logged in user

        public static bool purchased = false;

        public static bool started = false;
        public static bool started2 = false;
        public static bool browseLoadedOnce = false;

        public static DataGET getData = new DataGET();
        public static Guest guessst = new Guest();

        public static User user = new User();
        public static Address defaultAddress;

        public ProductList testList = new ProductList();


        public static bool advertisements = true;       // show advertisements

        /// <summary>
        /// Attempt to load the access token and access token secret.  If the files don't exist, set these variables to empty strings and create the files
        /// Located here because async methods cannot have out parameters
        /// </summary>
        /// <returns></returns>
        private async Task loadAccessToken()
        {
            string check1 = "", check2 = "";
            try
            {
                check1 = await FileIO.ReadEncryptedFileAsString("access_token");
                check2 = await FileIO.ReadEncryptedFileAsString("access_token_secret");
                logged_in = true;
            }
            catch(Exception e)
            {
                string message = e.Message;
            }
            finally
            {
                access_token = check1;
                access_token_secret = check2;
                // files will be created if the user decides to log in
            }            
        }

        /// <summary>
        /// Load the string representing the chosen image quality from storage.
        /// If this fails, default to high quality
        /// </summary>
        /// <returns></returns>
        public async Task loadImageQuality()
        {
            try
            {
                string q = await FileIO.ReadEncryptedFileAsString("image_quality");
                if (q == "high")
                    Loading._imageQuality = Loading.ImageQuality.high;
                else
                    Loading._imageQuality = Loading.ImageQuality.low;
            }
            catch (Exception exc)
            {
                Loading._imageQuality = Loading.ImageQuality.high;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static LicenseInformation checkLicenseState()
        {
            LicenseInformation licenseInformation = CurrentApp.LicenseInformation;

            var licenses = licenseInformation.ProductLicenses["noAdvertisements"];

            try
            {
                if (!licenseInformation.ProductLicenses["noAdvertisements"].IsActive)
                    advertisements = true;
                else
                {
                    advertisements = false;     // hide advertisements
                    purchased = true;
                }

                return licenseInformation;
            }
            catch (Exception)
            {
            }
            // ************************************ Show or Hide advertisements ***************************************

            return licenseInformation;
        }


        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            await loadImageQuality();

            await loadAccessToken();
            if (logged_in == true)
            {
                try
                {
                    user = await FileIO.DeserializeFromFile<User>("user");
                    User tempuser = await UserAccess.getUserFull(userID);       // Check to see if the permission is still granted
                    if (tempuser != null)
                        user = tempuser;    // keep user up to date if possible
                    else
                        App.logged_in = false;
                    // get default address
                    if (user != null)
                        if (user.Addresses != null)
                            foreach (var add in user.Addresses)
                                if (add.is_default_shipping == true)
                                    defaultAddress = add;
                }
                catch(Exception u)
                {
                    string message = u.Message;
                    App.logged_in = false;
                }
            }

            if (defaultAddress == null)
            {
                defaultAddress = new Address();
                defaultAddress.country_id = 209;    // default to US
            }

            checkLicenseState();                    // Check for ad-free purchase

            

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;
#endif

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
        }
#endif

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}