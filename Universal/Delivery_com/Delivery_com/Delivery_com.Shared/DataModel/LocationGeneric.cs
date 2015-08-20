using System;
using System.Collections.Generic;
using System.Text;

namespace Delivery_com.DataModel
{
    /// <summary>
    /// generic location object
    /// </summary>
    public class LocationGeneric
    {
        public LocationGeneric() { street = ""; }
        // constructor
        public LocationGeneric(String Street, String City, String State, String ZipCode, String Long, String Lat)
        {
            this.street = Street;
            this.city = City;
            this.state = State;
            this.zip_code = ZipCode;
            this.longitude = Long;
            this.latitutde = Lat;            
        }

        

        public string street { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip_code { get; set; }
        public string longitude { get; set; }
        public string latitutde { get; set; }    
    }
}
