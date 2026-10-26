using Microsoft.Data.SqlClient;
using RegistroProduzione.Core.Models;
 
namespace RegistroProduzione.Core.Repositories
{
    public class SqlProduzioniRepository : IProduzioniRepository
    {


        private readonly string stringaConnessione;

        public SqlProduzioniRepository(string stringaConnessione)
        {
            this.stringaConnessione = stringaConnessione;
        }


        private static  Produzione LeggiProduzione(SqlDataReader lettore)
        {
            var produzione = new Produzione(lettore.GetInt32(0), lettore.GetString(1), lettore.GetFieldValue<DateOnly>(2), lettore.GetByte(3),
                lettore.GetInt32(4), lettore.GetInt32(5));
            return produzione;
        }

        public bool Esiste(string codiceMacchina, DateOnly data, int turno)
        {
            using var connessione = new SqlConnection(stringaConnessione);
            connessione.Open();

            using var  comando = new SqlCommand(

                "SELECT COUNT(*) FROM Produzioni " +
                "WHERE CodiceMacchina=@codiceMacchina AND Data=@data AND Turno=@turno ",connessione
                );


            comando.Parameters.AddWithValue("@codiceMacchina", codiceMacchina);
            comando.Parameters.AddWithValue("@data", data);
            comando.Parameters.AddWithValue("@turno", turno);

            var risultato = (int)comando.ExecuteScalar();

            return risultato > 0;

        }

        public Produzione Registra(string codiceMacchina, DateOnly data, int turno, int pezziBuoni, int pezziScartati)
        {
            using  var connessione = new SqlConnection(stringaConnessione);
            connessione.Open();

            using var comando = new SqlCommand(

                "INSERT INTO Produzioni (CodiceMacchina,Data,Turno,PezziBuoni,PezziScartati) " +
                "OUTPUT INSERTED.Id,INSERTED.CodiceMacchina,INSERTED.Data,INSERTED.Turno,INSERTED.PezziBuoni,INSERTED.PezziScartati " +
                "VALUES (@codiceMacchina,@data,@turno,@pezziBuoni,@pezziScartati) ",connessione
                );

            comando.Parameters.AddWithValue("@codiceMacchina", codiceMacchina);
            comando.Parameters.AddWithValue("@data", data);
            comando.Parameters.AddWithValue("@turno", (byte)turno);
            comando.Parameters.AddWithValue("@pezziBuoni", pezziBuoni);
            comando.Parameters.AddWithValue("@pezziScartati", pezziScartati);

            Produzione produzione;

            using (var lettore = comando.ExecuteReader())
            {
                lettore.Read();
                produzione = LeggiProduzione(lettore);

            }
            return produzione;


        }

        public List<RiepilogoMacchina> Riepilogo()
        {
            using var connessione = new SqlConnection(stringaConnessione);
            connessione.Open();

            
            using var comando = new SqlCommand(

                "SELECT Macchine.Codice,Macchine.Nome,ISNULL(SUM(Produzioni.PezziBuoni),0) AS TotPezziBuoni,ISNULL(SUM(Produzioni.PezziScartati),0) AS TotPezziScartati " +
                "FROM Macchine " +
                "LEFT JOIN Produzioni ON Macchine.Codice = Produzioni.CodiceMacchina " +
                "GROUP BY Macchine.Codice ,Macchine.Nome " +
                "ORDER BY Macchine.Codice ", connessione

                );

            List<RiepilogoMacchina> riepilogo=new();

            using var lettore = comando.ExecuteReader();

            while (lettore.Read())
            {
                riepilogo.Add(new(lettore.GetString(0), lettore.GetString(1), lettore.GetInt32(2), lettore.GetInt32(3)));
            }

            return riepilogo;





        }
    }
}
