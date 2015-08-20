using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.Model
{
    public class KeyEnumPair<K, E, X>
    {
        public K key { get; set; }

        public E enumTemplate { get; set; }

        public X extraValue { get; set; }     // can be used to mark something such as a touch

        public KeyEnumPair(K Key, E enumtemplate, X extra)
        {
            this.key = Key;
            this.enumTemplate = enumtemplate;
            this.extraValue = extra;
        }

    }
}
