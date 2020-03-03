//---------------------------------------------------------------------------
// Некоторые ViewModel могут образовывать вместе что-то вроде набора вкладок,
// каждую из которых можно закрыть. Как, например, открытые файлы в Visual
// Studio образуют такой ряд под меню.
//
// Подобный "набор закрываемых вкладок" - суть коллекция vm'ей, у каждой из
// которых есть команда закрытия, приводящая к удалению vm из коллекции. А
// поскольку эта коллекция будет прибита ко View, то Wpf самостоятельно
// выполнит удаление вкладки из графического интерфейса после того, как мы
// ее программно удалим из коллекции.
//
// Реальный обработчик события закрытия ?????????????????????????????????????????????????????
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using NorthwindDesktopClientCore.Helpers;  // RelayCommand
using System.Windows.Input;  // ICommand

namespace NorthwindDesktopClientCore.ViewModel
{
    public abstract class ClosableViewModel : ViewModelBase
    {
        private RelayCommand _closeCommand;
        public event EventHandler RequestClose;


        private void OnRequestClose()
        {
            var handler = RequestClose;
            handler?.Invoke(this, EventArgs.Empty);
        }


        public ICommand CloseCommand {
            get {
                if (_closeCommand == null)
                    // c - это параметр object, передаваемый в Action
                    _closeCommand = new RelayCommand(c => OnRequestClose());

                return _closeCommand;
            }
        }

    }
}
