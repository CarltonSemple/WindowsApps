using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Etsy.Model.Cart
{
    public class ShopCart
    {
        public int cart_id { get; set; }
        public string shop_name { get; set; }
        public string message_to_seller { get; set; }
        public int destination_country_id { get; set; }
        public string coupon_code { get; set; }
        public string currency_code { get; set; }
        public string total { get; set; }
        public string subtotal { get; set; }
        public string shipping_cost { get; set; }
        public string tax_cost { get; set; }
        public string discount_amount { get; set; }
        public string shipping_discount_amount { get; set; }
        public string tax_discount_amount { get; set; }
        public string url { get; set; }
        public ObservableCollection<CartListing> listings { get; set; }
        public object shipping_option { get; set; }
    }

    /// <summary>
    /// Used to easily read the json response when an item is deleted from the cart.
    /// Only one ShopCart will be in the results, which is the remaining ShopCart after
    /// the item was removed from it.
    /// </summary>
    public class ShopCartHolder
    {
        public int count { get; set; }
        public ObservableCollection<ShopCart> results { get; set; }
        public Params @params { get; set; }
        public string type { get; set; }
        public Pagination pagination { get; set; }
    }
}
