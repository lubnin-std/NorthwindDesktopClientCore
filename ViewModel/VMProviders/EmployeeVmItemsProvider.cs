using System;
using System.Collections.Generic;
using System.Text;
using NorthwindDesktopClientCore.Helpers.DataVirtualization;
using NorthwindDesktopClientCore.ViewModel;
using NorthwindDesktopClientCore.Model;
using System.Linq;

namespace NorthwindDesktopClientCore.ViewModel.VMProviders
{
    public class EmployeeVmItemsProvider : IItemsProvider<EmployeeViewModel>
    {
        private EmployeesManager _empManager;

        public EmployeeVmItemsProvider(EmployeesManager employeesManager)
        {
            _empManager = employeesManager;
        }

        public int FetchCount()
        {
            return _empManager.FetchCount();
        }

        public IList<EmployeeViewModel> FetchRange(int startIndex, int count)
        {
            return _empManager.FetchRange(startIndex, count)
                .Select(e => new EmployeeViewModel(e, _empManager, $"{e.LastName} {e.FirstName}")).ToList();
        }
    }
}
