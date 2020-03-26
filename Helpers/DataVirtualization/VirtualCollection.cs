using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Diagnostics;

namespace NorthwindDesktopClientCore.Helpers.DataVirtualization
{
    public class VirtualCollection<T> : ObservableCollection<T>, IList<T>, IList where T : class
    {
        public VirtualCollection(IItemsProvider<T> itemsProvider)
        {
            ItemsProvider = itemsProvider;
        }

        public VirtualCollection(IItemsProvider<T> itemsProvider, int pageSize)
            : this(itemsProvider)
        {
            PageSize = pageSize;
        }

        public VirtualCollection(IItemsProvider<T> itemsProvider, int pageSize, long pageTimeout)
            : this(itemsProvider, pageSize)
        {
            PageTimeout = pageTimeout;
        }

        public IItemsProvider<T> ItemsProvider { get; }
        public int PageSize { get; } = 30;
        public long PageTimeout { get; } = 10_000;  // Милисекунды, 1000ms = 1s

        private class Page
        {
            public int Index { get; set; }
            public IList<T> Items { get; set; }
            public DateTime LastAccessTime { get; set; }
            public double Timeout => (DateTime.Now - LastAccessTime).TotalMilliseconds;
            public T GetItem(int offset) => Items[offset];
        }
        private readonly Dictionary<int, Page> _pages = new Dictionary<int, Page>();

        private int _count = -1;
        public new int Count {
            get {
                if (_count == -1)
                    _count = ItemsProvider.FetchCount();
                return _count;
            }
            protected set {
                _count = value;
            }
        }

        public new T this[int index] {
            get { return FetchItem(index); }
            set { throw new NotSupportedException(); }
        }

        object IList.this[int index] {
            get { return this[index]; }
            set { throw new NotSupportedException(); }
        }

        public new IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        private T FetchItem(int index)
        {
            // Определить номер страницы и позицию элемента относительно начала страницы
            int pageIndex = index / PageSize;
            int pageOffset = index % PageSize;

            // Запросить основную страницу
            RequestPage(pageIndex);

            // Если страницу уже пролистали на + или -50%, запросить след\предыдущую
            // && защита от загрузки страницы с номером maxНомер+1
            if (pageOffset > PageSize / 2 && pageIndex < Count / PageSize)
                RequestPage(pageIndex + 1);
            if (pageOffset < PageSize / 2 && pageIndex > 0)
                RequestPage(pageIndex - 1);

            // Удалить страницы, к которым долгое время не обращались
            CleanUpPages();

            // Защитная проверка в случае асинхронной загрузки
            if (_pages[pageIndex] == null)
                return default(T);

            // Вернуть запрошенный элемент данных
            // Такой доступ через [pageOffset] требует от набора данных страницы иметь индексатор
            return _pages[pageIndex].GetItem(pageOffset);
        }


        protected void RequestPage(int pageIndex)
        {
            if (PageExists(pageIndex))
            {
                UpdatePageAccessTime(pageIndex);
            }
            else
            {
                CreatePage(pageIndex);
            }
        }

        private bool PageExists(int pageIndex)
        {
            return _pages.ContainsKey(pageIndex);
        }

        private void UpdatePageAccessTime(int pageIndex)
        {
            Page page = _pages[pageIndex];
            page.LastAccessTime = DateTime.Now;
        }

        private void CreatePage(int pageIndex)
        {
            var page = new Page()
            {
                Index = pageIndex,
                LastAccessTime = DateTime.Now,
                Items = FetchPage(pageIndex)
            };
            _pages.Add(pageIndex, page);
            Debug.Print("Page #{0} created", pageIndex);
        }

        private IList<T> FetchPage(int pageIndex)
        {
            return ItemsProvider.FetchRange(pageIndex * PageSize, PageSize);
        }

        public void CleanUpPages()
        {
            foreach (var page in _pages.Values)
            {
                // Контрол ItemsControl часто обращается к первому элементу коллекции
                // Это особенность Wpf, поэтому трогать первую страницу не будем
                if (page.Index != 0)
                {
                    if (page.Timeout > PageTimeout)
                    {
                        Debug.Print("Page #{0} deleted", page.Index);
                        _pages.Remove(page.Index);
                    }
                }
            }
        }
    }
}
