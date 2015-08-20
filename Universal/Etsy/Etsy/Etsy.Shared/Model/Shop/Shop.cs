using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Etsy.Model.Shop
{
    public class Shop
    {
        public GeneralInfo general_info { get; set; }
        public Story story { get; set; }
        public Owner_Basics owner_basics { get; set; }
        public Owner_Detailed owner_detailed { get; set; }
        public RatingsAggregate ratings_aggregate { get; set; }
        public PaymentTemplate payment_template { get; set; }   // downloaded once listing detail page is visited
        public int ratings_count { get; set; }


        private string baseURL { get; set; }
        HttpClient client = new HttpClient();

        /**************** Functions ****************/

        
    }
}
