using System;
using System.Collections.Generic;
using System.Text;

namespace Etsy.DataBinding
{
    public interface IPagedResponse<T>
    {
        IEnumerable<T> Items { get; }
        int VirtualCount { get; }
    }
}
