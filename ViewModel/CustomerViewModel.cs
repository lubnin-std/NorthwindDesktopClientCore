using System;
using System.Collections.Generic;
using System.Text;
using NorthwindDesktopClientCore.Model.Entities;
using NorthwindDesktopClientCore.Model;

namespace NorthwindDesktopClientCore.ViewModel
{
    public class CustomerViewModel : ClosableViewModel
    {
        private Customers _customer;
        private CustomersManager _manager;

        public CustomerViewModel(Customers customer, CustomersManager manager)
        {
            DisplayName = "tmp tab name";
            _manager = manager;
            _customer = customer;
        }

        public string CompanyName {
            get { return _customer.CompanyName; }
            set {
                if (_customer.CompanyName != value)
                {
                    _customer.CompanyName = value;
                    OnPropertyChanged("CompanyName");
                }
            }
        }

        public string Country { 
            get { return _customer.Country; }
            set {
                if (_customer.Country != value)
                {
                    _customer.Country = value;
                    OnPropertyChanged("Country");
                }
            }
        }
    }
}
