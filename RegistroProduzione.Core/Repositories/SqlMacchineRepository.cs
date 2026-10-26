using Microsoft.Data.SqlClient;
using RegistroProduzione.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegistroProduzione.Core.Repositories
{
    public class SqlMacchineRepository : IMacchineRepository
    {


        private readonly string stringaConnessione;

        public SqlMacchineRepository(string stringaConnessione)
        {
            this.stringaConnessione = stringaConnessione;
        }

        private static Macchina LeggiMacchina(SqlDataReader lettore)
        {
            var macchina = new Macchina(lettore.GetString(0), lettore.GetString(1), 
                lettore.GetString(2), lettore.GetBoolean(3));
            return macchina;
        }

        public void Aggiungi(Macchina macchina)
        {
            using var connessione = new SqlConnection(stringaConnessione);
            connessione.Open();

            using var comando = new SqlCommand(

                "INSERT INTO Macchine(Codice,Nome,Reparto,Attiva) " +
                "VALUES (@codice,@nome,@reparto,@attiva) ",connessione 

                );

            comando.Parameters.AddWithValue("@codice", macchina.Codice);
            comando.Parameters.AddWithValue("@nome", macchina.Nome);
            comando.Parameters.AddWithValue("@reparto", macchina.Reparto);
            comando.Parameters.AddWithValue("@attiva",macchina.Attiva);

            comando.ExecuteNonQuery();


        }

        public List<Macchina> OttieniElenco()
        {
            
            using var connessione = new SqlConnection(stringaConnessione);
            connessione.Open();

            List<Macchina> macchine = new();

            using var comando = new SqlCommand(

                "SELECT Codice,Nome,Reparto,Attiva FROM Macchine ORDER BY Codice", connessione
                
                );

            using var lettore = comando.ExecuteReader();

            while (lettore.Read())
            {
                macchine.Add(LeggiMacchina(lettore));
            }
            return macchine;
           

        }

        public Macchina? TrovaPerCodice(string codice)
        {
            using var connessione = new SqlConnection(stringaConnessione);
            connessione.Open();

            using var comando = new SqlCommand(

                "SELECT Codice,Nome,Reparto,Attiva FROM Macchine WHERE Codice=@codice",connessione
                );

            comando.Parameters.AddWithValue("@codice", codice);

            using var lettore = comando.ExecuteReader();

            if (lettore.Read()) return LeggiMacchina(lettore);
            return null;
           
            


        }
    }
}
