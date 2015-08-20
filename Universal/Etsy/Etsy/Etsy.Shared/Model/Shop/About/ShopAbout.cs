using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Etsy.Model.Shop.About
{
    public class ShopAbout
    {
        public int shop_id { get; set; }
        public string status { get; set; }
        public string story_headline { get; set; }
        public string story_leading_paragraph { get; set; }
        public string story { get; set; }
        public RelatedLinks related_links { get; set; }
        public ObservableCollection<ShopImage> Images { get; set; }
    }
}
