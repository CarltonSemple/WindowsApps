using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Etsy.Model.ShippingNamespace
{
    public class ShippingHolder
    {
        public int? count { get; set; }
        public ObservableCollection<Shipping> results { get; set; }
        public sParams sparams { get; set; }
        public string type { get; set; }
        public sPagination pagination { get; set; }
    }
}
