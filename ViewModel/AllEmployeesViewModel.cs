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
using System.Collections.Specialized;
using NorthwindDesktopClientCore.Helpers;

namespace NorthwindDesktopClientCore.ViewModel
{
    public class AllEmployeesViewModel : ClosableViewModel
    {
        private readonly NorthwindDbContext _context;
        public ObservableCollection<EmployeeViewModel> AllEmployees { get; private set; }

        public ObservableCollection<Column> Columns { get; } = new ObservableCollection<Column>()
        {
            new Column("Табельный №", "EmployeeId"),
            new Column("Обращение", "TitleOfCourtesy"),
            new Column("Фамилия", "LastName"),
            new Column("Имя", "FirstName"),
            new Column("Должность", "Title"),
            new Column("Дата найма", "HireDate"),
            new Column("Подчиняется", "ReportsToNavigation")
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
            ValidateColumns();
            GetAllEmployees();
            AllEmployees.CollectionChanged += OnAllEmployeesCollectionChanged;
        }

        private void GetAllEmployees()
        {
            List<EmployeeViewModel> all = 
                (from emp in _context.Employees
                 select new EmployeeViewModel(emp, _context, "")).ToList();

            AllEmployees = new ObservableCollection<EmployeeViewModel>(all);
        }

        private void OnAllEmployeesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("AllEmployees");
        }

        private void ValidateColumns()
        {
            foreach (var c in Columns)
            {
                if (typeof(EmployeeViewModel).GetProperty(c.DisplayMember) == null)
                    throw new MissingFieldException($"В классе EmployeesViewModel нет свойства {c.DisplayMember}");
            }
        }

        private void DeleteEmployee()
        {
            var selected = AllEmployees.Where(e => e.EmpIsSelected).ToList();
            DeleteEmployeeFromDatabase(selected);
            DeleteEmployeeFromCollection(selected);
        }

        private void DeleteEmployeeFromDatabase(IEnumerable<EmployeeViewModel> selected)
        {
            foreach (var s in selected)
            {
                var emp = _context.Employees.FirstOrDefault(e => e.EmployeeId == s.EmployeeId) as Employees;
                if (emp != null)
                {
                    _context.Employees.Remove(emp);
                }
            }

            _context.SaveChanges();
        }

        private void DeleteEmployeeFromCollection(IEnumerable<EmployeeViewModel> selected)
        {
            foreach (var s in selected)
            {
                AllEmployees.Remove(s);
            }
        }

    }
}
