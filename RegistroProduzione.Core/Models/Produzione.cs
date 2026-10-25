 
namespace RegistroProduzione.Core.Models
{
    public class Produzione
    {

        public int Id { get; }
        public string CodiceMacchina{ get; }
        public DateOnly Data { get; }
        public int Turno { get; }
        public int PezziBuoni { get;}
        public int PezziScartati { get; }
        public int PezziTotali => (PezziBuoni + PezziScartati);
        public decimal PercentualeScarto => PezziTotali == 0 ? 0 : (decimal)PezziScartati / PezziTotali * 100;
        
        public Produzione(int id,string codiceMacchina, DateOnly data, int turno, int pezziBuoni, int pezziScartati)
        {
            Id = id;
            CodiceMacchina = codiceMacchina;
            Data = data;
            Turno = turno;
            PezziBuoni = pezziBuoni;
            PezziScartati = pezziScartati;
             
        }
    }
}
