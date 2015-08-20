using Etsy.DataTransfer;
using Etsy.Model;
using Etsy.Model.List;
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

namespace Etsy.DataBinding
{
    public class ListGetter : IPagedSource<Listing>
    {
        /// <summary>
        /// Return the desired list/>
        /// Images & Shops are loaded as well
        /// Add the product listings to the ProductList passed as a parameter
        /// </summary>
        /// <param name="desiredList"></param>
        /// <param name="passedList"></param>
        /// <returns></returns>
        public async Task<IPagedResponse<Listing>> GetPage( int? segments,
                                                            int? favorites,
                                                            int? shop_id,
                                                            Int64? shop_section_id,
                                                            string desiredList,
                                                            string searchTerms,
                                                            List<Parameter> Parameters,
                                                            int? country_shipTo_id,
                                                            int? pageIndex,
                                                            int? pageSize)
        {
            string baseURL = "", errorMessage = "good";
            HttpClient client = new HttpClient();

            List<Parameter> parameters = new List<Parameter>();
            parameters.AddRange(Parameters);
            // handle parameters
            if (favorites <= 0)     // this request has different associations (the following line isn't possible)
            {
                parameters.Add(new Parameter("includes", "Images,Shop,Section,ShippingInfo:100"));
                if (searchTerms != "")
                    parameters.Add(new Parameter("keywords", searchTerms));
            }
            else
                parameters.Add(new Parameter("includes", "Listing/Images,Listing/Shop,Listing/Section,Listing/ShippingInfo:100"));

            

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
                if (shop_id >= 0)   // shop & shop section
                {
                    if (shop_section_id >= 0)
                        baseURL = string.Format("{0}/shops/{1}/sections/{2}/listings/{3}", App.baseURL, shop_id, shop_section_id, desiredList);  // Load shop section
                    else
                        baseURL = string.Format("{0}/shops/{1}/listings/{2}", App.baseURL, shop_id, desiredList);  // Load shop
                }
                else if (favorites >= 0)     // favorite listings
                {
                    baseURL = string.Format("{0}/users/{1}/favorites/listings", App.baseURL, App.userID);
                }
                else if(segments >= 0)
                    baseURL = string.Format("{0}/segments/listings/{1}", App.baseURL, desiredList); 
                else  // simple list of listings
                    baseURL = string.Format("{0}/listings/{1}", App.baseURL, desiredList); 
                
                baseURL = DataGET.addAuthentication(baseURL, parameters);   // add signature
            }

            ProductListDeserializer list = new ProductListDeserializer();               // temporary list, can be returned
            FavoritesListingsDeserializer desList = new FavoritesListingsDeserializer();

            try
            {
                var jsonStream = await client.GetStreamAsync(baseURL);
                //var jsonString = await client.GetStringAsync(baseURL);

                if (favorites < 0)
                {
                    using (StreamReader reader = new StreamReader(jsonStream))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(ProductListDeserializer));

                        list = (ProductListDeserializer)serializer.ReadObject(jsonStream);
                    }
                }
                else // get favorites
                {
                    using (StreamReader reader = new StreamReader(jsonStream))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(FavoritesListingsDeserializer));

                        desList = (FavoritesListingsDeserializer)serializer.ReadObject(jsonStream);
                        desList.simplify(country_shipTo_id);
                    }
                }
            }
            catch (Exception e)
            {
                // State that the app has a loading error
                // Set the error message
                Loading.loadError = true;
                Loading.errorMessage = e.Message;

                // Return an empty list
                list = new ProductListDeserializer();
                list.results = new ObservableCollection<Listing>();
                IEnumerable<Listing> aList = list.results;
                return new ListResponse<Listing>(aList, 0);
            }

            // State that the app doesn't have a load error
            Loading.loadError = false;

            if (favorites >= 0)
            {
                IEnumerable<Listing> ieList = desList.listings;

                return new ListResponse<Listing>(ieList, desList.listings.Count);
            }
            

            try
            {
                // filter places that the items ship to, since the API doesn't provide a method for doing so
                if (country_shipTo_id >= 0)
                {
                    for (int i = 0; i < list.results.Count; )
                    {
                        Listing current = list.results[i];                          // item to be judged
                        current.shippingPractical = new ObservableCollection<Shipping>();
                        int successCount = 0;
                        ObservableCollection<Shipping> shipList = list.results[i].ShippingInfo;

                        if (shipList != null)
                        {
                            for (int s = 0; s < shipList.Count; s++)
                            {
                                if (shipList[s].destination_country_name == "Everywhere Else")  // ships everywhere
                                {
                                    successCount++;
                                    current.shippingPractical.Add(shipList[s]);
                                }
                                else if (shipList[s].destination_country_id == country_shipTo_id)
                                {
                                    successCount++;
                                    current.shippingPractical.Add(shipList[s]);
                                }
                            }

                            if (successCount == 0)
                                list.results.Remove(current);                           // remove the item. index won't change
                            else
                                i++;                                                    // move on to the next spot
                        }
                        else
                            i++;
                    }
                }

                foreach (var listin in list.results)
                {
                    listin.setup();
                }                
            }
            catch(Exception ue)
            {
                errorMessage = ue.Message;
            }


            IEnumerable<Listing> eList = list.results;

            return new ListResponse<Listing>(eList, list.count);
        }

    }


    public class ListResponse<T> : IPagedResponse<T>
    {
        public ListResponse(IEnumerable<T> items, int virtualCount)
        {
            this.Items = items;
            this.VirtualCount = virtualCount;
        }

        public IEnumerable<T> Items { get; private set; }
        public int VirtualCount { get; private set; }
    }
}
