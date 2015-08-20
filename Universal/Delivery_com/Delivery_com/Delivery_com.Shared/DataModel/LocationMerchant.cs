using System;
using System.Collections.Generic;
using System.Text;

namespace Delivery_com.DataModel
{
    public class LocationMerchant : LocationGeneric
    {
        public LocationMerchant(double Distance, String Street, String City, String State, 
                                String ZipCode, String Long, String Lat, String Landmark)
        {
            this.street = Street;
            this.city = City;
            this.state = State;
            this.zip_code = ZipCode;
            this.longitude = Long;
            this.latitutde = Lat;
            this.distance = Distance;
            this.landmark = Landmark;
        }


        /******* merchant-specific ******/

        // The distance in miles from this merchant to the searched location.
        public double distance { get; set; }
        // Extra info about where the merchant is located.
        public string landmark { get; set; }
    }
}
