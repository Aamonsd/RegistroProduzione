using System;
using System.Collections.Generic;
using System.Text;

namespace RegistroProduzione.Core.Models
{
    public record RiepilogoMacchina(string CodiceMacchina, string NomeMacchina, int TotPezziBuoni, int TotPezziScartati)
    {
        public int PezziTotali => TotPezziBuoni + TotPezziScartati;

        public decimal PercentualeScarto => PezziTotali == 0 ? 0 : (decimal)TotPezziScartati / PezziTotali * 100;
    }
   


}
