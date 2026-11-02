using RegistroProduzione.Core.Services;

namespace RegistroProduzione.Wpf.ViewModels
{
    public class RiepilogoViewModel : ViewModelBase
    {
        // La schermata viene ricreata a ogni clic sul menu: basta caricare una volta.
        public List<RigaRiepilogo> Righe { get; }

        // Numeri per le card in alto, calcolati dalle righe
        public int TotalePezziBuoni => Righe.Sum(r => r.Dati.TotPezziBuoni);
        public int TotalePezziScartati => Righe.Sum(r => r.Dati.TotPezziScartati);
        public decimal ScartoComplessivo =>
            TotalePezziBuoni + TotalePezziScartati == 0 ? 0
            : (decimal)TotalePezziScartati / (TotalePezziBuoni + TotalePezziScartati) * 100;
        public int MacchineSopraSoglia => Righe.Count(r => r.ScartoAlto);

        public RiepilogoViewModel(ServizioProduzione servizio)
        {
            Righe = servizio.OttieniRiepilogo()
                .Select(r => new RigaRiepilogo(r, r.PercentualeScarto >= ServizioProduzione.SogliaScarto))
                .ToList();
        }
    }
}
