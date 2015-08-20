using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Conversion_App.Resources;

namespace Conversion_App.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
            //System.Collections.Generic.List<Model.WeightCategory> weightCategories = new System.Collections.Generic.List<Model.WeightCategory>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }













      

        //public ConversionGroup length { get; set; }
        //public ConversionGroup weight { get; set; }
        //public ConversionGroup volume { get; set; }
        //public ConversionGroup temperature { get; set; }
        //public ConversionGroup energy { get; set; }

        ////public bool IsDataLoaded { get; set; }

        ////public ObservableCollection<ConversionData> Items { get; private set; }


        ////public void LoadData()
        ////{
        ////    //Load data into the model

        ////    length = CreatelengthGroup();
        ////    weight = CreateWeightGroup();

        ////    IsDataLoaded = true;
        ////}


        //private ConversionGroup CreatelengthGroup()
        //{
        //    //this.Items = new ObservableCollection<ConversionData>();

        //    ConversionGroup data = new ConversionGroup();
        //    data.Title = "length";

        //    data.Items.Add(new ConversionData { Title = "inches" });
        //    data.Items.Add(new ConversionData { Title = "feet" });
        //    data.Items.Add(new ConversionData { Title = "yards" });
        //    data.Items.Add(new ConversionData { Title = "miles" });
        //    data.Items.Add(new ConversionData { Title = "millimeters" });
        //    data.Items.Add(new ConversionData { Title = "centimeters" });
        //    data.Items.Add(new ConversionData { Title = "meters" });
        //    data.Items.Add(new ConversionData { Title = "kilometers" });

        //    return data;
        //}

        //private ConversionGroup CreateWeightGroup()
        //{
        //    ConversionGroup data = new ConversionGroup();
        //    data.Title = "weight";

        //    data.Items.Add(new ConversionData { Title = "ounces" });
        //    data.Items.Add(new ConversionData { Title = "pounds" });
        //    data.Items.Add(new ConversionData { Title = "tons (US)" });
        //    data.Items.Add(new ConversionData { Title = "tons (UK)" });
        //    data.Items.Add(new ConversionData { Title = "metric tons" });
        //    data.Items.Add(new ConversionData { Title = "grams" });
        //    data.Items.Add(new ConversionData { Title = "kilograms" });

        //    return data;
        //}

















        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {

            //length = CreatelengthGroup();
            //weight = CreateWeightGroup();
            // Sample data; replace with real data
            //this.Items.Add(new ItemViewModel() { LineOne = "inches", LineTwo = "ounces", LineThree = "Facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu" });
            //this.Items.Add(new ItemViewModel() { LineOne = "feet", LineTwo = "pounds", LineThree = "Suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus" });
            //this.Items.Add(new ItemViewModel() { LineOne = "yards", LineTwo = "tons (US)", LineThree = "Habitant inceptos interdum lobortis nascetur pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent" });
            //this.Items.Add(new ItemViewModel() { LineOne = "miles", LineTwo = "tons (UK)", LineThree = "Ultrices vehicula volutpat maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos" });
            //this.Items.Add(new ItemViewModel() { LineOne = "millimeters", LineTwo = "metric tons", LineThree = "Maecenas praesent accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur" });
            //this.Items.Add(new ItemViewModel() { LineOne = "centimeters", LineTwo = "grams", LineThree = "Pharetra placerat pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent" });
            //this.Items.Add(new ItemViewModel() { LineOne = "meters", LineTwo = "kilograms", LineThree = "Accumsan bibendum dictumst eleifend facilisi faucibus habitant inceptos interdum lobortis nascetur pharetra placerat" });
            //this.Items.Add(new ItemViewModel() { LineOne = "kilometers", LineThree = "Pulvinar sagittis senectus sociosqu suscipit torquent ultrices vehicula volutpat maecenas praesent accumsan bibendum" });

            
            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}