using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Etsy.Model.List
{
    /// <summary>
    /// Duplicate, but simpler, version of the ProductList class, one that the DataContractJsonSerializer can deserialize to.
    /// The values are then copied to the (slightly) more complex ProductList class, which has support for incremental loading
    /// </summary>
    public class ProductListDeserializer
    {
                //private string baseURL;
        public int count { get; set; }
        public ObservableCollection<Listing> results { get; set; }
        public Page_Parameters @params { get; set; }
        public string type { get; set; }
        public Pagination pagination { get; set; }


        public ProductListDeserializer()
        {
            results = new ObservableCollection<Listing>();
            @params = new Page_Parameters();
            pagination = new Pagination();
        }

    }
}
