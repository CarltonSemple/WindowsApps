using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.User
{
    public class UserContainer
    {
        public int user_id { get; set; }
        public object favorite_user_id { get; set; }
        public int target_user_id { get; set; }
        public int creation_tsz { get; set; }
        public User TargetUser { get; set; }
    }
}
