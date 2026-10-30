using Microsoft.Extensions.Configuration;
using RegistroProduzione.Core.Avvio;
using RegistroProduzione.Core.Events;
using RegistroProduzione.Core.Models;
using RegistroProduzione.Core.Repositories;
using RegistroProduzione.Core.Services;
 

namespace RegistroProduzione.App
{
    internal class Program
    {


        private static void ScartoAltoHandler(object? sender, ScartoAltoEventArgs e)
        {
            var produzione = e.Produzione;
            Scrivi($"ATTENZIONE: {produzione.CodiceMacchina}, turno {produzione.Turno} del {produzione.Data}: " +
                   $"scarto {produzione.PercentualeScarto:0.0}% (soglia {ServizioProduzione.SogliaScarto}%)",
                   ConsoleColor.Yellow);
        }

        static void Main(string[] args)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string stringaConnessione = configuration.GetConnectionString("RegistroProduzione")
                ?? throw new InvalidOperationException("Stringa di connessione 'RegistroProduzione' mancante in appsettings.json");

            var servizio = FabbricaServizio.Crea(stringaConnessione);

            servizio.ScartoAlto += ScartoAltoHandler;

            string? scelta;


            do
            {
                Console.Clear();
                StampaMenu();
                scelta = Chiedi("Scelta");
                Console.WriteLine();

                try
                {
                    switch (scelta)
                    {
                        case "1": AggiungiMacchina(servizio); break;
                        case "2": ElencaMacchine(servizio); break;
                        case "3": RegistraProduzione(servizio); break;
                        case "4": MostraRiepilogo(servizio); break;
                        case "0": break;
                        default: Scrivi("Scelta non valida", ConsoleColor.Red); break;
                    }
                }
                catch (ArgumentException ex)
                {
                    Scrivi($"Errore: {ex.Message}", ConsoleColor.Red);
                }
                catch (InvalidOperationException ex)
                {
                    Scrivi($"Operazione non permessa: {ex.Message}", ConsoleColor.Red);
                }

                if (scelta != "0")
                {
                    Console.WriteLine();
                    Console.Write("Premi un tasto per tornare al menu...");
                    Console.ReadKey(true);
                }


            } while (scelta != "0");
         
        }

        private static void StampaMenu()
        {
            Scrivi("╔══════════════════════════════════════╗", ConsoleColor.Cyan);
            Scrivi("║         REGISTRO PRODUZIONE          ║", ConsoleColor.Cyan);
            Scrivi("╚══════════════════════════════════════╝", ConsoleColor.Cyan);
            Console.WriteLine("  1. Aggiungi macchina");
            Console.WriteLine("  2. Elenca macchine");
            Console.WriteLine("  3. Registra produzione");
            Console.WriteLine("  4. Riepilogo");
            Console.WriteLine("  0. Esci");
            Console.WriteLine();
        }

        private static void AggiungiMacchina(ServizioProduzione servizio)
        {
            Titolo("NUOVA MACCHINA");

            string? codice = Chiedi("Codice macchina");
            if (string.IsNullOrWhiteSpace(codice))
            {
                Scrivi("Il codice è obbligatorio", ConsoleColor.Red);
                return;
            }

            string? nome = Chiedi("Nome");
            if (string.IsNullOrWhiteSpace(nome))
            {
                Scrivi("Il nome è obbligatorio", ConsoleColor.Red);
                return;
            }

            string? reparto = Chiedi("Reparto");
            if (string.IsNullOrWhiteSpace(reparto))
            {
                Scrivi("Il reparto è obbligatorio", ConsoleColor.Red);
                return;
            }

            servizio.AggiungiMacchina(new Macchina(codice.Trim(), nome.Trim(), reparto.Trim()));
            Scrivi("Macchina aggiunta", ConsoleColor.Green);
        }

