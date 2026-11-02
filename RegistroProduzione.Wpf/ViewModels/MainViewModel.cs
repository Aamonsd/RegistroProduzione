using RegistroProduzione.Core.Services;
using RegistroProduzione.Wpf.Commands;
using System.Windows.Input;

namespace RegistroProduzione.Wpf.ViewModels
{
    public class MainViewModel:ViewModelBase
    {

        private readonly ServizioProduzione servizio;

        public ICommand MostraMacchineCommand { get; }
        public ICommand MostraRegistraProduzioneCommand { get; }
        public ICommand MostraRiepilogoCommand { get; }

        

        private ViewModelBase? viewModelCorrente;

        public ViewModelBase? ViewModelCorrente
        {
            get { return viewModelCorrente; }
            private set 
            {
                (viewModelCorrente as IDisposable)?.Dispose();
                viewModelCorrente = value;
                OnPropertyChanged(nameof(ViewModelCorrente));
            }
        }
        public MainViewModel(ServizioProduzione servizio)
        {
            this.servizio = servizio;
            MostraMacchineCommand = new RelayCommand(() => ViewModelCorrente = new MacchineViewModel(servizio));
            MostraRegistraProduzioneCommand=new RelayCommand(() => ViewModelCorrente = new RegistraProduzioneViewModel(servizio));
            MostraRiepilogoCommand=new RelayCommand(() => ViewModelCorrente = new RiepilogoViewModel(servizio));
            ViewModelCorrente = new MacchineViewModel(servizio);

           
        }




    }
}
