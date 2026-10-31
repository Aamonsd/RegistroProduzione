
using System.ComponentModel;
 
namespace RegistroProduzione.Wpf.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string NomeProprieta)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(NomeProprieta));
        }
    }
}