        private static void ElencaMacchine(ServizioProduzione servizio)
        {
            Titolo("ELENCO MACCHINE");

            var macchine = servizio.OttieniMacchine();
            if (macchine.Count == 0)
            {
                Scrivi("Nessuna macchina registrata", ConsoleColor.Yellow);
                return;
            }

            Console.WriteLine($"{"Codice",-12} {"Nome",-25} {"Reparto",-15} {"Attiva",-6}");
            Console.WriteLine($"{new string('-', 12)} {new string('-', 25)} {new string('-', 15)} {new string('-', 6)}");

            foreach (var macchina in macchine)
            {
                Console.WriteLine($"{macchina.Codice,-12} {macchina.Nome,-25} {macchina.Reparto,-15} {(macchina.Attiva ? "Sì" : "No"),-6}");
            }
        }

        private static void RegistraProduzione(ServizioProduzione servizio)
        {
            Titolo("REGISTRA PRODUZIONE");

            string? codice = Chiedi("Codice macchina");
            if (string.IsNullOrWhiteSpace(codice))
            {
                Scrivi("Il codice è obbligatorio", ConsoleColor.Red);
                return;
            }

            string? testoData = Chiedi("Data (gg/mm/aaaa, Invio = oggi)");
            DateOnly data;
            if (string.IsNullOrWhiteSpace(testoData))
            {
                data = DateOnly.FromDateTime(DateTime.Today);
            }
            else if (!DateOnly.TryParse(testoData, out data))
            {
                Scrivi("Data non valida", ConsoleColor.Red);
                return;
            }

            if (!int.TryParse(Chiedi("Turno (1-3)"), out int turno))
            {
                Scrivi("Il turno deve essere un numero", ConsoleColor.Red);
                return;
            }

            if (!int.TryParse(Chiedi("Pezzi buoni"), out int pezziBuoni))
            {
                Scrivi("I pezzi buoni devono essere un numero intero", ConsoleColor.Red);
                return;
            }

            if (!int.TryParse(Chiedi("Pezzi scartati"), out int pezziScartati))
            {
                Scrivi("I pezzi scartati devono essere un numero intero", ConsoleColor.Red);
                return;
            }

            var produzione = servizio.RegistraProduzione(codice.Trim(), data, turno, pezziBuoni, pezziScartati);
            Scrivi($"Produzione n. {produzione.Id} registrata (scarto {produzione.PercentualeScarto:0.0}%)", ConsoleColor.Green);
        }

        private static void MostraRiepilogo(ServizioProduzione servizio)
        {
            Titolo("RIEPILOGO PER MACCHINA");

            var riepilogo = servizio.OttieniRiepilogo();
            if (riepilogo.Count == 0)
            {
                Scrivi("Nessuna macchina registrata", ConsoleColor.Yellow);
                return;
            }

            Console.WriteLine($"{"Codice",-12} {"Nome",-25} {"Buoni",8} {"Scarti",8} {"Scarto %",9}");
            Console.WriteLine($"{new string('-', 12)} {new string('-', 25)} {new string('-', 8)} {new string('-', 8)} {new string('-', 9)}");

            foreach (var riga in riepilogo)
            {
                string testo = $"{riga.CodiceMacchina,-12} {riga.NomeMacchina,-25} {riga.TotPezziBuoni,8} {riga.TotPezziScartati,8} {riga.PercentualeScarto,8:0.0}%";

                if (riga.PercentualeScarto >= ServizioProduzione.SogliaScarto)
                    Scrivi(testo, ConsoleColor.Red);
                else
                    Console.WriteLine(testo);
            }

            Console.WriteLine();
            Console.WriteLine($"In rosso: scarto pari o superiore al {ServizioProduzione.SogliaScarto}%");
        }

        private static string? Chiedi(string domanda)
        {
            Console.Write($"{domanda}: ");
            return Console.ReadLine();
        }

        private static void Scrivi(string testo, ConsoleColor colore)
        {
            Console.ForegroundColor = colore;
            Console.WriteLine(testo);
            Console.ResetColor();
        }

        private static void Titolo(string testo)
        {
            Scrivi($"=== {testo} ===", ConsoleColor.Cyan);
            Console.WriteLine();
        }
    }
}
