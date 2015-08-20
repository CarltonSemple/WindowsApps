using System;
using System.Collections.Generic;
using System.Text;
using Etsy.Model;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Runtime.Serialization.Json;
using Etsy.Model.List;
using Etsy.Model.Shop;
using Windows.Data.Json;
using Newtonsoft.Json;
using Etsy.Model.ShippingNamespace;
using Windows.Security.Authentication.Web;

using Etsy.Util;
using Etsy.DataBinding;
using Etsy.Model.Variations;
using Etsy.Model.Browse;

namespace Etsy.DataTransfer
{
    /// <summary>
    /// List of Listings. Gets results with paging info
    /// </summary>
    public class DataGET
    {
        public string baseURL;
        HttpClient client = new HttpClient();
        public static string errorMessage { get; set; }

        public DataGET()
        {

        }

        
        /// <summary>
        /// Obtain temporary credentials including a OAuth token and token secret
        /// Returns the login url if successful, or an error message if not
        /// </summary>
        /// <returns></returns>
        public async Task<string> getTemporaryCredentials(bool callback)
        {
            //OAuthBase oauth = new OAuthBase();                  // authentication helper
            OAuthBaseUpdated oauth = new OAuthBaseUpdated();
            Utility util = new Utility();                       // contains helper functions

            baseURL = string.Format("{0}/oauth/request_token?scope=listings_r transactions_r cart_rw feedback_r profile_r profile_w email_r address_r address_w favorites_rw", App.baseURL);    // base url with permissions

            string nonce = oauth.GenerateNonce();                                                           // nonce
            string timeStamp = oauth.GenerateTimeStamp();                                                   // time stamp

            string callback_url = "https://localhost";   //Uri.EscapeDataString("http://localhost");
            if (!callback)
                callback_url = "";

            string parametersBase = "&oauth_consumer_key=" + App.key
                                    + "&oauth_nonce=" + nonce
                                    + "&oauth_timestamp=" + timeStamp
                                    + "&oauth_signature_method=HMAC-SHA1"
                                    + "&oauth_callback=" + callback_url // doesn't get used
                                    + "&oauth_version=1.0";
            if(!callback)
            {
                parametersBase = "&oauth_consumer_key=" + App.key
                                    + "&oauth_nonce=" + nonce
                                    + "&oauth_timestamp=" + timeStamp
                                    + "&oauth_signature_method=HMAC-SHA1"
                                    + "&oauth_version=1.0";
            }

            string signature = oauth.GenerateSignature(                                                     // Signature
                                                        new Uri(baseURL),
                                                        callback_url,
                                                        App.key,
                                                        App.sharedSecret,
                                                        "",
                                                        "",
                                                        "GET",
                                                        timeStamp,
                                                        "",
                                                        nonce,
                                                        out baseURL,
                                                        out parametersBase);


            baseURL = string.Format("{0}?{1}&oauth_signature={2}", baseURL,
                                                                    parametersBase,
                                                                    oauth.UrlEncode(signature));            // final URL

            try
            {
                var ree = await client.GetStringAsync(new Uri(baseURL));
                string response = System.Net.WebUtility.UrlDecode(ree);                                     // Decode response from URL format to string

                // Make use of the response. Get token and token secret
                util.parseOAuthResponse(response,
                                        out App.login_url,
                                        out App.oauth_token,
                                        out App.oauth_token_secret,
                                        out App.oauth_callback_confirmed);
            }
            catch (Exception e)
            {
                string mes = "ERROR: " + e.Message;
                return mes;
            }


            return App.login_url;
        }

