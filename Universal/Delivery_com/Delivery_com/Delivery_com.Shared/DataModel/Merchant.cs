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
    /// merchant class contains a summary, ordering info, menu, hours, and reviews
    /// </summary>
    public class Merchant
    {
        public Merchant()
        {
            uniqueID = "-1";
            standard_schedule = new Schedule();
            current_schedule = new CurrentSchedule();
            //menu = new Menu();j
            menus = new ObservableCollection<Menu>();
        }

        public Merchant(String ID) 
        { 
            uniqueID = ID;
            standard_schedule = new Schedule();
            current_schedule = new CurrentSchedule();
            //menu = new Menu();
            menus = new ObservableCollection<Menu>();
        }

        // HttpClient used for all requests
        HttpClient client = new HttpClient();

        // local copy of the client id and secret
        private string client_id;
        private string secret;

        public LocationMerchant location;
        public Summary merchant_summary;
        public Ordering merchant_ordering;

        /********** Merchant Hours **********/
        public Schedule standard_schedule;
        public CurrentSchedule current_schedule;
        public Menu menu;
        public ObservableCollection<Menu> menus;

        public string uniqueID { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string description { get; set; } // here for databinding and ease of access
        public string notes { get; set; }
        public float delivery_charge { get; set; }
        public string phone { get; set; }
        public double distance { get; set; }

        public string distanceString { get; set; }
        /************* search-specific properties **************/

        // Whether or not the user has favorited this merchant. You must submit an access token with your request to see this parameter.
        public bool favorite { get; set; }

        /// <summary>
        /// Clear the previous schedules and retrieve the latest
        /// </summary>
        /// <returns></returns>
        public async Task GetSchedule()
        {
            // Clear the previous values if this method is called
            standard_schedule.business.Clear();
            standard_schedule.delivery.Clear();
            current_schedule.business.Clear();
            current_schedule.delivery.Clear();


            // keep these consistent
            client_id = (App.Current as App).client_id;
            secret = (App.Current as App).secret;

            // Call an HTTP GET request.  GET /merchant/{merchant_id}/hours
            string baseURL = "https://api.delivery.com/api/merchant/" + uniqueID    // the id of the merchant
                                                                      + "/hours?client_id=" + client_id;

            // Receive the response and parse it (Json)
            var responseString = await client.GetStringAsync(baseURL);
            JsonObject jsObject = JsonObject.Parse(responseString);

            // Standard schedule
            JsonObject standardSchedObject = jsObject["standard_schedule"].GetObject();
            {
                JsonObject sBusiness = standardSchedObject["business"].GetObject();
                {
                    JsonObject sSunday = sBusiness["sunday"].GetObject();
                        AddDay("standard_schedule", 1, "Sunday", sSunday);
                    JsonObject sMonday = sBusiness["monday"].GetObject();
                        AddDay("standard_schedule", 1, "Monday", sMonday);
                    JsonObject sTuesday = sBusiness["tuesday"].GetObject();
                        AddDay("standard_schedule", 1, "Tuesday", sTuesday);
                    JsonObject sWednesday = sBusiness["wednesday"].GetObject();
                        AddDay("standard_schedule", 1, "Wednesday", sWednesday);
                    JsonObject sThursday = sBusiness["thursday"].GetObject();
                        AddDay("standard_schedule", 1, "Thursday", sThursday);
                    JsonObject sFriday = sBusiness["friday"].GetObject();
                        AddDay("standard_schedule", 1, "Friday", sFriday);
                    JsonObject sSaturday = sBusiness["saturday"].GetObject();
                        AddDay("standard_schedule", 1, "Saturday", sSaturday);
                }

                JsonObject sDelivery = standardSchedObject["delivery"].GetObject();
                {
                    JsonObject sSunday = sDelivery["sunday"].GetObject();
                        AddDay("standard_schedule", 2, "Sunday", sSunday);
                    JsonObject sMonday = sDelivery["monday"].GetObject();
                        AddDay("standard_schedule", 2, "Monday", sMonday);
                    JsonObject sTuesday = sDelivery["tuesday"].GetObject();
                        AddDay("standard_schedule", 2, "Tuesday", sTuesday);
                    JsonObject sWednesday = sDelivery["wednesday"].GetObject();
                        AddDay("standard_schedule", 2, "Wednesday", sWednesday);
                    JsonObject sThursday = sDelivery["thursday"].GetObject();
                        AddDay("standard_schedule", 2, "Thursday", sThursday);
                    JsonObject sFriday = sDelivery["friday"].GetObject();
                        AddDay("standard_schedule", 2, "Friday", sFriday);
                    JsonObject sSaturday = sDelivery["saturday"].GetObject();
                        AddDay("standard_schedule", 2, "Saturday", sSaturday);
                }
            }

            // Current schedule
            JsonObject currentSchedObject = jsObject["current_schedule"].GetObject();
            {
                JsonObject cBusiness = currentSchedObject["business"].GetObject();
                {
                    JsonObject cSunday = cBusiness["sunday"].GetObject();
                        AddDay("current_schedule", 1, "Sunday", cSunday);
                    JsonObject cMonday = cBusiness["monday"].GetObject();
                        AddDay("current_schedule", 1, "Monday", cMonday);
                    JsonObject cTuesday = cBusiness["tuesday"].GetObject();
                        AddDay("current_schedule", 1, "Tuesday", cTuesday);
                    JsonObject cWednesday = cBusiness["wednesday"].GetObject();
                        AddDay("current_schedule", 1, "Wednesday", cWednesday);
                    JsonObject cThursday = cBusiness["thursday"].GetObject();
                        AddDay("current_schedule", 1, "Thursday", cThursday);
                    JsonObject cFriday = cBusiness["friday"].GetObject();
                        AddDay("current_schedule", 1, "Friday", cFriday);
                    JsonObject cSaturday = cBusiness["saturday"].GetObject();
                        AddDay("current_schedule", 1, "Saturday", cSaturday);
                }

                JsonObject cDelivery = currentSchedObject["delivery"].GetObject();
                {
                    JsonObject cSunday = cDelivery["sunday"].GetObject();
                        AddDay("current_schedule", 2, "Sunday", cSunday);
                    JsonObject cMonday = cDelivery["monday"].GetObject();
                        AddDay("current_schedule", 2, "Monday", cMonday);
                    JsonObject cTuesday = cDelivery["tuesday"].GetObject();
                        AddDay("current_schedule", 2, "Tuesday", cTuesday);
                    JsonObject cWednesday = cDelivery["wednesday"].GetObject();
                        AddDay("current_schedule", 2, "Wednesday", cWednesday);
                    JsonObject cThursday = cDelivery["thursday"].GetObject();
                        AddDay("current_schedule", 2, "Thursday", cThursday);
                    JsonObject cFriday = cDelivery["friday"].GetObject();
                        AddDay("current_schedule", 2, "Friday", cFriday);
                    JsonObject cSaturday = cDelivery["saturday"].GetObject();
                        AddDay("current_schedule", 2, "Saturday", cSaturday);
                }

                // availability object, specific to the current_schedule
                JsonObject cAvail = currentSchedObject["availability"].GetObject();
                {
                    bool pickup = cAvail["pickup"].GetBoolean();
                    bool delivery = cAvail["delivery"].GetBoolean();

                    // add the availability object
                    current_schedule.availability = new Availability(pickup, delivery);
                }
            }




        }

        /// <summary>
        /// Take a JsonObject that represents a day, and add it to the corresponding list
        /// </summary>
        /// <param name="schedule"></param> the schedule that this function will add the day to
        /// <param name="dayObject"></param> the JsonObject that represents a day and needs to be parsed to create the Day object
        /// <param name="businessOrDelivery"></param> 1 = business. 2 = delivery
        private void AddDay(string schedule, int businessOrDelivery, string nameOfDay, JsonObject dayObject)
        {
            // make a list for the array of times
            List<Times> times = new List<Times>();

            // prepare variables
            string start = "", end = "";

            // parse the dayObject
            JsonArray tArray = dayObject["times_open"].GetArray();
            
            foreach(JsonValue timePair in tArray)
            {
                JsonObject timeObject = timePair.GetObject();
                start = timeObject["start"].GetString();
                end = timeObject["end"].GetString();

                // make a time object and add it to the List<Times> times
                Times newTimePair = new Times(start, end);
                times.Add(newTimePair);
            }

            // create the day object
            Day newDay = new Day(nameOfDay, times);

            
            if(schedule == "standard_schedule")
            {   // add this to the standard_schedule

                if(businessOrDelivery == 1)
                {   // add to the business list
                    standard_schedule.business.Add(newDay);
                }
                else
                {   // add to the delivery list
                    standard_schedule.delivery.Add(newDay);
                }
            }
            else if(schedule == "current_schedule")
            {   // add this to the current_schedule
                // first add the corresponding date, which only appears in the current_schedule
                string date = dayObject["date"].GetString();
                newDay.date = date;

                if (businessOrDelivery == 1)
                {   // add to the business list
                    current_schedule.business.Add(newDay);
                }
                else
                {   // add to the delivery list
                    current_schedule.delivery.Add(newDay);
                }
            }


        }
    
        
        public async Task GetMenu()
        {
            // keep these consistent
            client_id = (App.Current as App).client_id;
            secret = (App.Current as App).secret;

            // Call an HTTP GET request.  GET /merchant/{merchant_id}/hours
            string baseURL = "https://api.delivery.com/api/merchant/" + uniqueID    // the id of the merchant
                                                                      + "/menu?client_id=" + client_id;

            // Receive the response and parse it (Json)
            var responseString = await client.GetStringAsync(baseURL);
            JsonObject jsObject = JsonObject.Parse(responseString);


            // Get the menu schedule......

            // Check for warnings

            // Get the menu(s)
            int scheduleID = -1;
            JsonArray menusArray = jsObject["menu"].GetArray();

            foreach (JsonValue m in menusArray)
            {
                JsonObject menuObject = m.GetObject();
                string id_menu = "", name_Menu = "", description_Menu = "", ob_type = "";
                id_menu = menuObject["id"].GetString();
                name_Menu = menuObject["name"].GetString();
                description_Menu = menuObject["description"].GetString();
                ob_type = menuObject["type"].GetString();

                // create the empty menu
                Menu newMenu = new Menu(id_menu, name_Menu, description_Menu, ob_type);

////////////////// Get the items of the menu
                JsonArray menuChildren = menuObject["children"].GetArray();
                string id = "", name = "", description = "", min_qty = "", max_qty = "", price = "", max_price = "", itype = "";
                foreach(JsonValue ii in menuChildren)
                {
                    JsonObject itemObj = ii.GetObject();
                    id = itemObj["id"].GetString();
                    name = itemObj["name"].GetString();
                    if(itemObj["description"].ValueType.ToString() == "String")
                        description = itemObj["description"].GetString();
                    
                    
                    // get the schedule id array if it exists for this item
                    List<int> scheduleIDs = new List<int>();
                    if (itemObj.ContainsKey("schedule"))
                    {
                        JsonArray itemSchedule = itemObj["schedule"].GetArray();
                        
                        foreach (JsonValue sVal in itemSchedule)
                        {
                            // get the id of the schedule that this item belongs to
                            scheduleID = Convert.ToInt32(sVal.GetNumber());
                            scheduleIDs.Add(scheduleID);
                        }
                    }

                    if (itemObj["min_qty"].ValueType.ToString() == "Number")
                        min_qty = itemObj["min_qty"].GetNumber().ToString();
                    if (itemObj["max_qty"].ValueType.ToString() == "Number")
                        max_qty = itemObj["max_qty"].GetNumber().ToString();
                    if (itemObj["price"].ValueType.ToString() == "Number")
                        price = itemObj["price"].GetNumber().ToString();
                    if (itemObj["max_price"].ValueType.ToString() == "Number")
                        max_price = itemObj["max_price"].GetNumber().ToString();
                    // get the item type
                    itype = itemObj["type"].GetString();

                    Item newItem = new Item(id, name, description, min_qty, max_qty, price, max_price, itype);
                    
             // attempt to get this item's children (images, price groups, option groups)
                    List<OptionGroup> itemOptions = new List<OptionGroup>();
                    List<PriceGroup> itemPrices = new List<PriceGroup>();
                    List<Image> itemImages = new List<Image>();
                    if (itemObj["children"].ValueType.ToString() == "Array")
                    {
                        JsonArray itemChildren = itemObj["children"].GetArray();

                        // item's children
                        foreach(JsonValue child in itemChildren)
                        {
                            JsonObject c_Object = child.GetObject();
                            string  c_Type = "", c_name = "", c_description = "", c_ID = "",
                                    c_url = "", c_min_selection = "", c_max_selection = "";
                            int c_sel_dep = 0;
                            c_Type = c_Object["type"].GetString();
                            c_ID = c_Object["id"].GetString();
                            if (c_Object["name"].ValueType.ToString() == "String")
                                c_name = c_Object["name"].GetString();
                            if (c_Object["description"].ValueType.ToString() == "String")
                                c_description = c_Object["description"].GetString();

                            // treat differently depending on what the object is
                            if(c_Type == "image")
                            {
                                c_url = c_Object["url"].GetString();
                                
                                Image newImage = new Image(c_ID, c_name, c_url, c_description, c_Type);
                                itemImages.Add(newImage);
                            }
                            else if(c_Type == "price group")
                            {
                                if (c_Object["min_selection"].ValueType.ToString() == "Number")
                                    c_min_selection = c_Object["min_selection"].GetNumber().ToString();
                                if (c_Object["max_selection"].ValueType.ToString() == "Number")
                                    c_max_selection = c_Object["max_selection"].GetNumber().ToString();

                                PriceGroup newPriceGroup = new PriceGroup(c_ID, c_name, c_description, c_min_selection, c_max_selection, c_Type);
                                // add the price group's children
                                if (c_Object["children"].ValueType.ToString() == "Array")
                                    newPriceGroup.childOptions = AddOptions(c_Object["children"].GetArray());
                                itemPrices.Add(newPriceGroup); 
                            }
                            else if(c_Type == "option group")
                            {
                                if (c_Object["min_selection"].ValueType.ToString() == "String")
                                    c_min_selection = c_Object["min_selection"].GetString();
                                if (c_Object["max_selection"].ValueType.ToString() == "String")
                                    c_max_selection = c_Object["max_selection"].GetString();
                                if (c_Object["sel_dep"].ValueType.ToString() == "Number")
                                    c_sel_dep = Convert.ToInt32(c_Object["sel_dep"].GetNumber());

                                OptionGroup newOptionGroup = new OptionGroup(c_ID, c_name, c_description, c_min_selection, c_max_selection, c_sel_dep, c_Type);
                                
                                // add the option group's children. call the AddOptions function in this case
                                if (c_Object["children"].ValueType.ToString() == "Array")
                                    newOptionGroup.childOptions = AddOptions(c_Object["children"].GetArray());

                                itemOptions.Add(newOptionGroup);
                            }
                        }
                        //////////////////////////////////////////////////////
                        newItem._optionGroup = itemOptions;
                        newItem._priceGroup = itemPrices;
                        newItem.Images = itemImages;
                    }
                    // add item to menu's children
                    newMenu.items.Add(newItem);
                }   // end item

                
                
                menus.Add(newMenu);
            }
            int i = 0;

        }

        public List<Option> AddOptions(JsonArray childArray)
        {
            List<Option> rootOptions = new List<Option>();

            string id = "", name = "", description = "", price = "", max_price = "-1", type = "";
            string min_selection = "", max_selection = "";
            int sel_dep = 0;

            foreach(JsonValue ch in childArray)
            {
                JsonObject child = ch.GetObject();

                id = child["id"].GetString();
                if (child["name"].ValueType.ToString() == "String")
                    name = child["name"].GetString();
                if (child["description"].ValueType.ToString() == "String")
                    description = child["description"].GetString();
                if (child["max_price"].ValueType.ToString() == "Number")
                    max_price = child["max_price"].GetNumber().ToString();
                type = child["type"].GetString();

                // cover the situation where there's a nested option group (an option has an option group as its child)
                if( type == "option")
                { 
                    Option cOption = new Option(id, name, description, price, max_price, type);
                    
                    // recursively get the children... except this actually calls a sister function that returns a List<OptionGroup> instead
                    if (child["children"].ValueType.ToString() == "Array")
                        cOption.childOptionGroups = AddOptionGroups(child["children"].GetArray());
                    
                    rootOptions.Add(cOption);
                }            
                
            }

            return rootOptions;
        }

        public List<OptionGroup> AddOptionGroups(JsonArray childArray)
        {
            List<OptionGroup> rootOptionGroups = new List<OptionGroup>();


            string id = "", name = "", description = "", price = "", max_price = "-1", type = "";
            string min_selection = "", max_selection = "";
            int sel_dep = 0;

            foreach (JsonValue ch in childArray)
            {
                JsonObject child = ch.GetObject();

                id = child["id"].GetString();
                if (child["name"].ValueType.ToString() == "String")
                    name = child["name"].GetString();
                if (child["description"].ValueType.ToString() == "String")
                    description = child["description"].GetString();
                if (child["max_price"].ValueType.ToString() == "Number")
                    max_price = child["max_price"].GetNumber().ToString();
                type = child["type"].GetString();


                if ( type == "option group")
                {
                    if (child["min_selection"].ValueType.ToString() == "String")
                        min_selection = child["min_selection"].GetString();
                    if (child["max_selection"].ValueType.ToString() == "String")
                        max_selection = child["max_selection"].GetString();
                    if (child["sel_dep"].ValueType.ToString() == "Number")
                        sel_dep = Convert.ToInt32(child["sel_dep"].GetNumber());

                    OptionGroup cOgroup = new OptionGroup(id, name, description, min_selection, max_selection, sel_dep, type);

                    // recursively get the children
                    if (child["children"].ValueType.ToString() == "Array")
                        cOgroup.childOptions = AddOptions(child["children"].GetArray());
                    
                    rootOptionGroups.Add(cOgroup);
                }
            }

            return rootOptionGroups;
        }


    }

    /// <summary>
    /// the summary for a merchant. contains various details
    /// </summary>
    public class Summary
    {
        //public Summary();
        public Summary(String Name, List<String> Cuisines, String Phone, String Description,
                        int OverallRating, int NumRatings, String Type, String TypeLabel, 
                        String Notes, url merchant_URL, String ActivationDate)
        {
            this.name = Name;
            this.cuisines = Cuisines;
            this.phone = Phone;
            this.description = Description;
            this.overall_rating = OverallRating;
            this.num_ratings = NumRatings;
            this.type = Type;
            this.type_label = TypeLabel;
            this.notes = Notes;
            this.url_merchant = merchant_URL;
            this.activation_date = ActivationDate;
        }

        /****** Variables ******/

        // Name of the merchant.

        public string name { get; set; }

        // Array of different cuisines that this merchant offers.
        public List<string> cuisines { get; set; }

        // Merchant phone number.
        public string phone { get; set; }

        // Description generated by the merchant.
        public string description { get; set; }

        // Rating of the merchant generated by user reviews, on a scale of 1-100. 0 means that the merchant hasn’t had any reviews yet.
        public int overall_rating { get; set; }

        public int num_ratings { get; set; }

        // Type
        public string type { get; set; }

        // Type of merchant. See Merchant Search.
        public string type_label { get; set; }    // R|C|F|I|W|U|P|Z

        // Extra info about ordering from this merchant that is usually more important to the user than the description. E.g. ‘Adding additional meat to a salad is $1.00 extra.’
        public string notes { get; set; }

        // (TODO)A link to the merchant listing on delivery.com. We’ll handle redirecting mobile users as necessary.
        // public string url { get; set; }
        // this is replaced with the url object
        public url url_merchant { get; set; }

        // The date that the merchant went live on delivery.com. The format is YYYY-MM-DD.
        public string activation_date { get; set; }

    }

    public class url
    {
        public url(String geo, String short_t, String complete)
        {
            this.geo_tag = geo;
            this.short_tag = short_t;
            this.completeURL = complete;
        }

        // since geo isn't working, use this constructor
        public url(String short_t, String complete)
        {
            this.short_tag = short_t;
            this.completeURL = complete;
        }

        public string geo_tag { get; set; }
        public string short_tag { get; set; }
        public string completeURL { get; set; }
    }

    /// <summary>
    /// Data related to placing orders from this merchant.
    /// </summary>
    public class Ordering
    {
        //public Ordering();
        public Ordering(    bool DeliveryProcessesCard,
                            List<String> PaymentTypes, 
                            bool IsRDS, 
                            int TimeNeeded, 
                            List<String> Specials,
                            String LastorNextOrderTime, 
                            float Minimum, 
                            bool IsOpen, 
                            float DeliveryCharge,
                            float DeliveryPercent 
                            )
        {
            this.payment_types = PaymentTypes;
            this.is_rds = IsRDS;
            this.time_needed = TimeNeeded;
            this.specials = Specials;
            this.last_or_next_order_time = LastorNextOrderTime;
            this.minimum = Minimum;
            this.is_open = IsOpen;
            this.delivery_charge = DeliveryCharge;
            this.delivery_percent = DeliveryPercent;
            this.delivery_processes_card = DeliveryProcessesCard;
        }

        Availability availability_info;

        /******* Variables ******/

        // The different payment options that this merchant accepts.
        public List<string> payment_types { get; set; }

        // If true, this merchant uses a 3rd party delivery service to deliver orders.
        public bool is_rds { get; set; }

        // How much time a merchant needs to prepare an order. For example, if you’re allowing customers to book future orders, you shouldn’t let them book a pickup order for the minute the merchant opens.
        public int time_needed { get; set; }

        // Array of specials that this merchant offers.
        public List<string> specials { get; set; }

        // If the merchant is currently accepting orders, this is the last time that you can place an order today. If they’re not accepting orders, it’s the next time that you can place an order for.
        public string last_or_next_order_time { get; set; }

        // Minimum order size for pickup/delivery depending on which endpoint you’re calling.
        public float minimum { get; set; }

        // Whether or not this merchant is open at the current point in time.
        public bool is_open { get; set; }

        // The flat fee that this merchant charges for delivery.
        public float delivery_charge { get; set; }

        // The percentage fee that this merchant charges for delivery. This ranges from 0-1, so 15% would be represented as 0.15.
        public float delivery_percent { get; set; }

        // If true, delivery.com charges the customer’s credit card. If false, the merchant charges it.
        public bool delivery_processes_card { get; set; }

    }

    /// <summary>
    /// general info specific properties. there's two versions, one that belongs to schedule,
    /// and one that belongs to ordering
    /// </summary>
    public class Availability
    {
        //public Availability();
        public Availability(bool Pickup, String l_pickup_time, String n_pickup_time, bool Deliver, String l_delivery_time, String n_delivery_time)
        {
            this.pickup = Pickup;
            this.last_pickup_time = l_pickup_time;
            this.next_pickup_time = n_pickup_time;
            this.delivery = Deliver;
            this.last_delivery_time = l_delivery_time;
            this.next_delivery_time = n_delivery_time;
        }

        // constructor for the schedule version
        public Availability(bool PickUp, bool Delivery)
        {
            this.pickup = PickUp;
            this.delivery = Delivery;
        }


        /******** Variables *********/

        // Whether or not this merchant is currently offering pickup orders.
        public bool pickup { get; set; }

        // Only here if pickup is currently availableLast possible pickup order time.
        public string last_pickup_time { get; set; }

        // Only here if pickup is not currently availableNext possible pickup order time.
        public string next_pickup_time { get; set; }

        // Whether or not this merchant is currently offering delivery orders.
        public bool delivery { get; set; }

        // Only here if delivery is not currently available. Last possible delivery order time.
        public string last_delivery_time { get; set; }

        // Only here if delivery is not currently available. Next possible delivery order time.
        public string next_delivery_time { get; set; }
    }


}


