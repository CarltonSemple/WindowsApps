using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using NewTask.Resources;
using NewTask.ViewModels;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Threading;
using System.Threading.Tasks;
using NewTask.Model;

using Microsoft.Phone.Tasks;

namespace NewTask
{
    public partial class MainPage : PhoneApplicationPage
    {
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


        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // testing.... // makeTaskToggle.IsChecked = false;

            //TitleBlock.Text = "why";

            // Set the data context of the LongListSelector control to the sample data
            this.DataContext = App.ViewModel;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();

            transforms.Children.Add(move);
            transforms.Children.Add(resize);
            //newTaskForm.RenderTransform = transforms;
            LayoutRoot.RenderTransform = transforms;

            newTaskForm.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(newTaskForm_ManipulationStarted);
            newTaskForm.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(newTaskForm_ManipulationDelta);
            newTaskForm.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(newTaskForm_ManipulationCompleted);

            Yorigin = move.Y;

            // transforms for delete buttons
            dtransforms.Children.Add(dmove);

            // transforms for menu options
            menuTransforms.Children.Add(mMove);
            menuOptions.RenderTransform = menuTransforms;
        }
        

        private void newTaskForm_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
        }

        private void newTaskForm_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
        }

        private void newTaskForm_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
        }



        protected override async void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            //App.ViewModel.SaveChangesToDB();
            //await App.ViewModel.writeData();
        }

        // Load data for the ViewModel Items
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            //await App.ViewModel.loadData();

            //if (!App.ViewModel.IsDataLoaded)
            //{
            //    App.ViewModel.LoadData();
            //}
        }

        // Handle selection changed on LongListSelector
        private void MainLongListSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
            NavigationService.Navigate(new Uri("/DetailsPage.xaml?selectedItem=" + ((MainLongListSelector.SelectedItem as ItemViewModel).Id - 1).ToString(), UriKind.Relative)); 
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
            makeTaskToggle.Content = AppResources.SaveTask;
            normalPosition = false;
            taskBox.Focus();

            // Disable the bottom menu button, for 16:9 screens
            oneThingAtATime = true;
        }

        private void makeTaskToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            move.Y -= 100;
            makeTaskToggle.Content = AppResources.CreateTask;
            normalPosition = true;

            // reenable the bottom menu button, for 16:9 screens...
            oneThingAtATime = false;

            // Save the text in the textbox, and then reset the textbox
            string taskstring = taskBox.Text;
            taskBox.Text = "";

            TaskItem newItem = new TaskItem
            {
                dateTime = DateTime.UtcNow.ToString(),
                Details = taskstring
            };

            if (backButtonClick == false)
            {
                if (taskstring != "")
                    App.ViewModel.AddTaskItem(newItem);

// ********************************** Update Live Tile *********************************
////////////////////////////////////////////////
////////////////////////////////////////////////
////////////////////////////////////////////////
////////////////////////////////////////////////
                IconicTileData tile = new IconicTileData();
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

            }
            backButtonClick = false;

        }

        // Change the behavior of the back button
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            // just move the position back to normal instead of exiting
            if ((normalPosition == false) && (menuOpen == false))
            {
                //move.Y = Yorigin;
                // The above line refuses to work properly, but the following does
                LayoutRoot.Margin = new Thickness(0, -100, 0, -140);
                makeTaskToggle.Content = AppResources.CreateTask;
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

                /************** Handle the arrows **************/
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
                base.OnBackKeyPress(e);
        }

        // Using the enter key to save the task
        private void taskBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.Equals(Key.Enter))
            {
                LayoutRoot.Margin = new Thickness(0, -100, 0, -140);
                makeTaskToggle.Content = AppResources.CreateTask;
                normalPosition = true;
                makeTaskToggle.IsChecked = false;
                this.Focus();
            }
        }



        // these 3 functions are not used... i don't think
        private void readText(object sender, GestureEventArgs e)
        {
            TextBlock tBlock = e.OriginalSource as TextBlock;
            string tempString = tBlock.Text;
            //string tempString = ((ItemViewModel)tBlock.DataContext).LineOne;
            //(e.OriginalSource as Button).Content = tempString;

            // loop it all
            int length = tempString.Length;
            for (int i = 0; i < length; i++)
            {
                tempString = tempString.Substring(1, tempString.Length-1);
                TitleBlock.Text = tempString;
                (e.OriginalSource as TextBlock).Text = tempString;
                //((ItemViewModel)tBlock.DataContext).LineOne = tempString;
                //await Task.Delay(500);
                Focus();
                
            }
            int oh = 0;
            (e.OriginalSource as TextBlock).Text = tBlock.Text;

            // remove first character from string to produce scrolling effect
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).RenderTransform = dtransforms;
            double x = dmove.X;
            if (dmove.X >= 100)
            {
                (sender as Button).Background = transformingBrush;
            }
        }

        private void deleteButton_Hold(object sender, GestureEventArgs e)
        {
            (sender as Button).RenderTransform = dtransforms;
            double x = dmove.X;
            if (dmove.X >= 100)
            {
                (sender as Button).Background = transformingBrush;
            }
        }




        bool menuOpen = false;

        /// <summary>
        /// Open up menu on the bottom ****************************
        /// </summary>
        private void menuPanel_Tap(object sender, GestureEventArgs e)
        {
            if (oneThingAtATime == true)
                return;
            if (normalPosition == true)
            {
                move.Y -= 140;
                normalPosition = false;
                menuOpen = true;

                /************** Handle the arrows **************/
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

                /************** Handle the arrows **************/
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


            
        }

        
        /*************** Share Option ***************/
        private void share_Tap(object sender, GestureEventArgs e)
        {
            ShareLinkTask shareLinkTask = new ShareLinkTask();

            shareLinkTask.Title = AppResources.LinkTitle;
            shareLinkTask.LinkUri = new Uri("http://windowsphone.com/s?appId=94db675a-1c4d-4dca-a2a3-519fa6ac60a6", UriKind.Absolute);
            shareLinkTask.Message = AppResources.LinkToApp;

            shareLinkTask.Show();
        }

        /********************************** Store Functions **********************************/

        // Show other apps by iTchyBandit
        private void searchApps(object sender, GestureEventArgs e)
        {
            MarketplaceSearchTask marketplaceSearchTask = new MarketplaceSearchTask();

            marketplaceSearchTask.SearchTerms = "iTchyBandit";

            marketplaceSearchTask.Show();
        }

        // prompt to buy
        private void purchase(object sender, GestureEventArgs e)
        {
            MessageBox.Show(AppResources.PurchasePrompt);

            MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask();

            marketplaceDetailTask.ContentIdentifier = "94db675a-1c4d-4dca-a2a3-519fa6ac60a6";
            marketplaceDetailTask.ContentType = MarketplaceContentType.Applications;

            marketplaceDetailTask.Show();
        }

        // Option to Review/Like
        private void review(object sender, GestureEventArgs e)
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();

            marketplaceReviewTask.Show();
        }

        /************************* Create Live Tile *************************/
        private void pinTileButton_Click(object sender, RoutedEventArgs e)
        {
            IconicTileData tile = new IconicTileData();
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
                    }break;
                case 3:
                    {
                        tile.WideContent1 = App.ViewModel.taskList.ElementAt<TaskItem>(0).Details;
                        tile.WideContent2 = App.ViewModel.taskList.ElementAt<TaskItem>(1).Details;
                        tile.WideContent3 = App.ViewModel.taskList.ElementAt<TaskItem>(App.ViewModel.taskList.Count()-1).Details;
                    }break;
                default:
                    {
                        tile.WideContent1 = App.ViewModel.taskList.ElementAt<TaskItem>(0).Details;
                        tile.WideContent2 = App.ViewModel.taskList.ElementAt<TaskItem>(1).Details;
                        tile.WideContent3 = App.ViewModel.taskList.ElementAt<TaskItem>(App.ViewModel.taskList.Count()-1).Details;
                    }
                    break;
            }
         
            // find the tile object for the application tile that using "Iconic" contains string in it.
            ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("Iconic".ToString()));

            if (TileToFind != null && TileToFind.NavigationUri.ToString().Contains("Iconic"))
            {
                //TileToFind.Delete();
                TileToFind.Update(tile);
                //ShellTile.Create(new Uri("/MainPage.xaml?id=Iconic", UriKind.Relative), oIcontile, true);
            }
            else
            {
                ShellTile.Create(new Uri("/MainPage.xaml?id=Iconic", UriKind.Relative), tile, true);
            }
        }


    }
}