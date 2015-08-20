using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Etsy.Model.Shop.About
{
    public class AboutDeserializeCapturer
    {
        public int count { get; set; }
        public ObservableCollection<ShopAbout> results { get; set; }
        public Params @params { get; set; }
        public string type { get; set; }
        public Pagination pagination { get; set; }
    }
}
