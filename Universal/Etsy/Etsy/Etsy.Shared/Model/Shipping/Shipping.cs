using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.ShippingNamespace
{
    public class Shipping
    {
        public object shipping_info_id { get; set; }
        public int? origin_country_id { get; set; }
        public int? destination_country_id { get; set; }
        public string currency_code { get; set; }
        public string primary_cost { get; set; }
        public string secondary_cost { get; set; }
        public int? listing_id { get; set; }
        public object region_id { get; set; }
        public string origin_country_name { get; set; }
        public string destination_country_name { get; set; }
    }
}