        /// <summary>
        /// Request permanent OAuth token credentials
        /// Sign the request with the OAuth token, OAuth token_secret, and OAuth verifier
        /// </summary>
        /// <param name="oauth_verifier"></param>
        /// <returns></returns>
        public async Task<bool> getAccessToken(string oauth_verifier)
        {
            OAuthBaseUpdated oauth_new = new OAuthBaseUpdated();    // authentication helper
            Utility util = new Utility();                           // contains helper functions

            baseURL = string.Format("{0}/oauth/access_token", App.baseURL);    // base url with permissions

            string nonce = oauth_new.GenerateNonce();                                                           // nonce
            string timeStamp = oauth_new.GenerateTimeStamp();                                                   // time stamp

            string parametersBase =   "&oauth_consumer_key=" + App.key
                                    + "&oauth_token=" + App.oauth_token
                                    + "&oauth_nonce=" + nonce
                                    + "&oauth_timestamp=" + timeStamp
                                    + "&oauth_signature_method=HMAC-SHA1"
                                    + "&oauth_verifier=" + oauth_verifier
                                    + "&oauth_version=1.0";

            string signature = oauth_new.GenerateSignature(new Uri(baseURL), "https://localhost", App.key, App.sharedSecret, App.oauth_token, App.oauth_token_secret, "GET", timeStamp, oauth_verifier, nonce, out baseURL, out parametersBase);

            baseURL = string.Format("{0}?{1}&oauth_signature={2}", baseURL,
                                                                    parametersBase,
                                                                    oauth_new.UrlEncode(signature));            // final URL

            try
            {
                var ree = await client.GetStringAsync(new Uri(baseURL));
                string response = System.Net.WebUtility.UrlDecode(ree);                                     // Decode response from URL format to string

                // Make use of the response. Get permanent access token and access token secret
                util.parseAccessTokenResponse(  response,
                                                out App.access_token,
                                                out App.access_token_secret);

                // Write the access token and access token secret to storage
                await FileIO.EncryptAndSave(App.access_token, "access_token");                              // encrypt & save to storage
                await FileIO.EncryptAndSave(App.access_token_secret, "access_token_secret");
            }
            catch (Exception e)
            {
                string mes = e.Message;
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// Add OAuth 1.0 authentication to the request
        /// </summary>
        /// <param name="baseURL"></param>
        /// <returns></returns>
        public static string addAuthentication(string baseURL, List<Parameter> parameters)      // "..." is the value, which must be URL encoded
        {
            OAuthBaseUpdated oauth_new = new OAuthBaseUpdated();    // authentication helper

            string nonce = oauth_new.GenerateNonce();                                                           // nonce
            string timeStamp = oauth_new.GenerateTimeStamp();                                                   // time stamp

            string parametersBase =   "&oauth_consumer_key=" + App.key
                                    + "&oauth_token=" + App.access_token
                                    + "&oauth_nonce=" + nonce
                                    + "&oauth_timestamp=" + timeStamp
                                    + "&oauth_signature_method=HMAC-SHA1"
                                    + "&oauth_version=1.0";

            string uriString = baseURL + "?";           // build uri that must be placed in signature
            for(int i = 0; i < parameters.Count; i++)
            {
                uriString += parameters[i].key + "=" + Uri.EscapeDataString(parameters[i].value);   // must URL encode parameterValue
                if (i < parameters.Count - 1)
                    uriString += "&";
            }
            string signature = oauth_new.GenerateSignature(new Uri(uriString),    // must URL encode parameterValue
                                                            "", App.key, App.sharedSecret, App.access_token, App.access_token_secret, "GET", timeStamp, "", nonce, out baseURL, out parametersBase);

            baseURL = string.Format("{0}?{1}&oauth_signature={2}",  baseURL,
                                                                    parametersBase,
                                                                    oauth_new.UrlEncode(signature));            // final URL

            return baseURL;
        }

        /// <summary>
        /// Get the segments for browsing.
        /// Don't require authentication
        /// </summary>
        /// <returns>Observable Collection of Browse Sections</returns>
        public static async Task<ObservableCollection<BrowseSegment>> findBrowseSegments(string path)
        {
            string baseURL = "", errorMessage = "";
            HttpClient client = new HttpClient();

            List<Parameter> parameters = new List<Parameter>();
            
            if(path != "")
                parameters.Add(new Parameter("path", path));    // path (optional)
            //else
            //    parameters.Add(new Parameter("Includes", ));
            
            if (App.logged_in == false)
            {
                if (path != "")
                    baseURL = string.Format("{0}/segments/posters?{1}&api_key={2}", App.baseURL, path, App.key);
            }
            else
            {
                baseURL = string.Format("{0}/segments/posters", App.baseURL);  // Load list 
                baseURL = DataGET.addAuthentication(baseURL, parameters);
            }

            ObservableCollection<BrowseSegment> sections = new ObservableCollection<BrowseSegment>();

            try
            {
                var jsonStream = await client.GetStreamAsync(baseURL);
                //var jsonString = await client.GetStringAsync(baseURL);

                using(StreamReader reader = new StreamReader(jsonStream))
                {
                    // Deserialize
                    var serializer = new DataContractJsonSerializer(typeof(SectionContainer));
                    //SectionContainer capturer = serializer.des
                    SectionContainer capturer = (SectionContainer)serializer.ReadObject(jsonStream);
                    sections = capturer.results;
                }
            }
            catch(Exception e)
            {
                errorMessage = e.Message;
                sections = new ObservableCollection<BrowseSegment>();   // return an empty collection if something goes wrong
            }

            
            return sections;
        }

        /// <summary>
        /// Get the segments for browsing along with their sub-segments
        /// Don't require authentication
        /// </summary>
        /// <returns>Observable Collection of Browse Sections</returns>
        public static async Task<ObservableCollection<BrowseSegment>> getBrowseSegments_Full(string path)
        {
            string baseURL = "", errorMessage = "";
            HttpClient client = new HttpClient();

            List<Parameter> parameters = new List<Parameter>();

            if (path != "")
                parameters.Add(new Parameter("path", path));    // path (optional)
            //else
            //    parameters.Add(new Parameter("Includes", ));

            if (App.logged_in == false)
            {
                if (path != "")
                    baseURL = string.Format("{0}/segments/posters?{1}&api_key={2}", App.baseURL, path, App.key);
            }
            else
            {
                baseURL = string.Format("{0}/segments/posters", App.baseURL);  // Load list 
                baseURL = DataGET.addAuthentication(baseURL, parameters);
            }

            ObservableCollection<BrowseSegment> sections = new ObservableCollection<BrowseSegment>();

            try
            {
                var jsonStream = await client.GetStreamAsync(baseURL);

                using (StreamReader reader = new StreamReader(jsonStream))
                {
                    // Deserialize
                    var serializer = new DataContractJsonSerializer(typeof(SectionContainer));
                    //SectionContainer capturer = serializer.des
                    SectionContainer capturer = (SectionContainer)serializer.ReadObject(jsonStream);
                    sections = capturer.results;
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            // Load the sub sections
            foreach (var section in sections)
            {
                section.sub_sections = await findBrowseSegments(section.path);
            }

            return sections;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shop_id"></param>
        /// <param name="shop_section_id"></param>
        /// <param name="desiredList"></param>
        /// <param name="searchTerms"></param>
        /// <param name="Parameters"></param>
        /// <param name="country_shipTo_id"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static async Task<ObservableCollection<Listing>> getListings(int? shop_id,
                                                                            Int64? shop_section_id,
                                                                            string desiredList,
                                                                            string searchTerms,
                                                                            List<Parameter> Parameters,
                                                                            int? country_shipTo_id,
                                                                            int? pageIndex,
                                                                            int? pageSize)
        {
            string baseURL = "", errorMessage = "";
            HttpClient client = new HttpClient();

            List<Parameter> parameters = new List<Parameter>();
            parameters.AddRange(Parameters);
            // handle parameters
            parameters.Add(new Parameter("includes", "Images,Shop,Section,ShippingInfo:100"));
            if (searchTerms != "")
                parameters.Add(new Parameter("keywords", searchTerms));

            // load by page ******************
            if (pageIndex < 1)
                pageIndex = 1;
            parameters.Add(new Parameter("page", pageIndex.ToString()));    // load more items from where the list previously left off

            parameters.Add(new Parameter("limit", pageSize.ToString()));    // default page size is 25

            if (App.logged_in == false)
            {
                // FIX FIX FIX FIX FIX FIX FIX FIX FIX *********************************************************************
                //***********************************************************

                // **********   ***************   ************
                //baseURL = string.Format("{0}/listings/{1}?{2}&api_key={3}", App.baseURL, desiredList, desiredList, App.key);    // Load images and owning shops in addition 
            }
            else
            {
                if (shop_id >= 0)
                    if (shop_section_id >= 0)
                        baseURL = string.Format("{0}/shops/{1}/sections/{2}/listings/{3}", App.baseURL, shop_id, shop_section_id, desiredList);  // Load shop section
                    else
                        baseURL = string.Format("{0}/shops/{1}/listings/{2}", App.baseURL, shop_id, desiredList);  // Load shop
                else
                    baseURL = string.Format("{0}/listings/{1}", App.baseURL, desiredList);  // Load list 
                baseURL = DataGET.addAuthentication(baseURL, parameters);
            }

            ProductListDeserializer list = new ProductListDeserializer();               // temporary list, can be returned

            try
            {
                var jsonStream = await client.GetStreamAsync(baseURL);
                //var jsonString = await client.GetStringAsync(baseURL);

                using (StreamReader reader = new StreamReader(jsonStream))
                {
                    var serializer = new DataContractJsonSerializer(typeof(ProductListDeserializer));

                    list = (ProductListDeserializer)serializer.ReadObject(jsonStream);
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            // filter places that the items ship to, since the API doesn't provide a method for doing so
            if (country_shipTo_id >= 0)
            {
                for (int i = 0; i < list.results.Count; )
                {
                    Listing current = list.results[i];                          // item to be judged
                    current.shippingPractical = new ObservableCollection<Shipping>();
                    int successCount = 0;
                    ObservableCollection<Shipping> shipList = list.results[i].ShippingInfo;

                    for (int s = 0; s < shipList.Count; s++)
                    {
                        if (shipList[s].destination_country_name == "Everywhere Else")  // ships everywhere
                        {
                            successCount++;
                            current.shippingPractical.Add(shipList[s]);
                            break;
                        }
                        else if (shipList[s].destination_country_id == country_shipTo_id)
                        {
                            successCount++;
                            current.shippingPractical.Add(shipList[s]);
                            break;
                        }
                        else if (shipList[s].origin_country_id == country_shipTo_id)  // apparently always ships to same country as origin
                        {
                            successCount++;
                            current.shippingPractical.Add(shipList[s]);
                            break;
                        }
                    }

                    if (successCount == 0)
                        list.results.Remove(current);                           // remove the item. index won't change
                    else
                        i++;                                                    // move on to the next spot
                }
            }

            foreach (var listin in list.results)
            {
                listin.setup();
            }

            return list.results;
        }

        /// <summary>
        /// Get variations for a particular listing. Requires OAuth; user must be logged in
        /// </summary>
        /// <param name="listing_id"></param>
        /// <returns></returns>
        public static async Task<Variations> getVariations(int listing_id)
        {
            Variations newVariations = new Variations();

            if (App.logged_in == false)
                return newVariations;       // require the user to be logged in

            HttpClient client = new HttpClient();
            string baseURL = App.baseURL, errorMessage = "";
            List<Parameter> parameters = new List<Parameter>();

            baseURL = string.Format("{0}/listings/{1}/variations", App.baseURL, listing_id.ToString()); 
            baseURL = AuthenticationAccess.addAuthentication(baseURL, parameters, "GET");
                        

            try
            {
                var jsonStream = await client.GetStreamAsync(baseURL);

                using (StreamReader reader = new StreamReader(jsonStream))
                {
                    var serializer = new DataContractJsonSerializer(typeof(Variations));

                    newVariations = (Variations)serializer.ReadObject(jsonStream);
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return newVariations;
        }
        
        
        /// <summary>
        /// Get the shop info of the given item ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ShopResultsHolder> getOwningShop(int item_id)
        {
            
            baseURL = string.Format("{0}/shops/listing/{1}?api_key={2}", App.baseURL, item_id, App.key);

            ShopResultsHolder resultsHolder = new ShopResultsHolder();


            // Do it the old fashioned way (manually parse) due to the json object structure 
                // using #'s as object types - the serializer doesn't like that
            var jsonString = await client.GetStringAsync(baseURL);

            resultsHolder = JsonConvert.DeserializeObject<ShopResultsHolder>(jsonString);

            // Get the details that the deserializer missed

            // outer json container object
            JsonObject jsOuter = JsonObject.Parse(jsonString);
            JsonObject shopObject = jsOuter["results"].GetObject();

            // General Info
            var jObj_generalInfo = shopObject["0"].Stringify();
            resultsHolder.results.general_info = JsonConvert.DeserializeObject<GeneralInfo>(jObj_generalInfo);
            
            // Story
            var jObj_story = shopObject["1"].Stringify();
            resultsHolder.results.story = JsonConvert.DeserializeObject<Story>(jObj_story);

            // Owner Basics
            var jObj_owner_basics = shopObject["2"].Stringify();
            resultsHolder.results.owner_basics = JsonConvert.DeserializeObject<Owner_Basics>(jObj_owner_basics);

            // Owner Detailed
            var jObj_owner_detailed = shopObject["3"].Stringify();
            resultsHolder.results.owner_detailed = JsonConvert.DeserializeObject<Owner_Detailed>(jObj_owner_detailed);                

            return resultsHolder;
        }
        

        /// <summary>
        /// Get the user reviews for the specified shop.
        /// Requires authentication
        /// </summary>
        /// <returns></returns>
        public static async Task<ObservableCollection<Etsy.Model.Shop.User_Feedback.TransactionFeedback>> getUserFeedback(int? shop_user_id)
        {
            string errorMessage, baseURL;
            HttpClient client = new HttpClient();
            Etsy.Model.Shop.User_Feedback.Feedback feedB = new Etsy.Model.Shop.User_Feedback.Feedback();

            if (App.logged_in == false)
                return feedB.results;

            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("includes", "Listing,Buyer"));

            baseURL = string.Format("{0}/users/{1}/feedback/as-seller", App.baseURL, shop_user_id);
            baseURL = AuthenticationAccess.addAuthentication(baseURL, parameters, "GET");            

            try
            {
                var jsonStream = await client.GetStreamAsync(baseURL);
                //var jsonString = await client.GetStringAsync(baseURL);

                using (StreamReader reader = new StreamReader(jsonStream))
                {

                    var serializer = new DataContractJsonSerializer(typeof(Etsy.Model.Shop.User_Feedback.Feedback));

                    feedB = (Etsy.Model.Shop.User_Feedback.Feedback)serializer.ReadObject(jsonStream);

                    foreach (var feedback in feedB.results)           // make aspects of the associated listing available for data binding
                    {
                        if(feedback.Listing != null)
                            if(feedback.Listing.title != null)
                                feedback.title = feedback.Listing.title;    // make the title of the associated listing accessible for binding
                    }
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return feedB.results;
        }
                                
    }

    
}
