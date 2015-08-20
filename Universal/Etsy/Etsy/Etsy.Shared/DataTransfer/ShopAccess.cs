using Etsy.Model;
using Etsy.Model.Shop;
using Etsy.Model.Shop.About;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Etsy.DataTransfer
{
    public class ShopAccess
    {
        /// <summary>
        /// Get extra detail about the given shop, including images
        /// </summary>
        /// <param name="shop"></param>
        /// <returns></returns>
        public static async Task<ShopAbout> getShopAbout(GeneralInfo shop)
        {
            HttpClient client = new HttpClient();
            string baseURL = App.baseURL, errorMessage = "";
            ShopAbout shopAbout = new ShopAbout();
            shopAbout.Images = new ObservableCollection<ShopImage>();

            if (App.logged_in == false)         // this function only applies to a logged in user
                return shopAbout;

            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("includes", "Images"));       // get the listings' details

            baseURL = string.Format("{0}/shops/{1}/about", baseURL, shop.shop_id);
            baseURL = AuthenticationAccess.addAuthentication(baseURL, parameters, "GET");

            AboutDeserializeCapturer aDes = new AboutDeserializeCapturer();
            

            try
            {
                var jsonStream = await client.GetStreamAsync(baseURL);

                using (StreamReader reader = new StreamReader(jsonStream))
                {
                    var serializer = new DataContractJsonSerializer(typeof(AboutDeserializeCapturer));

                    aDes = (AboutDeserializeCapturer)serializer.ReadObject(jsonStream);
                    if(aDes != null)
                        if (aDes.results != null)
                            if (aDes.results.Count > 0)
                                shopAbout = aDes.results[0];
                }

            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return shopAbout;
        }
    }
}
