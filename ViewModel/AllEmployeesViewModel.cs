using System;
using System.Collections.Generic;
using System.Text;
using NorthwindDesktopClientCore.Model.DataContext;
using NorthwindDesktopClientCore.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace NorthwindDesktopClientCore.ViewModel
{
    public class AllEmployeesViewModel : ClosableViewModel
    {
        private readonly NorthwindDbContext _context;
        public ObservableCollection<EmployeeViewModel> AllEmployees { get; private set; }

        public Dictionary<string, string> Columns = new Dictionary<string, string>()
        {
            { "EmployeeId", "Табельный №" },
            { "TitleOfCourtesy", "Обращение" },
            { "LastName", "Фамилия" },
            { "FirstName", "Имя" },
            { "Title", "Должность" },
            { "HireDate", "Дата найма" },
            { "ReportsTo", "Подчиняется" }
        };

        public AllEmployeesViewModel(NorthwindDbContext context, string displayName)
        {
            DisplayName = displayName;
            _context = context;
            //ValidateColumns();
            GetAllEmployees();
        }

        private void GetAllEmployees()
        {
            List<EmployeeViewModel> all = 
                (from emp in _context.Employees
                 select new EmployeeViewModel(emp, _context, "")).ToList();

            AllEmployees = new ObservableCollection<EmployeeViewModel>(all);
        }

        private void ValidateColumns()
        {
            foreach (var c in Columns)
            {
                if (typeof(EmployeeViewModel).GetProperty(c.Key) == null)
                    throw new MissingFieldException($"В классе EmployeesViewModel нет свойства {c.Key}");
            }
        }
    }
}
