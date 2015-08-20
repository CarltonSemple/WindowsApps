using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Etsy.Model.User
{
    /// <summary>
    /// Class that contains a user upon sending a get request for a specific user
    /// </summary>
    public class dataFetch
    {
        public int count { get; set; }
        public ObservableCollection<User> results { get; set; }
        public Params @params { get; set; }
        public string type { get; set; }
        public Pagination pagination { get; set; }
    }

    public class Params
    {
        public string user_id { get; set; }
    }

    public class Pagination
    {
    }
}
