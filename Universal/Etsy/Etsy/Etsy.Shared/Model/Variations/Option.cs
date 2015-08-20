using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.Variations
{
    public class Option
    {
        public Nullable<Int64> value_id { get; set; }
        public string value { get; set; }               // the readable value
        public string formatted_value { get; set; }
        public Nullable<bool> is_available { get; set; }
        public Nullable<double> price_diff { get; set; }
        public Nullable<double> price { get; set; }
    }
}
