using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.Shop
{
    public class Story
    {
        public int shop_id { get; set; }
        public string status { get; set; }
        public string story_headline { get; set; }
        public string story_leading_paragraph { get; set; }
        public string story { get; set; }
        public Related_Links related_links { get; set; }
    }
}
