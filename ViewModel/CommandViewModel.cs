//---------------------------------------------------------------------------
// Команды программы представлены ViewModel'ями, чтобы они тоже имели читаемое
// имя (DisplayName) в пользовательском интерфейсе.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;  // ICommand

namespace NorthwindDesktopClientCore.ViewModel
{
    public class CommandViewModel : ViewModelBase
    {
        // Пропустил тут геттер и сеттер при создании класса
        // и получил множественные страдания впоследствии
        public ICommand Command { get; private set; }

        public CommandViewModel(string displayName, ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            base.DisplayName = displayName;
            this.Command = command;
        }
    }
}
