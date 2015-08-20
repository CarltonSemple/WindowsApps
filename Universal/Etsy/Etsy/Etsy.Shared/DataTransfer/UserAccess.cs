using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

using Etsy.Model.User;

namespace Etsy.DataTransfer
{
    public class UserAccess
    {
        /// <summary>
        /// Get basic user info
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static async Task getUser(string user_id)
        {
            string baseURL = App.baseURL;
            HttpClient client = new HttpClient();

            if (App.logged_in == false && user_id == "__SELF__")    
            {
                // error if trying to access the logged in user and there is none
                return;
            }
            else if(App.logged_in == false)     // not logged in
            {
                baseURL = string.Format("{0}/users/{1}", baseURL, user_id);
            }
            else if(App.logged_in == true)      // logged in
            {
                baseURL = string.Format("{0}/users/{1}", baseURL, user_id);
                List<Parameter> parameters = new List<Parameter>();
                baseURL = AuthenticationAccess.addAuthentication(baseURL, parameters);
            }

            try
            {
                var jsonStream = await client.GetStreamAsync(baseURL);
                //var jsonString = await client.GetStringAsync(baseURL);
            }
            catch
            {

            }

            return;
        }

        /// <summary>
        /// Get basic user info, profile, receipt & transaction history, addresses
        /// Only works when logged in
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns>User Object</returns>
        public static async Task<User> getUserFull(string user_id)
        {
            string baseURL = App.baseURL;
            HttpClient client = new HttpClient();

            //string parameterKey = "includes", parameterValue = "Profile,BuyerReceipts,BuyerTransactions,Addresses";
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("includes", "Profile,BuyerReceipts,BuyerTransactions,Addresses"));

            User user = new User();

            if (App.logged_in == false && user_id == "__SELF__")    
            {
                // error if trying to access the logged in user and there is none
                return null;
            }
            else if(App.logged_in == false)     // not logged in
            {
                baseURL = string.Format("{0}/users/{1}", baseURL, user_id);
            }
            else if(App.logged_in == true)      // logged in, supposedly
            {
                baseURL = string.Format("{0}/users/{1}", baseURL, user_id);
                baseURL = AuthenticationAccess.addAuthentication(baseURL, parameters);
            }

            try
            {
                var jsonStream = await client.GetStreamAsync(baseURL);

                using(StreamReader reader = new StreamReader(jsonStream))
                {
                    var serializer = new DataContractJsonSerializer(typeof(Model.User.dataFetch));
                    var container = (Model.User.dataFetch)serializer.ReadObject(jsonStream);    // capture the object that holds the user

                    user = container.results[0];    // only one user requested; get the object at the 0 index
                }
            }
            catch(Exception e)
            {
                App.logged_in = false;      // the user is in fact not logged in
            }

            return user;
        }
        
    }
}
