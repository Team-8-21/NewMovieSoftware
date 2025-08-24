using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MovieOrganiser2000.Helpers
{
    public class RelayCommandMovieTheater : ICommand
    {
        private readonly Action _executeMT;
        private readonly Func<bool> _canExecuteMT;

        public RelayCommandMovieTheater(Action execute, Func<bool> canExecute = null)
        {
            _canExecuteMT = canExecute;
            _executeMT = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return _canExecuteMT?.Invoke() ?? true;
        }

        public void Execute(object? parameter)
        {
            _executeMT();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
