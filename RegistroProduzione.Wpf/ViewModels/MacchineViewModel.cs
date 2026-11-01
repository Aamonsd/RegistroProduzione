using RegistroProduzione.Core.Models;
using RegistroProduzione.Core.Services;
using RegistroProduzione.Wpf.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
namespace RegistroProduzione.Wpf.ViewModels
{
    public class MacchineViewModel:ViewModelBase
    {


        private readonly ServizioProduzione servizio;

        public ICommand AggiungiCommand { get; }

        private string codice="";

        public string Codice
        {
            get { return codice; }
            set 
            { 
                codice = value;
                OnPropertyChanged(nameof(Codice));
            }
        }

        private string nome="";

        public string Nome
        {
            get { return nome; }
            set 
            { 
                nome = value;
                OnPropertyChanged(nameof(Nome));
            }
        }

        private string reparto = "";

        public string Reparto
        {
            get { return reparto; }
            set 
            { 
                reparto = value;
                OnPropertyChanged(nameof(Reparto));
            }
        }

        private string messaggioErrore="";

        public string MessaggioErrore
        {
            get { return messaggioErrore; }
            private set 
            { messaggioErrore = value;
              OnPropertyChanged(nameof(MessaggioErrore));
            }
        }


        public ObservableCollection<Macchina> Macchine { get; } = new();


        public MacchineViewModel(ServizioProduzione servizio)
        {
            this.servizio = servizio;

            AggiungiCommand = new RelayCommand(Aggiungi);
            Carica();
        }


        private void Carica()
        {
            Macchine.Clear();
            foreach(var item in servizio.OttieniMacchine())
            {
                Macchine.Add(item);
            }
        }

        private void Aggiungi()
        {
            if(string.IsNullOrWhiteSpace(Codice)|| string.IsNullOrWhiteSpace(Nome)|| string.IsNullOrWhiteSpace(Reparto))
            {
                MessaggioErrore = "Obbligatorio mettere i dati";
                return;
            }

            try
            {
                servizio.AggiungiMacchina(new Macchina(Codice, Nome, Reparto));
                Carica();
                Codice = "";
                Nome = "";
                Reparto = "";
                MessaggioErrore = "";
            }
            catch (ArgumentException ex)
            {
                MessaggioErrore = ex.Message;
            }
            catch (InvalidOperationException ex)
            {
                MessaggioErrore = ex.Message;
            }
        }
    }
}
