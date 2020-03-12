using System;
using System.Collections.Generic;
using System.Text;
using NorthwindDesktopClientCore.Model.DataContext;
using NorthwindDesktopClientCore.Model.Entities;
using NorthwindDesktopClientCore.ViewModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using NorthwindDesktopClientCore.Helpers;


namespace NorthwindDesktopClientCore.ViewModel
{
    public class AllEmployeesViewModel : ClosableViewModel
    {
        private readonly NorthwindDbContext _context;
        public ObservableCollection<EmployeeViewModel> AllEmployees { get; private set; }

        // TODO: сделать, чтобы вкладки формировались динамически, а не описывались вручную в XAML
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

        private CommandViewModel _deleteCommand;
        public CommandViewModel DeleteCommand {
            get {
                if (_deleteCommand == null)
                    _deleteCommand = new CommandViewModel("Удалить", new RelayCommand(c => DeleteEmployee()));
                return _deleteCommand;
            }
        }

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

        private void DeleteEmployee()
        {
            var selected = AllEmployees.Where(e => e.EmpIsSelected).ToList();
            foreach (var s in selected)
            {
                var emp = _context.Employees.FirstOrDefault(e => e.EmployeeId == s.EmployeeId) as Employees;
                if (emp != null)
                {
                    _context.Employees.Remove(emp);
                    AllEmployees.Remove(s);
                }
            }

            _context.SaveChanges();
            OnPropertyChanged("AllEmployees");
        }

    }
}
