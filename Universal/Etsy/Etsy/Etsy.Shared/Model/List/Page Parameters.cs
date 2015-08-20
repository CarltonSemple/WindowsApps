using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.List
{
    public class Page_Parameters
    {
        public string limit { get; set; }
        public int? offset { get; set; }
        public string page { get; set; }
        public string keywords { get; set; }
        public string sort_on { get; set; }
        public string sort_order { get; set; }
        public object min_price { get; set; }
        public object max_price { get; set; }
        public object color { get; set; }
        public int? color_accuracy { get; set; }
        public object tags { get; set; }
        public object category { get; set; }
        public object location { get; set; }
        public object lat { get; set; }
        public object lon { get; set; }
        public object region { get; set; }
        public string geo_level { get; set; }
        public string accepts_gift_cards { get; set; }
        public string translate_keywords { get; set; }
    }
}
