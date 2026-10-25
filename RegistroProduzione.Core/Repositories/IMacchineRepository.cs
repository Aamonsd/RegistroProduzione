using RegistroProduzione.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegistroProduzione.Core.Repositories
{
    public interface IMacchineRepository
    {
         void Aggiungi(Macchina macchina);
         List<Macchina> OttieniElenco();
         Macchina? TrovaPerCodice(string codice);

    }
}
