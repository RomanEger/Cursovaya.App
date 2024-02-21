using System.Windows.Input;
#pragma warning disable CS8612 // Nullability of reference types in type doesn't match implicitly implemented member.

namespace CursovayaApp.WPF.Commands
{
    public class RelayCommand : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        {
            this._execute = execute;
            this._canExecute = canExecute;
        }

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public bool CanExecute(object parameter) =>
            this._canExecute == null || this._canExecute(parameter);
        

        public void Execute(object parameter) =>
            this._execute(parameter);
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        
    }
}
