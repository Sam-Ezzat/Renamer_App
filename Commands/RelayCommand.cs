using System;
using System.Windows.Input;

#nullable enable // Ensures nullability context is active for this file

namespace RenamerApp.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            _execute(parameter);
        }

        /// <summary>
        /// Method to raise CanExecuteChanged event.
        /// This is useful if CommandManager.RequerySuggested is not sufficient.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            // Instead of directly invoking the event, use CommandManager.InvalidateRequerySuggested()
            // This will trigger the CommandManager.RequerySuggested event, which our CanExecuteChanged is hooked to
            CommandManager.InvalidateRequerySuggested();
        }
    }
}