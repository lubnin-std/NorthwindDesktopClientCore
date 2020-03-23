using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NorthwindDesktopClientCore.Model.DataContext;
using NorthwindDesktopClientCore.Model.Entities;
using NorthwindDesktopClientCore.Helpers.DataVirtualization;
using System.Linq;

namespace NorthwindDesktopClientCore.Model
{
    public class DataProvider
    {
        private NorthwindDbContext Context { get; }

        public DataProvider(DbContext context)
        {
            if (context is NorthwindDbContext cnt) 
                Context = cnt;
            else 
                WrongContextGiven(context);
        }




        public IEnumerable<Employees> GetEmployees()
        {
            return Context.Employees.OrderBy(e => e.EmployeeId).Take(150).ToList();
        }

        public Employees GetNewEmployee()
        {
            return new Employees();
        }

       
        private void WrongContextGiven(DbContext context)
        {
            var msg = string.Format($"Data context must be of {0} type. {1} given",
                    typeof(NorthwindDbContext).Name,
                    context.GetType().Name);
            throw new ArgumentException(msg);
        }
    }
}
