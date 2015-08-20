using System;
using System.Collections.Generic;
using System.Text;

namespace Delivery_com.DataModel
{
    /// <summary>
    /// Detailed order summary. Contains the orderQuickInfo, which is just some of these details
    /// </summary>
    public class OrderDetailed
    {

        // An object which contains the order details.
        public OrderQuickInfo order { get; set; }

        // Time the order was placed in ISO 8601 format.
        public string order_date { get; set; }

        // Requested delivery time of the order in ISO 8601 format. If this is the same as order_date, it was an ASAP order.
        public string delivery_date { get; set; }

        // Unique id for the customer’s address.
        public int location_id { get; set; }

        // Latitude of the location that the order is delivered to.
        public double latitude { get; set; }

        // Longitude of the location that the order is delivered to.
        public double longitude { get; set; }

        // Dollar amount of the order that was discounted by the merchant.
        public float discount { get; set; }

        // Dollar amount of the delivery_fee.
        public float delivery_fee { get; set; }

        // Dollar amount of the tax.
        public float tax { get; set; }

        // Dollar amount of the subtotal.
        public float subtotal { get; set; }

        // Dollar amount of the tip.
        public float tip { get; set; }

        // Dollar amount of the total.
        public float total { get; set; }

        // Dollar amount that was paid with a gift card.
        public float gift_card_amount_used { get; set; }

        // Dollar amount that was paid with a delivery.com promotion.
        public float promo_amount_used { get; set; }

        // How many delivery points were earned for this order.
        public int points_earned { get; set; }

        // Type of the order.
        public string type { get; set; } // pickup | delivery

        // If true, the user has saved this order as a favorite.
        public bool favorite { get; set; } // true | false

        // If the order has been favorited, this is the name the user gave to it, like ‘My cafe Fresco Sunday brunch.’
        public string favorite_name { get; set; } // string | null

/********** Should have a VERTICAL ojbect here **************************************/
        public int MyProperty { get; set; }

        // Array of ‘order entities’ (items, option_groups, options) that describe the contents of the order. The structure is similar to our menu entities. “
        public cart _cart { get; set; }

    }


    /// <summary>
    /// Array of ‘order entities’ (items, option_groups, options) that describe the contents of the order. 
    /// The structure is similar to menu entities. “
    /// </summary>
    public class cart
    {

        // ID of the entity
        public string id { get; set; }

        // The item’s index within the array of entities.
        public int item_key { get; set; }

        // Name of the entity.
        public string name { get; set; }

        // Only on the top level entities (items). Not on options. 
        // Total price of the item, including selected options.
        public float price { get; set; }

        // What quantity of this entity.
        public float quantity { get; set; }

        // Array of child entities. Structure is the same as the ‘top-level entity’ 
        // except there is no price listed.
        public cart[] options { get; set; }
    }


    /// <summary>
    /// Used when viewing multiple orders at once; aka, in order history
    /// OrderDetailed includes this and more
    /// </summary>
    public class OrderQuickInfo
    {

        // Unique id for this order.
        public int order_id { get; set; }

        // Unique id of the merchant that this order was placed with.
        public int merchant_id { get; set; }

        // Name of the merchant.
        public int name { get; set; }

        // Street address of merchant.
        public string street { get; set; }

        // City of merchant.
        public string city { get; set; }

        // 2 character state of the merchant.
        public string state { get; set; }

        // 5 digit zip of the merchant.
        public string zip { get; set; }

        // Phone number of merchant.
        public string phone { get; set; }

        // Whether or not the order was confirmed by the merchant. If this is false, order was most likely cancelled.
        public bool confirmed { get; set; }

        // Any special delivery/pickup instructions left by the user.
        public string instructions { get; set; }

        // Description of the payment method used on the order.
        public string payment_method { get; set; }
    }





}
