using System;
using System.Collections.Generic;
using System.Text;
using NorthwindDesktopClientCore.Helpers.DataVirtualization;
using NorthwindDesktopClientCore.Model.Entities;
using NorthwindDesktopClientCore.Model.DataContext;

namespace NorthwindDesktopClientCore.Model
{
    public class EmployeesManager : IItemsProvider<Employees>
    {
        private NorthwindDbContext _context;
        private IItemsProvider<Employees> _empProvider;

        // TODO: Всегда ли класс должен требовать зависимости через конструктор или, например, в данном
        // случае логичнее будет, чтобы он сам создал себе провайдер элементов ?
        public EmployeesManager(NorthwindDbContext context, IItemsProvider<Employees> employeesProvider)
        {
            _context = context;
            _empProvider = employeesProvider;
        }

        public int FetchCount()
        {
            return _empProvider.FetchCount();
        }

        public IList<Employees> FetchRange(int startIndex, int count)
        {
            return _empProvider.FetchRange(startIndex, count);
        }

        public Employees GetNewEmployee()
        {
            return new Employees();
        }

        public void SaveEmployee(Employees employee)
        {
            if (_context.Employees.Find(employee.EmployeeId) == null)
            { 
                _context.Employees.Add(employee);
            }
            else
            { 
                _context.Employees.Update(employee);
            }

            _context.SaveChanges();
        }
    }
}
