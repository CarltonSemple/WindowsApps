using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.Shop
{
    public class PaymentTemplateHolder
    {
        public int count { get; set; }
        public List<PaymentTemplate> results { get; set; }
        public Params @params { get; set; }
        public string type { get; set; }
        public Pagination pagination { get; set; }
    }
}
