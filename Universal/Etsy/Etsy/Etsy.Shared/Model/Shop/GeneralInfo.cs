using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Etsy.Model.Shop.About;

namespace Etsy.Model.Shop
{
    public class GeneralInfo
    {
        public int shop_id { get; set; }
        public string shop_name { get; set; }
        public int? user_id { get; set; }
        public int creation_tsz { get; set; }
        public string title { get; set; }
        public object announcement { get; set; }
        public string currency_code { get; set; }
        public bool? is_vacation { get; set; }
        public string vacation_message { get; set; }
        public object sale_message { get; set; }
        public object digital_sale_message { get; set; }
        public int? last_updated_tsz { get; set; }
        public int? listing_active_count { get; set; }
        public string login_name { get; set; }
        public bool? accepts_custom_requests { get; set; }
        public object policy_welcome { get; set; }
        public object policy_payment { get; set; }
        public object policy_shipping { get; set; }
        public object policy_refunds { get; set; }
        public object policy_additional { get; set; }
        public object policy_seller_info { get; set; }
        public int? policy_updated_tsz { get; set; }
        public object vacation_autoreply { get; set; }
        public string url { get; set; }
        public string image_url_760x100 { get; set; }
        public int? num_favorers { get; set; }
        public ObservableCollection<string> languages { get; set; }

        public ShopAbout about { get; set; }        // ShopAbout object, obtained on the shop page
        public bool isFavorite { get; set; }


        public ObservableCollection<User_Feedback.TransactionFeedback> userFeedback { get; set; }
        private User_Feedback.Feedback feedbackContainer { get; set; }

        public PaymentTemplate payment_template { get; set; }   // downloaded once listing detail page is visited


        /******************* Functions ******************/
        string baseURL;
        HttpClient client = new HttpClient();

        /// <summary>
        /// Get the payment options
        /// </summary>
        /// <returns></returns>
        public async Task getPaymentTemplate()
        {
            baseURL = string.Format("{0}/shops/{1}/payment_templates?api_key={2}", App.baseURL, shop_id, App.key);
            PaymentTemplateHolder pHolder = new PaymentTemplateHolder();
            string errorMessage;

            try
            {
                var jsonStream = await client.GetStreamAsync(baseURL);

                using (StreamReader reader = new StreamReader(jsonStream))
                {

                    var serializer = new DataContractJsonSerializer(typeof(PaymentTemplateHolder));

                    pHolder = (PaymentTemplateHolder)serializer.ReadObject(jsonStream);
                    payment_template = pHolder.results[0];    // set the member payment template. only one is present in the list
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return;
        }


        /// <summary>
        /// Get the user reviews
        /// </summary>
        /// <returns></returns>
        public async Task getUserFeedback()
        {
            string errorMessage;

            if (App.logged_in == false)
                return;

            //baseURL = string.Format("{0}/users/{1}/feedback/as-seller?api_key={2}", App.baseURL, user_id, App.key);
            baseURL = string.Format("{0}/users/{1}/feedback/as-seller?includes=Listing&api_key={2}", App.baseURL, user_id, App.key);
            User_Feedback.Feedback feedB = new User_Feedback.Feedback();
            

            try
            {
                var jsonStream = await client.GetStreamAsync(baseURL);

                using (StreamReader reader = new StreamReader(jsonStream))
                {

                    var serializer = new DataContractJsonSerializer(typeof(User_Feedback.Feedback));

                    feedB = (User_Feedback.Feedback)serializer.ReadObject(jsonStream);
                    
                    userFeedback = feedB.results;   // set the publicly accessible member list of feedback

                    foreach(var feedback in userFeedback)           // make aspects of the associated listing available for data binding
                    {
                        feedback.title = feedback.Listing.title;    // make the title of the associated listing accessible for binding
                    }
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return;
        }
    }
}
