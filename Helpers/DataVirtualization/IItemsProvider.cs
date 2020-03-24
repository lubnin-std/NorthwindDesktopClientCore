using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NorthwindDesktopClientCore.Helpers.DataVirtualization
{
    public interface IItemsProvider<T>
    {
        public int FetchCount<T>() where T : class;
        IEnumerable<T> FetchRange<T>(int startIndex, int count) where T : class;
    }
}
