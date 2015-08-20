using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

using Microsoft.Phone.Tasks;
using Microsoft.Advertising;
//using Conversion_App.Calculations.Length_Calculation;

namespace Conversion_App
{
    public partial class MainPage : PhoneApplicationPage
    {

        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Default the Calculator Item as disabled
            Calculator.IsEnabled = false;
            //Calculator.Opacity = 0.1;


            // Make the Application Bar
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Minimized;
            ApplicationBar.Opacity = 1.0;
            ApplicationBar.BackgroundColor = (Color)Application.Current.Resources["PhoneAccentColor"];
            ApplicationBar.IsVisible = true;
            ApplicationBar.IsMenuEnabled = true;
            

            ApplicationBarMenuItem ratebutton = new ApplicationBarMenuItem();
            ratebutton.Text = "rate this app";
            ApplicationBar.MenuItems.Add(ratebutton);

            ratebutton.Click += new EventHandler(ratebutton_click);

            //ApplicationBarMenuItem buybutton = new ApplicationBarMenuItem();
            //buybutton.Text = "Get ad-free version";
            //ApplicationBar.MenuItems.Add(buybutton);

            //buybutton.Click += new EventHandler(buybutton_click);



            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            ReceiveDataInApplicationVariable();

            Panorama myPanorama = new Panorama();
            myPanorama.Name = "HomePanorama";

            /*********************** Set the Unit Boxes ************************/
            int conversionType = (Application.Current as App).conversionType;
            int unitIndex1 = (Application.Current as App).unitIndex1;
            int unitIndex2 = (Application.Current as App).unitIndex2;

            switch (conversionType)
            {
                case 1: // length
                    {
                        if (unitIndex1 == 0) // from inches
                            inputUnitBlock.Text = "inches";
                        else if (unitIndex1 == 1) // from feet
                            inputUnitBlock.Text = "feet";
                        else if (unitIndex1 == 2) // from yards
                            inputUnitBlock.Text = "yards";
                        else if (unitIndex1 == 3) // from miles
                            inputUnitBlock.Text = "miles";
                        else if (unitIndex1 == 4) // from millimeters
                            inputUnitBlock.Text = "millimeters";
                        else if (unitIndex1 == 5) // from centimeters
                            inputUnitBlock.Text = "centimeters";
                        else if (unitIndex1 == 6) // from meters
                            inputUnitBlock.Text = "meters";
                        else if (unitIndex1 == 7) // from kilometers
                            inputUnitBlock.Text = "kilometers";
                  // Output Unit Block
                        if (unitIndex2 == 0) // to inches
                            outputUnitBlock.Text = "inches";
                        else if (unitIndex2 == 1) // to feet
                            outputUnitBlock.Text = "feet";
                        else if (unitIndex2 == 2) // to yards
                            outputUnitBlock.Text = "yards";
                        else if (unitIndex2 == 3) // to miles
                            outputUnitBlock.Text = "miles";
                        else if (unitIndex2 == 4) // to millimeters
                            outputUnitBlock.Text = "millimeters";
                        else if (unitIndex2 == 5) // to centimeters
                            outputUnitBlock.Text = "centimeters";
                        else if (unitIndex2 == 6) // to meters
                            outputUnitBlock.Text = "meters";
                        else if (unitIndex2 == 7) // to kilometers
                            outputUnitBlock.Text = "kilometers"; 
                    } break;
                case 2: // weight
                    {
                        if (unitIndex1 == 0) // from ounces
                            inputUnitBlock.Text = "ounces";
                        else if (unitIndex1 == 1) // from pounds
                            inputUnitBlock.Text = "pounds";
                        else if (unitIndex1 == 2) // from tons (US)
                            inputUnitBlock.Text = "tons (US)";
                        else if (unitIndex1 == 3) // from tons (UK)
                            inputUnitBlock.Text = "tons (UK)";
                        else if (unitIndex1 == 4) // from metric tons
                            inputUnitBlock.Text = "metric tons";
                        else if (unitIndex1 == 5) // from grams
                            inputUnitBlock.Text = "grams";
                        else if (unitIndex1 == 6) // from kilograms
                            inputUnitBlock.Text = "kilograms";

                        // Output Unit Block
                        if (unitIndex2 == 0) // to ounces
                            outputUnitBlock.Text = "ounces";
                        else if (unitIndex2 == 1) // to pounds
                            outputUnitBlock.Text = "pounds";
                        else if (unitIndex2 == 2) // to tons (US)
                            outputUnitBlock.Text = "tons (US)";
                        else if (unitIndex2 == 3) // to tons (UK)
                            outputUnitBlock.Text = "tons (UK)";
                        else if (unitIndex2 == 4) // to metric tons
                            outputUnitBlock.Text = "metric tons";
                        else if (unitIndex2 == 5) // to grams
                            outputUnitBlock.Text = "grams";
                        else if (unitIndex2 == 6) // to kilograms
                            outputUnitBlock.Text = "kilograms"; 
                    } break;
                case 3: // volume
                    {
                        if (unitIndex1 == 0) // from liquid ounces
                            inputUnitBlock.Text = "liquid ounces";
                        else if (unitIndex1 == 1) // from pints (US)
                            inputUnitBlock.Text = "pints (US)";
                        else if (unitIndex1 == 2) // from cubic inches
                            inputUnitBlock.Text = "cubic inches";
                        else if (unitIndex1 == 3) // from cubic feet
                            inputUnitBlock.Text = "cubic feet";
                        else if (unitIndex1 == 4) // from cups (US)
                            inputUnitBlock.Text = "cups (US)";
                        else if (unitIndex1 == 5) // from metric cups
                            inputUnitBlock.Text = "metric cups";
                        else if (unitIndex1 == 6) // from gallons (US)
                            inputUnitBlock.Text = "gallons (US)";
                        else if (unitIndex1 == 7) // from milliliters
                            inputUnitBlock.Text = "milliliters";
                        else if (unitIndex1 == 8) // from centiliters
                            inputUnitBlock.Text = "centiliters";
                        else if (unitIndex1 == 9) // from liters
                            inputUnitBlock.Text = "liters";
                        else if (unitIndex1 == 10) // from barrels
                            inputUnitBlock.Text = "barrels";
                        // Output Unit Block
                        if (unitIndex2 == 0) // to liquid ounces
                            outputUnitBlock.Text = "liquid ounces";
                        else if (unitIndex2 == 1) // to pints (US)
                            outputUnitBlock.Text = "pints (US)";
                        else if (unitIndex2 == 2) // to cubic inches
                            outputUnitBlock.Text = "cubic inches";
                        else if (unitIndex2 == 3) // to cubic feet
                            outputUnitBlock.Text = "cubic feet";
                        else if (unitIndex2 == 4) // to cups (US)
                            outputUnitBlock.Text = "cups (US)";
                        else if (unitIndex2 == 5) // to metric cups
                            outputUnitBlock.Text = "metric cups";
                        else if (unitIndex2 == 6) // to gallons (US)
                            outputUnitBlock.Text = "gallons (US)";
                        else if (unitIndex2 == 7) // to milliliters
                            outputUnitBlock.Text = "milliliters";
                        else if (unitIndex2 == 8) // to centiliters
                            outputUnitBlock.Text = "centiliters";
                        else if (unitIndex2 == 9) // to liters
                            outputUnitBlock.Text = "liters";
                        else if (unitIndex2 == 10) // to barrels
                            outputUnitBlock.Text = "barrels";
                    } break;
                case 4: // data
                    {
                        // Input Units
                        if (unitIndex1 == 0) // bit
                            inputUnitBlock.Text = "bits";
                        else if (unitIndex1 == 1) // byte
                            inputUnitBlock.Text = "bytes";
                        else if (unitIndex1 == 2)
                            inputUnitBlock.Text = "kilobytes";
                        else if (unitIndex1 == 3)
                            inputUnitBlock.Text = "megabytes";
                        else if (unitIndex1 == 4)
                            inputUnitBlock.Text = "gigabytes";
                        else if (unitIndex1 == 5)
                            inputUnitBlock.Text = "terabytes";

                        // Output Units
                        if (unitIndex2 == 0)
                            outputUnitBlock.Text = "bits";
                        else if (unitIndex2 == 1)
                            outputUnitBlock.Text = "bytes";
                        else if (unitIndex2 == 2)
                            outputUnitBlock.Text = "kilobytes";
                        else if (unitIndex2 == 3)
                            outputUnitBlock.Text = "megabytes";
                        else if (unitIndex2 == 4)
                            outputUnitBlock.Text = "gigabytes";
                        else if (unitIndex2 == 5)
                            outputUnitBlock.Text = "terabytes";
                    } break;
                case 5: // temperature
                    {
                        // Input Units
                        if (unitIndex1 == 0) // kelvin
                            inputUnitBlock.Text = "kelvin";
                        else if (unitIndex1 == 1) // celsius
                            inputUnitBlock.Text = "celsius";
                        else if (unitIndex1 == 2) // fahrenheit
                            inputUnitBlock.Text = "fahrenheit";

                        // Output Units
                        if (unitIndex2 == 0) // kelvin
                            outputUnitBlock.Text = "kelvin";
                        else if (unitIndex2 == 1) // celsius
                            outputUnitBlock.Text = "celsius";
                        else if (unitIndex2 == 2) // fahrenheit
                            outputUnitBlock.Text = "fahrenheit";
                    } break;
                case 6: // cooking
                    {
                        if (unitIndex1 == 0) // from milliliters
                            inputUnitBlock.Text = "milliliters";
                        else if (unitIndex1 == 1) // from metric cups
                            inputUnitBlock.Text = "metric cups";
                        else if (unitIndex1 == 2) // from liters
                            inputUnitBlock.Text = "liters";
                        else if (unitIndex1 == 3) // from teaspoons (US)
                            inputUnitBlock.Text = "teaspoons (US)";
                        else if (unitIndex1 == 4) // from tablespoons (US)
                            inputUnitBlock.Text = "tablespoons (US)";
                        else if (unitIndex1 == 5) // from cups (US)
                            inputUnitBlock.Text = "cups (US)";
                        else if (unitIndex1 == 6) // from 
                            inputUnitBlock.Text = "pints (US)";
                        else if (unitIndex1 == 7) // from 
                            inputUnitBlock.Text = "quarts (US)";
                        else if (unitIndex1 == 8) // from gallons (US)
                            inputUnitBlock.Text = "gallons (US)";
                        else if (unitIndex1 == 9) // from 
                            inputUnitBlock.Text = "teaspoons (UK)";
                        else if (unitIndex1 == 10) // from 
                            inputUnitBlock.Text = "tablespoons (UK)";
                        else if (unitIndex1 == 11) // from 
                            inputUnitBlock.Text = "fluid ounces (UK)";
                        else if (unitIndex1 == 12) // from 
                            inputUnitBlock.Text = "gallons (UK)";
                        // Output Unit Block
                        if (unitIndex2 == 0) // to milliliters
                            outputUnitBlock.Text = "milliliters";
                        else if (unitIndex2 == 1) // from metric cups
                            outputUnitBlock.Text = "metric cups";
                        else if (unitIndex2 == 2) // from liters
                            outputUnitBlock.Text = "liters";
                        else if (unitIndex2 == 3) // from teaspoons (US)
                            outputUnitBlock.Text = "teaspoons (US)";
                        else if (unitIndex2 == 4) // from tablespoons (US)
                            outputUnitBlock.Text = "tablespoons (US)";
                        else if (unitIndex2 == 5) // from cups (US)
                            outputUnitBlock.Text = "cups (US)";
                        else if (unitIndex2 == 6) // from 
                            outputUnitBlock.Text = "pints (US)";
                        else if (unitIndex2 == 7) // from 
                            outputUnitBlock.Text = "quarts (US)";
                        else if (unitIndex2 == 8) // from gallons (US)
                            outputUnitBlock.Text = "gallons (US)";
                        else if (unitIndex2 == 9) // from 
                            outputUnitBlock.Text = "teaspoons (UK)";
                        else if (unitIndex2 == 10) // from 
                            outputUnitBlock.Text = "tablespoons (UK)";
                        else if (unitIndex2 == 11) // from 
                            outputUnitBlock.Text = "fluid ounces (UK)";
                        else if (unitIndex2 == 12) // from 
                            outputUnitBlock.Text = "gallons (UK)";
                    } break;
            };

