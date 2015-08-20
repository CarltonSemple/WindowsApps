using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Etsy.Model.Variations
{
    public class Variations
    {
        public Variations()
        {
            count = -1;
            results = new ObservableCollection<VariationsProperty>();
        }

        public int count { get; set; }
        public ObservableCollection<VariationsProperty> results { get; set; }
        public Params @params { get; set; }
        public string type { get; set; }
        public Pagination pagination { get; set; }
    }

    public class Params
    {
        public string listing_id { get; set; }
    }

    public class Pagination
    {
    }
}
