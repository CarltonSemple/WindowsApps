using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Etsy.Model.Browse
{
    public class BrowseSegment
    {
        public string name { get; set; }
        public string standalone_text { get; set; }
        public string path { get; set; }
        public string short_name { get; set; }
        public bool has_children { get; set; }
        public object MainImage { get; set; }  // featured image
        public string image_url { get; set; }   // featured listing image
        public int shop_id { get; set; }        // shop id of the featured listing
        public string shop_name { get; set; }   // shop name of the featured listing 
        public int listing_id { get; set; }     // featured listing

        public ObservableCollection<BrowseSegment> sub_sections { get; set; }   // Sub-sections
    }

    public class SectionContainer
    {
        public int count { get; set; }
        public ObservableCollection<BrowseSegment> results { get; set; }
        public Params @params { get; set; }
        public string type { get; set; }
        public Pagination pagination { get; set; }
    }

    public class Params
    {
        public string path { get; set; }
    }

    public class Pagination
    {
    }
}
