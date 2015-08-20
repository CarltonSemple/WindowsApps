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
using ASCII_Converter.Data;
using ASCII_Converter.Common;
using System.Collections.ObjectModel;

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace ASCII_Converter
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private static ObservableCollection<int> Binaries = new ObservableCollection<int>();
        private static string asciiCode = "";
        private static List<char> charList = new List<char>();
        private static string characters = "";

        private int bitCount = 7; // The amount of bits the user wants to enter/see. Either 7 or 8

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
            var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-4");
            this.DefaultViewModel["Section3Items"] = sampleDataGroup;
            

            // ASCII Test
            char c = 'o';
            int a = c;
            string h = a.ToString();

            this.DefaultViewModel["test"] = h;

            this.DefaultViewModel["asciiCode"] = asciiCode;
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
            for(int u = 0; u < tBox.Text.Length; u++)
            {
                if(tBox.Text[u] != '0' && tBox.Text[u] != '1' && tBox.Text[u] != ' ')
                {
                    tBox.Text = tBox.Text.Remove(u, 1);
                }
            }
            
            string text = tBox.Text;
            // remove all spaces
            for(int o = 0; o < text.Length; o++)
            {
                if (text[o] == ' ')
                    text = text.Remove(o, 1);
            }
                        

            List<int> byteList = new List<int>(); // list to hold the bytes of code
            
            // reset
            charList.Clear();
            characters = "";
            
            if(bitCount == 7)
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
                for(int a = 0; a < byteList.Count; a++)
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

        private void bitSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            // Set to 8 bits by default
            ToggleSwitch bitSwitch = (ToggleSwitch)sender;

            bitSwitch.IsOn = true;
        }

    }
}
