using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Windows.Input;

using NewTask.ViewModels;
using NewTask.Model;
using NewTask.Resources;


namespace NewTask
{
    public partial class TaskUserControl : UserControl
    {
        /****** transformation preparations for moving the UI *****/
        private TranslateTransform move = new TranslateTransform();
        private TranslateTransform dmove = new TranslateTransform();
        private ScaleTransform resize = new ScaleTransform();
        private TransformGroup transforms = new TransformGroup();
        private TransformGroup dtransforms = new TransformGroup();

        private Brush transformingBrush = new SolidColorBrush(Colors.Orange);

        public TaskUserControl()
        {
            InitializeComponent();
            //DataContext = App.ViewModel;

            // transforms for delete buttons
            dtransforms.Children.Add(dmove);
            deleteButton.RenderTransform = dmove;


            deleteButton.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(deleteButton_ManipulationStarted);
            deleteButton.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(deleteButton_ManipulationDelta);
            deleteButton.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(deleteButton_ManipulationCompleted);

            
        }

        private void deleteButton_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {
            
        }

        private void deleteButton_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (dmove.X > 260)
            {
                deleteButton.Background = transformingBrush;
            }

            // move it
            dmove.X += e.DeltaManipulation.Translation.X;
        }

        private void deleteButton_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (dmove.X > 260)
            {
                // delete the item
                var button = sender as Button;
                TaskItem tasktodelete = button.DataContext as TaskItem;

                App.ViewModel.DeleteTaskItem(tasktodelete);

/********************************** Update Live Tile **********************************/
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
                            tile.WideContent3 = App.ViewModel.taskList.ElementAt<TaskItem>(2).Details;
                        } break;
                    default:
                        {
                            tile.WideContent1 = App.ViewModel.taskList.ElementAt<TaskItem>(0).Details;
                            tile.WideContent2 = App.ViewModel.taskList.ElementAt<TaskItem>(1).Details;
                            tile.WideContent3 = App.ViewModel.taskList.ElementAt<TaskItem>(2).Details;
                        }
                        break;
                }

                // find the tile object for the application tile that using "Iconic" contains string in it.
                ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("Iconic".ToString()));

                if (TileToFind != null && TileToFind.NavigationUri.ToString().Contains("Iconic"))
                {
                    TileToFind.Update(tile);
                }

            // return the square to its original position/color
                for (double i = dmove.X; i > 0; i = i - 1)
                {
                    dmove.X -= 1;
                }
                deleteButton.Margin = new Thickness(-11, -12, 0, -13);
                SolidColorBrush backColor = new SolidColorBrush((Color)Application.Current.Resources["PhoneAccentColor"]);
                deleteButton.Background = backColor;
            }
            else if ((dmove.X <= 260) && (dmove.X >= 0))
            {
                for (double i = dmove.X; i > 0; i = i-1)
                {
                    dmove.X -= 1;
                }
                deleteButton.Margin = new Thickness(-11, -12, 0, -13);
            }
            else
            {
                for (double i = dmove.X; i < 0; i = i + 1)
                {
                    dmove.X += 1;
                }
            }
        }

        //private void deleteTask(object sender)
        //{
        //    var button = sender as Button;
        //    ItemViewModel task = button.DataContext
        //}


        //private OnButtonClick button_click;

        //public interface OnButtonClick
        //{

        //}
    }
}
