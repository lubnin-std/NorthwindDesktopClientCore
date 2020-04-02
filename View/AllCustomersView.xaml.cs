using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NorthwindDesktopClientCore.Helpers.Pagination;

namespace NorthwindDesktopClientCore.View
{
    /// <summary>
    /// Interaction logic for AllCustomersView.xaml
    /// </summary>
    public partial class AllCustomersView : UserControl
    {
        public AllCustomersView()
        {
            InitializeComponent();
        }

        void OnPageChangeRequest(object sender, RoutedEventArgs e)
        {
            var command = (ICommand)Tag;
            if (command == null)
                return;

            var hyperlink = (Hyperlink)sender;

            var pageNo = ((PageEntry)hyperlink.DataContext).PageNumber;
            command.Execute(pageNo);
        }
    }
}
