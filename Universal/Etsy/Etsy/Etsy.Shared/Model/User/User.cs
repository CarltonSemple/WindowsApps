using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Etsy.Model.User
{
    public class User
    {
        public int user_id { get; set; }
        public string login_name { get; set; }
        public string primary_email { get; set; }
        public int creation_tsz { get; set; }
        public object referred_by_user_id { get; set; }
        public FeedbackInfo feedback_info { get; set; }
        public int awaiting_feedback_count { get; set; }
        public UserProfile Profile { get; set; }
        public List<object> BuyerReceipts { get; set; }
        public List<object> BuyerTransactions { get; set; }
        public List<Address> Addresses { get; set; }
        public ObservableCollection<Shop.GeneralInfo> Shops { get; set; }
        public Shop.GeneralInfo shop { get; set; }
        public Cart.Cart cart { get; set; }                 // The user's cart
    }
}
