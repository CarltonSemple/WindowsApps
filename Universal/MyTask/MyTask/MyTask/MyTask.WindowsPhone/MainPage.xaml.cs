using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using System.Windows.Input;
using Windows.ApplicationModel.Resources;
using MyTask.Common;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Shapes;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MyTask
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // navigation and local view model
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
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


        // local reference to the global task model
        TaskModel taskModel = (App.Current as App).taskModel;

        // loader for resource strings
        ResourceLoader loader = new Windows.ApplicationModel.Resources.ResourceLoader();
        
        #region transformationProperties
        /****** transformation preparations for moving the UI *****/
        private TranslateTransform move = new TranslateTransform();
        private TranslateTransform dmove = new TranslateTransform();
        private TranslateTransform mMove = new TranslateTransform();
        private ScaleTransform resize = new ScaleTransform();
        private TransformGroup transforms = new TransformGroup();
        private TransformGroup dtransforms = new TransformGroup();
        private TransformGroup menuTransforms = new TransformGroup();

        private Brush transformingBrush = new SolidColorBrush(Colors.Orange);

        double Yorigin;
        #endregion

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;


            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            transforms.Children.Add(move);
            transforms.Children.Add(resize);
            //newTaskForm.RenderTransform = transforms;
            LayoutRoot.RenderTransform = transforms;

           /* newTaskForm.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(newTaskForm_ManipulationStarted);
            newTaskForm.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(newTaskForm_ManipulationDelta);
            newTaskForm.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(newTaskForm_ManipulationCompleted);
            */
            Yorigin = move.Y;

            // transforms for delete buttons
            dtransforms.Children.Add(dmove);

            // transforms for menu options
            menuTransforms.Children.Add(mMove);
            
            //menuOptions.RenderTransform = menuTransforms;

            // put the current location on the map
            
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

            //var group = await SampleDataSource.GetGroupAsync((string)e.NavigationParameter);
            //this.DefaultViewModel["Group"] = group;
            //this.DefaultViewModel["Items"] = group.Items;

            this.DefaultViewModel["taskList"] = taskModel.taskList;
            //title.Text = (App.Current as App).selectedMerchant.name;

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


        private void newTaskForm_ManipulationStarted(object sender, EventHandler<ManipulationStartedEventArgs> e)
        {
        }

        /*private void newTaskForm_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
        }*/

        private void newTaskForm_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
        }



        // Handle selection changed on LongListSelector
        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var MainLongListSelector = (ListView)sender;

            // If selected item is null (no selection) do nothing
            if (MainLongListSelector.SelectedItem == null)
                return;

            // If the dialogue for a new task is open, close it
            // possibly empty the dialogue, but keep it as it is for now
            if (!normalPosition)
            {
                LayoutRoot.Margin = new Thickness(0, -100, 0, 0);
                makeTaskToggle.Content = "Create";
                normalPosition = true;
                makeTaskToggle.IsChecked = false;
                return;
            }

            //pagename.Text = (MainLongListSelector.SelectedItem as ItemViewModel).Id.ToString();



            // Navigate to the new page
            //NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + ((MainLongListSelector.SelectedItem as ItemViewModel).Id - 1).ToString(), UriKind.Relative)); 
    // subtracted 1 so that the indexing works

            // Reset selected item to null (no selection)
            MainLongListSelector.SelectedItem = null;
        }

        bool normalPosition = true;

        bool backButtonClick = false;



        bool oneThingAtATime = false;
        /************************************ Making the task ************************************/
        private void makeTaskToggle_Checked(object sender, RoutedEventArgs e)
        {
            move.Y += 100;
            makeTaskToggle.Content = loader.GetString("SaveTask");
            normalPosition = false;
            taskBox.Focus(Windows.UI.Xaml.FocusState.Programmatic);

            // Disable the bottom menu button, for 16:9 screens
            oneThingAtATime = true;
        }

        private void makeTaskToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            move.Y -= 100;
            makeTaskToggle.Content = loader.GetString("CreateTask");
            normalPosition = true;

            // reenable the bottom menu button, for 16:9 screens...
            oneThingAtATime = false;

            // Save the text in the textbox, and then reset the textbox
            string taskstring = taskBox.Text;
            string title = titleBox.Text;
            titleBox.Text = "";
            taskBox.Text = "";

            TaskItem newItem = new TaskItem(DateTime.UtcNow, title, taskstring);           

            if (backButtonClick == false)
            {
                if (taskstring != "")
                    taskModel.AddTaskItem(newItem);

// ********************************** Update Live Tile *********************************
////////////////////////////////////////////////
////////////////////////////////////////////////
////////////////////////////////////////////////
////////////////////////////////////////////////
                /*IconicTileData tile = new IconicTileData();
                tile.Title = AppResources.ApplicationTitle;

                // Add the number of tasks to the tile
                tile.Count = App.ViewModel.taskList.Count;

                // empty the tiles
                tile.WideContent1 = "";
                tile.WideContent2 = "";
                tile.WideContent3 = "";

                // Add the images to the tile
                tile.IconImage = new Uri("Assets/Tiles/Iconic/202x202.png", UriKind.Relative);
                tile.SmallIconImage = new Uri("Assets/Tiles/Iconic/110x110.png", UriKind.Relative);

                // Set the wide tile content
                switch (tile.Count)
                {
                    case 0: tile.WideContent1 = AppResources.emptyTilePrompt;
                        break;
                    case 1: tile.WideContent1 = App.ViewModel.taskList.ElementAt<TaskItem>(0).Details;
                        break;
                    case 2:
                        {
                            tile.WideContent1 = App.ViewModel.taskList.ElementAt<TaskItem>(0).Details;
                            tile.WideContent2 = App.ViewModel.taskList.ElementAt<TaskItem>(1).Details;
                        } break;
                    case 3:
                        {
                            tile.WideContent1 = App.ViewModel.taskList.ElementAt<TaskItem>(0).Details;
                            tile.WideContent2 = App.ViewModel.taskList.ElementAt<TaskItem>(1).Details;
                            tile.WideContent3 = App.ViewModel.taskList.ElementAt<TaskItem>(App.ViewModel.taskList.Count() - 1).Details;
                        } break;
                    default:
                        {
                            tile.WideContent1 = App.ViewModel.taskList.ElementAt<TaskItem>(0).Details;
                            tile.WideContent2 = App.ViewModel.taskList.ElementAt<TaskItem>(1).Details;
                            tile.WideContent3 = App.ViewModel.taskList.ElementAt<TaskItem>(App.ViewModel.taskList.Count() - 1).Details;
                        }
                        break;
                }

                // find the tile object for the application tile that using "Iconic" contains string in it.
                ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("Iconic".ToString()));

                if (TileToFind != null && TileToFind.NavigationUri.ToString().Contains("Iconic"))
                {
                    TileToFind.Update(tile);
                }
                */
            }
            backButtonClick = false;

        }

        // Change the behavior of the back button
        /*protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            // just move the position back to normal instead of exiting
            if ((normalPosition == false) && (menuOpen == false))
            {
                //move.Y = Yorigin;
                // The above line refuses to work properly, but the following does
                LayoutRoot.Margin = new Thickness(0, -100, 0, -140);
                makeTaskToggle.Content = loader.GetString("CreateTask");
                normalPosition = true;

                // stops the toggle button from saving
                backButtonClick = true;

                makeTaskToggle.IsChecked = false;

                e.Cancel = true;                
            }
            else if ((normalPosition == false) && (menuOpen == true)) // when the menu is open instead of the new task context
            {
                move.Y += 140;
                LayoutRoot.Margin = new Thickness(0, -100, 0, -140);
                normalPosition = true;
                menuOpen = false;

                // leave the following alone in this case
                //backButtonClick = true;

                //************** Handle the arrows **************
                // make the up arrows visible
                upArrowLeft.Opacity = 1;
                upArrowRight.Opacity = 1;
                upArrowLeftlight.Opacity = 1;
                upArrowRightlight.Opacity = 1;

                // make the down arrows invisible
                downArrowLeft.Opacity = 0;
                downArrowRight.Opacity = 0;
                downArrowLeftlight.Opacity = 0;
                downArrowRightlight.Opacity = 0;

                e.Cancel = true;
            }
            else
            {
                //base.OnBackKeyPress(e);
            }
        }*/

        // Using the enter key to save the task
        /*private void taskBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyStatus.Equals(Key.Enter))
            {
                LayoutRoot.Margin = new Thickness(0, -100, 0, -140);
                makeTaskToggle.Content = loader.GetString("CreateTask");
                normalPosition = true;
                makeTaskToggle.IsChecked = false;
                this.Focus();
            }
        }*/



        bool menuOpen = false;

        private void Map_Loaded(object sender, RoutedEventArgs e)
        {
            MapControl map = (MapControl)sender;

            map.Center = (App.Current as App).GeoLocator.currentPosition.Coordinate.Point;

            map.ZoomLevel = 13;

            // Create a small circle to mark the current location.
            Ellipse myCircle = new Ellipse();
            myCircle.Fill = new SolidColorBrush(Colors.Blue);
            myCircle.Height = 20;
            myCircle.Width = 20;
            myCircle.Opacity = 50;

            MapItemsControl mEl = new MapItemsControl();
            MapIcon ii = new MapIcon();

            /*
            // Create a MapOverlay to contain the circle.
            Map myLocationOverlay = new MapOverlay();
            myLocationOverlay.Content = myCircle;
            myLocationOverlay.PositionOrigin = new Point(0.5, 0.5);
            myLocationOverlay.GeoCoordinate = myGeoCoordinate;*/


        }

        /// <summary>
        /// Open up menu on the bottom ****************************
        /// </summary>
        /*private void menuPanel_Tap(object sender, TappedEventArgs e)
        {
            if (oneThingAtATime == true)
                return;
            if (normalPosition == true)
            {
                move.Y -= 140;
                normalPosition = false;
                menuOpen = true;

                //************** Handle the arrows **************
                // make the up arrows invisible
                upArrowLeft.Opacity = 0;
                upArrowRight.Opacity = 0;
                upArrowLeftlight.Opacity = 0;
                upArrowRightlight.Opacity = 0;
                
                // make the down arrows visible
                downArrowLeft.Opacity = 1;
                downArrowRight.Opacity = 1;
                downArrowLeftlight.Opacity = 1;
                downArrowRightlight.Opacity = 1;
            }
            else if (normalPosition == false)
            {
                move.Y += 140;
                LayoutRoot.Margin = new Thickness(0, -100, 0, -140);
                normalPosition = true;
                menuOpen = false;

                //************** Handle the arrows **************
                // make the up arrows visible
                upArrowLeft.Opacity = 1;
                upArrowRight.Opacity = 1;
                upArrowLeftlight.Opacity = 1;
                upArrowRightlight.Opacity = 1;

                // make the down arrows invisible
                downArrowLeft.Opacity = 0;
                downArrowRight.Opacity = 0;
                downArrowLeftlight.Opacity = 0;
                downArrowRightlight.Opacity = 0;
            }


            
        }*/
 




    }
}
