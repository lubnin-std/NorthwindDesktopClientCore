//---------------------------------------------------------------------------
// Каждая ViewModel, если в ней что-то изменилось, должна уметь оповещать об
// этом систему биндинга. Поэтому я создаю базовый класс с этим функционалом,
// от которого будут наследоваться все ViewModel в программе.
//---------------------------------------------------------------------------

using System;
using System.ComponentModel;  // INotifyPropertyChanged, TypeDescriptor

namespace NorthwindDesktopClientCore.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        // user-friendly имя vm
        public string DisplayName { get; set; }
        public bool UnsavedChanges { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            UnsavedChanges = true;

            var e = new PropertyChangedEventArgs(propertyName);
            var handler = PropertyChanged;
            handler.Invoke(this, e);
        }

        protected void VerifyPropertyName(string propertyName)
        {
            string className = TypeDescriptor.GetClassName(this);

            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                throw new Exception($"Property {propertyName} does not exist in {className} class.");
            }
        }
    }
}
