using System;
using System.Diagnostics;
using System.Windows.Input;

namespace MusicPlayer.Utils
{
    public class RelayCommand : ICommand
    {
        #region Fields
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;
        protected EventHandler _internalCanExecuteChanged;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Creates a command that can always execute
        /// </summary>
        /// <param name="execute">The action to execute</param>
        public RelayCommand(Action<object> execute) : this(execute, null) { }

        /// <summary>
        /// Creates a new command where execution is bound to the passed predicate.
        /// </summary>
        /// <param name="execute">The action to execute</param>
        /// <param name="canExecute">The predicate that determines if the action can be executed.</param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion Constructors

        #region ICommandMembers
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                _internalCanExecuteChanged += value;
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_internalCanExecuteChanged != null)
                    _internalCanExecuteChanged -= value;
                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            if (_canExecute != null)
                OnCanExecuteChanged();
        }

        private void OnCanExecuteChanged()
        {
            EventHandler canExecuteChanged = _internalCanExecuteChanged;
            if (canExecuteChanged != null)
                canExecuteChanged(this, EventArgs.Empty);
        }

        #endregion ICommandMembers
    }
}