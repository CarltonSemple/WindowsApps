using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.Image
{
    public class Image
    {
        public int listing_image_id { get; set; }
        public string hex_code { get; set; }
        public int? red { get; set; }
        public int? green { get; set; }
        public int? blue { get; set; }
        public int? hue { get; set; }
        public int? saturation { get; set; }
        public int? brightness { get; set; }
        public Nullable<bool> is_black_and_white { get; set; }
        public int? creation_tsz { get; set; }
        public int? listing_id { get; set; }
        public int? rank { get; set; }
        public string url_75x75 { get; set; }
        public string url_170x135 { get; set; }
        public string url_570xN { get; set; }
        public string url_fullxfull { get; set; }
        public int? full_height { get; set; }
        public int? full_width { get; set; }
    }
}
