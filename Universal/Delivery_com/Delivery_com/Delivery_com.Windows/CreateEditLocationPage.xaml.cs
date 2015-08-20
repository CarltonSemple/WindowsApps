using Delivery_com.Common;
using Delivery_com.DataModel;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Delivery_com
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class CreateLocationPage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        // page variables
        string method = ""; // either create or edit
        string location_id = ""; // if editing a location, this is the location's id
        string street = "";
        string apt_unit = "";
        string city = "";
        string state = "";
        string phoneNumber = "";
        string zipcode = "";
        string crossStreets = "";

        // variables to enable the submit button
        bool valid_phone = false;
        bool valid_state = false;
        bool valid_zipcode = false;

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


        public CreateLocationPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
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

        /// <summary>
        /// Take the parameter and create or edit as a result
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);

            // the parameter is either create or edit
            method = e.Parameter.ToString();

            // fill in the text boxes if an address is being edited
            if (method == "edit")
                fillInTextBoxes();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        /// <summary>
        /// Pre-fill the text boxes with the selected location's information.
        /// Give the values to the corresponding strings.
        /// Assign the location_id variable
        /// </summary>
        void fillInTextBoxes()
        {
            // change the page title text
            pageTitle.Text = "edit address";

            LocationCustomer currentLocation = (App.Current as App).CustLocation;
            location_id = currentLocation.location_id;

            // street
            streetBox.Text = currentLocation.street;
            street = streetBox.Text;

            // apartment/ unit number
            aptBox.Text = currentLocation.unit_number;
            apt_unit = aptBox.Text;

            // city
            cityBox.Text = currentLocation.city;
            city = cityBox.Text;

            // state
            stateBox.Text = currentLocation.state;
            state = stateBox.Text;

            // zip code
            zipBox.Text = currentLocation.zip_code;
            zipcode = zipBox.Text;

            // phone number
            phoneBox.Text = currentLocation.phone;
            phoneNumber = phoneBox.Text;

            // cross streets
            crossStreetsBox.Text = currentLocation.cross_streets;
            crossStreets = crossStreetsBox.Text;

            // enable the submit button
            valid_phone = true;
            valid_state = true;
            valid_zipcode = true;
            ClearErrorBox();
        }



        /// <summary>
        /// set the street string = to the streetBox text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setStreet(object sender, RoutedEventArgs e)
        {
            street = streetBox.Text;
            ClearErrorBox();
        }

        private void apt_unitnumber_set(object sender, RoutedEventArgs e)
        {
            apt_unit = aptBox.Text;
            ClearErrorBox();
        }

        private void setCrossStreets(object sender, RoutedEventArgs e)
        {
            crossStreets = crossStreetsBox.Text;
            ClearErrorBox();
        }

        private void setCity(object sender, RoutedEventArgs e)
        {
            city = cityBox.Text;
            ClearErrorBox();
        }


        /// <summary>
        /// get rid of the error code and bring back the submit button if the 3 conditions are true. 
        /// </summary>
        void ClearErrorBox()
        {
            if(valid_phone == true && valid_state == true && valid_zipcode == true && 
                street.Length > 2 && city.Length > 1) // also wait for actual input to be entered for the street address & city name
            {
                RedTango.Visibility = Visibility.Collapsed;
                errorBlock.Text = "";
                errorBlock.Visibility = Visibility.Collapsed;

                submitButton.IsEnabled = true;
                submitButton.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="type"></param>
        private void DisplayError(int type)
        {
            string errorCode = "";
            bool eMode = false;

            switch (type)
            {
                case 1: // state length should only be two digits
                    errorCode = "Please enter the 2 digit abbreviation for the state";
                    break;
                case 2: // the phone number isn't 10 digits
                    errorCode = "Please enter a 10 digit phone number";
                    break;
                case 3: // the zip code isn't 5 digits
                    errorCode = "Please enter a 5 digit zip code";
                    break;
                case -1:    // clear the error code
                    errorCode = "";
                    eMode = true;
                    break;
                default:
                    break;
            }
            if (eMode == true)
            {
                // get rid of the red box and error code, but leave the submit button disabled
                RedTango.Visibility = Visibility.Collapsed;
                submitButton.IsEnabled = false;
                submitButton.Visibility = Visibility.Visible;
                errorBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                // change the color of the submit area
                RedTango.Visibility = Visibility.Visible;
                submitButton.IsEnabled = false;
                submitButton.Visibility = Visibility.Collapsed;

                // display the error code
                errorBlock.Text = errorCode;
                errorBlock.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// the state must be two characters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stateCodeErrorCheck(object sender, RoutedEventArgs e)
        {
            // get the state string from the stateBox
            state = stateBox.Text;

            // if the length is greater than 2, let the user know
            if (state.Length > 2)
            {
                DisplayError(1);
                valid_state = false;
            }
            else
            {
                // clear the error code
                DisplayError(-1);
                valid_state = true;
                ClearErrorBox();
            }
        }

        /// <summary>
        /// Check the phone number and tell the user if it's not acceptable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void phoneErrorCheck(object sender, RoutedEventArgs e)
        {
            // check to make sure the phone number is 10 - 12 characters; 12 if the dashes are there. so it should be 10 without dashes. no country code
            phoneNumber = phoneBox.Text;

            char[] pArray = phoneNumber.ToCharArray();
            List<char> pList = new List<char>(pArray);

            // remove '+' && '-'
            while(pList.Contains('+') || pList.Contains('-'))
            {
                pList.RemoveAll(plusPredicate);
                pList.RemoveAll(dashPredicate);
            }

            // remove characters that aren't digits
            int charCount = 0;
            int i = 0;
            while(i < pList.Count)
            {
                if(pList[i] != '0' && pList[i] != '1' && pList[i] != '2' && pList[i] != '3' &&  pList[i] != '4' &&  pList[i] != '5' &&  pList[i] != '6' &&  pList[i] != '7' &&  pList[i] != '8' &&  pList[i] != '9')
                {
                    pList.RemoveAt(i);  // remove the element that isn't a digit
                    i--;  // decrease so that the same spot is checked again
                }
                i++;
            }


            phoneNumber = "";
            for (int h = 0; h < pList.Count; h++)
                phoneNumber = phoneNumber.Insert(h,pList[h].ToString());

                // declare an error if the adjusted length is not 10 characters
                if (pList.Count != 10)
                {
                    DisplayError(2);
                    valid_phone = false;
                }
                else
                {
                    // clear the error code
                    DisplayError(-1);
                    valid_phone = true;
                    ClearErrorBox();
                }

        }

        /// <summary>
        /// used in phoneErrorCheck
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool plusPredicate(char c)
        {
            return c == '+';
        }

        /// <summary>
        /// used in phoneErrorCheck
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool dashPredicate(char c)
        {
            return c == '-';
        }

        /// <summary>
        /// make sure the zip code is a 5 digit number. remind the user if it isn't
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zipCodeCheck(object sender, RoutedEventArgs e)
        {
            List<char> zipList = new List<char>(zipBox.Text);

            // remove characters that aren't digits
            int charCount = 0;
            int i = 0;
            while (i < zipList.Count)
            {
                if (zipList[i] != '0' && zipList[i] != '1' && zipList[i] != '2' && zipList[i] != '3' && zipList[i] != '4' && zipList[i] != '5' && zipList[i] != '6' && zipList[i] != '7' && zipList[i] != '8' && zipList[i] != '9')
                {
                    zipList.RemoveAt(i);  // remove the element that isn't a digit
                    i--;  // decrease so that the same spot is checked again
                }
                i++;
            }

            zipcode = zipList.ToString();
            zipcode = "";
            for (int h = 0; h < zipList.Count; h++)
                zipcode = zipcode.Insert(h, zipList[h].ToString());

            // declare an error if the adjusted length is not 5 characters
            if (zipList.Count != 5)
            {
                DisplayError(3);
                valid_zipcode = false;
            }
            else
            {
                // clear the error code
                DisplayError(-1);
                valid_zipcode = true;
                ClearErrorBox();
            }
        }

        /// <summary>
        /// Create/Update the Location
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (method == "create")
                await (App.Current as App).customer.CreateLocation(street, apt_unit, city, state, phoneNumber, zipcode, "", crossStreets);
            else if (method == "edit")
            {
                // update & refresh local addresses
                await (App.Current as App).customer.UpdateLocation(location_id, street, apt_unit, city, state, phoneNumber, zipcode, "", crossStreets);
                
                // refresh the local customer locations
                //await (App.Current as App).customer.GetLocations();
            }

            // display success message

            // return to the previous page
            navigationHelper.GoBack();
        }

        

        


    }
}
