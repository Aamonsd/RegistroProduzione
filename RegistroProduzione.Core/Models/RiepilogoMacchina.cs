using System;
using System.Collections.Generic;
using System.Text;

namespace RegistroProduzione.Core.Models
{
    public record RiepilogoMacchina(string CodiceMacchina, string NomeMacchina, int TotPezziBuoni, int TotPezziScartati);
   


}
