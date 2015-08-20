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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MyTask.CustomControls
{
    public sealed partial class TaskExpansiveControl : UserControl
    {
        bool condensed = true;

        // transformations
        private TranslateTransform move = new TranslateTransform();
        private TransformGroup transforms = new TransformGroup();

        public TaskExpansiveControl()
        {
            this.InitializeComponent();

            transforms.Children.Add(move);

            titleBlock.RenderTransform = move;
            //backPanel.ManipulationDelta += new ManipulationDeltaEventHandler(backPanel_ManipulationDelta);
            titleBlock.ManipulationDelta += new ManipulationDeltaEventHandler(backPanel_ManipulationDelta);
        }


        private void backPanel_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            move.X += e.Delta.Translation.X;
            move.Y += e.Delta.Translation.Y;
        }


        /// <summary>
        /// Expand/Condense the control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(condensed)
            {
                Height = 201;
                backPanel.Height = Height;
                mainGrid.Height = 200;
                condensed = false;
            }
            else
            {
                Height = 51;
                backPanel.Height = Height;
                mainGrid.Height = 50;
                condensed = true;
            }

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            TaskItem taskToDelete = button.DataContext as TaskItem;

            (App.Current as App).taskModel.DeleteTaskItem(taskToDelete);
        }

        
    }
}
