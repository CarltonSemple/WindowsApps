using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Etsy.Model.Image
{
    public class ImageList
    {
        public int count { get; set; }
        public ObservableCollection<Image> results { get; set; }
        public Params @params { get; set; }
        public string type { get; set; }
        public Pagination pagination { get; set; }
    }
}
