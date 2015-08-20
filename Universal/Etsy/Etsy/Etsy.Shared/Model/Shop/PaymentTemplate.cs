using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model.Shop
{
    public class PaymentTemplate
    {
        public int payment_template_id { get; set; }    // id
        public bool allow_bt { get; set; }          // bank transfers
        public bool allow_check { get; set; }       // checks
        public bool allow_mo { get; set; }          // money order
        public bool allow_other { get; set; }       // other payments
        public bool allow_paypal { get; set; }      // paypal
        public bool allow_cc { get; set; }          // credit cards
        public int listing_payment_id { get; set; } // Provided for backward compatibility to ListingPayment. This will return the same value as payment_template_id.

    }
}
