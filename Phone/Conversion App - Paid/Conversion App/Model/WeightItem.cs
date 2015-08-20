using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversion_App.Model
{
    public class WeightItem
    {
        public string Name { get; private set; }
        //public System.Collections.Generic.List<WeightItem> Items { get; private set; }

        public WeightItem(string weightUnit)
        {
            Name = weightUnit;
        }
    }
}
