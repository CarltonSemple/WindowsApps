using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

using Etsy.DataTransfer;

namespace Etsy.DataBinding
{
    public class IncrementalSource<T, K> : ObservableCollection<K>, ISupportIncrementalLoading
        where T : IPagedSource<K>, new()
    {
        //public ObservableCollection<K> savedList = new ObservableCollection<K>();
        public int? segments { get; set; }
        public int? favorite_listings { get; set; }
        public int? shop_id { get; set; }
        public Int64? shop_section_id { get; set; }
        private List<Parameter> parameters { get; set; }
        public int? country_shipTo_id { get; set; }
        public string DesiredList { get; set; }
        public string Query { get; set; }
        public int? VirtualCount { get; set; }
        public int? CurrentPage { get; set; }
        private IPagedSource<K> Source { get; set; }

        public IncrementalSource()
        {
            this.Source = new T();
            this.VirtualCount = int.MaxValue;
            this.CurrentPage = 0;
            this.Query = "";
            this.country_shipTo_id = -1;
            this.favorite_listings = -1;
            this.segments = -1;
        }

        public IncrementalSource(string query)
        {
            this.shop_id = -1;
            this.shop_section_id = -1;
            this.Source = new T();
            this.VirtualCount = int.MaxValue;
            this.CurrentPage = 0;
            this.Query = query;
            this.country_shipTo_id = -1;
            this.favorite_listings = -1;
            this.segments = -1;
        }

        public IncrementalSource(string desiredList, string query, List<Parameter> Parameters)
        {
            this.shop_id = -1;
            this.shop_section_id = -1;
            this.DesiredList = desiredList;
            this.Source = new T();
            this.VirtualCount = int.MaxValue;
            this.CurrentPage = 0;
            this.Query = query;
            this.parameters = Parameters;
            this.country_shipTo_id = -1;
            this.favorite_listings = -1;
            this.segments = -1;
        }

        public IncrementalSource(string desiredList, string query, List<Parameter> Parameters, int country_shipToID)
        {
            this.shop_id = -1;
            this.shop_section_id = -1;
            this.DesiredList = desiredList;
            this.Source = new T();
            this.VirtualCount = int.MaxValue;
            this.CurrentPage = 0;
            this.Query = query;
            this.parameters = Parameters;
            this.country_shipTo_id = country_shipToID;
            this.favorite_listings = -1;
            this.segments = -1;
        }

        // Used when looking at a shop's listings
        public IncrementalSource(int? shopID, string desiredList, string query, List<Parameter> Parameters, int country_shipToID)
        {
            this.shop_id = shopID;
            this.shop_section_id = -1;
            this.DesiredList = desiredList;
            this.Source = new T();
            this.VirtualCount = int.MaxValue;
            this.CurrentPage = 0;
            this.Query = query;
            this.parameters = Parameters;
            this.country_shipTo_id = country_shipToID;
            this.favorite_listings = -1;
            this.segments = -1;
        }

        // Meant for shop section
        public IncrementalSource(int shopID, Int64 shopSectionID, string desiredList, string query, List<Parameter> Parameters, int country_shipToID)
        {
            this.shop_id = shopID;
            this.shop_section_id = shopSectionID;
            this.DesiredList = desiredList;
            this.Source = new T();
            this.VirtualCount = int.MaxValue;
            this.CurrentPage = 0;
            this.Query = query;
            this.parameters = Parameters;
            this.country_shipTo_id = country_shipToID;
            this.favorite_listings = -1;
            this.segments = -1;
        }

        // Favorite listings Constructor
        // Either favorites = -1 or segments = -1
        public IncrementalSource(int favorites, int Segments, List<Parameter> Parameters)
        {
            this.shop_id = -1;
            this.shop_section_id = -1;
            this.DesiredList = "";
            this.Source = new T();
            this.VirtualCount = int.MaxValue;
            this.CurrentPage = 0;
            this.Query = "";
            this.parameters = Parameters;
            this.country_shipTo_id = App.defaultAddress.country_id;
            this.favorite_listings = favorites;
            this.segments = Segments;
        }

        #region ISupportIncrementalLoading

        public bool HasMoreItems
        {
            get { return this.VirtualCount > this.CurrentPage * 25; }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            CoreDispatcher dispatcher = Window.Current.Dispatcher;

            return Task.Run<LoadMoreItemsResult>(
                async () =>
                {
#if WINDOWS_APP 
                    IPagedResponse<K> result = await Source.GetPage(this.segments, this.favorite_listings, this.shop_id, this.shop_section_id, this.DesiredList, this.Query, parameters, country_shipTo_id, ++this.CurrentPage, 10);   //this.Source.GetPage(this.Query, ++this.CurrentPage, 25);
#endif

#if WINDOWS_PHONE_APP // Load less items at a time for the phone
                    IPagedResponse<K> result = await Source.GetPage(this.segments, this.favorite_listings, this.shop_id, this.shop_section_id, this.DesiredList, this.Query, parameters, country_shipTo_id, ++this.CurrentPage, 6);   //this.Source.GetPage(this.Query, ++this.CurrentPage, 25);
#endif
                    this.VirtualCount = result.VirtualCount;

                    await dispatcher.RunAsync(
                        CoreDispatcherPriority.Normal,
                        () =>
                        {
                            foreach (K item in result.Items)
                            {
                                this.Add(item);
                                //savedList.Add(item);
                            }
                        });

                    return new LoadMoreItemsResult() { Count = (uint)result.Items.Count() };

                }).AsAsyncOperation<LoadMoreItemsResult>();
        }

        #endregion

        /// <summary>
        /// Save the variables that the DataContractJsonSerializer would not
        /// </summary>
        /// <typeparam name="IncrementalSource"></typeparam>
        /// <param name="incrementalSource"></param>
        /// <param name="fileName"></param>
        public async Task saveAttributes(string fileName)
        {
            VariableContainer contain = new VariableContainer(  // capture the variables
                                                                this.segments,
                                                                this.favorite_listings,
                                                                this.shop_id,
                                                                this.shop_section_id,
                                                                this.DesiredList,
                                                                this.Query,
                                                                this.VirtualCount,
                                                                this.CurrentPage,
                                                                this.parameters,
                                                                this.country_shipTo_id);
            try
            {
                await FileIO.SerializeAndSave(contain, fileName);
            }
            catch(Exception e)
            {
                string message = e.Message;
            }
            return;
        }

        /// <summary>
        /// Load the variables that the DataContractJsonSerializer would not
        /// </summary>
        /// <param name="fileName"></param>
        public async Task loadAttributes(string fileName)
        {
            VariableContainer capture = await FileIO.DeserializeFromFile<VariableContainer>(fileName);

            this.DesiredList = capture.DesiredList;
            this.Query = capture.Query;
            this.VirtualCount = capture.VirtualCount;
            this.CurrentPage = capture.CurrentPage;
            this.parameters = capture.parameters;
            this.country_shipTo_id = capture.country_shipTo_id;
            this.shop_id = capture.shop_id;
            this.shop_section_id = capture.shop_section_id;
            this.favorite_listings = capture.favorite_listings;
            this.segments = capture.segments;

            return;
        }

    }

    /// <summary>
    /// Hold the variables from IncrementalSource that the DataContractJsonSerializer wouldn't save
    /// </summary>
    public class VariableContainer
    {
        public int? segments { get; set; }
        public int? favorite_listings { get; set; }
        public int? shop_id { get; set; }
        public Int64? shop_section_id { get; set; }
        public List<Parameter> parameters { get; set; }
        public int? country_shipTo_id { get; set; }
        public string DesiredList { get; set; }
        public string Query { get; set; }
        public int? VirtualCount { get; set; }
        public int? CurrentPage { get; set; }

        public VariableContainer() { }

        public VariableContainer(int? Segments, int? favorites, int? shopID, Int64? shopSectionID, string d, string q, int? v, int? c, List<Parameter> par, int? countryID)
        {
            this.DesiredList = d;
            this.Query = q;
            this.VirtualCount = v;
            this.CurrentPage = c;
            this.parameters = new List<Parameter>();
            parameters.AddRange(par);
            this.country_shipTo_id = countryID;
            this.shop_id = shopID;
            this.shop_section_id = shopSectionID;
            this.favorite_listings = favorites;
            this.segments = Segments;
        }
    }
}
