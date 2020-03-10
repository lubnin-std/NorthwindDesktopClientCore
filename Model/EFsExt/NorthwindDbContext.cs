using System;
using System.Collections.Generic;
using System.Text;
using NorthwindDesktopClientCore.Misc.Events;
using NorthwindDesktopClientCore.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace NorthwindDesktopClientCore.Model.DataContext
{
    public partial class NorthwindDbContext
    {
        public event EventHandler<EmployeeAddedEventArgs> EmployeeAdded;

        private void OnEmployeeAdded(Employees emp)
        {
            var e = new EmployeeAddedEventArgs(emp);
            EmployeeAdded?.Invoke(this, e);
        }
    }
}
