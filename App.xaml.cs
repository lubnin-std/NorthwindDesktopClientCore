using System.Windows;
using NorthwindDesktopClientCore.ViewModel;

namespace NorthwindDesktopClientCore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            new MainWindow() { DataContext = new MainWindowViewModel() }.Show();
        }
    }
}
