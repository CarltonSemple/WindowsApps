using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.User
{
    public class Address
    {
        public long user_address_id { get; set; }
        public int user_id { get; set; }
        public string name { get; set; }
        public string first_line { get; set; }
        public string second_line { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public int country_id { get; set; }
        public string country_name { get; set; }
        public bool is_default_shipping { get; set; }
    }
}
