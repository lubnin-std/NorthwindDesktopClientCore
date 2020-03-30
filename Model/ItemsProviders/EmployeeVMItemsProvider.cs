using System;
using System.Collections.Generic;
using System.Text;
using NorthwindDesktopClientCore.Model.Entities;
using NorthwindDesktopClientCore.Helpers.DataVirtualization;
using NorthwindDesktopClientCore.ViewModel;
using System.Linq;

namespace NorthwindDesktopClientCore.Model.ItemsProviders
{
    public class EmployeeVMItemsProvider : IItemsProvider<EmployeeViewModel>
    {
        private IItemsProvider<Employees> _realDataProvider;

        public EmployeeVMItemsProvider(IItemsProvider<Employees> realDataProvider)
        {
            _realDataProvider = realDataProvider;
        }

        public int FetchCount()
        {
            return _realDataProvider.FetchCount();
        }

        public IList<EmployeeViewModel> FetchRange(int startIndex, int count)
        {
            return null;
            //return _realDataProvider.FetchRange(startIndex, count)
            //        .Select(e => new EmployeeViewModel(e, _realDataProvider.Context, "hello"))
            //        //.Select(e => new EmployeeViewModel(e, null, "hello"))
            //        .ToList();
                    
        }
    }
}
