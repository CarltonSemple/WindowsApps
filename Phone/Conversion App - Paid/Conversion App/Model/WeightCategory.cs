using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//apparently needed so that the LongListSelector can see the items...

namespace Conversion_App.Model
{
    public class WeightCategory : System.Collections.IEnumerable
    {
        public string Name { get; private set; }
        public System.Collections.Generic.List<WeightItem> Items { get; private set; }

        public WeightCategory(string categoryName)
        {
            Name = categoryName;
            Items = new System.Collections.Generic.List<WeightItem>();
        }

        public void AddWeightItem(WeightItem weightItem)
        {
            Items.Add(weightItem);
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }
    }
}
