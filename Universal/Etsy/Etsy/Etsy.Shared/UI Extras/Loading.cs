using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Etsy.DataBinding;
using Windows.Networking.Connectivity;
using Etsy.Model;
using Etsy.DataTransfer;

namespace Etsy.UI_Extras
{
    /// <summary>
    /// Give the user feedback to know that the app is working when items are being loaded
    /// </summary>
    public class Loading
    {
        public static ImageQuality _imageQuality = ImageQuality.high;
        public static bool loadError = false;
        public static string errorMessage = "";

        /// <summary>
        /// When the given collection is empty, make the progress ring visible
        /// If it's null, return false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="ring"></param>
        public static bool ControlProgressRing<T>(ObservableCollection<T> collection, ProgressRing ring)
        {
            if (collection != null)
            {
                if (collection.Count == 0)
                {
                    ring.IsActive = true;
                    ring.Visibility = Visibility.Visible;
                    return false;
                }
                else
                {
                    ring.IsActive = false;
                    ring.Visibility = Visibility.Collapsed;
                    return true;
                }
            }
            else
            {
                ring.IsActive = true;
                ring.Visibility = Visibility.Visible;
            }

            return false;
        }

        /// <summary>
        /// When the given collection is empty, make the progress ring visible
        /// If it's null or empty, return false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="ring"></param>
        public static bool ControlProgressRing(IncrementalSource<ListGetter, Listing> collection, ProgressRing ring)
        {
            if (collection != null)
            {
                if (collection.Count == 0)
                {
                    ring.IsActive = true;
                    ring.Visibility = Visibility.Visible;
                    return false;
                }
                else
                {
                    ring.IsActive = false;
                    ring.Visibility = Visibility.Collapsed;
                    return true;
                }       
            }
            else
            {
                ring.IsActive = true;
                ring.Visibility = Visibility.Visible;
            }

            return false;
        }

        /// <summary>
        /// When the given collection is empty, make the progress ring visible
        /// If it's null or empty, return false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="ring"></param>
        public static bool ControlProgressRing(ItemCollection collection, ProgressRing ring)
        {
            if (collection != null)
            {
                if (collection.Count == 0)
                {
                    ring.IsActive = true;
                    ring.Visibility = Visibility.Visible;
                    return false;
                }
                else
                {
                    ring.IsActive = false;
                    ring.Visibility = Visibility.Collapsed;
                    return true;
                }
            }
            else
            {
                ring.IsActive = true;
                ring.Visibility = Visibility.Visible;
            }

            return false;
        }

        /// <summary>
        /// Change the image quality and save it for later reference
        /// </summary>
        /// <param name="quality"></param>
        public static async void SetImageQuality(ImageQuality quality)
        {
            _imageQuality = quality;

            // Save
            if (_imageQuality == ImageQuality.high)
                await FileIO.EncryptAndSave("high", "image_quality");
            else
                await FileIO.EncryptAndSave("low", "image_quality"); ;
        }

        public enum ImageQuality
        {
            high,
            low
        }

        /// <summary>
        /// Return true if there's a loading error
        /// Send the error message to the given textblock
        /// Reset the loaderror value to false
        /// </summary>
        /// <param name="tblock"></param>
        /// <returns></returns>
        public static bool Notify_loadingError(TextBlock tblock)
        {
            if (!loadError)
                return false;

            tblock.Text = errorMessage;
            loadError = false;
            return true;
        }

        /// <summary>
        /// Pass a "null" error message to the given textblock and return true if the testobject is null
        /// </summary>
        /// <param name="tblock"></param>
        public static bool Null_Error<T>(T testobject, TextBlock tblock)
        {
            if (testobject == null)
            {
                if (tblock != null)
                    tblock.Text = "Uh oh, looks like something important wasn't loaded.  Please go back and try again";
                return true;
            }
            return false;
        }
    }
}
