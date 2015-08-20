using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Etsy.Model.User
{
    public class UserDeserializer
    {
        public int count { get; set; }
        public ObservableCollection<UserContainer> results { get; set; }
        public ObservableCollection<User> users { get; set; }
        public Params @params { get; set; }
        public string type { get; set; }
        public Pagination pagination { get; set; }

        public void simplify()
        {
            users = new ObservableCollection<User>();
            try
            {
                foreach (var u in results)
                {
                    u.TargetUser.shop = u.TargetUser.Shops[0];
                    users.Add(u.TargetUser);
                }
            }
            catch
            {}
        }
    }
}
