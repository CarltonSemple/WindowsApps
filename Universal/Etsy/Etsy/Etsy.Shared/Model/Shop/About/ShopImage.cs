using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.Shop.About
{
    /// <summary>
    /// An image specifically tied to shops, different enough from regular Image class to avoid inheritance
    /// </summary>
    public class ShopImage
    {
        public int shop_id { get; set; }
        public long image_id { get; set; }
        public object caption { get; set; }
        public int rank { get; set; }
        public string url_178x178 { get; set; }
        public string url_545xN { get; set; }
        public string url_760xN { get; set; }
        public string url_fullxfull { get; set; }
    }
}
