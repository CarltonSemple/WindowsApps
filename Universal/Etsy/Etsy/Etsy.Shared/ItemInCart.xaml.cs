using Etsy.DataTransfer;
using Etsy.Model.Cart;
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
    /// <summary>
    /// An item's appearance when inside the cart
    /// </summary>
    public sealed partial class ItemInCart : UserControl
    {
        public ItemInCart()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Remove the item from the cart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button deleter = sender as Button;

            CartListing itemToDelete = deleter.DataContext as CartListing;

            await CartAccess.deleteFromCart(App.user.cart, itemToDelete);      // DELETE
        }
    }
}
