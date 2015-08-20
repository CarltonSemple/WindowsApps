using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.DataTransfer
{
    /// <summary>
    /// Lightweight class to pass request parameter-value pairs with
    /// </summary>
    public class Parameter
    {
        public Parameter()
        {
            key = "";
            value = "";
        }
        public Parameter(string Key, string Value)
        {
            this.key = Key;
            this.value = Value;
        }

        public string key { get; set; }
        public string value { get; set; }
    }
}
