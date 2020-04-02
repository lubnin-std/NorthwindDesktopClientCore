using System;
using System.Collections.Generic;
using System.Text;

namespace NorthwindDesktopClientCore.Helpers.Pagination
{
    public class PageEntry
    {
        public int PageNumber { get; }
        public PageEntryType Type { get; }
        public PageEntry(int num, PageEntryType type) { PageNumber = num; Type = type; }
    }
}
