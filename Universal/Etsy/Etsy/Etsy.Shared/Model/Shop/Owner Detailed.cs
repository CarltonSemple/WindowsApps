using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.Shop
{
    public class Owner_Detailed
    {
        public int? user_profile_id { get; set; }
        public int? user_id { get; set; }
        public string login_name { get; set; }
        public string bio { get; set; }
        public string gender { get; set; }
        public string birth_month { get; set; }
        public string birth_day { get; set; }
        public int? join_tsz { get; set; }
        public string materials { get; set; }
        public int? country_id { get; set; }
        public string region { get; set; }
        public string city { get; set; }
        public object location { get; set; }
        public int? avatar_id { get; set; }
        public int? transaction_buy_count { get; set; }
        public int? transaction_sold_count { get; set; }
        public bool is_seller { get; set; }
        public string image_url_75x75 { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }
}
