using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.Cart
{
    public class CartListing
    {
        public Int64 listing_id { get; set; }
        public int purchase_quantity { get; set; }
        public string purchase_state { get; set; }
        public string title { get; set; }
        public string price { get; set; }
        public string currency_code { get; set; }
        public string image_url_75x75 { get; set; }
        public bool is_digital { get; set; }
        public string file_data { get; set; }
        public Int64 listing_customization_id { get; set; }
        public bool has_variations { get; set; }
        public bool variations_available { get; set; }
        public List<object> selected_variations { get; set; }   // MUST fill in once the variation class is defined
    }


}
