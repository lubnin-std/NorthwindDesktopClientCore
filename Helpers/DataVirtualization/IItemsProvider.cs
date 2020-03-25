using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NorthwindDesktopClientCore.Helpers.DataVirtualization
{
    public interface IItemsProvider<T> where T : class
    {
        public int FetchCount();
        IEnumerable<T> FetchRange(int startIndex, int count);
    }
}
