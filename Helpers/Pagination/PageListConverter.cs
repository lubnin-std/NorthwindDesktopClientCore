using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace NorthwindDesktopClientCore.Helpers.Pagination
{
    public class PageListConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int currentPage = (int)values[0];
            int numberOfPages = (int)values[1];
            return RecalcList(currentPage, numberOfPages);
        }

        private IEnumerable<PageEntry> RecalcList(int currentPage, int numberOfPages)
        {
            const int pagesAroundCurrent = 3;
            const int pagesAroundEnd = 2;

            var min = Math.Max(0, currentPage - pagesAroundCurrent);
            var max = Math.Min(numberOfPages - 1, currentPage + pagesAroundCurrent);

            bool separateLeftEnd = pagesAroundEnd + 1 < min;
            if (!separateLeftEnd)
                min = 0;

            bool separateRightEnd = numberOfPages - 1 - pagesAroundEnd - 1 > max;
            if (!separateRightEnd)
                max = numberOfPages - 1;

            if (separateLeftEnd)
            {
                for (int n = 0; n < pagesAroundEnd; n++)
                    yield return new PageEntry(n, PageEntryType.Normal);

                yield return new PageEntry(-1, PageEntryType.Ellipsis);
            }

            for (int n = min; n <= max; n++)
                yield return new PageEntry(n, (n == currentPage) ? PageEntryType.Current : PageEntryType.Normal);

            if (separateRightEnd)
            {
                yield return new PageEntry(-1, PageEntryType.Ellipsis);

                for (int n = numberOfPages - pagesAroundEnd; n < numberOfPages; n++)
                    yield return new PageEntry(n, PageEntryType.Normal);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
