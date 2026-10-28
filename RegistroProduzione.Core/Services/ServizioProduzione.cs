using RegistroProduzione.Core.Events;
using RegistroProduzione.Core.Models;
using RegistroProduzione.Core.Repositories;

namespace RegistroProduzione.Core.Services
{
    public class ServizioProduzione
    {
        private readonly IMacchineRepository repositoryMacchine;
        private readonly IProduzioniRepository repositoryProduzioni;

        public event EventHandler<ScartoAltoEventArgs>? ScartoAlto;
        public const decimal SogliaScarto = 5m;

        public ServizioProduzione(IMacchineRepository repositoryMacchine, IProduzioniRepository repositoryProduzione)
        {
            this.repositoryMacchine = repositoryMacchine;
            this.repositoryProduzioni = repositoryProduzione;
        }

        public void AggiungiMacchina(Macchina macchina)
        {
            if (repositoryMacchine.TrovaPerCodice(macchina.Codice) != null) 
                throw new InvalidOperationException("Macchina gia esistente");

            repositoryMacchine.Aggiungi(macchina);

        }

        public List<Macchina> OttieniMacchine()
        {
            return repositoryMacchine.OttieniElenco();
        }

        public Produzione RegistraProduzione(string codiceMacchina, DateOnly data, int turno, int pezziBuoni, int pezziScartati)
        {
            if (turno<1||turno>3) 
                throw new ArgumentException("turno non valido");

            if (pezziBuoni < 0 || pezziScartati < 0) 
                throw new ArgumentException("il numero di pezzi non puo essere negativo");

            if (data > DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("La data non può essere nel futuro.");

            var macchina = repositoryMacchine.TrovaPerCodice(codiceMacchina);

            if (macchina is null) 
                throw new ArgumentException("macchina non esiste");

            if (!macchina.Attiva)
                throw new InvalidOperationException("Macchina inattiva");

            if (repositoryProduzioni.Esiste(macchina.Codice, data, turno)) 
                throw new InvalidOperationException($"Il turno {turno} del {data} è già registrato per {codiceMacchina}");

            var registrazioneProduzione= repositoryProduzioni.Registra(macchina.Codice, data, turno, pezziBuoni, pezziScartati);

            if (registrazioneProduzione.PercentualeScarto >= SogliaScarto) ScartoAlto?.Invoke(this, new ScartoAltoEventArgs(registrazioneProduzione));
            return registrazioneProduzione;
            
        }

        public List<RiepilogoMacchina> OttieniRiepilogo()
        {
            return repositoryProduzioni.Riepilogo();
        }





    }
}
