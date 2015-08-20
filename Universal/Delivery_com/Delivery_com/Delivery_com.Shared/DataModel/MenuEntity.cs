using System;
using System.Collections.Generic;
using System.Text;

namespace Delivery_com.DataModel
{

    /// <summary>
    /// acts as a template for menu, item, option group.
    /// those three will inherit from this
    /// </summary>
    public class Entity
    {
        public Entity() { type = ""; }

        public string id { get; set; }
        
        public string description { get; set; }

        // If the entity is only valid at certain times, this array will be present and contain the schedule ids. The schedules are listed in the Menu Endpoint. This property is only included when applicable, and only on menus and items.
        public int[] schedule { get; set; }
        public string type { get; set; }

        // sub-menus, sub-options
        // The child entities of this entity. For example, an option group would have an array of options as children.
        public Entity[] children { get; set; }
        
    }


    // Menus are collec
    public class Menu : Entity
    {
        public Menu(string ID, string Name, string Description, string Type)
        {
            this.id = ID;
            this.name = Name;
            this.description = Description;
            this.type = Type;
            items = new List<Item>();
        }

        public string name { get; set; }

        public List<Item> items { get; set; }
    }


    public class Item : Entity
    {

        public Item()
        {

        }

        public Item(string id, string name, string description, string min_qty1, string max_qty1, string price1, string max_price1, string itype)
        {
            // TODO: Complete member initialization
            this.id = id;
            this.name = name;
            this.description = description;
            this.min_qty = Convert.ToSingle(min_qty1);
            this.max_qty = Convert.ToSingle(max_qty1);
            this.price = Convert.ToSingle(price1);
            this.max_price = Convert.ToSingle(max_price1);
            this.type = itype;
        }
        






        /********** properties ***********/

        public string name { get; set; }

        // The mininum quantity that you can select of this item. If you have a quantity select, this should be the first option.
        public float min_qty { get; set; }

        // The maximum quantity that you can select of this item. If you have a quantity select, this should be the last option.
        public float max_qty { get; set; }

        // The base price of this item.
        public float price { get; set; }

        // How much this item could cost after selecting options.
        public float max_price { get; set; }

        // How much to increment the quantity select by. This is usually 1, but if you’re buying something like deli meat, it might be 0.25 (for lb.)
        public float increment { get; set; }

        // Sometimes included. Description of the quantity select. For most items, you’re just selecting {increment} {name}, like ’1 Burger’, but sometimes you’re selecting {qty * increment} {qty_name_singular} of {name}, like ’1 lb of Chicken Salad’.
        public string qty_name_singular { get; set; }

        // Sometimes included. See above, except with the example ’1.5 lbs of Chicken Salad’.
        public string qty_name_plural { get; set; }

        public List<OptionGroup> _optionGroup { get; set; }

        public List<PriceGroup> _priceGroup { get; set; }

        public List<Image> Images { get; set; }
    }


    public class OptionGroup : Entity
    {
        public OptionGroup()
        {

        }


        public OptionGroup(string id, string name, string description, string min_selection1, string max_selection1, int sel_dep1, string type)
        {
            // TODO: Complete member initialization
            this.id = id;
            this.name = name;
            this.description = description;
            this.min_selection = min_selection1;
            this.max_selection = max_selection1;
            this.sel_dep = sel_dep1;
            this.type = type;
        }
        //public OptionGroup();

        public string name { get; set; }

        // The minimum number of children options that must be selected within this option group.
        public string min_selection { get; set; }

        // The maximum number of children options that can be selected within this option group.
        public string max_selection { get; set; }

        // When this equals 1, the ‘min/max_selection’ values should be multiplied by the quantity that the user selects. For example, if a the item is “Dozen Bagels” with a min and max selection of 12 and the user selects a quantity of 2 (dozen bagels), the actual min and max selection would be 24.
        public int sel_dep { get; set; }

        public List<Option> childOptions { get; set; }

    }

    /*Some items have options. An option group (“Pizza Toppings”) is a collection of options (“Cheese, Mushrooms”).*/
    public class Option : Entity
    {
        public Option(string id, string name, string description, string price1, string max_price1, string type)
        {
            // TODO: Complete member initialization
            this.id = id;
            this.name = name;
            this.description = description;
            if (price1 == "")
                this.price = 0;
            else
                this.price = Convert.ToSingle(price1);
            if (max_price1 == "")
                this.max_price = 0;
            else
                this.max_price = Convert.ToSingle(max_price1);
            this.type = type;
        }
        //public Option();

        public string name { get; set; }

        // The base price of this option.
        public float price { get; set; }

        // The maximum price of this option when considering children entities that have additional charges.
        public float max_price { get; set; }

        // in case the option has an option group(s) as its children
        public List<OptionGroup> childOptionGroups { get; set; }

    }

    public class PriceGroup : OptionGroup // price groups have the same properties as option groups
    {
        public PriceGroup()
        {
            this.id = "-1";
        }

        
        public PriceGroup(string c_ID, string c_name, string c_description, string c_min_selection, string c_max_selection, string c_Type)
        {
            // TODO: Complete member initialization
            this.id = c_ID;
            this.name = c_name;
            this.description = c_description;
            this.min_selection = c_min_selection;
            this.max_selection = c_max_selection;
            this.type = c_Type;
        }
       

    }


    public class Image : Entity
    {
        public Image(string c_ID, string c_name, string c_url, string c_description, string c_Type)
        {
            // TODO: Complete member initialization
            this.id = c_ID;
            this.name = c_name;
            this.url = c_url;
            this.description = c_description;
            this.type = c_Type;
        }
        //public Image();

        public string name { get; set; }

        // The relative URL of this image. You can find the actual article by prefixing it with https://www.delivery.com.
        public string url { get; set; }
    }


}
