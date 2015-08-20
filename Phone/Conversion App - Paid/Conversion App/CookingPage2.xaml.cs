using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Conversion_App.ViewModels;

namespace Conversion_App
{
    public partial class CookingPage2 : PhoneApplicationPage
    {
        public CookingPage2()
        {
            InitializeComponent();
            DataContext = App.ViewModel;

            continueButton.IsEnabled = false;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            // Set the data for the From Unit Box "fromBox"
            int fromIndex;
            fromIndex = (Application.Current as App).unitIndex1;

            switch (fromIndex)
            {
                case 0:
                    fromBox.Text = "milliliters";
                    break;
                case 1:
                    fromBox.Text = "metric cups";
                    break;
                case 2:
                    fromBox.Text = "liters";
                    break;
                case 3:
                    fromBox.Text = "teaspoons (US)";
                    break;
                case 4:
                    fromBox.Text = "tablespoons (US)";
                    break;
                case 5:
                    fromBox.Text = "cups (US)";
                    break;
                case 6:
                    fromBox.Text = "pints (US)";
                    break;
                case 7:
                    fromBox.Text = "quarts (US)";
                    break;
                case 8:
                    fromBox.Text = "gallons (US)";
                    break;
                case 9:
                    fromBox.Text = "teaspoons (UK)";
                    break;
                case 10:
                    fromBox.Text = "tablespoons (UK)";
                    break;
                case 11:
                    fromBox.Text = "fluid ounces (UK)";
                    break;
                case 12:
                    fromBox.Text = "gallons (UK)";
                    break;
            };
        }

        private void submit(object sender, SelectionChangedEventArgs e)
        {
            int type = 6; // conversion type

            // If selected item is null (no selection) do nothing
            if (unitList.SelectedItem == null)
                return;

            // Enable the continue button
            continueButton.IsEnabled = true;


            // Get the index of the selected item in the LongListSelector
            int selectedIndex = App.ViewModel.cooking.Items.IndexOf(unitList.SelectedItem as ConversionData);

            string indexstring = selectedIndex.ToString();
            truthBlock2.Text = indexstring; // use this to pass the value of selectedIndex to PassDataInApplicationVariable()

            try
            {

                string unitIndexString2 = truthBlock2.Text;
                int unitIndex2 = Convert.ToInt32(unitIndexString2);
                (Application.Current as App).unitIndex2 = unitIndex2; //Pass data in an application-level variable

                // Set the public variable conversion type
                (Application.Current as App).conversionType = type;
            }
            catch (Exception exc)
            {
            }


            // Set the name of the to "unit" box to the unit currently selected
            switch (selectedIndex)
            {
                case 0:
                    unitBox.Text = "milliliters";
                    break;
                case 1:
                    unitBox.Text = "metric cups";
                    break;
                case 2:
                    unitBox.Text = "liters";
                    break;
                case 3:
                    unitBox.Text = "teaspoons (US)";
                    break;
                case 4:
                    unitBox.Text = "tablespoons (US)";
                    break;
                case 5:
                    unitBox.Text = "cups (US)";
                    break;
                case 6:
                    unitBox.Text = "pints (US)";
                    break;
                case 7:
                    unitBox.Text = "quarts (US)";
                    break;
                case 8:
                    unitBox.Text = "gallons (US)";
                    break;
                case 9:
                    unitBox.Text = "teaspoons (UK)";
                    break;
                case 10:
                    unitBox.Text = "tablespoons (UK)";
                    break;
                case 11:
                    unitBox.Text = "fluid ounces (UK)";
                    break;
                case 12:
                    unitBox.Text = "gallons (UK)";
                    break;
            };

            // Reset selected item to null (no selection)
            unitList.SelectedItem = null;
        }


        private void Continue_Click_1(object sender, RoutedEventArgs e)
        {
            // Only continue if the unit indexes is acceptable
            if (((Application.Current as App).unitIndex1 >= 0) && ((Application.Current as App).unitIndex1 <= 12))
            {
                if (((Application.Current as App).unitIndex2 >= 0) && ((Application.Current as App).unitIndex2 <= 12))
                {
                    // Navigate to the main page
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
            }
        }




    }
}