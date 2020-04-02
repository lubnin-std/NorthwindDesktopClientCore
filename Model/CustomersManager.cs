using System;
using System.Collections.Generic;
using System.Text;
using NorthwindDesktopClientCore.Model.Entities;
using NorthwindDesktopClientCore.Model.DataContext;
using System.Linq;

namespace NorthwindDesktopClientCore.Model
{
    public class CustomersManager
    {
        private NorthwindDbContext _context;
        
        public CustomersManager(NorthwindDbContext context)
        {
            _context = context;
        }

        
        public IQueryable<Customers> GetAllCustomers()
        {
            return _context.Customers;
        }

        public Customers GetNewCustomer()
        {
            return new Customers();
        }

        public void SaveCustomer(Customers customer)
        {
            if (_context.Customers.Find(customer.CustomerId) == null)
            {
                _context.Customers.Add(customer);
            }
            else
            {
                _context.Customers.Update(customer);
            }

            _context.SaveChanges();
        }
    }
}
