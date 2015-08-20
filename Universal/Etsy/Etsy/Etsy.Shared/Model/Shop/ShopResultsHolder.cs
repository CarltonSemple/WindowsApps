using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.Shop
{
    public class ShopResultsHolder
    {
        public int count { get; set; }
        public Shop results { get; set; }
        public Params @params { get; set; }
        public string type { get; set; }
        public Pagination pagination { get; set; }
    }
}
