//---------------------------------------------------------------------------
//    Workspace - свойство, хранящее открытые "окна". Визуальная сущность окна 
// добавляется\удаляется в графический интерфейс автоматически благодаря системе 
// биндинга Wpf, когда программная сущность этого окна (объект ViewModel) добавлятеся\удаляется
// в коллекцию Workspaces.
//    Для этой коллекции выбран тип ObservableCollection потому, что при добавлении
// окна нам нужно задать ему обработчик команды закрытия, а этот тип коллекции как раз
// позволяет перехватить момент изменения коллекции. Наследники ClosableViewModel
// имеют команду закрытия, но не выполняют непосредственно закрытие, а лишь вызывают
// прицепленный обработчик, потому что не знают, в чем заключается это закрытие.
//    А заключается оно в том, что инициировавший закрытие объект ViewModel нужно
// удалить из коллекции Workspace.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;  // ReadOnlyCollection, ObservableCollection
using NorthwindDesktopClientCore.Model.DataContext;
using NorthwindDesktopClientCore.Helpers;
using NorthwindDesktopClientCore.Model.Entities;
using NorthwindDesktopClientCore.Model;
using System.Collections.Specialized;  // NotifyCollectionChangedEventArgs
using System.Diagnostics;
using NorthwindDesktopClientCore.Resx;  // ViewModelNames
using System.ComponentModel;  // ICollectionView
using System.Windows.Data;  // CollectionViewSource
using System.Linq;

namespace NorthwindDesktopClientCore.ViewModel
{
    // У Джоша Смита она была WorkspaceViewModel, потому что он вешал в старте App
    // на нее обработчик закрытия. Но я решил пока что так не делать и посмотреть,
    // что из этого получится.
    public class MainWindowViewModel : ViewModelBase
    {
        // TODO: Впоследствии хорошо бы переделать боковую панель в панель быстрых
        // команд с возможностью накидывания туда команд в рантайме
        private ReadOnlyCollection<CommandViewModel> _commands;
        private ObservableCollection<ClosableViewModel> _workspace;

        private NorthwindDbContext _context;


        public ReadOnlyCollection<CommandViewModel> Commands {
            get {
                if (_commands == null)
                {
                    List<CommandViewModel> cmds = CreateCommands();
                    _commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }

                return _commands;
            }
        }


        public ObservableCollection<ClosableViewModel> Workspace {
            get { 
                if (_workspace == null)
                {
                    _workspace = new ObservableCollection<ClosableViewModel>();
                    // Когда окна добавляются в коллекцию (читай, коллекция изменяется),
                    // им нужно приделать обработчик закрытия
                    _workspace.CollectionChanged += OnWorkspaceChanged;
                }

                return _workspace;
            }
        }


        public MainWindowViewModel()
        {
            _context = new NorthwindDbContext();
            //_context = new NorthwindDbContextVirtual();
        }


        private List<CommandViewModel> CreateCommands()
        {
            return new List<CommandViewModel>()
            {
                new CommandViewModel("Список сотрудников", new RelayCommand(с => ShowAllEmployees())),
                new CommandViewModel("Создать сотрудника", new RelayCommand(c => CreateNewEmployee())),
                new CommandViewModel("Список партнеров", new RelayCommand(c => ShowAllCustomers()))
            };
        }

        
        // Каждый потомок ClosableViewModel имеет событие RequestClose и команду Close.
        // Из нее вызывается обработчик, подписанный на это событие, и в этот обработчик
        // этот самый потомок передает ссылку на себя. Таким образом метод OnWorkspaceClose
        // и получает ссылку на объект, который надо удалить из коллекции Workspace.
        private void OnWorkspaceChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (ClosableViewModel cvm in e.NewItems)
                    cvm.RequestClose += OnWorkspaceClose;

            // Если не очистить событие удаляемых окон от обработчиков, то сборщик мусора
            // не сможет удалить эти окна из памяти
            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (ClosableViewModel cvm in e.OldItems)
                    cvm.RequestClose -= OnWorkspaceClose;
        }

        // Непосредственно удаляем окно, которое инициировало свое закрытие, из коллекции Workspace
        private void OnWorkspaceClose(object sender, EventArgs e)
        {
            var ws = sender as ClosableViewModel;
            if (ws.UnsavedChanges == true)
                Debug.Print("Есть несохраненные изменения. Выйти без сохранения?");
            Workspace.Remove(ws);
        }


        private void ShowAllEmployees()
        {
            var all = Workspace.FirstOrDefault(vm => vm is AllEmployeesViewModel) as AllEmployeesViewModel;

            if (all == null)
            {
                var empItemsProvider = new ItemsProvider<Employees>(_context);
                var empManager = new EmployeesManager(_context, empItemsProvider);

                all = new AllEmployeesViewModel(empManager, ViewModelNames.AllEmployeesViewModel_DisplayName);
                Workspace.Add(all);
            }
            
            SetActiveWorkspace(all);
        }

        private void ShowAllCustomers()
        {
            var all = Workspace.FirstOrDefault(vm => vm is AllCustomersViewModel) as AllCustomersViewModel;

            if (all == null)
            {
                var manager = new CustomersManager(_context);
                all = new AllCustomersViewModel(manager);
                Workspace.Add(all);
            }

            SetActiveWorkspace(all);
        }

        private void CreateNewEmployee()
        {
            var empItemsProvider = new ItemsProvider<Employees>(_context);
            var empManager = new EmployeesManager(_context, empItemsProvider);
            var employee = empManager.GetNewEmployee();

            var vm = new EmployeeViewModel(employee, empManager, ViewModelNames.EmployeeViewModel_DisplayName);
            Workspace.Add(vm);
            SetActiveWorkspace(vm);
        }

        // ??? Перемещение на вновь открытую вкладку фиксит проблему, когда первая новая вкладка открывается без разметки
        private void SetActiveWorkspace(ClosableViewModel workspace)
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(Workspace);
            if (cv != null)
                cv.MoveCurrentTo(workspace);
        }
    }
}
