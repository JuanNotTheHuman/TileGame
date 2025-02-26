using System;
using System.Diagnostics;
using System.Windows.Input;

namespace TileGame
{
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter=null)
        {
            bool result = false;
            if(parameter == null)
            {
                result = _canExecute == null || _canExecute(default);
            }
            else if (parameter is T typedParameter)
            {
                result = _canExecute == null || _canExecute(typedParameter);

            }
            return result;
        }
        public void Execute(object parameter)
        {
            if (parameter is T typedParameter)
                _execute(typedParameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
