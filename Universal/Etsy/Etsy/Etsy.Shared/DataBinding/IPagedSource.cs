using Etsy.DataTransfer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Etsy.DataBinding
{
    public interface IPagedSource<T>
    {
        Task<IPagedResponse<T>> GetPage(int? segments, int? favorite_listings, int? shopID, Int64? shopSectionID, string desiredlist, string query, List<Parameter> Parameters, int? country_shipTo_id, int? pageIndex, int? pageSize);
    }
}
