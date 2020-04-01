using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace NorthwindDesktopClientCore.Helpers.DataVirtualization
{
    public class AsyncVirtualCollection<T> : VirtualCollection<T>//, INotifyCollectionChanged, INotifyPropertyChanged
        where T : class
    {
        public AsyncVirtualCollection(IItemsProvider<T> itemsProvider)
            : base(itemsProvider)
        {
            _synchronizationContext = SynchronizationContext.Current;
        }

        public AsyncVirtualCollection(IItemsProvider<T> itemsProvider, int pageSize)
            : base(itemsProvider, pageSize)
        {
            _synchronizationContext = SynchronizationContext.Current;
        }

        public AsyncVirtualCollection(IItemsProvider<T> itemsProvider, int pageSize, long pageTimeout)
            : base(itemsProvider, pageSize, pageTimeout)
        {
            _synchronizationContext = SynchronizationContext.Current;
        }

        private readonly SynchronizationContext _synchronizationContext;
        protected SynchronizationContext SynchronizationContext {
            get { return _synchronizationContext; }
        }

        private void FireCollectionReset()
        {
            var e = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(e);
        }

        private void FirePropertyChanged(string propertyName)
        {
            var e = new PropertyChangedEventArgs(propertyName);
            OnPropertyChanged(e);
        }

        private bool _isLoading;
        public bool IsLoading {
            get { return _isLoading; }
            set {
                if (value != _isLoading)
                    _isLoading = value;
                FirePropertyChanged("IsLoading");
            }
        }

        protected override void LoadCount()
        {
            
        }
    }
}
