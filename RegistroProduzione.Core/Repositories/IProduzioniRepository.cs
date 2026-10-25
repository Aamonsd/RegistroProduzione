using RegistroProduzione.Core.Models;

namespace RegistroProduzione.Core.Repositories
{
    public interface IProduzioniRepository
    {

        Produzione Registra(string codiceMacchina,DateOnly data,int turno, int pezziBuoni,int pezziScartati);
        bool Esiste(string codiceMacchina, DateOnly data, int turno);
        List<RiepilogoMacchina> Riepilogo();

    }
}
