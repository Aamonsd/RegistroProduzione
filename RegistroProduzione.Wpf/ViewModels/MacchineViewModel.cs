using RegistroProduzione.Core.Services;
namespace RegistroProduzione.Wpf.ViewModels
{
    public class MacchineViewModel:ViewModelBase
    {


        private readonly ServizioProduzione servizio;

        public MacchineViewModel(ServizioProduzione servizio)
        {
            this.servizio = servizio;
        }
    }
}
