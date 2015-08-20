using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http; // important for finding a specific merchant in the observable collection

namespace Delivery_com.DataModel
{
    public class SearchSource
    {
        private static SearchSource _searchSource = new SearchSource();

        private ObservableCollection<Merchant> _merchants = new ObservableCollection<Merchant>();
        public ObservableCollection<Merchant> Merchants
        {
            get { return this._merchants; }
        }

        // used to get the merchants
        public static async Task<IEnumerable<Merchant>> GetMerchantsAsync()
        {
            // get the latest data, while first clearing the previous data
            await _searchSource.GetSearchDataAsync();

            return _searchSource.Merchants;
        }

        // get an individual merchant
        public static async Task<Merchant> GetMerchantAsync(string uniqueID)
        {
            await _searchSource.GetSearchDataAsync();

            // linear search through an observable collection
            var matches = _searchSource.Merchants.Where((merchant) => merchant.uniqueID.Equals(uniqueID));

            // return the merchant object if there is only one match (there should be)
            if (matches.Count() == 1)
                return matches.First();

            // else
            return null;
        }


        // Get the data from the search api..
        private async Task GetSearchDataAsync()
        {
             // Clear the previous results
            this._merchants.Clear();
            this.Merchants.Clear();

            // this line might not actually be used..
            Uri dataUri = new Uri((App.Current as App).searchURL);

            HttpClient client = new HttpClient();
            string jsonText = "";
            try
            {
                jsonText = await client.GetStringAsync((App.Current as App).searchURL);
            }
            catch
            {

            }
            JsonObject jsonObject = JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["merchants"].GetArray();

            foreach (JsonValue merchantValue in jsonArray)
            {   
                // create new merchant objects for each that are returned
                JsonObject merchObject = merchantValue.GetObject();

                // create the merchant object, taking the id first
                Merchant merchant = new Merchant(merchObject["id"].GetString());

                // need to work way through the order of objects to reach cuisines.. merchant -> summary -> cuisines
                JsonObject sumObject = merchObject["summary"].GetObject();


        // PROTECTION
                string cuisineCheck = sumObject["cuisines"].ValueType.ToString();
                // referencing just the current cuisine object
                JsonArray cuisineArray = new JsonArray();
                List<String> Cuisines = new List<String>();

                // create a cuisines list to be added to the summary
                if (cuisineCheck == cuisineArray.ValueType.ToString())
                {
                    cuisineArray = sumObject["cuisines"].GetArray();
                    foreach (JsonValue itemValue in cuisineArray)
                    {
                        Cuisines.Add(itemValue.GetString());
                    }
                }
      

                // create a url object to add to the summary
                JsonObject urlObject = sumObject["url"].GetObject();

        // PROTECTION *********************************************/

                // short tag
                string shorttagcheck = urlObject["short_tag"].ValueType.ToString();
                string short_tag = "";
                if (shorttagcheck == "String")
                    short_tag = urlObject["short_tag"].GetString();

                // complete url
                string completeurlCheck = urlObject["complete"].ValueType.ToString();
                string complete = "";
                if (completeurlCheck == "String")
                    complete = urlObject["complete"].GetString();
                
                /////////////////////////////////////////////////////////////

                // finish creating the url object
                url m_url = new url(    short_tag,
                                        complete);

        // PROTECTION!
                    // name
                    string nameCheck = sumObject["name"].ValueType.ToString();
                    string name = "";
                    if (nameCheck == "String")
                        name = sumObject["name"].GetString();

                    // phone
                    string phoneCheck = sumObject["phone"].ValueType.ToString();
                    string phone = "";
                    if (phoneCheck == "String")
                        phone = sumObject["phone"].GetString();
                    
                    // description
                    string descriptionCheck = sumObject["description"].ValueType.ToString();
                    string description = "";
                    if (descriptionCheck == "String")
                        description = sumObject["description"].GetString();
                    // overall rating
                    string overallratingCheck = sumObject["overall_rating"].ValueType.ToString();
                    int overall_rating = 0;
                    if (overallratingCheck == "Number")
                        overall_rating = Convert.ToInt32(sumObject["overall_rating"].GetNumber());

                    // number of ratings
                    string num_ratingsCheck = sumObject["num_ratings"].ValueType.ToString();
                    int num_ratings = 0;
                    if (num_ratingsCheck == "Number")
                        num_ratings = Convert.ToInt32(sumObject["num_ratings"].GetNumber());
                    
                    // type
                    string typeCheck = sumObject["type"].ValueType.ToString();
                    string type = "";
                    if (typeCheck == "String")
                        type = sumObject["type"].GetString();

                    // type label
                    string typelabelCheck = sumObject["type_label"].ValueType.ToString();
                    string type_label = "";
                    if (typelabelCheck == "String")
                        type_label = sumObject["type_label"].GetString();

                    // notes
                    string notecheck = sumObject["notes"].ValueType.ToString();
                    string notes = "";
                    if (notecheck == "String")
                        notes = sumObject["notes"].GetString();

                    // activation date
                    string activationdateCheck = sumObject["activation_date"].ValueType.ToString();
                    string activation_date = "";
                    if (activationdateCheck == "String")
                        activation_date = sumObject["activation_date"].GetString();

                ///////////////////////////////////////////////////////////////////

                // for each merchant, create a summary
                Summary summary = new Summary(  name,
                                                Cuisines, // cuisines list
                                                phone,
                                                description,
                                                overall_rating, // convert the string to a number
                                                num_ratings, // convert the string to a number
                                                type,
                                                type_label,
                                                notes,
                                                m_url, // url object
                                                activation_date
                                                );

                
                // get the ordering object from merchant
                JsonObject orderingObject = merchObject["ordering"].GetObject();

                string pCheck = orderingObject["payment_types"].ValueType.ToString();
                JsonArray paymentArray = new JsonArray();

                // create a list of payment types to insert into ordering
                List<String> PaymentTypes = new List<String>();
      // PROTECTION
                if (pCheck == "Array")
                {
                    // get the payment types from the ordering object...
                    paymentArray = orderingObject["payment_types"].GetArray();
                    foreach (JsonValue itemVal in paymentArray)
                    {
                        PaymentTypes.Add(itemVal.GetString());
                    }
                }

                // get the specials array from the orderingObject
      // PROTECTION
                string sCheck = orderingObject["specials"].ValueType.ToString();
                // create a list of specials to insert into ordering
                JsonArray specialsArray = new JsonArray();
                List<String> Specials = new List<String>();         
                if (sCheck == "Array") // if the array exists
                {
                    specialsArray = orderingObject["specials"].GetArray();
                    foreach (JsonValue itemV in specialsArray)
                    {
                        Specials.Add(itemV.GetString());
                    }
                }

        // PROTECTION ***************************************************************************/
                
                    // delivery_processes_card
                    string deliveryprocessescardCheck = orderingObject["delivery_processes_card"].ValueType.ToString();
                    bool delivery_processes_card = false;
                    if (deliveryprocessescardCheck == "Boolean")
                        delivery_processes_card = orderingObject["delivery_processes_card"].GetBoolean();

                    // is_rds
                    string isrdsCheck = orderingObject["is_rds"].ValueType.ToString();
                    bool is_rds = false;
                    if (isrdsCheck == "Boolean")
                        is_rds = orderingObject["is_rds"].GetBoolean();

                    // time_needed
                    string timeneededCheck = orderingObject["time_needed"].ValueType.ToString();
                    int time_needed = 0;
                    if (timeneededCheck == "Number")
                        time_needed = Convert.ToInt32(orderingObject["time_needed"].GetNumber());

                    // last_or_next_order_time
                    string lastornextordertimeCheck = orderingObject["last_or_next_order_time"].ValueType.ToString();
                    string last_or_next_order_time = "";
                    if (lastornextordertimeCheck == "String")
                        last_or_next_order_time = orderingObject["last_or_next_order_time"].GetString();

                    // minimum
                    string minimumCheck = orderingObject["minimum"].ValueType.ToString();
                    float minimum = 0;
                    if (minimumCheck == "Number")
                        minimum = Convert.ToSingle(orderingObject["minimum"].GetNumber());

                    // is_open
                    string isopenCheck = orderingObject["is_open"].ValueType.ToString();
                    bool is_open = false;
                    if (isopenCheck == "Boolean")
                        is_open = orderingObject["is_open"].GetBoolean();

                    // delivery charge
                    string deliverychargeCheck = orderingObject["delivery_charge"].ValueType.ToString();
                    float delivery_charge = 0;
                    if (deliverychargeCheck == "Number")
                        delivery_charge = Convert.ToSingle(orderingObject["delivery_charge"].GetNumber());

                    // delivery percent
                    string deliverypercentCheck = orderingObject["delivery_percent"].ValueType.ToString();
                    float delivery_percent = 0;
                    if (deliverypercentCheck == "Number")
                        delivery_percent = Convert.ToSingle(orderingObject["delivery_percent"].GetNumber());

                ////////////////////////////////////////////////////////////////////

                // for each merchant, create an ordering object
                Ordering orderingInfo = new Ordering(   delivery_processes_card,
                                                        PaymentTypes,
                                                        is_rds,
                                                        time_needed,
                                                        Specials,
                                                        last_or_next_order_time,
                                                        minimum,
                                                        is_open,
                                                        delivery_charge,
                                                        delivery_percent
                                                        );

                // for each merchant, create a location object
                JsonObject locationObject = merchObject["location"].GetObject();

        // PROTECTION ***************************************************************************/

                // distance
                string distanceCheck = locationObject["distance"].ValueType.ToString();
                double distance = 0;
                if (distanceCheck == "Number")
                    distance = locationObject["distance"].GetNumber();

                // street
                string streetCheck = locationObject["street"].ValueType.ToString();
                string street = "";
                if (streetCheck == "String")
                    street = locationObject["street"].GetString();

                // city
                string cityCheck = locationObject["city"].ValueType.ToString();
                string city = "";
                if (cityCheck == "String")
                    city = locationObject["city"].GetString();

                // state
                string stateCheck = locationObject["state"].ValueType.ToString();
                string state = "";
                if (stateCheck == "String")
                    state = locationObject["state"].GetString();

                // zip
                string zipCheck = locationObject["zip"].ValueType.ToString();
                string zip = "";
                if (zipCheck == "String")
                    zip = locationObject["zip"].GetString();

                // longitude
                string longitudeCheck = locationObject["longitude"].ValueType.ToString();
                string longitude = "";
                if (longitudeCheck == "Number")
                    longitude = Convert.ToString(locationObject["longitude"].GetNumber());

                // latitude
                string latitudeCheck = locationObject["latitude"].ValueType.ToString();
                string latitude = "";
                if (latitudeCheck == "Number")
                    latitude = Convert.ToString(locationObject["latitude"].GetNumber());

                // landmark
                string landmarkCheck = locationObject["landmark"].ValueType.ToString();
                string landmark = "";
                if (landmarkCheck == "String")
                    landmark = locationObject["landmark"].GetString();
                ////////////////////////////////////////////////////////////////////

                LocationMerchant Location = new LocationMerchant(   distance,
                                                                    street,
                                                                    city,
                                                                    state,
                                                                    zip,
                                                                    longitude,
                                                                    latitude,
                                                                    landmark);
                
                
                
                
                
                merchant.merchant_summary = summary;
                merchant.merchant_ordering = orderingInfo;
                merchant.location = Location;
                merchant.name = summary.name;
                merchant.description = summary.description;
                merchant.notes = summary.notes;
                merchant.delivery_charge = orderingInfo.delivery_charge;
                merchant.phone = summary.phone;
                merchant.address = Location.street + ", " + Location.city + ", " + Location.state + " " + Location.zip_code;
                merchant.distance = Location.distance;
                merchant.distanceString = merchant.distance.ToString() + " miles";

                this.Merchants.Add(merchant);
            }

        }



    }
}
