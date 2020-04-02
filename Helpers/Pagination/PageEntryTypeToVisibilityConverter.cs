using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace NorthwindDesktopClientCore.Helpers.Pagination
{
    public class PageEntryTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            PageEntryType expectedType = (PageEntryType)parameter;
            PageEntryType actualType = ((PageEntry)value).Type;
            return expectedType == actualType ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
