
using System.Windows.Input;

namespace RegistroProduzione.Wpf.Commands
{
    internal class RelayCommand : ICommand
    {
        private readonly Action azione;
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action azione)
        {
            this.azione=azione;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            azione();
        }
    }
}
