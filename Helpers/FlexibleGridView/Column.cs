using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindDesktopClientCore.Helpers.FlexibleGridView
{
    public class Column
    {
        public string Header { get; }
        public string DisplayMember { get; }

        public Column(string header, string displayMember)
        {
            Header = header;
            DisplayMember = displayMember;
        }
    }
}
