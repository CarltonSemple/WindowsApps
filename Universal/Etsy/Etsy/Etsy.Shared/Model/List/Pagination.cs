using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.List
{
    public class Pagination
    {
        public int? effective_limit { get; set; }
        public int? effective_offset { get; set; }
        public int? next_offset { get; set; }
        public int? effective_page { get; set; }
        public int? next_page { get; set; }
    }
}
