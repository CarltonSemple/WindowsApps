using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.Shop
{
    public class Owner_Basics
    {
        public int user_id { get; set; }
        public string login_name { get; set; }
        public int creation_tsz { get; set; }
        public object referred_by_user_id { get; set; }
        public FeedbackInfo feedback_info { get; set; }
    }
}
