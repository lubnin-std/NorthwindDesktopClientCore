using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindDesktopClientCore.Helpers.DataVirtualisation
{
    public interface IItemsProvider<T>
    {
        public int FetchCount();
        IList<T> FetchRange(int startIndex, int count);
    }
}
