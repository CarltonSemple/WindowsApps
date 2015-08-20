using Etsy.Common;
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
using Etsy.UI_Extras;

namespace Etsy
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        #endregion

        /// <summary>
        /// Change the image quality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (imgSwitch.IsOn)
                Loading._imageQuality = Loading.ImageQuality.high;
            else
                Loading._imageQuality = Loading.ImageQuality.low;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgSwitch_Loaded(object sender, RoutedEventArgs e)
        {
            switch (Loading._imageQuality)
            {
                case Loading.ImageQuality.high:
                    imgSwitch.IsOn = true;
                    break;
                case Loading.ImageQuality.low:
                    imgSwitch.IsOn = false;
                    break;
                default:
                    break;
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            if(this.Frame.CanGoBack)
                this.Frame.GoBack();
        }
    }
}
