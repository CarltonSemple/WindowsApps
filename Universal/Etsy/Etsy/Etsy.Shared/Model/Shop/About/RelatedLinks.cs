using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.Shop.About
{
    public class Link
    {
        public string title { get; set; }
        public string url { get; set; }
    }

    public class RelatedLinks
    {
        public Link link_1 { get; set; }
    }
}
