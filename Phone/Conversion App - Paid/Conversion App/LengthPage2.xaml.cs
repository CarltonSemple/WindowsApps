using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Conversion_App.Resources;
using Conversion_App.ViewModels;


namespace Conversion_App
{
    public partial class LengthPage2 : PhoneApplicationPage
    {

        PassingDataMethod method;

        public LengthPage2()
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
                    fromBox.Text = "inches";
                    break;
                case 1:
                    fromBox.Text = "feet";
                    break;
                case 2:
                    fromBox.Text = "yards";
                    break;
                case 3:
                    fromBox.Text = "miles";
                    break;
                case 4:
                    fromBox.Text = "millimeters";
                    break;
                case 5:
                    fromBox.Text = "centimeters";
                    break;
                case 6:
                    fromBox.Text = "meters";
                    break;
                case 7:
                    fromBox.Text = "kilometers";
                    break;
            };
        }

        private void submit(object sender, SelectionChangedEventArgs e)
        {
            int type = 1; // conversion type

            // If selected item is null (no selection) do nothing
            if (lengthLeftList.SelectedItem == null)
                return;

            // Get the index of the selected item in the LongListSelector
            int selectedIndex = App.ViewModel.length.Items.IndexOf(lengthLeftList.SelectedItem as ConversionData);

            string indexstring = selectedIndex.ToString();
            truthBlock2.Text = indexstring; // use this to pass the value of selectedIndex to PassDataInApplicationVariable()

            try
            {

                string unitIndexString2 = truthBlock2.Text;
                int unitIndex2 = Convert.ToInt32(unitIndexString2);
                (Application.Current as App).unitIndex2 = unitIndex2; //Pass data in an application-level variable

                (Application.Current as App).conversionType = type;
            }
            catch (Exception exc)
            {
            }


            // Enable the continue button
            continueButton.IsEnabled = true;



            // Set the name of the to "unit" box to the unit currently selected
            switch (selectedIndex)
            {
                case 0:
                    unitBox.Text = "inches";
                    break;
                case 1:
                    unitBox.Text = "feet";
                    break;
                case 2:
                    unitBox.Text = "yards";
                    break;
                case 3:
                    unitBox.Text = "miles";
                    break;
                case 4:
                    unitBox.Text = "millimeters";
                    break;
                case 5:
                    unitBox.Text = "centimeters";
                    break;
                case 6:
                    unitBox.Text = "meters";
                    break;
                case 7:
                    unitBox.Text = "kilometers";
                    break;
            };

            // Reset selected item to null (no selection)
            lengthLeftList.SelectedItem = null;
        }


        private void Continue_Click_1(object sender, RoutedEventArgs e)
        {
            // Only continue if the unit indexes is acceptable
            if (((Application.Current as App).unitIndex1 >= 0) && ((Application.Current as App).unitIndex1 <= 7))
            {
                if (((Application.Current as App).unitIndex2 >= 0) && ((Application.Current as App).unitIndex2 <= 7))
                {
                    // Navigate to the main page
                    NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                }
            }
        }

        


    }
}