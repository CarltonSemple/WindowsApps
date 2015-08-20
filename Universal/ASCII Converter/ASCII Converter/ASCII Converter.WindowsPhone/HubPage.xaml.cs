using ASCII_Converter.Common;
using ASCII_Converter.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
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

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace ASCII_Converter
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();

        private static ObservableCollection<int> Binaries = new ObservableCollection<int>();
        private static string asciiCode = "";
        private static List<char> charList = new List<char>();
        private static string characters = "";

        private int bitCount = 7; // The amount of bits the user wants to enter/see. Either 7 or 8

        public HubPage()
        {
            this.InitializeComponent();

            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            this.NavigationCacheMode = NavigationCacheMode.Required;

            

            

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
        /// <see cref="Frame.Navigate(Type, object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // Hide the status bar
            await statusBar.HideAsync();
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

        /// <summary>
        /// Convert text to ASCII code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 238 is the limit

            // Reset ASCII values
            Binaries.Clear();
            asciiCode = "";

            TextBox tBox = (TextBox)sender;
            // limit the text box length to 238
            if (tBox.Text.Length > 238)
                tBox.Text = tBox.Text.Substring(0, 238);

            string text = tBox.Text;

            for (int i = 0; i < text.Length; i++)
            {
                Binaries.Add(text[i]);
                // guarantee that the length is what the user wants. must be 7 or 8
                string charString = Convert.ToString(Binaries[i], 2);
                while (charString.Length < bitCount)
                {
                    charString = "0" + charString;
                }

                asciiCode += charString + " "; // convert to binary and add it to the string
            }

            this.DefaultViewModel["asciiCode"] = asciiCode;
        }

        /// <summary>
        /// Convert the user input of 0's and 1's (ASCII code), to characters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tBox = (TextBox)sender;
            // Remove everything that isn't a 0 or 1 or space
            for (int u = 0; u < tBox.Text.Length; u++)
            {
                if (tBox.Text[u] != '0' && tBox.Text[u] != '1' && tBox.Text[u] != ' ')
                {
                    tBox.Text = tBox.Text.Remove(u, 1);
                }
            }

            string text = tBox.Text;
            // remove all spaces
            for (int o = 0; o < text.Length; o++)
            {
                if (text[o] == ' ')
                    text = text.Remove(o, 1);
            }


            List<int> byteList = new List<int>(); // list to hold the bytes of code

            // reset
            charList.Clear();
            characters = "";

            if (bitCount == 7)
            {   // separate in counts of 7
                int start = 0;
                int i = start;
                while (start + 7 <= text.Length)
                {
                    string cString = "0" + text.Substring(start, 7);
                    byteList.Add(Convert.ToInt32(cString, 2));

                    start = start + 7;
                }

                // convert to characters
                for (int a = 0; a < byteList.Count; a++)
                {
                    characters += Convert.ToChar(byteList[a]);
                }

            }
            else
            {   // separate in counts of 8
                int start = 0;
                int i = start;
                while (start + 8 <= text.Length)
                {
                    string cString = text.Substring(start, 8);
                    byteList.Add(Convert.ToInt32(cString, 2));

                    start = start + 8;
                }

                // convert to characters
                for (int a = 0; a < byteList.Count; a++)
                {
                    characters += Convert.ToChar(byteList[a]);
                }

            }

            this.DefaultViewModel["characters"] = characters;
        }

        /// <summary>
        /// Change the value of the preferred bit count
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bitSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            ToggleSwitch switcher = (ToggleSwitch)sender;

            if (switcher.IsOn) // set to 8
                bitCount = 8;
            else
                bitCount = 7;

        }

        /********************************** Store Functions **********************************/

        // Show other apps by iTchyBandit
        //private void searchApps(object sender, GestureEventArgs e)
        //{
        //    MarketplaceSearchTask marketplaceSearchTask = new MarketplaceSearchTask();

        //    marketplaceSearchTask.SearchTerms = "iTchyBandit";

        //    marketplaceSearchTask.Show();
        //}

        //// prompt to buy
        //private void purchase(object sender, GestureEventArgs e)
        //{
        //    MessageBox.Show(AppResources.PurchasePrompt);

        //    MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();

        //    marketplaceDetailTask.ContentIdentifier = "94db675a-1c4d-4dca-a2a3-519fa6ac60a6";
        //    marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;

        //    marketplaceDetailTask.Show();
        //}

        // Option to Review/Like
        private async void reviewButton_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(
                new Uri("ms-windows-store:reviewapp?appid=" + App.AppID));
        }

        private void bitSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            ToggleSwitch bitSwitch = (ToggleSwitch)sender;

            // set this to 8 bits by default
            bitSwitch.IsOn = true;
        }

        /// <summary>
        /// The source list for decimal-hex-binary conversion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sourceChoice_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox list = (ListBox)sender;
            
            // Add the options
            list.Items.Add("Decimal");
            list.Items.Add("Hexadecimal");
            list.Items.Add("Binary");

            list.SelectedIndex = 0; // Default to the first option
        }


    }
}