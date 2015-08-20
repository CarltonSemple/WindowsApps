using Etsy.DataTransfer;
using Etsy.Model;
using Etsy.Model.List;
using Etsy.Model.ShippingNamespace;
using Etsy.Model.Shop;
using Etsy.Model.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Etsy.DataBinding
{
    public class UserGetter : IPagedSource<User>
    {
        /// <summary>
        /// Return list of user profiles and their shops
        /// </summary>
        /// <returns></returns>
        public async Task<IPagedResponse<User>> GetPage(    int? unused0,
                                                            int? unused,
                                                            int? unused1,
                                                            Int64? unused2,
                                                            string unused3,
                                                            string unused4,
                                                            List<Parameter> Parameters,
                                                            int? unused5,
                                                            int? pageIndex,
                                                            int? pageSize)
        {
            string baseURL = "", errorMessage = "";
            HttpClient client = new HttpClient();

            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("includes", "TargetUser/Profile,TargetUser/Shops"));



            // load by page ******************
            if (pageIndex < 1)
                pageIndex = 1;
            parameters.Add(new Parameter("page", pageIndex.ToString()));    // load more items from where the list previously left off
            parameters.Add(new Parameter("limit", pageSize.ToString()));    // default page size is 25

            if (App.logged_in == false)
            {

            }
            else
            {
                baseURL = string.Format("{0}/users/{1}/favorites/users", App.baseURL, App.userID);
                baseURL = DataGET.addAuthentication(baseURL, parameters);   // add signature
            }

            UserDeserializer uDes = new UserDeserializer();

            try
            {
                var jsonStream = await client.GetStreamAsync(baseURL);
                //var jsonString = await client.GetStringAsync(baseURL);

                using (StreamReader reader = new StreamReader(jsonStream))
                {
                    var serializer = new DataContractJsonSerializer(typeof(UserDeserializer));

                    uDes = (UserDeserializer)serializer.ReadObject(jsonStream);
                    uDes.simplify();
                }
                
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
            
            IEnumerable<User> ieList = uDes.users;

            return new ListResponse<User>(ieList, uDes.users.Count);
        }

    }

}
