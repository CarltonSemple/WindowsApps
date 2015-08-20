using Etsy.Model;
using Etsy.Model.Cart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Etsy.DataTransfer
{
    public class CartAccess
    {
        public static async Task<ShopCart> addToCart(Listing listing)
        {
            int listing_id = listing.listing_id;
            HttpClient client = new HttpClient();
            string baseURL = App.baseURL, errorMessage = "";
            ShopCart current_cart = new ShopCart();

            if (App.logged_in == false)         // this function only applies to a logged in user
                return current_cart;

            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("listing_id", listing_id.ToString()));

            baseURL = string.Format("{0}/users/{1}/carts", baseURL, App.userID);
            baseURL = AuthenticationAccess.addAuthentication(baseURL, parameters, "POST");

            // Create the POST request content
            var values = new List<KeyValuePair<string, string>>
            {
                // add the information pair to the values list
                new KeyValuePair<string, string>("listing_id", listing_id.ToString())
            };          

            try
            {
                HttpResponseMessage response = await client.PostAsync(baseURL, new FormUrlEncodedContent(values));

                var jsonStream = await response.Content.ReadAsStreamAsync();
                //var jsonString = await response.Content.ReadAsStringAsync();

                using (StreamReader reader = new StreamReader(jsonStream))
                {
                    var serializer = new DataContractJsonSerializer(typeof(ShopCart));

                    current_cart = (ShopCart)serializer.ReadObject(jsonStream);
                }                
            }
            catch(Exception e)
            {
                errorMessage = e.Message;
            }

            return current_cart;
        }

        public static async Task<ShopCart> addToCart_WithVariations(Listing listing)
        {
            int listing_id = listing.listing_id;
            HttpClient client = new HttpClient();
            string baseURL = App.baseURL, errorMessage = "";
            ShopCart current_cart = new ShopCart();

            if (App.logged_in == false)         // this function only applies to a logged in user
                return current_cart;

            // Build variations string
            string variations = "{";
            foreach (var variation in listing.variations.results)
            {
                if (variation.selected_option_id != -1)
                {
                    variations += "\"" + Convert.ToString(variation.property_id) + "\"" + ":" + Convert.ToString(variation.selected_option_id) + ",";
                }
            }
            if (variations[variations.Length - 1] == ',')
                variations = variations.Substring(0, variations.Length - 1) + '}';

            // parameters
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("listing_id", listing_id.ToString()));
            parameters.Add(new Parameter("quantity", listing.quantity_chosen.ToString()));
            parameters.Add(new Parameter("selected_variations", variations));

            baseURL = string.Format("{0}/users/{1}/carts", baseURL, App.userID);
            baseURL = AuthenticationAccess.addAuthentication(baseURL, parameters, "POST");       

            // Create the POST request content
            var values = new List<KeyValuePair<string, string>>
            {
                // add the information pair to the values list
                new KeyValuePair<string, string>("listing_id", listing_id.ToString()),
                new KeyValuePair<string, string>("quantity", listing.quantity_chosen.ToString()),
                new KeyValuePair<string, string>("selected_variations", variations)
            };

            try
            {
                HttpResponseMessage response = await client.PostAsync(baseURL, new FormUrlEncodedContent(values));

                var jsonStream = await response.Content.ReadAsStreamAsync();
                //var jsonString = await response.Content.ReadAsStringAsync();

                using (StreamReader reader = new StreamReader(jsonStream))
                {
                    var serializer = new DataContractJsonSerializer(typeof(ShopCart));

                    current_cart = (ShopCart)serializer.ReadObject(jsonStream);
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return current_cart;
        }

        /// <summary>
        /// Get all of the shop carts along with their respective listings
        /// </summary>
        /// <returns></returns>
        public static async Task<Cart> getCarts()
        {
            HttpClient client = new HttpClient();
            string baseURL = App.baseURL, errorMessage = "";
            Cart cart = new Cart();

            if (App.logged_in == false)         // this function only applies to a logged in user
                return cart;

            List<Parameter> parameters = new List<Parameter>();
            baseURL = string.Format("{0}/users/{1}/carts", baseURL, App.userID);
            baseURL = AuthenticationAccess.addAuthentication(baseURL, parameters);

            try
            {
                var jsonStream = await client.GetStreamAsync(baseURL);
                //var jsonString = await client.GetStringAsync(baseURL);

                using (StreamReader reader = new StreamReader(jsonStream))
                {
                    var serializer = new DataContractJsonSerializer(typeof(Cart));

                    cart = (Cart)serializer.ReadObject(jsonStream);
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return cart;
        }

        /// <summary>
        /// Send a DELETE request for the specified item.
        /// Update the local cart
        /// </summary>
        /// <param name="userCart"></param>
        /// <param name="itemToDelete"></param>
        /// <returns></returns>
        public static async Task deleteFromCart(Cart userCart, CartListing itemToDelete)
        {
            HttpClient client = new HttpClient();
            string baseURL = App.baseURL, errorMessage = "";
            ShopCartHolder resultantShopCartHolder = new ShopCartHolder();

            if (App.logged_in == false)         // this function only applies to a logged in user
                return;

            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("listing_id", itemToDelete.listing_id.ToString()));
            parameters.Add(new Parameter("listing_customization_id", itemToDelete.listing_customization_id.ToString()));

            baseURL = string.Format("{0}/users/{1}/carts", baseURL, App.userID);
            baseURL = AuthenticationAccess.addAuthentication(baseURL, parameters, "DELETE");

            try
            {
                HttpResponseMessage response = await client.DeleteAsync(baseURL);

                var jsonStream = await response.Content.ReadAsStreamAsync();
                //var jsonString = await response.Content.ReadAsStringAsync();

                using (StreamReader reader = new StreamReader(jsonStream))
                {
                    var serializer = new DataContractJsonSerializer(typeof(ShopCartHolder));

                    resultantShopCartHolder = (ShopCartHolder)serializer.ReadObject(jsonStream);
                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            // Replace the corresponding ShopCart inside the User's Cart with this updated version. Remove if empty
            ShopCart updatedSC = resultantShopCartHolder.results[0];
            for(int i = 0; i < userCart.results.Count; i++)
            {
                ShopCart sCart = userCart.results[i];
                if(sCart.cart_id == updatedSC.cart_id)
                {
                    if (updatedSC.listings.Count == 0)
                    {
                        userCart.results.Remove(sCart);     // remove shopCart if it contains no more items
                        userCart.count--;                   // decrement count
                        break;  // break due to how the foreach loop will be messed up since an item was removed. also saves time
                    }
                    else
                        sCart.listings = updatedSC.listings;
                }
            }

        }
    }
}
