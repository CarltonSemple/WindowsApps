using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Conversion_App.Resources;

namespace Conversion_App.ViewModels
{
    public class ConversionModel
    {
        public ConversionModel()
        {
            //this.Items = new ObservableCollection<ConversionData>();
        }

        public ConversionGroup length { get; set; }
        public ConversionGroup weight { get; set; }
        public ConversionGroup volume { get; set; }
        public ConversionGroup data { get; set; }
        public ConversionGroup temperature { get; set; }
        public ConversionGroup cooking { get; set; }
        public ConversionGroup energy { get; set; }

        public bool IsDataLoaded { get; set; }

        //public ObservableCollection<ConversionData> Items { get; private set; }


        public void LoadData()
        {
            //Load data into the model

            length = CreatelengthGroup();
            weight = CreateWeightGroup();
            volume = CreateVolumeGroup();
            data = CreateDataGroup();
            temperature = CreateTemperatureGroup();
            cooking = CreateCookingGroup();
            energy = CreateEnergyGroup();

            IsDataLoaded = true;
        }

        

        private ConversionGroup CreatelengthGroup()
        {
            ConversionGroup data = new ConversionGroup();
            data.Title = "length";

            data.Items.Add(new ConversionData { Title = "inches" });
            data.Items.Add(new ConversionData { Title = "feet" });
            data.Items.Add(new ConversionData { Title = "yards" });
            data.Items.Add(new ConversionData { Title = "miles" });
            data.Items.Add(new ConversionData { Title = "millimeters" });
            data.Items.Add(new ConversionData { Title = "centimeters" });
            data.Items.Add(new ConversionData { Title = "meters" });
            data.Items.Add(new ConversionData { Title = "kilometers" });
            //data.Items.RemoveAt(1);

            return data;
        }

        private ConversionGroup CreateWeightGroup()
        {
            ConversionGroup data = new ConversionGroup();
            data.Title = "weight";

            data.Items.Add(new ConversionData { Title = "ounces" });
            data.Items.Add(new ConversionData { Title = "pounds" });
            data.Items.Add(new ConversionData { Title = "tons (US)" });
            data.Items.Add(new ConversionData { Title = "tons (UK)" });
            data.Items.Add(new ConversionData { Title = "metric tons" });
            data.Items.Add(new ConversionData { Title = "grams" });
            data.Items.Add(new ConversionData { Title = "kilograms" });

            return data;
        }

        private ConversionGroup CreateVolumeGroup()
        {
            ConversionGroup data = new ConversionGroup();
            data.Title = "volume";

            data.Items.Add(new ConversionData { Title = "liquid ounces" });
            data.Items.Add(new ConversionData { Title = "pints (US)" });
            data.Items.Add(new ConversionData { Title = "cubic inches" });
            data.Items.Add(new ConversionData { Title = "cubic feet" });
            data.Items.Add(new ConversionData { Title = "cups (US)" });
            data.Items.Add(new ConversionData { Title = "metric cups" });
            data.Items.Add(new ConversionData { Title = "gallons (US)" });
            data.Items.Add(new ConversionData { Title = "milliliters" });
            data.Items.Add(new ConversionData { Title = "centiliters" });
            data.Items.Add(new ConversionData { Title = "liters" });
            data.Items.Add(new ConversionData { Title = "barrels" });


            return data;
        }

        private ConversionGroup CreateDataGroup()
        {
            ConversionGroup data = new ConversionGroup();
            data.Title = "data";

            data.Items.Add(new ConversionData { Title = "bit" });
            data.Items.Add(new ConversionData { Title = "byte" });
            data.Items.Add(new ConversionData { Title = "kilobyte" });
            data.Items.Add(new ConversionData { Title = "megabyte" });
            data.Items.Add(new ConversionData { Title = "gigabyte" });
            data.Items.Add(new ConversionData { Title = "terabyte" });

            return data;
        }

        private ConversionGroup CreateTemperatureGroup()
        {
            ConversionGroup data = new ConversionGroup();
            data.Title = "temperature";

            data.Items.Add(new ConversionData { Title = "kelvin" });
            data.Items.Add(new ConversionData { Title = "celsius" });
            data.Items.Add(new ConversionData { Title = "fahrenheit" });

            return data;
        }

        private ConversionGroup CreateCookingGroup()
        {
            ConversionGroup data = new ConversionGroup();
            data.Title = "cooking";

            data.Items.Add(new ConversionData { Title = "milliliters" });
            data.Items.Add(new ConversionData { Title = "metric cups" });
            data.Items.Add(new ConversionData { Title = "liters" });
            data.Items.Add(new ConversionData { Title = "teaspoons (US)" });
            data.Items.Add(new ConversionData { Title = "tablespoons (US)" });
            data.Items.Add(new ConversionData { Title = "cups (US)" });
            data.Items.Add(new ConversionData { Title = "pints (US)" });
            data.Items.Add(new ConversionData { Title = "quarts (US)" });
            data.Items.Add(new ConversionData { Title = "gallons (US)" });
            data.Items.Add(new ConversionData { Title = "teaspoons (UK)" });
            data.Items.Add(new ConversionData { Title = "tablespoons (UK)" });
            data.Items.Add(new ConversionData { Title = "fluid ounces (UK)" });
            data.Items.Add(new ConversionData { Title = "gallons (UK)" });

            return data;
        }

        private ConversionGroup CreateEnergyGroup()
        {
            ConversionGroup data = new ConversionGroup();
            data.Title = "energy";

            data.Items.Add(new ConversionData { Title = "ounces" });
            data.Items.Add(new ConversionData { Title = "pounds" });
            data.Items.Add(new ConversionData { Title = "tons (US)" });
            data.Items.Add(new ConversionData { Title = "tons (UK)" });
            data.Items.Add(new ConversionData { Title = "metric tons" });
            data.Items.Add(new ConversionData { Title = "grams" });
            data.Items.Add(new ConversionData { Title = "kilograms" });

            return data;
        }

    }
}
