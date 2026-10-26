using RegistroProduzione.Core.Models;
 
namespace RegistroProduzione.Core.Events
{
    public class ScartoAltoEventArgs:EventArgs
    {
        public Produzione Produzione { get; }

        public ScartoAltoEventArgs(Produzione produzione)
        {
            Produzione = produzione;
        }
    }
}
