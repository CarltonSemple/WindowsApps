using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Etsy.Model.Cart
{
    /// <summary>
    /// Contains shop-specific carts, which contain listings
    /// </summary>
    public class Cart
    {
        public int count { get; set; }
        public ObservableCollection<ShopCart> results { get; set; }
        public Params @params { get; set; }
        public string type { get; set; }
        public Pagination pagination { get; set; }
    }

    public class Params
    {
        public string user_id { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public object page { get; set; }
    }

    public class Pagination
    {
        public int effective_limit { get; set; }
        public int effective_offset { get; set; }
        public object next_offset { get; set; }
        public int effective_page { get; set; }
        public object next_page { get; set; }
    }
}
