using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Conversion_App.Resources;
using Conversion_App.ViewModels;
using Windows.Storage.Streams;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.Advertising;

namespace Conversion_App
{
    public partial class ConversionSelectionPage : PhoneApplicationPage
    {
        //private ObservableCollection<MyListViewModel> myVM;

        PassingDataMethod method;

        const string METHOD_TEMPLATE = "Recipe:{0}{1}";
        const string TARGETPAGE_URI_TEMPLATE = "/MainPage.xaml?method={0}";

        public ConversionSelectionPage()
        {
            InitializeComponent();
            
            

        // Length items
            //List<string> lengthItems = new List<string>();
            //lengthItems.Add("inches");
            //lengthItems.Add("feet");
            //lengthItems.Add("yards");
            //lengthItems.Add("centimeters");
            //lengthItems.Add("meters");
            //lengthItems.Add("test1");
            //lengthItems.Add("test2");
            //lengthItems.Add("test3");
            //lengthItems.Add("test4");

            // Bind to control
            //lengthLeftList.ItemsSource = lengthRightList.ItemsSource = lengthItems;

            //attempt 3
            DataContext = App.ViewModel;

            //following is a different method that we won't be using, but useful to know
            //Loaded += SelectionPage_Loaded;

        }


        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
           
        }






// Handle selection changed on length Top LongListSelector



        private void lengthLeftList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (lengthLeftList.SelectedItem == null)
                return;

            // Get the index of the selected item in the LongListSelector
            int selectedIndex = App.ViewModel.length.Items.IndexOf(lengthLeftList.SelectedItem as ConversionData);

            string indexstring = selectedIndex.ToString();
            truthBlock.Text = indexstring; // use this to pass the value of selectedIndex to PassDataInApplicationVariable()



            method = PassingDataMethod.ApplicationVariable;

                string unitIndexString = truthBlock.Text;
                int unitIndex1 = Convert.ToInt32(unitIndexString);
                (Application.Current as App).unitIndex1 = unitIndex1;

                
            




            // Navigate to the next page
            NavigationService.Navigate(new Uri("/LengthPage2.xaml", UriKind.Relative));


            // Reset selected item to null (no selection)
            lengthLeftList.SelectedItem = null;
        }