            //EqualSign.Source = new System.Windows.Media.Imaging.BitmapImage("/Images/Equal Sign tan.jpg");

            // If RemoveBackEntry is called on an empty back stack, an InvalidOperationException is thrown.
            // Check to make sure the BackStack has entries before calling RemoveBackEntry.

            //if (RootFrame.BackStack.Count() > 0)
            //    App.RootFrame.RemoveBackEntry();

            //// Refresh the history list since the back stack has been modified.
            //UpdateHistory();

        }

        private void ratebutton_click(object sender, EventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();

            marketplaceReviewTask.Show();
        }

        private void buybutton_click(object sender, EventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();

            marketplaceReviewTask.Show();
        }

        private void zip_click(object sender, RoutedEventArgs e)
        {
            MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();

            marketplaceDetailTask.ContentIdentifier = "c2c3f727-cb20-40d0-8b78-f484bd8fe03c";
            marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;

            marketplaceDetailTask.Show();
        }

        private void zippro_click(object sender, RoutedEventArgs e)
        {
            MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();

            marketplaceDetailTask.ContentIdentifier = "39bffa0e-fcec-4250-adff-4e0c4f2eb884";
            marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;

            marketplaceDetailTask.Show();
        }
        /// <summary>
        /// Use the BackStack property to refresh the navigation history listbox with the latest history.
        /// </summary>
        void UpdateHistory()
        {
            //historyListBox.Items.Clear();
            //int i = 0;

            //// The BackStack property is a collection of JournalEntry objects.
            //foreach (JournalEntry journalEntry in RootFrame.BackStack.Reverse())
            //{
            //    historyListBox.Items.Insert(0, i + ": " + journalEntry.Source);
            //    i++;
            //}

            //currentPageTextBlock.Text = "[" + RootFrame.Source + "]";

            //if (popLastButton != null)
            //{
            //    popLastButton.IsEnabled = (historyListBox.Items.Count() > 0);
            //}
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Meant for trial version feature enabling, etc.
            //base.OnNavigatedTo(e);

            //if ((Application.Current as App).IsTrial)
            //{
            //    removeadsbutton.Visibility = System.Windows.Visibility.Visible;
            //}
            //else
            //{
            //    advertisement.Visibility = System.Windows.Visibility.Collapsed;
            //    removeadsbutton.Visibility = System.Windows.Visibility.Collapsed;
            //}




            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            int i = App.RootFrame.BackStack.Count();
            string ii = i.ToString();
            //outputUnitBlock.Text = ii;

            if (App.RootFrame.BackStack.Count() > 0)
            {
                do
                {
                    App.RootFrame.RemoveBackEntry();
                }
                while (App.RootFrame.BackStack.Count() != 0);
            }
        }


        // Meant for trial version feature enabling, etc.
        //protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        //{
        //    base.OnNavigatedTo(e);

        //    if ((Application.Current as App).IsTrial)
        //    {
        //        removeadsbutton.Visibility = System.Windows.Visibility.Visible;
        //    }
        //    else
        //    {
        //        advertisement.Visibility = System.Windows.Visibility.Collapsed;
        //        removeadsbutton.Visibility = System.Windows.Visibility.Collapsed;
        //    }
        //}


        // Recipe 4. Receive data in an application-level variable
        // defined in App.xaml.cs.
        private void ReceiveDataInApplicationVariable()
        {

            // Read the contents of the application-level variable.
            //inputUnitBlock.Text = (Application.Current as App).sourceData;
            //outputUnitBlock.Text = "Conversion type: " + (Application.Current as App).conversionType.ToString();
        }

        private void PanoramaItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string currentPanoramaItem = e.AddedItems[0] as string;
            

            switch (((Panorama)sender).SelectedIndex)
            {
                case 0:
                    {
                        (Application.Current as App).selectedPanoramaItem = 0;
                        Converter.IsEnabled = true;
                        //Converter.Opacity = 1.0;
                        //Calculator.Opacity = 0.1;
                        Calculator.IsEnabled = false;
                    }
                    break;
                case 1:
                    (Application.Current as App).selectedPanoramaItem = 1;
                    //Converter.Opacity = 0.1;
                    Converter.IsEnabled = false;
                    Calculator.IsEnabled = true;
                    //Calculator.Opacity = 1.0;
                    break;
                case 2:
                    (Application.Current as App).selectedPanoramaItem = 2;
                    Converter.IsEnabled = false;
                    Calculator.IsEnabled = false;
                    break;

                default:
                    return;
            }

            string item = (Application.Current as App).selectedPanoramaItem.ToString();

