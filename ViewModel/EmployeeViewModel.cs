using System;
using System.Collections.Generic;
using System.Text;
using NorthwindDesktopClientCore.Model.DataContext;
using NorthwindDesktopClientCore.Model.Entities;
using System.Linq;
using NorthwindDesktopClientCore.Helpers;
using System.Diagnostics;
using NorthwindDesktopClientCore.Helpers.Validation;

namespace NorthwindDesktopClientCore.ViewModel
{
    public class EmployeeViewModel : ClosableViewModel
    {
        private readonly NorthwindDbContext _context;
        private Employees _emp;
        private bool _isSelected;
        private bool _unsavedChanges;
        private IValidation _validator;

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

        private IEnumerable<Employees> _empLocal;
        public IEnumerable<Employees> ReportsToList {
            get {
                if (_empLocal == null)
                    _empLocal = _context.Employees.ToList();

                return _empLocal;
            }
        }

        public Employees ReportsToEmp {
            set {
                _emp.ReportsTo = value.EmployeeId;
            }
        }

        public bool IsSelected {
            get { return _isSelected; }
            set {
                if (_isSelected == value)
                    return;

                _isSelected = value;
                base.OnPropertyChanged("IsSelected");
            }
        }

        public EmployeeViewModel(Employees employee, NorthwindDbContext context, IValidation validator, string vmDisplayName)
        {
            if (employee == null)
                throw new ArgumentNullException("employee");

            if (context == null)
                throw new ArgumentNullException("context");

            _context = context;
            _emp = employee;
            _validator = validator;

            base.DisplayName = vmDisplayName;
        }


        private CommandViewModel _saveCommand;

        public CommandViewModel SaveCommand {
            get {
                if (_saveCommand == null)
                    _saveCommand = new CommandViewModel("Сохранить", new RelayCommand(c => Save()));

                return _saveCommand;
            }
        }

        private void Save()
        {
            try
            { 
                if (_validator.IsValid())
                { 
                    if (EmployeeIsNew())
                    { 
                        _context.Employees.Add(_emp);
                        _context.SaveChanges();

                        // После сохранения объекта в БД он получает автоId, который надо сообщить свойству
                        EmployeeId = _emp.EmployeeId;
                    }
                    else
                    {
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.InnerException.Message);
            }
        }

        private bool EmployeeIsNew()
        {
            return _emp.EmployeeId == 0 ? true : false;
        }
    }
}