// Handle selection changed on weight LongListSelector
        private void weightTopList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (weightTopList.SelectedItem == null)
                return;

            //int selectedIndex = App.ViewModel.Items.IndexOf(weightTopList.SelectedItem as ConversionModel);
            int selectedIndex = App.ViewModel.weight.Items.IndexOf(weightTopList.SelectedItem as ConversionData);


            string indexstring = selectedIndex.ToString();
            truthBlock3.Text = indexstring; // use this to pass the value of selectedIndex to PassDataInApplicationVariable()


            method = PassingDataMethod.ApplicationVariable;

            string unitIndexString = truthBlock3.Text;
            int unitIndex1 = Convert.ToInt32(unitIndexString);
            (Application.Current as App).unitIndex1 = unitIndex1;



            // Navigate to the next page
            NavigationService.Navigate(new Uri("/WeightPage2.xaml", UriKind.Relative));

            // Reset selected item to null (no selection)
            weightTopList.SelectedItem = null;
        }

        private void volumeList_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (volumeList.SelectedItem == null)
                return;

            //int selectedIndex = App.ViewModel.Items.IndexOf(weightTopList.SelectedItem as ConversionModel);
            int selectedIndex = App.ViewModel.volume.Items.IndexOf(volumeList.SelectedItem as ConversionData);


            string indexstring = selectedIndex.ToString();
            truthBlock3.Text = indexstring; // use this to pass the value of selectedIndex to PassDataInApplicationVariable()

            method = PassingDataMethod.ApplicationVariable;

            string unitIndexString = truthBlock3.Text;
            int unitIndex1 = Convert.ToInt32(unitIndexString);
            (Application.Current as App).unitIndex1 = unitIndex1;

            // Navigate to the next page
            NavigationService.Navigate(new Uri("/VolumePage2.xaml", UriKind.Relative));

            // Reset selected item to null (no selection)
            volumeList.SelectedItem = null;
        }

        private void dataList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (dataList.SelectedItem == null)
                return;

            //int selectedIndex = App.ViewModel.Items.IndexOf(weightTopList.SelectedItem as ConversionModel);
            int selectedIndex = App.ViewModel.data.Items.IndexOf(dataList.SelectedItem as ConversionData);


            string indexstring = selectedIndex.ToString();
            truthBlock3.Text = indexstring; // use this to pass the value of selectedIndex to PassDataInApplicationVariable()

            method = PassingDataMethod.ApplicationVariable;

            string unitIndexString = truthBlock3.Text;
            int unitIndex1 = Convert.ToInt32(unitIndexString);
            (Application.Current as App).unitIndex1 = unitIndex1;

            // Navigate to the next page
            NavigationService.Navigate(new Uri("/DataPage2.xaml", UriKind.Relative));

            // Reset selected item to null (no selection)
            dataList.SelectedItem = null;
        }

        private void temperatureList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (temperatureList.SelectedItem == null)
                return;

            //int selectedIndex = App.ViewModel.Items.IndexOf(weightTopList.SelectedItem as ConversionModel);
            int selectedIndex = App.ViewModel.temperature.Items.IndexOf(temperatureList.SelectedItem as ConversionData);


            string indexstring = selectedIndex.ToString();
            truthBlock3.Text = indexstring; // use this to pass the value of selectedIndex to PassDataInApplicationVariable()

            method = PassingDataMethod.ApplicationVariable;

            string unitIndexString = truthBlock3.Text;
            int unitIndex1 = Convert.ToInt32(unitIndexString);
            (Application.Current as App).unitIndex1 = unitIndex1;

            // Navigate to the next page
            NavigationService.Navigate(new Uri("/TemperaturePage2.xaml", UriKind.Relative));

            // Reset selected item to null (no selection)
            temperatureList.SelectedItem = null;
        }

        private void cookingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If selected item is null (no selection) do nothing
            if (cookingList.SelectedItem == null)
                return;

            //int selectedIndex = App.ViewModel.Items.IndexOf(weightTopList.SelectedItem as ConversionModel);
            int selectedIndex = App.ViewModel.cooking.Items.IndexOf(cookingList.SelectedItem as ConversionData);


            string indexstring = selectedIndex.ToString();
            truthBlock3.Text = indexstring; // use this to pass the value of selectedIndex to PassDataInApplicationVariable()

            method = PassingDataMethod.ApplicationVariable;

            string unitIndexString = truthBlock3.Text;
            int unitIndex1 = Convert.ToInt32(unitIndexString);
            (Application.Current as App).unitIndex1 = unitIndex1;

            // Navigate to the next page
            NavigationService.Navigate(new Uri("/CookingPage2.xaml", UriKind.Relative));

            // Reset selected item to null (no selection)
            cookingList.SelectedItem = null;
        }


        // SUBMIT BUTTONS

        private void lengthSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            int type = 1;
            int button = 1;

            try
            {
                PassDataInApplicationVariable(button);
                (Application.Current as App).conversionType = type;
            }
            catch (Exception exc)
            {
            }

        
        }

        private void weightSubmitButton_Click(object sender, RoutedEventArgs e)
        {
            int type = 2;
            int button = 2;

            try
            {
                PassDataInApplicationVariable(button);
                (Application.Current as App).conversionType = type;
            }
            catch (Exception exc)
            {
            }
        }




        //Pass data in an application-level variable
        // defined in App.xaml.cs.
        private void PassDataInApplicationVariable(int button)
        {
            method = PassingDataMethod.ApplicationVariable;

        //Change the textblock source depending on which convert button is pressed
            if (button == 1)
            {
                string unitIndexString = truthBlock.Text;
                int unitIndex1 = Convert.ToInt32(unitIndexString);

                string unitIndexString2 = truthBlock2.Text;
                int unitIndex2 = Convert.ToInt32(unitIndexString2);

                (Application.Current as App).unitIndex1 = unitIndex1;
                (Application.Current as App).unitIndex2 = unitIndex2;
            }
            else if (button == 2)
            {
                string unitIndexString = truthBlock3.Text;
                int unitIndex1 = Convert.ToInt32(unitIndexString);

                string unitIndexString2 = truthBlock4.Text;
                int unitIndex2 = Convert.ToInt32(unitIndexString2);

                (Application.Current as App).unitIndex1 = unitIndex1;
                (Application.Current as App).unitIndex2 = unitIndex2;
            }

 
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }




        /// <summary>
        /// Recursive get the item.
        /// </summary>
        /// <typeparam name="T">The item to get.</typeparam>
        /// <param name="parents">Parent container.</param>
        /// <param name="objectList">Item list</param>
        public static void GetItemsRecursive<T>(DependencyObject parents, ref List<T> objectList) where T : DependencyObject
        {
            

            var childrenCount = VisualTreeHelper.GetChildrenCount(parents);


            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parents, i);


                if (child is T)
                {
                    objectList.Add(child as T);
                }


                GetItemsRecursive<T>(child, ref objectList);
            }


            return;
        }

        

        








    }

}