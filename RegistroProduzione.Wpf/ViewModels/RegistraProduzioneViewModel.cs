using RegistroProduzione.Core.Events;
using RegistroProduzione.Core.Models;
using RegistroProduzione.Core.Services;
using RegistroProduzione.Wpf.Commands;
using System.Windows.Input;

namespace RegistroProduzione.Wpf.ViewModels
{
    public class RegistraProduzioneViewModel : ViewModelBase, IDisposable
    {

        

        private readonly ServizioProduzione servizio;

        public List<Macchina> Macchine { get; }
        public List<int> Turni { get; } = [1, 2, 3];
        public ICommand RegistraCommand { get; }

        private Macchina? macchinaSelezionata = null;

        public Macchina? MacchinaSelezionata
        {
            get { return macchinaSelezionata; }
            set
            {
                macchinaSelezionata = value;
                OnPropertyChanged(nameof(MacchinaSelezionata));
            }
        }


        private DateTime? data= DateTime.Today;
        public DateTime? Data
        {
            get { return data; }
            set
            {
                data = value;
                OnPropertyChanged(nameof(Data));
            }
        }

        private int turnoSelezionato=1;
        public int TurnoSelezionato
        {
            get { return turnoSelezionato; }
            set
            {
                turnoSelezionato = value;
                OnPropertyChanged(nameof(TurnoSelezionato));
            }
        }


        private string pezziBuoni = "";
        public string PezziBuoni
        {
            get { return pezziBuoni; }
            set
            {
                pezziBuoni = value;
                OnPropertyChanged(nameof(PezziBuoni));
            }
        }

        private string pezziScartati = "";
        public string PezziScartati
        {
            get { return pezziScartati; }
            set
            {
                pezziScartati = value;
                OnPropertyChanged(nameof(PezziScartati));
            }
        }
        private string messaggioErrore = "";

        public string MessaggioErrore
        {
            get { return messaggioErrore; }
            private set
            {
                messaggioErrore = value;
                OnPropertyChanged(nameof(MessaggioErrore));

            }
        }

        private string messaggioEsito = "";

        public string MessaggioEsito
        {
            get { return messaggioEsito; }
            private set
            {
                messaggioEsito = value;
                OnPropertyChanged(nameof(MessaggioEsito));

            }
        }

        private string avvisoScarto = "";
        public string AvvisoScarto
        {
            get { return avvisoScarto; }
            private set
            {
                avvisoScarto = value;
                OnPropertyChanged(nameof(AvvisoScarto));
            }
        }


        public RegistraProduzioneViewModel(ServizioProduzione servizio)
        {
            
            this.servizio = servizio;
            servizio.ScartoAlto += ScartoHandler;
            Macchine = servizio.OttieniMacchine().Where(m => m.Attiva).ToList();
            RegistraCommand = new RelayCommand(Registra);
        }

        private void Registra()
        {
            MessaggioErrore = "";
            MessaggioEsito = "";
            AvvisoScarto = "";
             
            if (MacchinaSelezionata is null)
            {
                MessaggioErrore = "macchina non selezionata";
                return;
            }
            if(Data is null)
            {
                MessaggioErrore = "data non selezionata";
                return;
            }

            if(!int.TryParse(PezziBuoni,out int pezziBuoniconv))
            {
                MessaggioErrore = "numero non valido";
                return;
            }
            if(!int.TryParse(PezziScartati,out int pezziScartaticonv))
            {
                MessaggioErrore = "numero non valido";
                return;
            }

            
            try
            {
                var produzione = servizio.RegistraProduzione(MacchinaSelezionata.Codice, DateOnly.FromDateTime(Data.Value), TurnoSelezionato, pezziBuoniconv, pezziScartaticonv);
                MessaggioEsito = $"ID: {produzione.Id} percentuale scarto: {produzione.PercentualeScarto}";
                PezziBuoni = "";
                PezziScartati = "";
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
        private void ScartoHandler(object? sender, ScartoAltoEventArgs e)
        {
            AvvisoScarto = $"Scarto alto {e.Produzione.CodiceMacchina}: {e.Produzione.PercentualeScarto:0.0}%";
        }

        public void Dispose()
        {
            servizio.ScartoAlto -= ScartoHandler;
        }

       


        



    }
}