            testbaby.Content = item;
        }

        private void Number_Click(object sender, RoutedEventArgs e)
        {
            //int panoramaIndex = (Application.Current as App).selectedPanoramaItem;

            // Testing the panorama index
            //if (panoramaIndex == 0)
            //    panoramaIdentifierBlock.Text = "Panorama Item 0";
            //else if (panoramaIndex == 1)
            //    panoramaIdentifierBlock.Text = "Panorama Item 1";


            // Input from buttons goes to corresponding text box
            // then do the right calculation (converter or calculator)

            Button b = (Button)sender;
            userInput.Text += b.Content.ToString();

            try
            {
                result();
            }
            catch (Exception exc)
            {
                userInput.Text = userInput.Text.Substring(0, userInput.Text.Length - 1);
            }


            //attempt to use panorama item's Index
            //if (panoramaIndex == 0) // Converter
            //{
            //    userInput.Text += b.Content.ToString();

            //    try
            //    {
            //        result(); // Converter
            //    }
            //    catch (Exception exc)
            //    {
            //        userInput.Text = userInput.Text.Substring(0, userInput.Text.Length - 1);
            //    }
            //}
            //else if (panoramaIndex == 1) // Calculator
            //{
            //    calculatorUserInput.Text += b.Content.ToString();

            //    //try
            //    //{
            //    //    result();
            //    //}
            //    //catch (Exception exc)
            //    //{
            //    //    userInput.Text = userInput.Text.Substring(0, userInput.Text.Length - 1);
            //    //}
            //}
        }







        // Converter Calculations

        private double Calculate(double inputNum)
        {
            double answer = -1;
            int conversionType = (Application.Current as App).conversionType;
            int unitIndex1 = (Application.Current as App).unitIndex1;
            int unitIndex2 = (Application.Current as App).unitIndex2;

            if (conversionType == 1) // Length 
            {
                answer = Length_Calculation(inputNum, unitIndex1, unitIndex2);
            }
            else if (conversionType == 2) // Weight
            {
                answer = Weight_Calculation(inputNum, unitIndex1, unitIndex2);
            }
            else if (conversionType == 3) // Volume
            {
                answer = Volume_Calculation(inputNum, unitIndex1, unitIndex2);
            }
            else if (conversionType == 4) // Data
            {
                answer = Data_Calculation(inputNum, unitIndex1, unitIndex2);
            }
            else if (conversionType == 5) // Temperature
            {
                answer = Temperature_Calculation(inputNum, unitIndex1, unitIndex2);
            }
            else if (conversionType == 6) // Cooking
            {
                answer = Cooking_Calculation(inputNum, unitIndex1, unitIndex2);
            }

            return (answer);
        }

        private int result()
        {
            String answer;
            double inputNum = Convert.ToDouble(userInput.Text);
            double calculatedNum;   //number to be calculated based on the conversion type selected by the user


            calculatedNum = Calculate(inputNum);
            answer = Convert.ToString(calculatedNum);
            resultBlock.Text = answer;

            return (0);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            userInput.Text = "";
            resultBlock.Text = "";
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (userInput.Text.Length > 0)
            {
                if (userInput.Text.Length == 1)
                {
                    userInput.Text = "";
                    resultBlock.Text = "";  //here to empty the result block as well, when there's only one number left that's being deleted
                }
                else
                {
                    userInput.Text = userInput.Text.Substring(0, userInput.Text.Length - 1);
                    result();
                }
            }
            if (userInput.Text.Length == 0)
            {
                userInput.Text = "";
                resultBlock.Text = "";
            }




            //display result instantly
           // result();
            //the above failed because the result will be displayed next time a number button is clicked
            //instead, just clear the result textblock if there shouldn't be a number there anymore ( see above above)
        }

        private void ConversionTypeButton_Click(object sender, RoutedEventArgs e)
        {
            //inputUnitBlock.Text = "";
            //outputUnitBlock.Text = "";
            userInput.Text = "";
            resultBlock.Text = "";
            NavigationService.Navigate(new Uri("/ConversionSelectionPage.xaml", UriKind.Relative));
        }

        // Length Calculations
        private double Length_Calculation(double inputNum, int unitIndex1, int unitIndex2)
        {
            double answer = -1;

            // Conversions

            if (unitIndex1 == 0) // from inches
            {
                inputUnitBlock.Text = "inches";
                if (unitIndex2 == 0) // to inches
                {answer = inputNum; outputUnitBlock.Text = "inches";}
                else if (unitIndex2 == 1) // to feet
                { answer = inputNum / 12; outputUnitBlock.Text = "feet"; }
                else if (unitIndex2 == 2) // to yards
                { answer = inputNum / 36; outputUnitBlock.Text = "yards"; }
                else if (unitIndex2 == 3) // to miles
                { answer = inputNum / 63360; outputUnitBlock.Text = "miles"; }
                else if (unitIndex2 == 4) // to millimeters
                { answer = inputNum * 25.4; outputUnitBlock.Text = "millimeters"; }
                else if (unitIndex2 == 5) // to centimeters
                { answer = inputNum * 2.54; outputUnitBlock.Text = "centimeters"; }
                else if (unitIndex2 == 6) // to meters
                { answer = inputNum * 0.0254; outputUnitBlock.Text = "meters"; }
                else if (unitIndex2 == 7) // to kilometers
                { answer = inputNum / 39370.1; outputUnitBlock.Text = "kilometers"; }
            }
            else if (unitIndex1 == 1) // from feet
            {
                inputUnitBlock.Text = "feet";
                if (unitIndex2 == 0) // to inches
                { answer = inputNum * 12; outputUnitBlock.Text = "inches"; }
                else if (unitIndex2 == 1) // to feet
                { answer = inputNum; outputUnitBlock.Text = "feet"; }
                else if (unitIndex2 == 2) // to yards
                { answer = inputNum / 3; outputUnitBlock.Text = "yards"; }
                else if (unitIndex2 == 3) // to miles
                { answer = inputNum / 5280; outputUnitBlock.Text = "miles"; }
                else if (unitIndex2 == 4) // to millimeters
                { answer = inputNum * 304.8; outputUnitBlock.Text = "millimeters"; }
                else if (unitIndex2 == 5) // to centimeters
                { answer = inputNum * 30.48; outputUnitBlock.Text = "centimeters"; }
                else if (unitIndex2 == 6) // to meters
                { answer = inputNum * 0.3048; outputUnitBlock.Text = "meters"; }
                else if (unitIndex2 == 7) // to kilometers
                { answer = inputNum / 3280.84; outputUnitBlock.Text = "kilometers"; }
            }
            else if (unitIndex1 == 2) // from yards
            {
                inputUnitBlock.Text = "yards";
                if (unitIndex2 == 0) // to inches
                { answer = inputNum * 36; outputUnitBlock.Text = "inches"; }
                else if (unitIndex2 == 1) // to feet
                { answer = inputNum * 3; outputUnitBlock.Text = "feet"; }
                else if (unitIndex2 == 2) // to yards
                { answer = inputNum; outputUnitBlock.Text = "yards"; }
                else if (unitIndex2 == 3) // to miles
                { answer = inputNum / 1760; outputUnitBlock.Text = "miles"; }
                else if (unitIndex2 == 4) // to millimeters
                { answer = inputNum * 914.4; outputUnitBlock.Text = "millimeters"; }
                else if (unitIndex2 == 5) // to centimeters
                { answer = inputNum * 91.44; outputUnitBlock.Text = "centimeters"; }
                else if (unitIndex2 == 6) // to meters
                { answer = inputNum * 0.9144; outputUnitBlock.Text = "meters"; }
                else if (unitIndex2 == 7) // to kilometers
                { answer = inputNum / 1093.61; outputUnitBlock.Text = "kilometers"; }
            }
            else if (unitIndex1 == 3) // from miles
            {
                inputUnitBlock.Text = "miles";
                if (unitIndex2 == 0) // to inches
                { answer = inputNum * 63360; outputUnitBlock.Text = "inches"; }
                else if (unitIndex2 == 1) // to feet
                { answer = inputNum * 5280; outputUnitBlock.Text = "feet"; }
                else if (unitIndex2 == 2) // to yards
                { answer = inputNum * 1760; outputUnitBlock.Text = "yards"; }
                else if (unitIndex2 == 3) // to miles
                { answer = inputNum;  outputUnitBlock.Text = "miles"; }
                else if (unitIndex2 == 4) // to millimeters
                { answer = inputNum * 1609344.002; outputUnitBlock.Text = "millimeters"; }
                else if (unitIndex2 == 5) // to centimeters
                { answer = inputNum * 160934; outputUnitBlock.Text = "centimeters"; }
                else if (unitIndex2 == 6) // to meters
                { answer = inputNum * 1609.34; outputUnitBlock.Text = "meters"; }
                else if (unitIndex2 == 7) // to kilometers
                { answer = inputNum * 1.60934; outputUnitBlock.Text = "kilometers"; }
            }
            else if (unitIndex1 == 4) // from millimeters
            {
                inputUnitBlock.Text = "millimeters";
                if (unitIndex2 == 0) // to inches
                { answer = inputNum / 25.40000004; outputUnitBlock.Text = "inches"; }
                else if (unitIndex2 == 1) // to feet
                { answer = inputNum / 304.8000005; outputUnitBlock.Text = "feet"; }
                else if (unitIndex2 == 2) // to yards
                { answer = inputNum / 914.4000014; outputUnitBlock.Text = "yards"; }
                else if (unitIndex2 == 3) // to miles
                { answer = inputNum / 1609344.002; outputUnitBlock.Text = "miles"; }
                else if (unitIndex2 == 4) // to millimeters
                { answer = inputNum; outputUnitBlock.Text = "millimeters"; }
                else if (unitIndex2 == 5) // to centimeters
                { answer = inputNum / 10; outputUnitBlock.Text = "centimeters"; }
                else if (unitIndex2 == 6) // to meters
                { answer = inputNum / 1000; outputUnitBlock.Text = "meters"; }
                else if (unitIndex2 == 7) // to kilometers
                { answer = inputNum / 1000000; outputUnitBlock.Text = "kilometers"; }
            }
            else if (unitIndex1 == 5) // from centimeters
            {
                inputUnitBlock.Text = "centimeters";
                if (unitIndex2 == 0) // to inches
                { answer = inputNum / 2.54; outputUnitBlock.Text = "inches"; }
                else if (unitIndex2 == 1) // to feet
                { answer = inputNum / 30.48; outputUnitBlock.Text = "feet"; }
                else if (unitIndex2 == 2) // to yards
                { answer = inputNum / 91.44; outputUnitBlock.Text = "yards"; }
                else if (unitIndex2 == 3) // to miles
                { answer = inputNum / 160934; outputUnitBlock.Text = "miles"; }
                else if (unitIndex2 == 4) // to millimeters
                { answer = inputNum * 10; outputUnitBlock.Text = "millimeters"; }
                else if (unitIndex2 == 5) // to centimeters
                { answer = inputNum; outputUnitBlock.Text = "centimeters"; }
                else if (unitIndex2 == 6) // to meters
                { answer = inputNum * 0.01; outputUnitBlock.Text = "meters"; }
                else if (unitIndex2 == 7) // to kilometers
                { answer = inputNum * 0.00001; outputUnitBlock.Text = "kilometers"; }
            }
            else if (unitIndex1 == 6) // from meters
            {
                inputUnitBlock.Text = "meters";
                if (unitIndex2 == 0) // to inches
                { answer = inputNum * 39.3701; outputUnitBlock.Text = "inches"; }
                else if (unitIndex2 == 1) // to feet
                { answer = inputNum * 3.28084; outputUnitBlock.Text = "feet"; }
                else if (unitIndex2 == 2) // to yards
                { answer = inputNum * 1.09361; outputUnitBlock.Text = "yards"; }
                else if (unitIndex2 == 3) // to miles
                { answer = inputNum / 1609.34; outputUnitBlock.Text = "miles"; }
                else if (unitIndex2 == 4) // to millimeters
                { answer = inputNum * 1000; outputUnitBlock.Text = "millimeters"; }
                else if (unitIndex2 == 5) // to centimeters
                { answer = inputNum * 100; outputUnitBlock.Text = "centimeters"; }
                else if (unitIndex2 == 6) // to meters
                { answer = inputNum; outputUnitBlock.Text = "meters"; }
                else if (unitIndex2 == 7) // to kilometers
                { answer = inputNum * 0.001; outputUnitBlock.Text = "kilometers"; }
            }
            else if (unitIndex1 == 7) // from kilometers
            {
                inputUnitBlock.Text = "kilometers";
                if (unitIndex2 == 0) // to inches
                { answer = inputNum * 39370.1; outputUnitBlock.Text = "inches"; }
                else if (unitIndex2 == 1) // to feet
                { answer = inputNum * 3280.84; outputUnitBlock.Text = "feet"; }
                else if (unitIndex2 == 2) // to yards
                { answer = inputNum * 1093.61; outputUnitBlock.Text = "yards"; }
                else if (unitIndex2 == 3) // to miles
                { answer = inputNum * 0.621371; outputUnitBlock.Text = "miles"; }
                else if (unitIndex2 == 4) // to millimeters
                { answer = inputNum * 1000000; outputUnitBlock.Text = "millimeters"; }
                else if (unitIndex2 == 5) // to centimeters
                { answer = inputNum * 100000; outputUnitBlock.Text = "centimeters"; }
                else if (unitIndex2 == 6) // to meters
                { answer = inputNum * 1000; outputUnitBlock.Text = "meters"; }
                else if (unitIndex2 == 7) // to kilometers
                { answer = inputNum; outputUnitBlock.Text = "kilometers"; }
            }

            return (answer);
        }

        // Weight Calculations
        private double Weight_Calculation(double inputNum, int unitIndex1, int unitIndex2)
        {
            double answer = -1;

            // Conversions

            if (unitIndex1 == 0) // FROM OUNCES
            {
                inputUnitBlock.Text = "ounces";
                if (unitIndex2 == 0) // to ounces
                { answer = inputNum; outputUnitBlock.Text = "ounces"; }
                else if (unitIndex2 == 1) // to pounds
                { answer = inputNum * 0.0625; outputUnitBlock.Text = "pounds"; }
                else if (unitIndex2 == 2) // to tons (US)
                { answer = inputNum * 0.00003125; outputUnitBlock.Text = "tons (US)"; }
                else if (unitIndex2 == 3) // to tons (UK)
                { answer = inputNum * 0.000027902; outputUnitBlock.Text = "tons (UK)"; }
                else if (unitIndex2 == 4) // to metric tons
                { answer = inputNum * 0.00002835; outputUnitBlock.Text = "metric tons"; }
                else if (unitIndex2 == 5) // to grams
                { answer = inputNum * 28.3495; outputUnitBlock.Text = "grams"; }
                else if (unitIndex2 == 6) // to kilograms
                { answer = inputNum * 0.0283495; outputUnitBlock.Text = "kilograms"; }
            }
            else if (unitIndex1 == 1) // From pounds
            {
                inputUnitBlock.Text = "pounds";
                if (unitIndex2 == 0) // to ounces
                { answer = inputNum * 16; outputUnitBlock.Text = "ounces"; }
                else if (unitIndex2 == 1) // to pounds
                {   answer = inputNum; outputUnitBlock.Text = "pounds"; }
                else if (unitIndex2 == 2) // to tons (US) / "Short ton"
                {   answer = inputNum / 2000; outputUnitBlock.Text = "tons (US)"; }
                else if (unitIndex2 == 3) // to tons (UK) / "Long ton"
                {   answer = inputNum / 2240; outputUnitBlock.Text = "tons (UK)"; }
                else if (unitIndex2 == 4) // to metric tons
                {   answer = inputNum * 2204.62; outputUnitBlock.Text = "metric tons"; }
                else if (unitIndex2 == 5) // to grams
                {   answer = inputNum * 453.592; outputUnitBlock.Text = "grams"; }
                else if (unitIndex2 == 6) // to kilograms
                {   answer = inputNum * 0.453592; outputUnitBlock.Text = "kilograms"; }
            }
            else if (unitIndex1 == 2) // From Short tons (US)
            {
                inputUnitBlock.Text = "tons (US)";
                if (unitIndex2 == 0) // to ounces
                {    answer = inputNum * 32000; outputUnitBlock.Text = "ounces"; }
                else if (unitIndex2 == 1) // to pounds
                {   answer = inputNum * 2000; outputUnitBlock.Text = "pounds"; }
                else if (unitIndex2 == 2) // to tons (US)
                {   answer = inputNum; outputUnitBlock.Text = "tons (US)"; }
                else if (unitIndex2 == 3) // to tons (UK)
                {   answer = inputNum * 0.892857; outputUnitBlock.Text = "tons (UK)"; }
                else if (unitIndex2 == 4) // to metric tons
                {   answer = inputNum * 0.907185; outputUnitBlock.Text = "metric tons"; }
                else if (unitIndex2 == 5) // to grams
                {   answer = inputNum * 907185; outputUnitBlock.Text = "grams"; }
                else if (unitIndex2 == 6) // to kilograms
                {   answer = inputNum * 907.185; outputUnitBlock.Text = "kilograms"; }
            }

            else if (unitIndex1 == 3) // From Long tons (UK)
            {
                inputUnitBlock.Text = "tons (UK)";
                if (unitIndex2 == 0) // to ounces
                {   answer = inputNum * 35840; outputUnitBlock.Text = "ounces"; }
                else if (unitIndex2 == 1) // to pounds
                {   answer = inputNum * 2240; outputUnitBlock.Text = "pounds"; }
                else if (unitIndex2 == 2) // to tons (US)
                {   answer = inputNum * 1.12; outputUnitBlock.Text = "tons (US)"; }
                else if (unitIndex2 == 3) // to tons (UK)
                {   answer = inputNum; outputUnitBlock.Text = "tons (UK)"; }
                else if (unitIndex2 == 4) // to metric tons
                {   answer = inputNum * 1.01605; outputUnitBlock.Text = "metric tons"; }
                else if (unitIndex2 == 5) // to grams
                {   answer = inputNum * 1016046.91; outputUnitBlock.Text = "grams"; }
                else if (unitIndex2 == 6) // to kilograms
                {   answer = inputNum * 1016.04691; outputUnitBlock.Text = "kilograms"; }
            }
            else if (unitIndex1 == 4) // From metric tons
            {
                inputUnitBlock.Text = "metric tons";
                if (unitIndex2 == 0) // to ounces
                {   answer = inputNum * 35274; outputUnitBlock.Text = "ounces"; }
                else if (unitIndex2 == 1) // to pounds
                {   answer = inputNum * 2204.62; outputUnitBlock.Text = "pounds"; }
                else if (unitIndex2 == 2) // to tons (US)
                {   answer = inputNum * 1.10231; outputUnitBlock.Text = "tons (US)"; }
                else if (unitIndex2 == 3) // to tons (UK)
                {   answer = inputNum * 0.984207; outputUnitBlock.Text = "tons (UK)"; }
                else if (unitIndex2 == 4) // to metric tons
                {   answer = inputNum; outputUnitBlock.Text = "metric tons"; }
                else if (unitIndex2 == 5) // to grams
                {   answer = inputNum * 1000000.001; outputUnitBlock.Text = "grams"; }
                else if (unitIndex2 == 6) // to kilograms
                {   answer = inputNum * 1000.000001; outputUnitBlock.Text = "kilograms"; }
            }
            else if (unitIndex1 == 5) // From grams
            {
                inputUnitBlock.Text = "grams";
                if (unitIndex2 == 0) // to ounces
                {   answer = inputNum / 28.3495; outputUnitBlock.Text = "ounces"; }
                else if (unitIndex2 == 1) // to pounds
                {   answer = inputNum / 453.592; outputUnitBlock.Text = "pounds"; }
                else if (unitIndex2 == 2) // to tons (US)
                {   answer = inputNum / 907185; outputUnitBlock.Text = "tons (US)"; }
                else if (unitIndex2 == 3) // to tons (UK)
                {   answer = inputNum / 1016046.91; outputUnitBlock.Text = "tons (UK)"; }
                else if (unitIndex2 == 4) // to metric tons
                {   answer = inputNum / 1000000.001; outputUnitBlock.Text = "metric tons"; }
                else if (unitIndex2 == 5) // to grams
                {   answer = inputNum; outputUnitBlock.Text = "grams"; }
                else if (unitIndex2 == 6) // to kilograms
                {   answer = inputNum / 1000.000001; outputUnitBlock.Text = "kilograms"; }
            }
            else if (unitIndex1 == 6) // From kilograms
            {
                inputUnitBlock.Text = "kilograms";
                if (unitIndex2 == 0) // to ounces
                {   answer = inputNum * 35.274; outputUnitBlock.Text = "ounces"; }
                else if (unitIndex2 == 1) // to pounds
                {   answer = inputNum * 2.20462; outputUnitBlock.Text = "pounds"; }
                else if (unitIndex2 == 2) // to tons (US)
                {   answer = inputNum / 907.185; outputUnitBlock.Text = "tons (US)"; }
                else if (unitIndex2 == 3) // to tons (UK)
                {   answer = inputNum / 1016.05; outputUnitBlock.Text = "tons (UK)"; }
                else if (unitIndex2 == 4) // to metric tons
                {   answer = inputNum / 1000; outputUnitBlock.Text = "metric tons"; }
                else if (unitIndex2 == 5) // to grams
                {   answer = inputNum * 1000; outputUnitBlock.Text = "grams"; }
                else if (unitIndex2 == 6) // to kilograms
                { answer = inputNum; outputUnitBlock.Text = "kilograms"; }
            }

            return (answer);
        }

        // Volume Calculations
        private double Volume_Calculation(double inputNum, int unitIndex1, int unitIndex2)
        {
            double answer = -1;

            // Conversions

            if (unitIndex1 == 0) // FROM FLUID OUNCES 
            {
                inputUnitBlock.Text = "fluid ounces";
                if (unitIndex2 == 0) // to liquid ounces
                { answer = inputNum; outputUnitBlock.Text = "fluid ounces"; }
                else if (unitIndex2 == 1) // to pints (US)
                { answer = inputNum * 0.062500; outputUnitBlock.Text = "liquid pints (US)"; }
                else if (unitIndex2 == 2) // to cubic inches
                { answer = inputNum * 0.5541087161301047; outputUnitBlock.Text = "cubic inches"; }
                else if (unitIndex2 == 3) // to cubic feet
                { answer = inputNum * 0.0010444; outputUnitBlock.Text = "cubic feet"; }
                else if (unitIndex2 == 4) // to cups (US)
                { answer = inputNum / 8; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 5) // to metric cups
                { answer = inputNum / 8.4535; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 6) // to gallons (US)
                { answer = inputNum / 128; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 7) // to milliliters
                { answer = inputNum * 29.5735295625; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 8) // to centiliters
                { answer = inputNum * 2.95735296; outputUnitBlock.Text = "centiliters"; }
                else if (unitIndex2 == 9) // to liters
                { answer = inputNum * 0.0295735295625; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 10) // to barrels
                { answer = inputNum * 0.000248015873; outputUnitBlock.Text = "barrels"; }
            }
            else if (unitIndex1 == 1) // FROM LIQUID PINTS (US)
            {
                inputUnitBlock.Text = "liquid pints (US)";
                if (unitIndex2 == 0) // to liquid ounces
                { answer = inputNum / 0.062500; outputUnitBlock.Text = "fluid ounces"; }
                else if (unitIndex2 == 1) // to pints (US)
                { answer = inputNum; outputUnitBlock.Text = "liquid pints (US)"; }
                else if (unitIndex2 == 2) // to cubic inches
                { answer = inputNum * 28.875; outputUnitBlock.Text = "cubic inches"; }
                else if (unitIndex2 == 3) // to cubic feet
                { answer = inputNum * 0.016710; outputUnitBlock.Text = "cubic feet"; }
                else if (unitIndex2 == 4) // to cups (US)
                { answer = inputNum * 2; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 5) // to metric cups
                { answer = inputNum * 0.52834; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 6) // to gallons (US)
                { answer = inputNum / 8; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 7) // to milliliters
                { answer = inputNum * 473.18; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 8) // to centiliters
                { answer = inputNum * 47.318; outputUnitBlock.Text = "centiliters"; }
                else if (unitIndex2 == 9) // to liters
                { answer = inputNum * 0.47318; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 10) // to barrels
                { answer = inputNum / 252; outputUnitBlock.Text = "barrels"; }
            }
            else if (unitIndex1 == 2) // FROM CUBIC INCHES
            {
                inputUnitBlock.Text = "cubic inches";
                if (unitIndex2 == 0) // to liquid ounces
                { answer = inputNum * 0.55411; outputUnitBlock.Text = "fluid ounces"; }
                else if (unitIndex2 == 1) // to pints (US)
                { answer = inputNum * 0.0346; outputUnitBlock.Text = "liquid pints (US)"; }
                else if (unitIndex2 == 2) // to cubic inches
                { answer = inputNum; outputUnitBlock.Text = "cubic inches"; }
                else if (unitIndex2 == 3) // to cubic feet
                { answer = inputNum * 0.00057870; outputUnitBlock.Text = "cubic feet"; }
                else if (unitIndex2 == 4) // to cups (US)
                { answer = inputNum * 0.069264; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 5) // to metric cups
                { answer = inputNum * 0.065548; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 6) // to gallons (US)
                { answer = inputNum * 0.004329; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 7) // to milliliters
                { answer = inputNum * 16.387064; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 8) // to centiliters
                { answer = inputNum * 1.6387064; outputUnitBlock.Text = "centiliters"; }
                else if (unitIndex2 == 9) // to liters
                { answer = inputNum * 0.0163871; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 10) // to barrels
                { answer = inputNum * 0.000137428709; outputUnitBlock.Text = "barrels"; }
            }
            else if (unitIndex1 == 3) // FROM CUBIC FEET
            {
                inputUnitBlock.Text = "cubic feet";
                if (unitIndex2 == 0) // to liquid ounces
                { answer = inputNum * 957.506; outputUnitBlock.Text = "fluid ounces"; }
                else if (unitIndex2 == 1) // to pints (US)
                { answer = inputNum * 59.8442; outputUnitBlock.Text = "liquid pints (US)"; }
                else if (unitIndex2 == 2) // to cubic inches
                { answer = inputNum * 1728; outputUnitBlock.Text = "cubic inches"; }
                else if (unitIndex2 == 3) // to cubic feet
                { answer = inputNum; outputUnitBlock.Text = "cubic feet"; }
                else if (unitIndex2 == 4) // to cups (US)
                { answer = inputNum * 119.69; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 5) // to metric cups
                { answer = inputNum * 113.27; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 6) // to gallons (US)
                { answer = inputNum * 7.48052; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 7) // to milliliters
                { answer = inputNum * 28316.8466; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 8) // to centiliters
                { answer = inputNum * 2831.68466; outputUnitBlock.Text = "centiliters"; }
                else if (unitIndex2 == 9) // to liters
                { answer = inputNum * 28.3168; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 10) // to barrels
                { answer = inputNum * 0.237476809; outputUnitBlock.Text = "barrels"; }
            }
            else if (unitIndex1 == 4) // FROM CUPS (US)
            {
                inputUnitBlock.Text = "cups (US)";
                if (unitIndex2 == 0) // to liquid ounces
                { answer = inputNum * 8; outputUnitBlock.Text = "fluid ounces"; }
                else if (unitIndex2 == 1) // to pints (US)
                { answer = inputNum * 0.5; outputUnitBlock.Text = "liquid pints (US)"; }
                else if (unitIndex2 == 2) // to cubic inches
                { answer = inputNum * 14.4375; outputUnitBlock.Text = "cubic inches"; }
                else if (unitIndex2 == 3) // to cubic feet
                { answer = inputNum * 0.00835503; outputUnitBlock.Text = "cubic feet"; }
                else if (unitIndex2 == 4) // to cups (US)
                { answer = inputNum; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 5) // to metric cups
                { answer = inputNum * 0.94635; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 6) // to gallons (US)
                { answer = inputNum * 0.0625; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 7) // to milliliters
                { answer = inputNum * 236.588236; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 8) // to centiliters
                { answer = inputNum * 23.6588236; outputUnitBlock.Text = "centiliters"; }
                else if (unitIndex2 == 9) // to liters
                { answer = inputNum * 0.23658836; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 10) // to barrels
                { answer = inputNum * 0.00198412698; outputUnitBlock.Text = "barrels"; }
            }
            else if (unitIndex1 == 5) // FROM METRIC CUPS
            {
                inputUnitBlock.Text = "metric cups";
                if (unitIndex2 == 0) // to liquid ounces
                { answer = inputNum * 8.45; outputUnitBlock.Text = "fluid ounces"; }
                else if (unitIndex2 == 1) // to pints (US)
                { answer = inputNum * 0.5283441047163; outputUnitBlock.Text = "liquid pints (US)"; }
                else if (unitIndex2 == 2) // to cubic inches
                { answer = inputNum * 15.255936; outputUnitBlock.Text = "cubic inches"; }
                else if (unitIndex2 == 3) // to cubic feet
                { answer = inputNum * 0.0088286668; outputUnitBlock.Text = "cubic feet"; }
                else if (unitIndex2 == 4) // to cups (US)
                { answer = inputNum * 1.0567; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 5) // to metric cups
                { answer = inputNum; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 6) // to gallons (US)
                { answer = inputNum * 0.066043012; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 7) // to milliliters
                { answer = inputNum * 250; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 8) // to centiliters
                { answer = inputNum * 25.0; outputUnitBlock.Text = "centiliters"; }
                else if (unitIndex2 == 9) // to liters
                { answer = inputNum * .250; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 10) // to barrels
                { answer = inputNum * 0.0020966036; outputUnitBlock.Text = "barrels"; }
            }
            else if (unitIndex1 == 6) // FROM GALLONS (US)
            {
                inputUnitBlock.Text = "gallons (US)";
                if (unitIndex2 == 0) // to liquid ounces
                { answer = inputNum * 128; outputUnitBlock.Text = "fluid ounces"; }
                else if (unitIndex2 == 1) // to pints (US)
                { answer = inputNum * 8; outputUnitBlock.Text = "liquid pints (US)"; }
                else if (unitIndex2 == 2) // to cubic inches
                { answer = inputNum * 231; outputUnitBlock.Text = "cubic inches"; }
                else if (unitIndex2 == 3) // to cubic feet
                { answer = inputNum * 0.133681; outputUnitBlock.Text = "cubic feet"; }
                else if (unitIndex2 == 4) // to cups (US)
                { answer = inputNum * 16; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 5) // to metric cups
                { answer = inputNum * 15.141647136; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 6) // to gallons (US)
                { answer = inputNum; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 7) // to milliliters
                { answer = inputNum * 3785.41178; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 8) // to centiliters
                { answer = inputNum * 378.541178; outputUnitBlock.Text = "centiliters"; }
                else if (unitIndex2 == 9) // to liters
                { answer = inputNum * 3.78541; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 10) // to barrels
                { answer = inputNum * 0.0317460317; outputUnitBlock.Text = "barrels"; }
            }
            else if (unitIndex1 == 7) // FROM MILLILITERS
            {
                inputUnitBlock.Text = "milliliters";
                if (unitIndex2 == 0) // to liquid ounces
                { answer = inputNum * 0.033814; outputUnitBlock.Text = "fluid ounces"; }
                else if (unitIndex2 == 1) // to pints (US)
                { answer = inputNum * 0.00211338; outputUnitBlock.Text = "liquid pints (US)"; }
                else if (unitIndex2 == 2) // to cubic inches
                { answer = inputNum * 0.0610237; outputUnitBlock.Text = "cubic inches"; }
                else if (unitIndex2 == 3) // to cubic feet
                { answer = inputNum * 0.000035314667; outputUnitBlock.Text = "cubic feet"; }
                else if (unitIndex2 == 4) // to cups (US)
                { answer = inputNum * 0.0042267528; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 5) // to metric cups
                { answer = inputNum * 0.0040000000; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 6) // to gallons (US)
                { answer = inputNum * 0.00026417205; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 7) // to milliliters
                { answer = inputNum; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 8) // to centiliters
                { answer = inputNum * 0.1; outputUnitBlock.Text = "centiliters"; }
                else if (unitIndex2 == 9) // to liters
                { answer = inputNum * 0.001; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 10) // to barrels
                { answer = inputNum * 0.00000838641436; outputUnitBlock.Text = "barrels"; }
            }
            else if (unitIndex1 == 8) // FROM CENTILITERS              
            {
                inputUnitBlock.Text = "centiliters";
                if (unitIndex2 == 0) // to liquid ounces
                { answer = inputNum * 0.33814; outputUnitBlock.Text = "fluid ounces"; }
                else if (unitIndex2 == 1) // to pints (US)
                { answer = inputNum * 0.0211338; outputUnitBlock.Text = "liquid pints (US)"; }
                else if (unitIndex2 == 2) // to cubic inches
                { answer = inputNum * 0.610237; outputUnitBlock.Text = "cubic inches"; }
                else if (unitIndex2 == 3) // to cubic feet
                { answer = inputNum * 0.00035314667; outputUnitBlock.Text = "cubic feet"; }
                else if (unitIndex2 == 4) // to cups (US)
                { answer = inputNum * 0.042267528; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 5) // to metric cups
                { answer = inputNum * 0.040000000; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 6) // to gallons (US)
                { answer = inputNum * 0.0026417205; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 7) // to milliliters
                { answer = inputNum * 10; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 8) // to centiliters
                { answer = inputNum; outputUnitBlock.Text = "centiliters"; }
                else if (unitIndex2 == 9) // to liters
                { answer = inputNum * 0.01; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 10) // to barrels
                { answer = inputNum * 0.0000838641436; outputUnitBlock.Text = "barrels"; }
            }
            else if (unitIndex1 == 8) // FROM LITERS             
            {
                inputUnitBlock.Text = "liters";
                if (unitIndex2 == 0) // to liquid ounces
                { answer = inputNum * 33.814; outputUnitBlock.Text = "fluid ounces"; }
                else if (unitIndex2 == 1) // to pints (US)
                { answer = inputNum * 2.11338; outputUnitBlock.Text = "liquid pints (US)"; }
                else if (unitIndex2 == 2) // to cubic inches
                { answer = inputNum * 61.0237; outputUnitBlock.Text = "cubic inches"; }
                else if (unitIndex2 == 3) // to cubic feet
                { answer = inputNum * 0.035314667; outputUnitBlock.Text = "cubic feet"; }
                else if (unitIndex2 == 4) // to cups (US)
                { answer = inputNum * 4.2267528; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 5) // to metric cups
                { answer = inputNum * 4.0000000; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 6) // to gallons (US)
                { answer = inputNum * 0.26417205; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 7) // to milliliters
                { answer = inputNum * 1000; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 8) // to centiliters
                { answer = inputNum * 100; outputUnitBlock.Text = "centiliters"; }
                else if (unitIndex2 == 9) // to liters
                { answer = inputNum; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 10) // to barrels
                { answer = inputNum * 0.00838641436; outputUnitBlock.Text = "barrels"; }
            }

            return answer;
        }

        // Data Calculations
        private double Data_Calculation(double inputNum, int unitIndex1, int unitIndex2)
        {
            double answer = -1;


            if (unitIndex1 == 0) // FROM BITS 
            {
                inputUnitBlock.Text = "bits";
                if (unitIndex2 == 0) // to bits
                {   answer = inputNum; outputUnitBlock.Text = "bits"; }
                else if (unitIndex2 == 1) // to bytes
                {   answer = inputNum / 8; outputUnitBlock.Text = "bytes"; }
                else if (unitIndex2 == 2) // to kilobytes
                {    answer = inputNum / 8192; outputUnitBlock.Text = "kilobytes"; }
                else if (unitIndex2 == 3) // to megabytes
                {    answer = inputNum / 8388608; outputUnitBlock.Text = "megabytes"; } 
                else if (unitIndex2 == 4) // to gigabytes
                {    answer = inputNum / 8589934592; outputUnitBlock.Text = "gigabytes"; }
                else if (unitIndex2 == 5) // to terabytes
                {    answer = inputNum / 8796093022208; outputUnitBlock.Text = "terabytes"; }
            }
            else if (unitIndex1 == 1) // FROM BYTES
            {
                inputUnitBlock.Text = "bytes";
                if (unitIndex2 == 0) // to bits
                {    answer = inputNum * 8; outputUnitBlock.Text = "bits"; }
                else if (unitIndex2 == 1) // to bytes
                {    answer = inputNum; outputUnitBlock.Text = "bytes"; }
                else if (unitIndex2 == 2) // to kilobytes
                {    answer = inputNum / 1024; outputUnitBlock.Text = "kilobytes"; }
                else if (unitIndex2 == 3) // to megabytes
                {    answer = inputNum / 1048576; outputUnitBlock.Text = "megabytes"; } 
                else if (unitIndex2 == 4) // to gigabytes
                {    answer = inputNum / 1073741824; outputUnitBlock.Text = "gigabytes"; }
                else if (unitIndex2 == 5) // to terabytes
                {    answer = inputNum / 1099511627776; outputUnitBlock.Text = "terabytes"; }
            }
            else if (unitIndex1 == 2) // FROM KILOBYTES
            {
                inputUnitBlock.Text = "kilobytes";
                if (unitIndex2 == 0) // to bits
                {    answer = inputNum * 8192; outputUnitBlock.Text = "bits"; }
                else if (unitIndex2 == 1) // to bytes
                {    answer = inputNum * 1024; outputUnitBlock.Text = "bytes"; }
                else if (unitIndex2 == 2) // to kilobytes
                {    answer = inputNum; outputUnitBlock.Text = "kilobytes"; }
                else if (unitIndex2 == 3) // to megabytes
                {    answer = inputNum / 1024; outputUnitBlock.Text = "megabytes"; } 
                else if (unitIndex2 == 4) // to gigabytes
                {    answer = inputNum / 1048576; outputUnitBlock.Text = "gigabytes"; }
                else if (unitIndex2 == 5) // to terabytes
                {    answer = inputNum / 1073741824; outputUnitBlock.Text = "terabytes"; }
            }
            else if (unitIndex1 == 3) // FROM MEGABYTES
            {
                inputUnitBlock.Text = "megabytes";
                if (unitIndex2 == 0) // to bits
                {    answer = inputNum * 8388608; outputUnitBlock.Text = "bits"; }
                else if (unitIndex2 == 1) // to bytes
                {    answer = inputNum * 1048576; outputUnitBlock.Text = "bytes"; }
                else if (unitIndex2 == 2) // to kilobytes
                {    answer = inputNum * 1024; outputUnitBlock.Text = "kilobytes"; }
                else if (unitIndex2 == 3) // to megabytes
                {    answer = inputNum; outputUnitBlock.Text = "megabytes"; } 
                else if (unitIndex2 == 4) // to gigabytes
                {    answer = inputNum / 1024; outputUnitBlock.Text = "gigabytes"; }
                else if (unitIndex2 == 5) // to terabytes
                {    answer = inputNum / 1048576; outputUnitBlock.Text = "terabytes"; }
            }
            else if (unitIndex1 == 4) // FROM GIGABYTES
            {
                inputUnitBlock.Text = "gigabytes";
                if (unitIndex2 == 0) // to bits
                {    answer = inputNum * 8589934592; outputUnitBlock.Text = "bits"; }
                else if (unitIndex2 == 1) // to bytes
                {    answer = inputNum * 1073741824; outputUnitBlock.Text = "bytes"; }
                else if (unitIndex2 == 2) // to kilobytes
                {   answer = inputNum * 1048576; outputUnitBlock.Text = "kilobytes"; }
                else if (unitIndex2 == 3) // to megabytes
                {    answer = inputNum * 1024; outputUnitBlock.Text = "megabytes"; } 
                else if (unitIndex2 == 4) // to gigabytes
                {    answer = inputNum; outputUnitBlock.Text = "gigabytes"; }
                else if (unitIndex2 == 5) // to terabytes
                {    answer = inputNum / 1024; outputUnitBlock.Text = "terabytes"; }
            }
            else if (unitIndex1 == 5) // FROM TERABYTES
            {
                inputUnitBlock.Text = "terabytes";
                if (unitIndex2 == 0) // to bits
                {    answer = inputNum * 8796093022208; outputUnitBlock.Text = "bits"; }
                else if (unitIndex2 == 1) // to bytes
                {    answer = inputNum * 1099511627776; outputUnitBlock.Text = "bytes"; }
                else if (unitIndex2 == 2) // to kilobytes
                {    answer = inputNum * 1073741824; outputUnitBlock.Text = "kilobytes"; }
                else if (unitIndex2 == 3) // to megabytes
                {    answer = inputNum * 1048576; outputUnitBlock.Text = "megabytes"; } 
                else if (unitIndex2 == 4) // to gigabytes
                {    answer = inputNum * 1024; outputUnitBlock.Text = "gigabytes"; }
                else if (unitIndex2 == 5) // to terabytes
                {    answer = inputNum; outputUnitBlock.Text = "terabytes"; }
            }
            


            return answer;
        }

        // Temperature Calculations
        private double Temperature_Calculation(double inputNum, int unitIndex1, int unitIndex2)
        {
            double answer = -1;


            if (unitIndex1 == 0) // FROM KELVIN 
            {
                inputUnitBlock.Text = "kelvin";
                if (unitIndex2 == 0) // to kelvin
                { answer = inputNum; outputUnitBlock.Text = "kelvin"; }
                else if (unitIndex2 == 1) // to celsius
                { answer = inputNum - 273.15; outputUnitBlock.Text = "celsius"; }
                else if (unitIndex2 == 2) // to fahrenheit
                { answer = ((inputNum - 273.15) * 1.8) + 32; outputUnitBlock.Text = "fahrenheit"; }
            }
            else if (unitIndex1 == 1) // FROM CELSIUS
            {
                inputUnitBlock.Text = "celsius";
                if (unitIndex2 == 0) // to kelvin
                { answer = inputNum + 273.15; outputUnitBlock.Text = "kelvin"; }
                else if (unitIndex2 == 1) // to celsius
                { answer = inputNum; outputUnitBlock.Text = "celsius"; }
                else if (unitIndex2 == 2) // to fahrenheit
                { answer = (inputNum * 1.8) + 32; outputUnitBlock.Text = "fahrenheit"; }
            }
            else if (unitIndex1 == 2) // FROM FAHRENHEIT
            {
                inputUnitBlock.Text = "fahrenheit";
                if (unitIndex2 == 0) // to kelvin
                { answer = (((inputNum - 32) * 5) / 9) + 273.15; outputUnitBlock.Text = "kelvin"; }
                else if (unitIndex2 == 1) // to celsius
                { answer = ((inputNum - 32) * 5) / 9; outputUnitBlock.Text = "celsius"; }
                else if (unitIndex2 == 2) // to fahrenheit
                { answer = inputNum; outputUnitBlock.Text = "fahrenheit"; }
            }

            return answer;
        }

        // Cooking Calculations
        private double Cooking_Calculation(double inputNum, int unitIndex1, int unitIndex2)
        {
            double answer = -1;


            if (unitIndex1 == 0) // FROM MILLILITERS 
            {
                inputUnitBlock.Text = "milliliters";
                if (unitIndex2 == 0) // to milliliters
                { answer = inputNum; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 1) // to 
                { answer = inputNum * 0.004; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 2) // to 
                { answer = inputNum * 0.001; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 3) // to 
                { answer = inputNum * 0.20288414; outputUnitBlock.Text = "teaspoons (US)"; }
                else if (unitIndex2 == 4) // to 
                { answer = inputNum * 0.067628; outputUnitBlock.Text = "tablespoons (US)"; }
                else if (unitIndex2 == 5) // to 
                { answer = inputNum * 0.00422675; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 6) // to 
                { answer = inputNum * 0.00211338; outputUnitBlock.Text = "pints (US)"; }
                else if (unitIndex2 == 7) // to 
                { answer = inputNum * 0.00105669; outputUnitBlock.Text = "quarts (US)"; }
                else if (unitIndex2 == 8) // to 
                { answer = inputNum * 0.000264172; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 9) // to 
                { answer = inputNum * 0.168936; outputUnitBlock.Text = "teaspoons (UK)"; }
                else if (unitIndex2 == 10) // to 
                { answer = inputNum * 0.0563121; outputUnitBlock.Text = "tablespoons (UK)"; }
                else if (unitIndex2 == 11) // to 
                { answer = inputNum * 0.035195080; outputUnitBlock.Text = "fluid ounces (UK)"; }
                else if (unitIndex2 == 12) // to 
                { answer = inputNum * 0.00021996925; outputUnitBlock.Text = "gallons (UK)"; }
            }
            else if (unitIndex1 == 1) // FROM METRIC CUPS
            {
                inputUnitBlock.Text = "metric cups";
                if (unitIndex2 == 0) // to milliliters
                { answer = inputNum * 250; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 1) // to 
                { answer = inputNum; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 2) // to 
                { answer = inputNum * 0.25; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 3) // to 
                { answer = inputNum * 50.721035; outputUnitBlock.Text = "teaspoons (US)"; }
                else if (unitIndex2 == 4) // to 
                { answer = inputNum * 16.907011; outputUnitBlock.Text = "tablespoons (US)"; }
                else if (unitIndex2 == 5) // to 
                { answer = inputNum * 1.0566882; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 6) // to 
                { answer = inputNum * 0.5283441; outputUnitBlock.Text = "pints (US)"; }
                else if (unitIndex2 == 7) // to 
                { answer = inputNum * 0.26417205; outputUnitBlock.Text = "quarts (US)"; }
                else if (unitIndex2 == 8) // to 
                { answer = inputNum * 0.066043012; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 9) // to 
                { answer = inputNum * 70.39000000; outputUnitBlock.Text = "teaspoons (UK)"; }
                else if (unitIndex2 == 10) // to 
                { answer = inputNum * 17.597525; outputUnitBlock.Text = "tablespoons (UK)"; }
                else if (unitIndex2 == 11) // to 
                { answer = inputNum * 8.7987700; outputUnitBlock.Text = "fluid ounces (UK)"; }
                else if (unitIndex2 == 12) // to 
                { answer = inputNum * 0.054992313; outputUnitBlock.Text = "gallons (UK)"; }
            }
            else if (unitIndex1 == 2) // FROM LITERS
            {
                inputUnitBlock.Text = "liters";
                if (unitIndex2 == 0) // to milliliters
                { answer = inputNum * 1000; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 1) // to 
                { answer = inputNum * 4; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 2) // to 
                { answer = inputNum; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 3) // to 
                { answer = inputNum * 202.884; outputUnitBlock.Text = "teaspoons (US)"; }
                else if (unitIndex2 == 4) // to 
                { answer = inputNum * 67.628; outputUnitBlock.Text = "tablespoons (US)"; }
                else if (unitIndex2 == 5) // to 
                { answer = inputNum * 4.22675; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 6) // to 
                { answer = inputNum * 2.11338; outputUnitBlock.Text = "pints (US)"; }
                else if (unitIndex2 == 7) // to 
                { answer = inputNum * 1.05669; outputUnitBlock.Text = "quarts (US)"; }
                else if (unitIndex2 == 8) // to 
                { answer = inputNum * 0.264172; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 9) // to 
                { answer = inputNum * 168.936; outputUnitBlock.Text = "teaspoons (UK)"; }
                else if (unitIndex2 == 10) // to 
                { answer = inputNum * 56.3121; outputUnitBlock.Text = "tablespoons (UK)"; }
                else if (unitIndex2 == 11) // to 
                { answer = inputNum * 35.1951; outputUnitBlock.Text = "fluid ounces (UK)"; }
                else if (unitIndex2 == 12) // to 
                { answer = inputNum * 0.219969; outputUnitBlock.Text = "gallons (UK)"; }
            }
            else if (unitIndex1 == 3) // FROM TEASPOONS (US)
            {
                inputUnitBlock.Text = "teaspoons (US)";
                if (unitIndex2 == 0) // to milliliters
                { answer = inputNum * 4.92892; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 1) // to 
                { answer = inputNum * 0.019715686; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 2) // to 
                { answer = inputNum * 0.00492892; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 3) // to 
                { answer = inputNum; outputUnitBlock.Text = "teaspoons (US)"; }
                else if (unitIndex2 == 4) // to 
                { answer = inputNum / 3; outputUnitBlock.Text = "tablespoons (US)"; }
                else if (unitIndex2 == 5) // to 
                { answer = inputNum * 0.0208333; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 6) // to 
                { answer = inputNum * 0.0104167; outputUnitBlock.Text = "pints (US)"; }
                else if (unitIndex2 == 7) // to 
                { answer = inputNum * 0.00520833; outputUnitBlock.Text = "quarts (US)"; }
                else if (unitIndex2 == 8) // to 
                { answer = inputNum * 0.00130208; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 9) // to 
                { answer = inputNum * 0.832674; outputUnitBlock.Text = "teaspoons (UK)"; }
                else if (unitIndex2 == 10) // to 
                { answer = inputNum * 0.277558; outputUnitBlock.Text = "tablespoons (UK)"; }
                else if (unitIndex2 == 11) // to 
                { answer = inputNum * 0.173474; outputUnitBlock.Text = "fluid ounces (UK)"; }
                else if (unitIndex2 == 12) // to 
                { answer = inputNum * 0.00108421; outputUnitBlock.Text = "gallons (UK)"; }
            }
            else if (unitIndex1 == 4) // FROM TABLESPOONS (US)
            {
                inputUnitBlock.Text = "tablespoons (US)";
                if (unitIndex2 == 0) // to milliliters
                { answer = inputNum * 14.7868; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 1) // to 
                { answer = inputNum * 0.059147; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 2) // to 
                { answer = inputNum * 0.0147868; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 3) // to 
                { answer = inputNum * 3; outputUnitBlock.Text = "teaspoons (US)"; }
                else if (unitIndex2 == 4) // to 
                { answer = inputNum; outputUnitBlock.Text = "tablespoons (US)"; }
                else if (unitIndex2 == 5) // to 
                { answer = inputNum * 0.0625; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 6) // to 
                { answer = inputNum * 0.03125; outputUnitBlock.Text = "pints (US)"; }
                else if (unitIndex2 == 7) // to 
                { answer = inputNum * 0.015625; outputUnitBlock.Text = "quarts (US)"; }
                else if (unitIndex2 == 8) // to 
                { answer = inputNum * 0.00390625; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 9) // to 
                { answer = inputNum * 2.49802; outputUnitBlock.Text = "teaspoons (UK)"; }
                else if (unitIndex2 == 10) // to 
                { answer = inputNum * 0.832674; outputUnitBlock.Text = "tablespoons (UK)"; }
                else if (unitIndex2 == 11) // to 
                { answer = inputNum * 0.520421; outputUnitBlock.Text = "fluid ounces (UK)"; }
                else if (unitIndex2 == 12) // to 
                { answer = inputNum * 0.00325263; outputUnitBlock.Text = "gallons (UK)"; }
            }
            else if (unitIndex1 == 5) // FROM CUPS (US)
            {
                inputUnitBlock.Text = "cups (US)";
                if (unitIndex2 == 0) // to milliliters
                { answer = inputNum * 236.588; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 1) // to 
                { answer = inputNum * 0.94635; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 2) // to 
                { answer = inputNum * 0.236588; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 3) // to 
                { answer = inputNum * 48; outputUnitBlock.Text = "teaspoons (US)"; }
                else if (unitIndex2 == 4) // to 
                { answer = inputNum * 16; outputUnitBlock.Text = "tablespoons (US)"; }
                else if (unitIndex2 == 5) // to 
                { answer = inputNum; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 6) // to 
                { answer = inputNum * 0.5; outputUnitBlock.Text = "pints (US)"; }
                else if (unitIndex2 == 7) // to 
                { answer = inputNum * 0.25; outputUnitBlock.Text = "quarts (US)"; }
                else if (unitIndex2 == 8) // to 
                { answer = inputNum * 0.0625; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 9) // to 
                { answer = inputNum * 39.9683; outputUnitBlock.Text = "teaspoons (UK)"; }
                else if (unitIndex2 == 10) // to 
                { answer = inputNum * 13.3228; outputUnitBlock.Text = "tablespoons (UK)"; }
                else if (unitIndex2 == 11) // to 
                { answer = inputNum * 8.32674; outputUnitBlock.Text = "fluid ounces (UK)"; }
                else if (unitIndex2 == 12) // to 
                { answer = inputNum * 0.0520421; outputUnitBlock.Text = "gallons (UK)"; }
            }
            else if (unitIndex1 == 6) // FROM PINTS (US)
            {
                inputUnitBlock.Text = "pints (US)";
                if (unitIndex2 == 0) // to milliliters
                { answer = inputNum * 473.176; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 1) // to 
                { answer = inputNum * 1.8927; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 2) // to 
                { answer = inputNum * 0.473176; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 3) // to 
                { answer = inputNum * 96; outputUnitBlock.Text = "teaspoons (US)"; }
                else if (unitIndex2 == 4) // to 
                { answer = inputNum * 32; outputUnitBlock.Text = "tablespoons (US)"; }
                else if (unitIndex2 == 5) // to 
                { answer = inputNum * 2; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 6) // to 
                { answer = inputNum; outputUnitBlock.Text = "pints (US)"; }
                else if (unitIndex2 == 7) // to 
                { answer = inputNum * 0.5; outputUnitBlock.Text = "quarts (US)"; }
                else if (unitIndex2 == 8) // to 
                { answer = inputNum * 0.125; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 9) // to 
                { answer = inputNum * 79.9367; outputUnitBlock.Text = "teaspoons (UK)"; }
                else if (unitIndex2 == 10) // to 
                { answer = inputNum * 26.6456; outputUnitBlock.Text = "tablespoons (UK)"; }
                else if (unitIndex2 == 11) // to 
                { answer = inputNum * 16.6535; outputUnitBlock.Text = "fluid ounces (UK)"; }
                else if (unitIndex2 == 12) // to 
                { answer = inputNum * 0.104084; outputUnitBlock.Text = "gallons (UK)"; }
            }
            else if (unitIndex1 == 7) // FROM QUARTS (US)
            {
                inputUnitBlock.Text = "quarts (US)";
                if (unitIndex2 == 0) // to milliliters
                { answer = inputNum * 946.353; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 1) // to 
                { answer = inputNum * 3.7854; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 2) // to 
                { answer = inputNum * 0.946353; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 3) // to 
                { answer = inputNum * 192; outputUnitBlock.Text = "teaspoons (US)"; }
                else if (unitIndex2 == 4) // to 
                { answer = inputNum * 64; outputUnitBlock.Text = "tablespoons (US)"; }
                else if (unitIndex2 == 5) // to 
                { answer = inputNum * 4; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 6) // to 
                { answer = inputNum * 2; outputUnitBlock.Text = "pints (US)"; }
                else if (unitIndex2 == 7) // to 
                { answer = inputNum; outputUnitBlock.Text = "quarts (US)"; }
                else if (unitIndex2 == 8) // to 
                { answer = inputNum * 0.25; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 9) // to 
                { answer = inputNum * 159.873; outputUnitBlock.Text = "teaspoons (UK)"; }
                else if (unitIndex2 == 10) // to 
                { answer = inputNum * 53.2911; outputUnitBlock.Text = "tablespoons (UK)"; }
                else if (unitIndex2 == 11) // to 
                { answer = inputNum * 33.307; outputUnitBlock.Text = "fluid ounces (UK)"; }
                else if (unitIndex2 == 12) // to 
                { answer = inputNum * 0.208168; outputUnitBlock.Text = "gallons (UK)"; }
            }
            else if (unitIndex1 == 8) // FROM GALLONS (US)
            {
                inputUnitBlock.Text = "gallons (US)";
                if (unitIndex2 == 0) // to milliliters
                { answer = inputNum * 3785.41; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 1) // to 
                { answer = inputNum * 15.141647136; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 2) // to 
                { answer = inputNum * 3.78541; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 3) // to 
                { answer = inputNum * 768; outputUnitBlock.Text = "teaspoons (US)"; }
                else if (unitIndex2 == 4) // to 
                { answer = inputNum * 256; outputUnitBlock.Text = "tablespoons (US)"; }
                else if (unitIndex2 == 5) // to 
                { answer = inputNum * 16; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 6) // to 
                { answer = inputNum * 8; outputUnitBlock.Text = "pints (US)"; }
                else if (unitIndex2 == 7) // to 
                { answer = inputNum * 4; outputUnitBlock.Text = "quarts (US)"; }
                else if (unitIndex2 == 8) // to 
                { answer = inputNum; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 9) // to 
                { answer = inputNum * 639.494; outputUnitBlock.Text = "teaspoons (UK)"; }
                else if (unitIndex2 == 10) // to 
                { answer = inputNum * 213.165; outputUnitBlock.Text = "tablespoons (UK)"; }
                else if (unitIndex2 == 11) // to 
                { answer = inputNum * 133.228; outputUnitBlock.Text = "fluid ounces (UK)"; }
                else if (unitIndex2 == 12) // to 
                { answer = inputNum * 0.832674; outputUnitBlock.Text = "gallons (UK)"; }
            }
            else if (unitIndex1 == 9) // FROM TEASPOONS (UK)
            {
                inputUnitBlock.Text = "teaspoons (UK)";
                if (unitIndex2 == 0) // to milliliters
                { answer = inputNum * 5.91939; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 1) // to 
                { answer = inputNum * 0.014206563; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 2) // to 
                { answer = inputNum * 0.00591939; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 3) // to 
                { answer = inputNum * 1.20095; outputUnitBlock.Text = "teaspoons (US)"; }
                else if (unitIndex2 == 4) // to 
                { answer = inputNum * 0.400317; outputUnitBlock.Text = "tablespoons (US)"; }
                else if (unitIndex2 == 5) // to 
                { answer = inputNum * 0.0250198; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 6) // to 
                { answer = inputNum * 0.0125099; outputUnitBlock.Text = "pints (US)"; }
                else if (unitIndex2 == 7) // to 
                { answer = inputNum * 0.00625495; outputUnitBlock.Text = "quarts (US)"; }
                else if (unitIndex2 == 8) // to 
                { answer = inputNum / 639.494; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 9) // to 
                { answer = inputNum; outputUnitBlock.Text = "teaspoons (UK)"; }
                else if (unitIndex2 == 10) // to 
                { answer = inputNum / 3; outputUnitBlock.Text = "tablespoons (UK)"; }
                else if (unitIndex2 == 11) // to 
                { answer = inputNum / 4.8; outputUnitBlock.Text = "fluid ounces (UK)"; }
                else if (unitIndex2 == 12) // to 
                { answer = inputNum / 768; outputUnitBlock.Text = "gallons (UK)"; }
            }
            else if (unitIndex1 == 10) // FROM TABLESPOONS (UK)
            {
                inputUnitBlock.Text = "tablespoons (UK)";
                if (unitIndex2 == 0) // to milliliters
                { answer = inputNum * 17.7582; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 1) // to 
                { answer = inputNum * 0.056826173; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 2) // to 
                { answer = inputNum * 0.0177582; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 3) // to 
                { answer = inputNum * 3.60285; outputUnitBlock.Text = "teaspoons (US)"; }
                else if (unitIndex2 == 4) // to 
                { answer = inputNum * 1.20095; outputUnitBlock.Text = "tablespoons (US)"; }
                else if (unitIndex2 == 5) // to 
                { answer = inputNum * 0.0750594; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 6) // to 
                { answer = inputNum * 0.0375297; outputUnitBlock.Text = "pints (US)"; }
                else if (unitIndex2 == 7) // to 
                { answer = inputNum * 0.0187649; outputUnitBlock.Text = "quarts (US)"; }
                else if (unitIndex2 == 8) // to 
                { answer = inputNum * 0.00469121; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 9) // to 
                { answer = inputNum * 3; outputUnitBlock.Text = "teaspoons (UK)"; }
                else if (unitIndex2 == 10) // to 
                { answer = inputNum; outputUnitBlock.Text = "tablespoons (UK)"; }
                else if (unitIndex2 == 11) // to 
                { answer = inputNum * 0.625; outputUnitBlock.Text = "fluid ounces (UK)"; }
                else if (unitIndex2 == 12) // to 
                { answer = inputNum / 256; outputUnitBlock.Text = "gallons (UK)"; }
            }
            else if (unitIndex1 == 11) // FROM FLUID OUNCES (UK)
            {
                inputUnitBlock.Text = "fluid ounces (UK)";
                if (unitIndex2 == 0) // to milliliters
                { answer = inputNum * 28.4131; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 1) // to 
                { answer = inputNum * 0.11365225; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 2) // to 
                { answer = inputNum * 0.284131; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 3) // to 
                { answer = inputNum * 5.76456; outputUnitBlock.Text = "teaspoons (US)"; }
                else if (unitIndex2 == 4) // to 
                { answer = inputNum * 1.92152; outputUnitBlock.Text = "tablespoons (US)"; }
                else if (unitIndex2 == 5) // to 
                { answer = inputNum * 0.120095; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 6) // to 
                { answer = inputNum * 0.0600475; outputUnitBlock.Text = "pints (US)"; }
                else if (unitIndex2 == 7) // to 
                { answer = inputNum * 0.0300238; outputUnitBlock.Text = "quarts (US)"; }
                else if (unitIndex2 == 8) // to 
                { answer = inputNum * 0.00750594; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 9) // to 
                { answer = inputNum * 4.8; outputUnitBlock.Text = "teaspoons (UK)"; }
                else if (unitIndex2 == 10) // to 
                { answer = inputNum * 1.6; outputUnitBlock.Text = "tablespoons (UK)"; }
                else if (unitIndex2 == 11) // to 
                { answer = inputNum; outputUnitBlock.Text = "fluid ounces (UK)"; }
                else if (unitIndex2 == 12) // to 
                { answer = inputNum / 160; outputUnitBlock.Text = "gallons (UK)"; }
            }
            else if (unitIndex1 == 12) // FROM GALLONS (UK)
            {
                inputUnitBlock.Text = "gallons (UK)";
                if (unitIndex2 == 0) // to milliliters
                { answer = inputNum * 4546.09; outputUnitBlock.Text = "milliliters"; }
                else if (unitIndex2 == 1) // to 
                { answer = inputNum * 18.18436; outputUnitBlock.Text = "metric cups"; }
                else if (unitIndex2 == 2) // to 
                { answer = inputNum * 4.54609; outputUnitBlock.Text = "liters"; }
                else if (unitIndex2 == 3) // to 
                { answer = inputNum * 922.33; outputUnitBlock.Text = "teaspoons (US)"; }
                else if (unitIndex2 == 4) // to 
                { answer = inputNum * 307.443; outputUnitBlock.Text = "tablespoons (US)"; }
                else if (unitIndex2 == 5) // to 
                { answer = inputNum * 19.2152; outputUnitBlock.Text = "cups (US)"; }
                else if (unitIndex2 == 6) // to 
                { answer = inputNum * 9.6076; outputUnitBlock.Text = "pints (US)"; }
                else if (unitIndex2 == 7) // to 
                { answer = inputNum * 4.8038; outputUnitBlock.Text = "quarts (US)"; }
                else if (unitIndex2 == 8) // to 
                { answer = inputNum * 1.20095; outputUnitBlock.Text = "gallons (US)"; }
                else if (unitIndex2 == 9) // to 
                { answer = inputNum * 768; outputUnitBlock.Text = "teaspoons (UK)"; }
                else if (unitIndex2 == 10) // to 
                { answer = inputNum * 256; outputUnitBlock.Text = "tablespoons (UK)"; }
                else if (unitIndex2 == 11) // to 
                { answer = inputNum * 160; outputUnitBlock.Text = "fluid ounces (UK)"; }
                else if (unitIndex2 == 12) // to 
                { answer = inputNum; outputUnitBlock.Text = "gallons (UK)"; }
            }

            return answer;
        }




        public int continueEquation;
        /************************************************************* Calculator Functions *******************************************************************
         //
         //
         //
         //
         //
         //
         //
         //
         */

        // Prevent the user from messing things up with . ..
        public int decimalstatus = 0; // 1 = already clicked
        public int numbersInserted = 0; // only allow operations if there's numbers;

        private void CalculatorNumber_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            calculatorUserInput.Text += b.Content.ToString();


            // reset the equation block if it has a previous equation, unless a plus-minus-multiplication-division button is clicked
            if (equationBlock.Text.Contains("="))
            {
                if (continueEquation == 0)
                    equationBlock.Text = "";
            }

            // add number to equation block instantly

            equationBlock.Text += b.Content.ToString();

            if (calculatorResultBlock.Text.Length > 0)
                calculatorResultBlock.Text = "";

            if (b.Content.ToString() != "0")
                numbersInserted++;
        }

        private void decimal_Click(object sender, RoutedEventArgs e)
        {
            if (decimalstatus == 1)
                return;
            Button b = (Button)sender;
            calculatorUserInput.Text += b.Content.ToString();


            // Prevent the user from messing things up with . ..

            if (equationBlock.Text.Contains("."))
            {
                return;
            }

            // reset the equation block if it has a previous equation, unless a plus-minus-multiplication-division button is clicked
            if (equationBlock.Text.Contains("="))
            {
                if (continueEquation == 0)
                    equationBlock.Text = "";
            }

            // add number to equation block instantly

            equationBlock.Text += b.Content.ToString();

            if (calculatorResultBlock.Text.Length > 0)
                calculatorResultBlock.Text = "";

            decimalstatus = 1; // decimal has been clicked
        }




        private void CalculatorClearButton_Click(object sender, RoutedEventArgs e)
        {
            calculatorUserInput.Text = "";
            calculatorResultBlock.Text = "";
            equationBlock.Text = "";
            calculatorEqualsBlock1.Text = "";

            decimalstatus = 0;
            numbersInserted = 0;
        }

        private void CalculatorDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (calculatorUserInput.Text.Length > 0)
            {
                if (calculatorUserInput.Text.Length == 1)
                {
                    calculatorUserInput.Text = "";
                    equationBlock.Text = equationBlock.Text.Substring(0, equationBlock.Text.Length - 1);

                    //resultBlock.Text = "";  //here to empty the result block as well, when there's only one number left that's being deleted
                }
                else
                {
                    calculatorUserInput.Text = calculatorUserInput.Text.Substring(0, calculatorUserInput.Text.Length - 1);
                    equationBlock.Text = equationBlock.Text.Substring(0, equationBlock.Text.Length - 1);
                    //result();
                }
            }
            if (calculatorUserInput.Text.Length == 0)
            {
                calculatorUserInput.Text = "";
                //resultBlock.Text = "";
            }

            if (!calculatorUserInput.Text.Contains(".")) // reenable the decimal button if there is no decimal in the string
                decimalstatus = 0;

            numbersInserted--;

            //display result instantly
            // result();
            //the above failed because the result will be displayed next time a number button is clicked
            //instead, just clear the result textblock if there shouldn't be a number there anymore ( see above above)
        }

        private void CalculatorNegateButton_Click(object sender, RoutedEventArgs e)
        {
            if ((equationBlock.Text.Contains(".")) && (numbersInserted == 0))
            {
                return;
            }
            if (calculatorUserInput.Text.Length > 0)
            {
                if (calculatorUserInput.Text.Substring(0, 1).Contains("-") == false)
                {
                    firstNumberString = calculatorUserInput.Text;
                    calculatorUserInput.Text = "-" + firstNumberString;
                    equationBlock.Text = "-" + firstNumberString;
                }
                else
                {
                    if (calculatorUserInput.Text.Length > 0)
                    {
                        string temp;
                        temp = calculatorUserInput.Text.Substring(1);
                        calculatorUserInput.Text = temp;
                    }
                }
            }
        }


        // CALCULATION TYPE VARIABLE
        public int calculationType;
        public double calculatorFirstNum;
        public double calculatorSecondNum;
        public string firstNumberString;
        public string secondNumberString;
        public string calculatorOperator;

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            if ((equationBlock.Text.Contains(".")) && (numbersInserted == 0))
            {
                return;
            }
            if (calculatorUserInput.Text.Length > 0)
            {
                calculationType = 1; // 1 for addition

                // get user input from text block
                firstNumberString = calculatorUserInput.Text;

                calculatorOperator = "+";

                equationBlock.Text = firstNumberString + "+";

                calculatorFirstNum = Convert.ToDouble(calculatorUserInput.Text);

                // reset User Input block
                calculatorUserInput.Text = "";
            }

            if (calculatorResultBlock.Text.Length > 0)
            {
                calculationType = 1; // 1 for addition

                equationBlock.Text += "+";
                calculatorEqualsBlock1.Text = "+";
                firstNumberString = calculatorResultBlock.Text.Substring(0, calculatorResultBlock.Text.Length);
                //equationBlock.Text += firstNumberString;
                calculatorFirstNum = Convert.ToDouble(firstNumberString);

                continueEquation = 1;
               
            }
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            if ((equationBlock.Text.Contains(".")) && (numbersInserted == 0))
            {
                return;
            }
            if (calculatorUserInput.Text.Length > 0)
            {
                calculationType = 2; // 2 for subtraction

                // get user input from text block
                firstNumberString = calculatorUserInput.Text;

                calculatorOperator = "-";

                equationBlock.Text = firstNumberString + "-";

                calculatorFirstNum = Convert.ToDouble(calculatorUserInput.Text);

                // reset User Input block
                calculatorUserInput.Text = "";
            }

            if (calculatorResultBlock.Text.Length > 0)
            {
                calculationType = 2;

                equationBlock.Text += "-";
                calculatorEqualsBlock1.Text = "-";
                firstNumberString = calculatorResultBlock.Text.Substring(0, calculatorResultBlock.Text.Length);
                //equationBlock.Text += firstNumberString;
                calculatorFirstNum = Convert.ToDouble(firstNumberString);

                continueEquation = 1;
                
            }
        }

        private void MultiplicationButton_Click(object sender, RoutedEventArgs e)
        {
            if ((equationBlock.Text.Contains(".")) && (numbersInserted == 0))
            {
                return;
            }
            if (calculatorUserInput.Text.Length > 0)
            {
                calculationType = 3;

                // get user input from text block
                firstNumberString = calculatorUserInput.Text;

                calculatorOperator = "*";

                equationBlock.Text = firstNumberString + "*";

                calculatorFirstNum = Convert.ToDouble(calculatorUserInput.Text);

                // reset User Input block
                calculatorUserInput.Text = "";
            }

            if (calculatorResultBlock.Text.Length > 0)
            {
                calculationType = 3;

                equationBlock.Text += "*";
                calculatorEqualsBlock1.Text = "*";
                firstNumberString = calculatorResultBlock.Text.Substring(0, calculatorResultBlock.Text.Length);
                //equationBlock.Text += firstNumberString;
                calculatorFirstNum = Convert.ToDouble(firstNumberString);

                continueEquation = 1;
                
            }
        }

        private void DivisionButton_Click(object sender, RoutedEventArgs e)
        {
            if ((equationBlock.Text.Contains(".")) && (numbersInserted == 0))
            {
                return;
            }
            if (calculatorUserInput.Text.Length > 0)
            {
                calculationType = 4;

                // get user input from text block
                firstNumberString = calculatorUserInput.Text;

                calculatorOperator = "/";

                equationBlock.Text = firstNumberString + "/";

                calculatorFirstNum = Convert.ToDouble(calculatorUserInput.Text);

                // reset User Input block
                calculatorUserInput.Text = "";
            }

            if (calculatorResultBlock.Text.Length > 0)
            {
                calculationType = 4;

                equationBlock.Text += "/";
                calculatorEqualsBlock1.Text = "/";
                firstNumberString = calculatorResultBlock.Text.Substring(0, calculatorResultBlock.Text.Length);
                //equationBlock.Text += firstNumberString;
                calculatorFirstNum = Convert.ToDouble(firstNumberString);

                continueEquation = 1;
                
            }

        }


        // Calculator Result / Calculations

        private void EqualsButton_click(object sender, RoutedEventArgs e)
        {
            if ((equationBlock.Text.Contains(".")) && (numbersInserted == 0))
            {
                return;
            }
            if (calculatorUserInput.Text.Length != 0)
            {
                secondNumberString = calculatorUserInput.Text;
                calculatorSecondNum = Convert.ToDouble(calculatorUserInput.Text);
                double calculatorAnswer = -19;



                if (calculationType == 1)
                {
                    calculatorAnswer = calculatorFirstNum + calculatorSecondNum;
                    calculatorEqualsBlock.Text = "=";
                    calculatorResultBlock.Text = Convert.ToString(calculatorAnswer);
                    calculatorUserInput.Text = "";
                    equationBlock.Text += "=" + Convert.ToString(calculatorAnswer);
                }
                else if (calculationType == 2)
                {
                    calculatorAnswer = calculatorFirstNum - calculatorSecondNum;
                    calculatorEqualsBlock.Text = "=";
                    calculatorResultBlock.Text = Convert.ToString(calculatorAnswer);
                    calculatorUserInput.Text = "";
                    equationBlock.Text += "=" + Convert.ToString(calculatorAnswer);
                }
                else if (calculationType == 3)
                {
                    calculatorAnswer = calculatorFirstNum * calculatorSecondNum;
                    calculatorEqualsBlock.Text = "=";
                    calculatorResultBlock.Text = Convert.ToString(calculatorAnswer);
                    calculatorUserInput.Text = "";
                    equationBlock.Text += "=" + Convert.ToString(calculatorAnswer);
                }
                else if (calculationType == 4)
                {
                    calculatorAnswer = calculatorFirstNum / calculatorSecondNum;
                    calculatorEqualsBlock.Text = "=";
                    calculatorResultBlock.Text = Convert.ToString(calculatorAnswer);
                    calculatorUserInput.Text = "";
                    equationBlock.Text += "=" + Convert.ToString(calculatorAnswer);
                }
                else // in case there is no second number
                {
                    calculatorFirstNum = Convert.ToDouble(calculatorUserInput.Text);
                    calculatorAnswer = calculatorFirstNum;
                    calculatorEqualsBlock.Text = "=";
                    calculatorResultBlock.Text = Convert.ToString(calculatorAnswer);
                    calculatorUserInput.Text = "";
                    equationBlock.Text += "=" + Convert.ToString(calculatorAnswer);
                }

                    //calculatorResultBlock.Text = "Error!";
                calculatorEqualsBlock1.Text = "";
            }

            calculatorFirstNum = 0;
            calculatorSecondNum = 0;
            firstNumberString = "";
            secondNumberString = "";
            calculationType = -1;
            //calculatorEqualsBlock.Text = "";
        }

        private void CalculatorResults(int calculationType, double calculatorInput)
        {
            String answer;
            double calculatorInputNum = Convert.ToDouble(calculatorUserInput.Text);
            double calculatedNum;


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Support.xaml", UriKind.Relative));
        }

        private void ReviewButton_Click(object sender, RoutedEventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();

            marketplaceReviewTask.Show();
        }






    }

    //public class Panorama : ItemsControl
    //{
    //    //Panorama Constructor
    //    public Panorama()
    //    {
            
    //        public Panorama DefaultItem
    //        {
    //            get;
    //            set;
    //        }
    //    }
    //}
                

}