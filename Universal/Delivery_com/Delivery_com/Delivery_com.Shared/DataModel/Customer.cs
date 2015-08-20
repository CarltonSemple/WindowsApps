using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace Delivery_com.DataModel
{
    /// <summary>
    /// The class that contains the customer's profile.  Once the access_code is received, get the customer's information
    /// </summary>
    public class Customer
    {
        string _token;
        HttpClient client = new HttpClient();
        // The customer cart state is maintained server-side, so use calls to add/modify/remove/retrieve items

        /// <summary>
        /// Take the parameters and get the current location. If unsuccessful, get the error so it can be displayed to the user
        /// </summary>
        /// <param name="Street"></param>
        /// <param name="UnitNumber"></param>
        /// <param name="City"></param>
        /// <param name="State"></param>
        /// <param name="Phone"></param>
        /// <param name="ZipCode"></param>
        /// <param name="Company"></param>
        /// <param name="CrossStreets"></param>
        /// <returns></returns>
        public async Task CreateLocation(String Street, String UnitNumber, String City, String State,
                                    String Phone, String ZipCode, String Company, String CrossStreets)
        {
            
            // prepare to accept the response
            string cross_streets;
            string company;
            string phone;
            string unit_number;
            string street, city, state, zip_code;

            // Return early and change an error variable to a specific number if any of the following 
            // don't meet the requirements
            if (Street.Length < 5)
                locationCreationError = -1;
            if (City.Length < 2)
                locationCreationError = -2;
            if (State.Length != 2)
                locationCreationError = -3;
            if (ZipCode.Length != 5)
                locationCreationError = -4;
            if (Phone.Length < 10 || Phone.Length > 12)
                locationCreationError = -5;

            // assuming there's no error, set the local variables
            street = Street; 
            city = City; 
            state = State; 
            zip_code = ZipCode;
            company = Company;
            phone = Phone;
            unit_number = UnitNumber;
            cross_streets = CrossStreets;
            
            // 
            _token = (App.Current as App).access_token;

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(_token);

            // url for the request
            string url = (App.Current as App).location_base;

            // Create the request content
            var values = new List<KeyValuePair<string, string>>
            {
                // add the information pairs to the values list
                new KeyValuePair<string, string>("street", street),
                new KeyValuePair<string, string>("unit_number", unit_number),
                new KeyValuePair<string, string>("city", city),
                new KeyValuePair<string, string>("state", state),
                new KeyValuePair<string, string>("phone", phone),
                new KeyValuePair<string, string>("zip_code", zip_code),
                new KeyValuePair<string, string>("company", company),
                new KeyValuePair<string, string>("cross_streets", cross_streets)
            };

            // receive the response
            HttpResponseMessage response = await client.PostAsync(url, new FormUrlEncodedContent(values));

            //response.EnsureSuccessStatusCode();
            var CreateResponseString = await response.Content.ReadAsStringAsync();

            // Parse into Json
            JsonObject responseObject = JsonObject.Parse(CreateResponseString);

            // handle the bad address response
            string type = responseObject["message"].ValueType.ToString();
            if(type ==  "object")
            {
                JsonObject message = responseObject["message"].GetObject();
                string code = message["code"].GetString();
                (App.Current as App).user_msg = message["user_msg"].GetString();
                (App.Current as App).dev_msg = message["dev_msg"].GetString();
                
                // output user error message

                return;
            }

            // else, receive the response and add it to the local list of addresses rather than calling GetLocations
            JsonObject locationObject = responseObject["location"].GetObject();

            // add the location id to the variable that represents the chosen current location for the customer
            currentLocationID = locationObject["location_id"].GetNumber().ToString();

            // add the location to the list of objects
            parseAndAddLocation(locationObject, -1);


            return;
        }


        /// <summary>
        /// Take the changes in values for a location and submit the changes in a PUT http request.
        /// The old values should initially appear for the user so they can easily change them
        /// </summary>
        /// <param name="location_id"></param>
        /// <param name="Street"></param>
        /// <param name="UnitNumber"></param>
        /// <param name="City"></param>
        /// <param name="State"></param>
        /// <param name="Phone"></param>
        /// <param name="ZipCode"></param>
        /// <param name="Company"></param>
        /// <param name="CrossStreets"></param>
        /// <returns></returns>
        public async Task UpdateLocation(string location_id, string Street, string UnitNumber, string City, string State,
                                    string Phone, string ZipCode, string Company, string CrossStreets)
        {
            // url for the request needs the location id as well
            // PUT /customer/location/{location_id}
            //string url = "https://api.delivery.com/api/customer/location/?";
            string url = "https://api.delivery.com/api/customer/location/" + location_id;

            

            // prepare to accept the response
            string cross_streets, company, phone, unit_number, street, city, state, zip_code;

            // Return early and change an error variable to a specific number if any of the following 
            // don't meet the requirements
            if (Street.Length < 5)
                locationCreationError = -1;
                if (City.Length < 2)
                    locationCreationError = -2;
                    if (State.Length != 2)
                        locationCreationError = -3;
                        if (ZipCode.Length != 5)
                            locationCreationError = -4;
                            if (Phone.Length < 10 || Phone.Length > 12)
                                locationCreationError = -5;

            // assuming there's no error, set the local variables
            street = Street;
                city = City;
                    state = State;
                       zip_code = ZipCode;
                            company = Company;
                                phone = Phone;
                            unit_number = UnitNumber;
                    cross_streets = CrossStreets;

            // 
            _token = (App.Current as App).access_token;

            // authorization
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(_token);


            // Create the request content
            var values = new List<KeyValuePair<string, string>>
            {
                // add the information pairs to the values list
                new KeyValuePair<string, string>("street", street),
                new KeyValuePair<string, string>("unit_number", unit_number),
                new KeyValuePair<string, string>("city", city),
                new KeyValuePair<string, string>("state", state),
                new KeyValuePair<string, string>("phone", phone),
                new KeyValuePair<string, string>("zip_code", zip_code),
                new KeyValuePair<string, string>("company", company),
                new KeyValuePair<string, string>("cross_streets", cross_streets)
            };


            // receive the response
            HttpResponseMessage response = await client.PutAsync(url, new FormUrlEncodedContent(values));

            //response.EnsureSuccessStatusCode();
            var CreateResponseString = await response.Content.ReadAsStringAsync();

            // Parse into Json
            JsonObject responseObject = JsonObject.Parse(CreateResponseString);

            // get the Json location object
            JsonObject locationObject = responseObject["location"].GetObject();

            // find the local version of the location so as to replace it with the new one
            // the location id will be the same, so use it to find the index
            for (int i = 0; i < customer_Addresses.Count; i++)
            {
                if(customer_Addresses[i].location_id == location_id)
                {   
                    // call parseAndAddLocation with the index, which will replace the location at this index with the new version
                    parseAndAddLocation(locationObject, i);
                    break;
                }
            }

        }

        
        /// <summary>
        /// Delete the location with this id
        /// </summary>
        /// <param name="location_id"></param>
        /// <returns></returns>
        public async Task DeleteLocation(string location_id)
        {
            // DELETE /customer/location/{location_id}
            //string url = (App.Current as App).location_base + location_id;
            string url = "https://api.delivery.com/api/customer/location/" + location_id;

            HttpResponseMessage response = await client.DeleteAsync(url);

            // remove the local copy
            for (int i = 0; i < customer_Addresses.Count; i++)
            {
                if(customer_Addresses[i].location_id == location_id)
                {
                    customer_Addresses.RemoveAt(i);
                }
            }
        }

        
        /// <summary>
        /// GET request
        /// Add the customer's locations to the local collection of addresses. 
        /// Empties the local address list upon doing so
        /// </summary>
        /// <returns></returns>
        public async Task GetLocations()
        {
            // empty the local address list to avoid any issues
            customer_Addresses.Clear();

            Uri StartUri = new Uri((App.Current as App).location_base);

            _token = (App.Current as App).access_token;

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(_token);

            string jsonText = await client.GetStringAsync((App.Current as App).location_base);

            /************************************   Parse into Json   ***********************************/
            JsonObject responseObject = JsonObject.Parse(jsonText);


            /****************** Handle the case where there are no locations ******************/
            
            string type = responseObject["message"].ValueType.ToString();
            if (type == "Object")
            {
                JsonObject message = responseObject["message"].GetObject();
                string code = message["code"].GetString();
                if (code == "loc_none")
                {
                    // output message, give option to add a location
                    return;
                }
            }
        
            /*****************************     Get Locations     ***************************/

            JsonArray locations_Array = responseObject["locations"].GetArray();

            // grab and build each location
            foreach (JsonValue locationValue in locations_Array)
            {
                // create new location object for each found in the array
                JsonObject locationObject = locationValue.GetObject();

                parseAndAddLocation(locationObject, -1);
            }

        }

        /// <summary>
        /// take a locationObject, parse it and add it to the list of customer addresses. 
        /// only use the index when updating/replacing a location: index -1 means you're not doing that
        /// </summary>
        /// <param name="locationObject"></param>
        private void parseAndAddLocation(JsonObject locationObject, int index)
        {
            string location_id = "", street = "", city = "", state = "", zip_code = "", phone = "", longitude = "", latitude = "",
                      unit_number = "", company = "", cross_streets = "";    // this line of properties are optional

            // ****************************************
            // Get the properties to create a location, 
            // while checking to see if they exist

            if (locationObject["location_id"].ValueType.ToString() == "String")
                location_id = locationObject["location_id"].GetString();

            if (locationObject["street"].ValueType.ToString() == "String")
                street = locationObject["street"].GetString();

            if (locationObject["city"].ValueType.ToString() == "String")
                city = locationObject["city"].GetString();

            if (locationObject["state"].ValueType.ToString() == "String")
                state = locationObject["state"].GetString();

            if (locationObject["zip_code"].ValueType.ToString() == "String")
                zip_code = locationObject["zip_code"].GetString();

            if (locationObject["phone"].ValueType.ToString() == "String")
                phone = locationObject["phone"].GetString();

            if (locationObject["unit_number"].ValueType.ToString() == "String")
                unit_number = locationObject["unit_number"].GetString();

            if (locationObject["company"].ValueType.ToString() == "String")
                company = locationObject["company"].GetString();

            if (locationObject["cross_streets"].ValueType.ToString() == "String")
                cross_streets = locationObject["cross_streets"].GetString();

            if (locationObject["longitude"].ValueType.ToString() == "String")
                longitude = locationObject["longitude"].GetString();

            if (locationObject["latitude"].ValueType.ToString() == "String")
                latitude = locationObject["latitude"].GetString();

            // Create the new location
            LocationCustomer nLocation = new LocationCustomer(street, city, state, zip_code, longitude, latitude, location_id, phone, cross_streets, unit_number, company);

            if (index < 0)
            {
                // add the location to the collection
                // if the index is not actually an index (i < 0)
                this.customer_Addresses.Add(nLocation);
            }
            else
            {   // replace the location at the index with the new one
                this.customer_Addresses[index] = nLocation;
            }
        }



        // the id of the current/chosen location. automatically set in createlocation
        public string currentLocationID { get; set; }

        // used for errors in Create Location
        public int locationCreationError { get; set; }

        public List<CreditCard> customer_PaymentMethods { get; set; }

        public ObservableCollection<LocationCustomer> customer_Addresses = new ObservableCollection<LocationCustomer>();

        public LocationCustomer selectedAddress = new LocationCustomer();
    }

 

    /// <summary>
    /// Credit cards belong to the customer
    /// </summary>
    public class CreditCard
    {
        public CreditCard(String cardID, String cardType, String LastFour, int ExpMonth, int ExpYear)
        {
            this.cc_id = cardID;
            this.type = cardType;
            this.last_four = LastFour;
            this.exp_month = ExpMonth;
            this.exp_year = ExpYear;
        }

        // Unique identifier for this credit card.
        public string cc_id { get; set; }

        // Long description of credit card.
        public string type { get; set; }

        // Last 4 digits of credit card.
        public string last_four { get; set; }

        // Expiration month of credit card.
        public int exp_month { get; set; }

        // Expiration year of credit card.
        public int exp_year { get; set; }
    }
}
