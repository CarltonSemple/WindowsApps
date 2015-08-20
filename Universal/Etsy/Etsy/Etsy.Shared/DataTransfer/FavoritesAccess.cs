using Etsy.DataBinding;
using Etsy.Model;
using Etsy.Model.List;
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
using Windows.UI.Xaml.Controls;

namespace Etsy.DataTransfer
{
    public class FavoritesAccess
    {
        public static ListView favoritesListView;
        public static Etsy.DataBinding.IncrementalSource<ListGetter, Listing> favoriteListings;
        public static Etsy.DataBinding.IncrementalSource<UserGetter, User> favoriteUsers;

        /// <summary>
        /// Add the given listing to the user's favorites. Mark it as a favorite upon success.
        /// Return the new listing optionally
        /// </summary>
        /// <param name="listing_id"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static async Task AddFavoriteListing(Listing listing, string user_id)
        {
            HttpClient client = new HttpClient();
            string baseURL = App.baseURL, errorMessage = "";
            
            if (App.logged_in == false)
                return;

            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("listing_id", listing.listing_id.ToString()));

            baseURL = string.Format("{0}/users/{1}/favorites/listings/{2}", baseURL, App.userID, listing.listing_id);
            baseURL = AuthenticationAccess.addAuthentication(baseURL, parameters, "POST");

            // Create the POST request content
            var values = new List<KeyValuePair<string, string>>
            {
                // add the information pair to the values list
                new KeyValuePair<string, string>("listing_id", listing.listing_id.ToString())
            };

            try
            {
                HttpResponseMessage response = await client.PostAsync(baseURL, new FormUrlEncodedContent(values));

                var jsonString = await response.Content.ReadAsStringAsync();
                                
                listing.isFavorite = true;  
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return;
        }


        public static async Task AddFavoriteUserShop(Etsy.Model.Shop.GeneralInfo shop, string user_id)
        {
            HttpClient client = new HttpClient();
            string baseURL = App.baseURL, errorMessage = "";

            if (App.logged_in == false)
                return;

            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("target_user_id", shop.user_id.ToString()));

            baseURL = string.Format("{0}/users/{1}/favorites/users/{2}", baseURL, App.userID, shop.user_id);
            baseURL = AuthenticationAccess.addAuthentication(baseURL, parameters, "POST");

            // Create the POST request content
            var values = new List<KeyValuePair<string, string>>
            {
                // add the information pair to the values list
                new KeyValuePair<string, string>("target_user_id", shop.user_id.ToString())
            };

            try
            {
                HttpResponseMessage response = await client.PostAsync(baseURL, new FormUrlEncodedContent(values));

                var jsonString = await response.Content.ReadAsStringAsync();

                shop.isFavorite = true;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return;
        }

        /// <summary>
        /// Send a DELETE request to remove the specified listing from the user's favorites 
        /// </summary>
        /// <param name="listing"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static async Task RemoveFavoriteListing(Listing listing, string user_id)
        {
            HttpClient client = new HttpClient();
            string baseURL = App.baseURL, errorMessage = "";

            if (App.logged_in == false)         // this function only applies to a logged in user
                return;

            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("listing_id", listing.listing_id.ToString()));

            baseURL = string.Format("{0}/users/{1}/favorites/listings/{2}", baseURL, App.userID, listing.listing_id);
            baseURL = AuthenticationAccess.addAuthentication(baseURL, parameters, "DELETE");

            try
            {
                HttpResponseMessage response = await client.DeleteAsync(baseURL);

                var jsonString = await response.Content.ReadAsStringAsync();

                listing.isFavorite = false;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
        }

        /// <summary>
        /// Send a DELETE request to remove the specified user from the logged in user's favorites 
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static async Task RemoveFavoriteUser(GeneralInfo shop)
        {
            HttpClient client = new HttpClient();
            string baseURL = App.baseURL, errorMessage = "";

            if (App.logged_in == false)         // this function only applies to a logged in user
                return;

            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("target_user_id", shop.user_id.ToString()));

            baseURL = string.Format("{0}/users/{1}/favorites/users/{2}", baseURL, App.userID, shop.user_id);
            baseURL = AuthenticationAccess.addAuthentication(baseURL, parameters, "DELETE");

            try
            {
                HttpResponseMessage response = await client.DeleteAsync(baseURL);

                var jsonString = await response.Content.ReadAsStringAsync();

                shop.isFavorite = false;
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }
        }

        /// <summary>
        /// Load the list of the user's favorite listings along with the listing details
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
        public static async Task<ObservableCollection<Listing>> getFavoriteListings(string user_id)
        {
            HttpClient client = new HttpClient();
            string baseURL = App.baseURL, errorMessage = "";

            if (App.logged_in == false)         // this function only applies to a logged in user
                return new ObservableCollection<Listing>();

            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("includes", "Listing/Images"));       // get the listings' details

            baseURL = string.Format("{0}/users/{1}/favorites/listings", baseURL, App.userID);
            baseURL = AuthenticationAccess.addAuthentication(baseURL, parameters, "GET");

            FavoritesListingsDeserializer desList = new FavoritesListingsDeserializer();

            try
            {
                var jsonStream = await client.GetStreamAsync(baseURL);

                using (StreamReader reader = new StreamReader(jsonStream))
                {
                    var serializer = new DataContractJsonSerializer(typeof(FavoritesListingsDeserializer));

                    desList = (FavoritesListingsDeserializer)serializer.ReadObject(jsonStream);
                    desList.simplify(App.defaultAddress.country_id);
                }

            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            if(desList.listings != null)
                return desList.listings;                // return the collection of listings

            return new ObservableCollection<Listing>(); // return something, if nothing else
        }
    
    
    }
}
