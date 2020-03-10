using System;
using System.Collections.Generic;
using System.Text;
using NorthwindDesktopClientCore.Model.DataContext;
using NorthwindDesktopClientCore.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace NorthwindDesktopClientCore.ViewModel
{
    public class AllEmployeesViewModel : ClosableViewModel
    {
        private readonly NorthwindDbContext _context;
        public ObservableCollection<EmployeeViewModel> AllEmployees { get; private set; }

        public AllEmployeesViewModel(NorthwindDbContext context, string displayName)
        {
            DisplayName = displayName;
            _context = context;
            GetAllEmployees();
        }

        private void GetAllEmployees()
        {
            List<EmployeeViewModel> all = 
                (from emp in _context.Employees
                 select new EmployeeViewModel(emp, _context, "")).ToList();

            AllEmployees = new ObservableCollection<EmployeeViewModel>(all);
        }
    }
}
