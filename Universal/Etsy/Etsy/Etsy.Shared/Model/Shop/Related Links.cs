using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.Shop
{
    public class Link
    {
        public string title { get; set; }
        public string url { get; set; }
    }

    public class Related_Links
    {
        public Link link1 { get; set; }
        public Link link2 { get; set; }
        public Link link3 { get; set; }
    }
}
