using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.Shop
{
    public class ShopSection
    {
        public Int64? shop_section_id { get; set; }
        public string title { get; set; }
        public int? rank { get; set; }
        public Int64? user_id { get; set; }
        public int? active_listing_count { get; set; }
    }
}
