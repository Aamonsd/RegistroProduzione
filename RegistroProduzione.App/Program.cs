using Microsoft.Extensions.Configuration;
using RegistroProduzione.Core.Events;
using RegistroProduzione.Core.Repositories;
using RegistroProduzione.Core.Services;

namespace RegistroProduzione.App
{
    internal class Program
    {


        private static void ScartoAltoHandler(object? sender,ScartoAltoEventArgs e)
        {
            Console.WriteLine($"ATTENZIONE: Codice macchina: {e.Produzione.CodiceMacchina} superata la soglia di scarto");
        }
        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string stringaConnessione = configuration.GetConnectionString("RegistroProduzione") 
                ?? throw new InvalidOperationException("Stringa di connessione 'RegistroProduzione' mancante in appsettings.json");

            ServizioProduzione servizio = new ServizioProduzione(
                new SqlMacchineRepository(stringaConnessione),
                new SqlProduzioniRepository(stringaConnessione)
                );

            servizio.ScartoAlto += ScartoAltoHandler;

             














        }
    }
}
