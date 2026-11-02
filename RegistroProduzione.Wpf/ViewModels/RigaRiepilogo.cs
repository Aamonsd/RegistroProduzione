using RegistroProduzione.Core.Models;

namespace RegistroProduzione.Wpf.ViewModels
{
    // Una riga della tabella riepilogo: i dati di Core più l'informazione
    // "da evidenziare", che serve solo alla grafica (il DataTrigger non sa fare >=).
    public record RigaRiepilogo(RiepilogoMacchina Dati, bool ScartoAlto);
}
