using Etsy.Model.Image;
using Etsy.Model.ShippingNamespace;
using Etsy.UI_Extras;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Etsy.Model
{
    public class Listing
    {
        public int listing_id { get; set; }
        // capture the shipping when viewing an item's page
        public ObservableCollection<Shipping> ShippingInfo { get; set; }
        public ObservableCollection<Shipping> shippingPractical { get; set; }   // list of shipping details that would apply to the user's preferred address
        // capture the owning shop
        public Shop.GeneralInfo Shop { get; set; }
        public Shop.ShopSection Section { get; set; }
        public Shop.Shop shop_in_full { get; set; }
        public string state { get; set; }
        public int? user_id { get; set; }
        public int? category_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int? creation_tsz { get; set; }
        public int? ending_tsz { get; set; }
        public int? original_creation_tsz { get; set; }
        public int? last_modified_tsz { get; set; }
        public string price { get; set; }
        public string currency_code { get; set; }
        public int? quantity { get; set; }
        public Nullable<int> quantity_chosen { get; set; }
        public ObservableCollection<string> tags { get; set; }
        public ObservableCollection<string> category_path { get; set; }
        public ObservableCollection<int> category_path_ids { get; set; }
        public ObservableCollection<object> materials { get; set; }
        public int? shop_section_id { get; set; }
        public int? featured_rank { get; set; }
        public int? state_tsz { get; set; }
        public string url { get; set; }
        public int? views { get; set; }
        public bool isFavorite { get; set; }                        // to be set if the item is a favorite
        public int? num_favorers { get; set; }
        public long? shipping_template_id { get; set; }
        public int? processing_min { get; set; }
        public int? processing_max { get; set; }
        public string who_made { get; set; }
        public string is_supply { get; set; }
        public string when_made { get; set; }
        public Nullable<bool> is_private { get; set; }
        public string recipient { get; set; }
        public string occasion { get; set; }
        public ObservableCollection<string> style { get; set; }
        public Nullable<bool> non_taxable { get; set; }
        public Nullable<bool> is_customizable { get; set; }
        public Nullable<bool> is_digital { get; set; }
        public string file_data { get; set; }
        public string language { get; set; }
        public Nullable<bool> has_variations { get; set; }
        public Nullable<bool> used_manufacturer { get; set; }

        // Additional data, attach images to this item *****************************
        
        // the image of choice will be the first image in the image list
        public string image_of_choice_url { get; set; }
        public string shop_name { get; set; }
        public List<Image.Image> Images { get; set; }
        public Variations.Variations variations { get; set; }               // Variations such as color, size

        public List<string> error_messages { get; set; }
        


        // Functions ******************************************************************
        //*****************************************************************************

        // Necessary URL and HttpClient
        private string baseURL { get; set; }

        private HttpClient client = new HttpClient();

        /// <summary>
        /// Get Images associated with this item
        /// </summary>
        /// <returns></returns>
        public async Task getImages()
        {
            baseURL = string.Format("{0}/listings/{1}/images?api_key={2}", App.baseURL, listing_id, App.key);

            ImageList imagContainer = new ImageList();
            try
            {
                var jsonStream = await client.GetStreamAsync(baseURL);
                //var jString = await client.GetStringAsync(baseURL);
                string s;

                using (StreamReader reader = new StreamReader(jsonStream))
                {

                    var serializer = new DataContractJsonSerializer(typeof(ImageList));

                    imagContainer = (ImageList)serializer.ReadObject(jsonStream);

                    if(Images == null)
                        Images = new List<Image.Image>();
                    for (int i = 0; i < imagContainer.results.Count; i++)    // Copy the results to the Images List
                        Images.Add(imagContainer.results[i]);

                    // set the default image
                    if(Images.Count > 0)
                        image_of_choice_url = Images[0].url_570xN;
                }
            }
            catch (Exception e)
            {
                string message = e.Message;
            }

            return;
        }

        /// <summary>
        /// Set up the image_of_choice_url and the shop name
        /// Useful for data binding
        /// </summary>
        public void setup()
        {
            if (Images != null)
            {
                if (Images.Count > 0)
                {
                    if (Loading._imageQuality == Loading.ImageQuality.high)
                        image_of_choice_url = Images[0].url_570xN;
                    else
                        image_of_choice_url = Images[0].url_170x135;
                }
            }

            if (Shop != null)
                shop_name = Shop.shop_name;
        }


        
        ///// <summary>
        ///// Return the shipping info for the given 
        ///// </summary>
        ///// <param name="listingID"></param>
        ///// <returns></returns>
        //public async Task<ObservableCollection<Shipping>> getShippingInfo()
        //{
        //    baseURL = string.Format("{0}/listings/{1}/shipping/info?api_key={2}", App.baseURL, listing_id, App.key);
        //    ObservableCollection<Shipping> shippingList = new ObservableCollection<Shipping>();
        //    ShippingHolder shipHold = new ShippingHolder();

        //    try
        //    {
        //        var jsonStream = await client.GetStreamAsync(baseURL);
        //        //var jString = await client.GetStringAsync(baseURL);
        //        string s;

        //        using (StreamReader reader = new StreamReader(jsonStream))
        //        {

        //            var serializer = new DataContractJsonSerializer(typeof(ShippingHolder));

        //            shipHold = (ShippingHolder)serializer.ReadObject(jsonStream);

        //            int i = 0;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        string message = e.Message;
        //    }

        //    shippingList = shipHold.results;

        //    // set the member list
        //    shipping_info = shippingList;

        //    // return the list... optionally
        //    return shippingList;
        //}

    }
}
