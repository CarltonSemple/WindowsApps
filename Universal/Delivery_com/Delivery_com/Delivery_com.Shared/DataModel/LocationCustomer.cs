using System;
using System.Collections.Generic;
using System.Text;

namespace Delivery_com.DataModel
{
    /// <summary>
    /// Similar to a Generic Location, with the addition of:
    /// location_id
    /// phone
    /// cross_streets
    /// unit_number
    /// company (optional company name to help the merchant find the customer)
    /// </summary>
    public class LocationCustomer:LocationGeneric
    {
        public LocationCustomer() { location_id = ""; }
        public LocationCustomer(String Street, String City, String State, String ZipCode, String Long, String Lat,
            String LocationID, String PhoneNum, String CrossStreets, String UnitNumber, String Company)
        {
            this.street = Street;
            this.city = City;
            this.state = State;
            this.zip_code = ZipCode;
            this.longitude = Long;
            this.latitutde = Lat;
            
            // customer-specific assignments
            this.location_id = LocationID;
            this.phone = PhoneNum;
            this.cross_streets = CrossStreets;
            this.unit_number = UnitNumber;
            this.company = Company;

            this.summation = street + ", " + city + ", " + state;
            this.searchAddress = street + " " + zip_code;
        }

        // for the user to see when looking at the list of addresses
        public string summation { get; set; }

        // the address format used in the search . leave the spaces in for proper requests to avoid error
        public string searchAddress { get; set; }
        public string unit_number { get; set; }
        public string location_id { get; set; }
        public string phone { get; set; }
        public string cross_streets { get; set; }
        public string company { get; set; }


        // override the ToString() method so that the objects can properly be displayed in a list

        /************* UPDATE ***************/
        // not needed, but still useful to know. itemtemplate, THEN data template, is necessary for the listbox/listview
        //public override string ToString()
        //{
        //    return street + ", " + city + ", " + state + " " + zip_code;
        //}
    }
}
