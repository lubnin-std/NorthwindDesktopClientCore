using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace NorthwindDesktopClientCore.Helpers.DataVirtualisation
{
    public class VirtualCollection<T> : IList<T>, IList
    {
        #region VirtualCollection<T> part
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
        public long PageTimeout { get; } = 10000;

        private readonly Dictionary<int, IList<T>> _pages = new Dictionary<int, IList<T>>();
        private readonly Dictionary<int, DateTime> _pageTouchTimes = new Dictionary<int, DateTime>();

        private int _count = -1;
        public virtual int Count {
            get {
                if (_count == -1)
                    LoadCount();
                return _count;
            }
            protected set {
                _count = value;
            }
        }

        protected virtual void LoadCount()
        {
            Count = FetchCount();
        }

        protected int FetchCount()
        {
            return ItemsProvider.FetchCount();
        }

        public T this[int index] {
            get {
                // Определить номер страницы и смещение внутри страницы
                int pageIndex = index / PageSize;
                int pageOffset = index % PageSize;

                // Запросить основную страницу
                RequestPage(pageIndex);

                // Если страницу уже пролистали на + или -50%, запросить след\предыдущую
                if (pageOffset > PageSize/2 && pageIndex < Count / PageSize)
                    RequestPage(pageIndex + 1);
                if (pageOffset < PageSize/2 && pageIndex > 0)
                    RequestPage(pageIndex - 1);

                // Удалить страницы, к которым долгое время не обращались
                CleanUpPages();

                // Защитная проверка в случае асинхронной загрузки
                if (_pages[pageIndex] == null)
                    return default(T);

                // Вернуть запрошенный элемент данных
                return _pages[pageIndex][pageOffset];
            }
            set { throw new NotSupportedException(); }
        }

        object IList.this[int index] {
            get { return this[index]; }
            set { throw new NotSupportedException(); }
        }

        protected virtual void RequestPage(int pageIndex)
        {
            if (!_pages.ContainsKey(pageIndex))
            {
                _pages.Add(pageIndex, null);
                _pageTouchTimes.Add(pageIndex, DateTime.Now);
                LoadPage(pageIndex);
            }
            else
            {
                _pageTouchTimes[pageIndex] = DateTime.Now;
            }
        }

        protected virtual void LoadPage(int pageIndex)
        {
            PopulatePage(pageIndex, FetchPage(pageIndex));
        }

        protected virtual void PopulatePage(int pageIndex, IList<T> page)
        {
            if (_pages.ContainsKey(pageIndex))
                _pages[pageIndex] = page;
        }

        protected IList<T> FetchPage(int pageIndex)
        {
            return ItemsProvider.FetchRange(pageIndex * PageSize, PageSize);
        }

        public void CleanUpPages()
        {
            List<int> pageIndexes = new List<int>(_pageTouchTimes.Keys);
            foreach (int pi in pageIndexes)
            {
                // Контрол ItemsControl часто обращается к первому элементу коллекции
                // Это особенность Wpf, поэтому трогать первую страницу не будем
                if (pi != 0)
                {
                    if ((DateTime.Now - _pageTouchTimes[pi]).TotalMilliseconds > PageTimeout)
                    {
                        _pages.Remove(pi);
                        _pageTouchTimes.Remove(pi);
                    }
                }
            }
        }
        #endregion


        // 99% функционала типичной коллекции не относится к назначению класса и поэтому не поддерживается
        #region Заглушки для функционала обычной коллекции
        public IEnumerator<T> GetEnumerator()
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

        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        int IList.Add(object value)
        {
            throw new NotSupportedException();
        }

        bool IList.Contains(object value)
        {
            return Contains((T)value);
        }

        public bool Contains(T item)
        {
            return false;
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        int IList.IndexOf(object value)
        {
            return IndexOf((T)value);
        }

        public int IndexOf(T item)
        {
            return -1;
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, (T)value);
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object value)
        {
            throw new NotSupportedException();
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotSupportedException();
        }

        public object SyncRoot {
            get { return this; }
        }

        public bool IsSynchronized {
            get { return false; }
        }

        public bool IsReadOnly {
            get { return true; }
        }

        public bool IsFixedSize {
            get { return false; }
        }
        #endregion
    }
}
