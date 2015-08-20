using Etsy.DataTransfer;
using Etsy.Model.User;
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

namespace Etsy
{
    public sealed partial class ShopFavorite_Phone : UserControl
    {
        User user;
        public ShopFavorite_Phone()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Remove from favorites
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void favoriteButton_Click(object sender, RoutedEventArgs e)
        {
            user = favoriteButton.DataContext as User;
            await FavoritesAccess.RemoveFavoriteUser(user.shop);

            // Remove from the local collection
            for(int i = 0; i < FavoritesAccess.favoriteUsers.Count; i++)
            {
                var u = FavoritesAccess.favoriteUsers[i];
                if (u.user_id == user.user_id)
                {
                    FavoritesAccess.favoriteUsers.Remove(user);
                    break;
                }
            }
        }
    }
}
