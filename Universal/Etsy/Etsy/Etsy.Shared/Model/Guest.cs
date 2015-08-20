using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Etsy.Model
{
    public class Guest
    {
        string baseURL;
        string guest_id;
        string checkout_url;
        HttpClient client = new HttpClient();

        public Guest()
        {
            baseURL = string.Format("{0}/guests/generator?api_key={1}",App.baseURL,App.key);
        }

        public async Task getGuest()
        {
            try
            {
                var jsonString = await client.GetStringAsync(baseURL);
                // parse the old fashioned way
                JsonObject responseObject = JsonObject.Parse(jsonString);
                JsonArray guest_Array = responseObject["results"].GetArray();
                foreach (JsonValue val in guest_Array)
                {
                    JsonObject gObj = val.GetObject();
                    guest_id = gObj["guest_id"].GetString();
                    checkout_url = gObj["checkout_url"].GetString();
                }
            }
            catch(Exception e)
            {
                string message = e.Message;
            }

            return;
        }



    }
}
