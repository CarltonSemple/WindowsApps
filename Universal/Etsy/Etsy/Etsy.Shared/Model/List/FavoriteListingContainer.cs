using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.List
{
    public class FavoriteListingContainer
    {
        public Int64 listing_id { get; set; }
        public Int64 user_id { get; set; }
        public string listing_state { get; set; }
        public Int64 create_date { get; set; }
        public Listing Listing { get; set; }
    }
}
