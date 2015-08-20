using Etsy.Model.ShippingNamespace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Etsy.Model.List
{
    public class FavoritesListingsDeserializer
    {
        public int? count { get; set; }
        public ObservableCollection<FavoriteListingContainer> results { get; set; }
        public Page_Parameters @params { get; set; }
        public Pagination pagination { get; set; }

        // Extract the Listings from the FavoriteListingContainers here, for simplicity
        public ObservableCollection<Listing> listings { get; set; }

        public void simplify(int? country_shipTo_id)
        {
            listings = new ObservableCollection<Listing>();
            foreach(var lContainer in results)
            {
                lContainer.Listing.setup();

                // filter shipping options by country code
                // filter places that the items ship to, since the API doesn't provide a method for doing so
                if (country_shipTo_id >= 0)
                {
                    for (int i = 0; i < results.Count; )
                    {
                        Listing current = results[i].Listing;                          // item to be judged
                        current.shippingPractical = new ObservableCollection<Shipping>();
                        int successCount = 0;
                        ObservableCollection<Shipping> shipList = results[i].Listing.ShippingInfo;

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
                            // don't remove favorites
                            i++;                                                    // move on to the next spot
                        }
                        else
                            i++;
                    }
                }
                
                listings.Add(lContainer.Listing);
            }
        }
    }
}
