using System;
using System.Collections.Generic;
using NorthwindDesktopClientCore.Model.Entities;
using NorthwindDesktopClientCore.Helpers;
using System.ComponentModel;
using NorthwindDesktopClientCore.Model;

namespace NorthwindDesktopClientCore.ViewModel
{
    public class EmployeeViewModel : ClosableViewModel, IDataErrorInfo
    {
        private Employees _emp;
        private bool _empIsSelected;
        private bool _unsavedChanges;
        private EmployeesManager _empManager;


        // TODO: сделать событие изменения значения свойства через [CallerMemberName] как у VladD
        // https://ru.stackoverflow.com/questions/615927/wpf-%d0%a2%d0%b0%d0%b1%d0%bb%d0%b8%d1%86%d0%b0-xaml/616413#616413
        public int EmployeeId {
            get { return _emp.EmployeeId; }
            // Когда сотрудник сохраняется в БД и получает автоId, оно сразу записывается в это поле,
            // вызывая событие изменения, и за счет этого новое полученное автоId сразу отображается в интерфейсе
            set {
                OnPropertyChanged("EmployeeId");
            }
        }

        public string LastName {
            get { return _emp.LastName; }
            set {
                if (_emp.LastName == value)
                    return;

                _emp.LastName = value;
                base.OnPropertyChanged("LastName");
            }
        }

        public string FirstName {
            get { return _emp.FirstName; }
            set {
                if (_emp.FirstName == value)
                    return;

                _emp.FirstName = value;
                base.OnPropertyChanged("FirstName");
            }
        }

        public string Title {
            get { return _emp.Title; }
            set {
                if (_emp.Title == value)
                    return;

                _emp.Title = value;
                OnPropertyChanged("Title");
            }
        }

        public string TitleOfCourtesy {
            get { return _emp.TitleOfCourtesy; }
            set {
                if (_emp.TitleOfCourtesy == value)
                    return;

                _emp.TitleOfCourtesy = value;
                OnPropertyChanged("TitleOfCourtesy");
            }
        }

        public DateTime? BirthDate {
            get { return _emp.BirthDate; }
            set {
                if (_emp.BirthDate == value)
                    return;

                _emp.BirthDate = value;
                OnPropertyChanged("BirthDate");
            }
        }

        public DateTime? HireDate {
            get { return _emp.HireDate; }
            set {
                if (_emp.HireDate == value)
                    return;

                _emp.HireDate = value;
                OnPropertyChanged("BirthDate");
            }
        }

        public string Address {
            get { return _emp.Address; }
            set {
                if (_emp.Address == value)
                    return;

                _emp.Address = value;
                OnPropertyChanged("Address");
            }
        }

        public string City {
            get { return _emp.City; }
            set {
                if (_emp.City == value)
                    return;

                _emp.City = value;
                OnPropertyChanged("City");
            }
        }

        public string Region {
            get { return _emp.Region; }
            set {
                if (_emp.Region == value)
                    return;

                _emp.Region = value;
                OnPropertyChanged("Region");
            }
        }

        public string PostalCode {
            get { return _emp.PostalCode; }
            set {
                if (_emp.PostalCode == value)
                    return;

                _emp.PostalCode = value;
                OnPropertyChanged("PostalCode");
            }
        }

        public string Country {
            get { return _emp.Country; }
            set {
                if (_emp.Country == value)
                    return;

                _emp.Country = value;
                OnPropertyChanged("Country");
            }
        }

        public string HomePhone {
            get { return _emp.HomePhone; }
            set {
                if (_emp.HomePhone == value)
                    return;

                _emp.HomePhone = value;
                OnPropertyChanged("HomePhone");
            }
        }

        public string Extension {
            get { return _emp.Extension; }
            set {
                if (_emp.Extension == value)
                    return;

                _emp.Extension = value;
                OnPropertyChanged("Extension");
            }
        }

        public string Notes {
            get { return _emp.Notes; }
            set {
                if (_emp.Notes == value)
                    return;

                _emp.Notes = value;
                OnPropertyChanged("Notes");
            }
        }

        // Свойство для заполнения ComboBox'а со списком сотрудников
        private IEnumerable<Employees> _empLocal;
        public IEnumerable<Employees> ReportsToList {
            get {
                if (_empLocal == null)
                    // TODO: Когда в базе десятки тысяч сотрудников, такой метод больше не подходит
                    // Сделать через модальное окно
                    //_empLocal = _context.Employees.ToList();
                    _empLocal = null;

                return _empLocal;
            }
        }

        public Employees ReportsToEmp {
            set {
                _emp.ReportsTo = value.EmployeeId;
            }
        }

        // Объект начальника
        public Employees ReportsToNavigation {
            get {
                return _emp.ReportsToNavigation;
            }
        }

        public bool EmpIsSelected {
            get { return _empIsSelected; }
            set {
                if (_empIsSelected == value)
                    return;

                _empIsSelected = value;
                base.OnPropertyChanged("EmpIsSelected");
            }
        }

        public EmployeeViewModel(Employees employee, EmployeesManager employeesManager, string vmDisplayName)
        {
            _empManager = employeesManager;

            _emp = employee;
            base.DisplayName = vmDisplayName;
        }


        private CommandViewModel _saveCommand;

        public CommandViewModel SaveCommand {
            get {
                if (_saveCommand == null)
                    _saveCommand = new CommandViewModel("Сохранить", new RelayCommand(c => Save(), p => _emp.IsValid == true));

                return _saveCommand;
            }
        }

        private void Save()
        {
            _empManager.SaveEmployee(_emp);
            this.EmployeeId = _emp.EmployeeId;
            UnsavedChanges = false;
        }

        
        // Реальную валидацию VM делегирует объекту Employees.
        // Порядок обработки свойств - как они идут в разметке
        // TODO: подумать над переносом валидации в отдельный класс и формировании детального
        // списка ошибок для каждого поля.
        string IDataErrorInfo.Error {
            get { return (_emp as IDataErrorInfo).Error; }
        }

        string IDataErrorInfo.this[string propertyName] {
            get {
                string error = null;

                error = (_emp as IDataErrorInfo)[propertyName];

                return error;
            }
        }
    }
}
