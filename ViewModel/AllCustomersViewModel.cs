using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NorthwindDesktopClientCore.ViewModel;
using NorthwindDesktopClientCore.Model;
using NorthwindDesktopClientCore.Model.Entities;
using System.Linq;
using System.Windows.Input;
using NorthwindDesktopClientCore.Helpers;

namespace NorthwindDesktopClientCore.ViewModel
{
    public class AllCustomersViewModel : ClosableViewModel
    {
        private CustomersManager _manager;

        public AllCustomersViewModel(CustomersManager manager)
        {
            _manager = manager;
            RequestPageChange = new RelayCommand(p => OnPageChangeRequest((int)p));
            StartInitialPopulate().IgnoreResult();
        }

        async Task StartInitialPopulate()
        {
            TotalPages = await Task.Run(() => GetPageCount(CancellationToken.None));
            _currentPageNo = 0;
            await PopulateCurrentPage();
        }

        private int _totalPages;
        public int TotalPages {
            get {  return _totalPages; }
            set {
                if (value < 0)
                    throw new ArgumentException(nameof(TotalPages));

                if (_totalPages == value)
                    return;
                
                _totalPages = value;
                OnPropertyChanged("TotalPages");
                HavePages = TotalPages > 0;
            }
        }

        private bool _havePages;
        public bool HavePages { 
            get { return _havePages; }
            set {
                if (_havePages == value)
                    return;
                
                _havePages = value;
                OnPropertyChanged("HavePages");
            } 
        }

        private int _currentPageNo;
        public int CurrentPageNo {
            get { return _currentPageNo; }
            set {
                if (value < 0)
                    throw new ArgumentException(nameof(CurrentPageNo));
                if (value >= TotalPages && HavePages)
                    throw new ArgumentException(nameof(CurrentPageNo));
                if (value != 0 && !HavePages)
                    throw new ArgumentException(nameof(CurrentPageNo));

                if (_currentPageNo == value)
                    return;

                _currentPageNo = value;
                OnPropertyChanged("CurrentPageNo");
                PopulateCurrentPage().IgnoreResult();
            }
        }

        IEnumerable<CustomerViewModel> _currentPage;
        public IEnumerable<CustomerViewModel> CurrentPage {
            get { return _currentPage; }
            set {
                //if (EqualityComparer<CustomerViewModel>.Default.Equals(_currentPage, value))
                if (EqualityComparer<CustomerViewModel>.Equals(_currentPage, value))
                    return;

                _currentPage = value;
                OnPropertyChanged("CurrentPage");
            }
        }

        CancellationTokenSource populationTaskCts = null;
        async Task PopulateCurrentPage()
        {
            populationTaskCts?.Cancel();

            CurrentPage = null;
            if (!HavePages)
                return;

            using (var cts = new CancellationTokenSource())
            {
                populationTaskCts = cts;
                var workPageNo = CurrentPageNo;

                try
                {
                    var modelPage = await Task.Run(() =>
                        GetAllCustomers(workPageNo, cts.Token), cts.Token);
                    
                    if (cts.IsCancellationRequested)
                        return;

                    var vmPage = modelPage.Select(c => new CustomerViewModel(c, _manager)).ToList();

                    if (cts.IsCancellationRequested)
                        return;

                    CurrentPage = vmPage;
                }
                catch (OperationCanceledException) when (cts.IsCancellationRequested)
                {

                }
                finally
                {
                    if (cts == populationTaskCts)
                        populationTaskCts = null;
                }
            }
        }

        private int _pageSize = 10;
        IEnumerable<Customers> GetAllCustomers(int pageNo, CancellationToken ct)
        {
            return _manager.GetAllCustomers().Skip(pageNo * _pageSize).Take(_pageSize).ToList();
        }

        private int GetPageCount(CancellationToken ct)
        {
            var customersCount = _manager.GetAllCustomers().Count();
            return (int)Math.Ceiling((double)customersCount / _pageSize);
        }
        
        public ICommand RequestPageChange { get; }

        private void OnPageChangeRequest(int newPage)
        {
            if (!HavePages)
                return;
            if (newPage < 0 || newPage >= TotalPages)
                return;
            CurrentPageNo = newPage;
        }

        
    }

    public static class TaskExtensions
    {
        public static void IgnoreResult(this Task t) { }
    }
}
