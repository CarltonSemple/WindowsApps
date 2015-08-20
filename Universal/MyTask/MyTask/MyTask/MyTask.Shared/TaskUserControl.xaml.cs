using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace MyTask
{
    public sealed partial class TaskUserControl : UserControl
    {
        /****** transformation preparations for moving the UI *****/
        private TranslateTransform move = new TranslateTransform();
        private TranslateTransform dmove = new TranslateTransform();
        private ScaleTransform resize = new ScaleTransform();
        private TransformGroup transforms = new TransformGroup();
        private TransformGroup dtransforms = new TransformGroup();

        private Brush transformingBrush = new SolidColorBrush(Colors.Orange);

        TaskModel taskModel = (App.Current as App).taskModel;

        public TaskUserControl()
        {
            this.InitializeComponent();

            dtransforms.Children.Add(dmove);
            deleteButton.RenderTransform = dmove;


            //deleteButton.ManipulationStarted += new EventHandler<ManipulationStartedEventArgs>(deleteButton_ManipulationStarted);
            //deleteButton.ManipulationDelta += new EventHandler<ManipulationDeltaEventArgs>(deleteButton_ManipulationDelta);
            //deleteButton.ManipulationCompleted += new EventHandler<ManipulationCompletedEventArgs>(deleteButton_ManipulationCompleted);

        }


        private void deleteButton_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
        {

        }

        /*
        private void deleteButton_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (dmove.X > 260)
            {
                deleteButton.Background = transformingBrush;
            }

            // move it
            dmove.X += e.DeltaManipulation.Translation.X;
        }
        */


        private void deleteButton_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (dmove.X > 260)
            {
                // delete the item
                var button = sender as Button;
                TaskItem tasktodelete = button.DataContext as TaskItem;

                taskModel.DeleteTaskItem(tasktodelete);

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
                for (double i = dmove.X; i > 0; i = i - 1)
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
            
    }
}
