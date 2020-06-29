using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FileSelectorDemo.CodeBase
{
    public class DelegateCommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        protected readonly Func<object, Task> _executeMethod;
        protected readonly Func<object, bool> _canExecuteMethod;
        protected DelegateCommandBase(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
            {
                throw new ArgumentNullException("executeMethod or canExecuteMethod can not be null!");
            }  
            _executeMethod = (arg) => 
            {
                executeMethod(arg);
                return Task.Delay(0);
            };
            _canExecuteMethod = canExecuteMethod;
        }

        protected DelegateCommandBase(Func<object, Task> executeMethod, Func<object, bool> canExecuteMethod)
        {
            if (executeMethod == null || canExecuteMethod == null)
            {
                throw new ArgumentNullException("executeMethod or canExecuteMethod can not be null!");
            } 
            _executeMethod = executeMethod;
            _canExecuteMethod = canExecuteMethod;
        }
        protected bool CanExecute(object parameter)
        {           
            return _canExecuteMethod == null || _canExecuteMethod(parameter);
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(parameter);
        }

        async void ICommand.Execute(object parameter)
        {
            await Execute(parameter); 
        }

        protected async Task Execute(object parameter)
        {           
            await _executeMethod(parameter);
        }
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
            {
                Application.Current.Dispatcher.Invoke((Action)(() => { CanExecuteChanged(this, EventArgs.Empty); }));
            }
        }
    }
    
}
