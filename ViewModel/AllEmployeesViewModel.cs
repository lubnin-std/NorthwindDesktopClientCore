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
using NorthwindDesktopClientCore.Helpers.FlexibleGridView;
using NorthwindDesktopClientCore.Model;
using NorthwindDesktopClientCore.Helpers.DataVirtualization;
using NorthwindDesktopClientCore.ViewModel.VMProviders;

namespace NorthwindDesktopClientCore.ViewModel
{
    public class AllEmployeesViewModel : ClosableViewModel
    {
        private EmployeesManager _empManager;
        private IItemsProvider<EmployeeViewModel> _empVmProvider;

        public VirtualCollection<EmployeeViewModel> AllEmployees { get; private set; }

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

        public AllEmployeesViewModel(EmployeesManager employeesManager, string displayName)
        {
            _empManager = employeesManager;
            _empVmProvider = new EmployeeVmItemsProvider(_empManager);
            DisplayName = displayName;

            ValidateColumns();
            GetAllEmployees();

            AllEmployees.CollectionChanged += OnAllEmployeesCollectionChanged;
        }

        private void GetAllEmployees()
        {
            AllEmployees = new VirtualCollection<EmployeeViewModel>(_empVmProvider);
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
            //// TODO: Если из бд не удалится по какой-то причине, надо об этом сообщить, чтобы из коллекции тоже не удалялось.
            //// Либо после удаления из бд инициировать перезагрузку коллекции.
            //DeleteEmployeeFromDatabase(selected);
            //DeleteEmployeeFromCollection(selected);
        }

        private void DeleteEmployeeFromDatabase(IEnumerable<EmployeeViewModel> selected)
        {
            //foreach (var s in selected)
            //{
            //    var emp = _context.Employees.FirstOrDefault(e => e.EmployeeId == s.EmployeeId) as Employees;
            //    if (emp != null)
            //    {
            //        _context.Employees.Remove(emp);
            //    }
            //}

            //_context.SaveChanges();
        }

        private void DeleteEmployeeFromCollection(IEnumerable<EmployeeViewModel> selected)
        {
            //foreach (var s in selected)
            //{
            //    AllEmployees.Remove(s);
            //}
        }

    }
}
