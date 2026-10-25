using System;
using System.Collections.Generic;
using System.Text;

namespace RegistroProduzione.Core.Models
{
    public class Macchina
    {
        public string Codice { get; }
        public string Nome { get;}
        public string Reparto { get;}
        public bool Attiva { get; set; }

        public Macchina(string codice, string nome, string reparto, bool attiva=true)
        {
            Codice = codice;
            Nome = nome;
            Reparto = reparto;
            Attiva = attiva;
        }
    }
}
