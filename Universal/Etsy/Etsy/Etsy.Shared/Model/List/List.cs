using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Etsy.Model.List;
using Etsy.DataBinding;
using System.Threading.Tasks;

namespace Etsy.Model.List
{
    /// <summary>
    /// Container for product listings. 
    /// Inherits the IncrementalLoadingBase class for data binding loading while scrolling
    /// </summary>
    public class ProductList 
    {
        //private string baseURL;
        public int count { get; set; }
        public ObservableCollection<Listing> results { get; set; }
        public Page_Parameters @params { get; set; }
        public string type { get; set; }
        public Pagination pagination { get; set; }

        // Extra
        public string listType { get; set; }        // ex: active, trending... to be put into the baseURL
        public string searchTerm { get; set; }      // keep track of what the list contains. updated when the list obtains new items
        public int page { get; set; }       // used to keep track of what page the list has loaded up to
        
        public ProductList()
        {
            results = new ObservableCollection<Listing>();
            @params = new Page_Parameters();
            pagination = new Pagination();
            page = 1;
        }

        public ProductList(string Type)
        {
            listType = Type;
            page = 1;
            results = new ObservableCollection<Listing>();
            @params = new Page_Parameters();
            pagination = new Pagination();
        }

        /// <summary>
        /// Acts as a constructor for the variables that rely on the variables obtained through a serializer
        /// </summary>
        public void InitializeOthersAfterDeserializing()
        {
            
        }

        //protected async override Task<IList<object>> LoadMoreItemsOverrideAsync(System.Threading.CancellationToken c, uint count)
        //{
        //    //uint toGenerate = System.Math.Min(count, _maxCount - _count);

        //    // Wait for work 
        //    await Task.Delay(10);

        //    // This code simply generates
        //    var values = await App.getData.getListofListings(listType, searchTerm, this);   // get more from the same list/search

        //    // subarray with only the newest items
        //    List<Listing> nList = new List<Listing>();
        //    for (int i = 0; i < results.Count; i++)
        //        nList.Add(results[i]);

        //    _count = Convert.ToUInt32(results.Count);   // update starting position

        //    return nList.ToArray();
        //}

        //protected override bool HasMoreItemsOverride()
        //{
        //    return _count < _maxCount;
        //}

        //#region State

        ////Func<int, T> _generator;
        //uint _count = 0;
        //uint _maxCount;

        //#endregion 

    }
}
