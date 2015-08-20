using Etsy.DataTransfer;
using Etsy.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Etsy
{
    public sealed partial class ItemInListView : UserControl
    {
        Listing currentListing;
        public ItemInListView()
        {
            this.InitializeComponent();
        }

        private void priceBlock_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock pricer = (TextBlock)sender;
            Listing listing = pricer.DataContext as Listing;
            if(listing.state == "removed" || listing.state == "sold_out" || listing.state == "expired")
            {
                pricer.Foreground = new SolidColorBrush(Colors.Red);
                pricer.Text = listing.state;
            }
        }

        /// <summary>
        /// Show the correct heart
        /// Mark/unmark as a favorite, and add/remove to the favorites
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void favoriteButton_Click(object sender, RoutedEventArgs e)
        {
            heart_filled.Visibility = Visibility.Visible;            

            if(currentListing.isFavorite == false)
            {
                await FavoritesAccess.AddFavoriteListing(currentListing, App.userID);
                if (currentListing.isFavorite)  // will be changed upon completion of the add favorite function
                    heart_filled.Visibility = Visibility.Visible;
            }
            else // remove from the favorites
            {
                await FavoritesAccess.RemoveFavoriteListing(currentListing, App.userID);
                if (!currentListing.isFavorite)
                    heart_filled.Visibility = Visibility.Collapsed;
            }
        }

        private void heart_filled_Loaded(object sender, RoutedEventArgs e)
        {
            Listing cur = heart_empty.DataContext as Listing;
            currentListing = cur;
            if (cur.isFavorite)
                heart_filled.Visibility = Visibility.Visible;
            else
                heart_filled.Visibility = Visibility.Collapsed;
        }


    }
}
