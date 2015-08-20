using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Etsy.Model.Variations
{
    public class VariationsProperty
    {
        public VariationsProperty()
        {
            selected_option_id = -1;
        }

        public int property_id { get; set; }
        public string formatted_name { get; set; }
        public ObservableCollection<Option> options { get; set; }

        public Int64? selected_option_id { get; set; }       // the option that the user selects
    }
}
