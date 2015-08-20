using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Conversion_App.ViewModels
{
    public class ConversionGroup
    {
        public ConversionGroup()
        {
            Items = new List<ConversionData>();
        }
        public List<ConversionData> Items { get; set; }
        public string Title { get; set; }
    }
}
