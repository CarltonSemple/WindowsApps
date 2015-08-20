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
    public partial class TemperaturePage2 : PhoneApplicationPage
    {
        public TemperaturePage2()
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
                    fromBox.Text = "kelvin";
                    break;
                case 1:
                    fromBox.Text = "celsius";
                    break;
                case 2:
                    fromBox.Text = "fahrenheit";
                    break;
            };
        }

        private void submit(object sender, SelectionChangedEventArgs e)
        {
            int type = 5; // conversion type

            // If selected item is null (no selection) do nothing
            if (unitList.SelectedItem == null)
                return;

            // Enable the continue button
            continueButton.IsEnabled = true;


            // Get the index of the selected item in the LongListSelector
            int selectedIndex = App.ViewModel.temperature.Items.IndexOf(unitList.SelectedItem as ConversionData);

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
                    unitBox.Text = "kelvin";
                    break;
                case 1:
                    unitBox.Text = "celsius";
                    break;
                case 2:
                    unitBox.Text = "fahrenheit";
                    break;
            };

            // Reset selected item to null (no selection)
            unitList.SelectedItem = null;
        }


        private void Continue_Click_1(object sender, RoutedEventArgs e)
        {
            // Only continue if the unit indexes is acceptable
            if (((Application.Current as App).unitIndex1 >= 0) && ((Application.Current as App).unitIndex1 <= 2))
            {
                if (((Application.Current as App).unitIndex2 >= 0) && ((Application.Current as App).unitIndex2 <= 2))
                {
                    // Navigate to the main page
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
            }
        }
    }
}