using Microsoft.Extensions.Configuration;
using RegistroProduzione.Core.Avvio;
using RegistroProduzione.Wpf.ViewModels;
using System.Windows;

namespace RegistroProduzione.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string stringaConnessione = configuration.GetConnectionString("RegistroProduzione")
                ?? throw new InvalidOperationException("Stringa di connessione 'RegistroProduzione' mancante in appsettings.json");

            var servizio = FabbricaServizio.Crea(stringaConnessione);

            var finestra = new MainWindow();
            finestra.DataContext = new MainViewModel(servizio);
            finestra.Show();
        }
    }

}
