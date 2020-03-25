using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace NorthwindDesktopClientCore.Helpers.DataVirtualization
{
    public interface IItemsProvider<T> where T : class
    {
        public int FetchCount();
        // Должен возвращать IList, потому что к элементу страницы будет доступ по индексу
        IList<T> FetchRange(int startIndex, int count);
    }
}
