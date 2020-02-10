using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileSelectorDemo.CodeBase
{
    public class DelegateCommand : DelegateCommandBase
    {
        public DelegateCommand(Action executeMethod)
            : this(executeMethod, () => true)
        {

        }
       
        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base((o) => executeMethod(), (o) => canExecuteMethod())
        {
            if (executeMethod == null || canExecuteMethod == null)
            {
                throw new ArgumentNullException("executeMethod or canExecuteMethod can not be null!");
            }                
        } 

       
        public virtual async Task Execute()
        {
            await Execute(null);
        }
    
        public virtual bool CanExecute()
        {
            return CanExecute(null);
        }

        #region 异步调用
        public static DelegateCommand FromAsyncHandler(Func<Task> executeMethod)
        {
            return new DelegateCommand(executeMethod);
        }

        public static DelegateCommand FromAsyncHandler(Func<Task> executeMethod, Func<bool> canExecuteMethod)
        {
            return new DelegateCommand(executeMethod, canExecuteMethod);
        }

        private DelegateCommand(Func<Task> executeMethod)
            : this(executeMethod, () => true)
        {

        }

        private DelegateCommand(Func<Task> executeMethod, Func<bool> canExecuteMethod)
            : base((o) => executeMethod(), (o) => canExecuteMethod())
        {
            if (executeMethod == null || canExecuteMethod == null)
            {
                throw new ArgumentNullException("executeMethod or canExecuteMethod can not be null!");
            }
        }
        #endregion
    }

    public class DelegateCommand<T> : DelegateCommandBase
    {

        public DelegateCommand(Action<T> executeMethod)
            : this(executeMethod, (o) => true)
        {

        }

        public DelegateCommand(Action<T> executeMethod, Func<T, bool> canExecuteMethod)
            : base((o) => executeMethod((T)o), (o) => canExecuteMethod((T)o))
        {
            if (executeMethod == null || canExecuteMethod == null)
            {
                throw new ArgumentNullException("executeMethod or canExecuteMethod can not be null!");
            }
            TypeInfo genericTypeInfo = typeof(T).GetTypeInfo();

            if (genericTypeInfo.IsValueType)
            {
                if ((!genericTypeInfo.IsGenericType)
                    || (!typeof(Nullable<>).GetTypeInfo().IsAssignableFrom(genericTypeInfo.GetGenericTypeDefinition().GetTypeInfo())))
                {
                    throw new InvalidCastException("InvalidType");
                }
            }
        }

        public virtual bool CanExecute(T parameter)
        {
            return CanExecute(parameter);
        }

        public virtual async Task Execute(T parameter)
        {
            await Execute(parameter);
        }

        #region 异步调用
        public static DelegateCommand<T> FromAsyncHandler(Func<T, Task> executeMethod)
        {
            return new DelegateCommand<T>(executeMethod);
        }

        public static DelegateCommand<T> FromAsyncHandler(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
        {
            return new DelegateCommand<T>(executeMethod, canExecuteMethod);
        }

        private DelegateCommand(Func<T, Task> executeMethod)
            : this(executeMethod, (o) => true)
        {

        }

        private DelegateCommand(Func<T, Task> executeMethod, Func<T, bool> canExecuteMethod)
            : base((o) => executeMethod((T)o), (o) => canExecuteMethod((T)o))
        {
            if (executeMethod == null || canExecuteMethod == null)
            {
                throw new ArgumentNullException("executeMethod or canExecuteMethod can not be null!");
            }
        }
        #endregion
       
    }
}
