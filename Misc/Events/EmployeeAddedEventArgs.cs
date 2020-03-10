using System;
using System.Collections.Generic;
using System.Text;
using NorthwindDesktopClientCore.Model.Entities;

namespace NorthwindDesktopClientCore.Misc.Events
{
    public class EmployeeAddedEventArgs : EventArgs
    {
        public Employees NewEmployee {
            get;
            private set;
        }

        public EmployeeAddedEventArgs(Employees emp)
        {
            NewEmployee = emp;
        }
    }
}
