using RegistroProduzione.Core.Repositories;
using RegistroProduzione.Core.Services;
 
namespace RegistroProduzione.Core.Avvio
{
    public static class FabbricaServizio
    {
        public static ServizioProduzione Crea(string stringaConnessione)
        {

            return new ServizioProduzione(

                 new SqlMacchineRepository(stringaConnessione),
                 new SqlProduzioniRepository(stringaConnessione)
                );
        }
    }
}
