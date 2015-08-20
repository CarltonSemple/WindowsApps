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
    public sealed partial class ItemInFavoritesList_Phone : UserControl
    {
        Listing currentListing;
        public ItemInFavoritesList_Phone()
        {
            this.InitializeComponent();
        }

        private void priceBlock_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock pricer = (TextBlock)sender;
            Listing listing = pricer.DataContext as Listing;
            try
            {
                if (listing.state == "removed" || listing.state == "sold_out" || listing.state == "expired")
                {
                    pricer.Foreground = new SolidColorBrush(Colors.Red);
                    pricer.Text = listing.state;
                }
            }
            catch
            {
                pricer.Text = "uh oh";
            }
        }

        /// <summary>
        /// Mark/unmark as a favorite, and add/remove to the favorites
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void favoriteButton_Click(object sender, RoutedEventArgs e)
        {
            currentListing = favoriteButton.DataContext as Listing;
            await FavoritesAccess.RemoveFavoriteListing(currentListing, App.userID);

            // Remove from the collection locally, to avoid refreshing via a GET call
            for (int a = 0; a < FavoritesAccess.favoriteListings.Count; a++)
            {
                Listing li = FavoritesAccess.favoriteListings[a];
                if (li.listing_id == currentListing.listing_id)
                {
                    FavoritesAccess.favoriteListings.Remove(li);
                    break;
                }
            }
        }

        private void heart_filled_Loaded(object sender, RoutedEventArgs e)
        {
            heart_filled.Visibility = Visibility.Visible;
        }
    }
}
