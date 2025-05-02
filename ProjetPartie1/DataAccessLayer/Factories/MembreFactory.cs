using MySql.Data.MySqlClient;
using ProjetPartie1.Models;

namespace Projet.DataAccessLayer.Factories
{
    public class MembreFactory
    {
        private Membre CreateFromReader(MySqlDataReader mySqlDataReader)
        {
            int id = (int)mySqlDataReader["Id"];
            string nom = mySqlDataReader["Nom"].ToString() ?? string.Empty;
            string courriel = mySqlDataReader["Courriel"].ToString() ?? string.Empty;
            string telephone = mySqlDataReader["Telephone"].ToString() ?? string.Empty;
            DateTime dateCreation = (DateTime)mySqlDataReader["DateCreation"];
            string apiKey = mySqlDataReader["ApiKey"].ToString() ?? string.Empty;

            return new Membre(id, nom, courriel, telephone, dateCreation, apiKey);
        }
        public Membre CreateEmpty()
        {
            return new Membre(0, string.Empty, string.Empty, string.Empty, DateTime.MinValue, string.Empty);
        }

        public Membre[] GetAll()
        {
            List<Membre> membres = new List<Membre>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                MySqlCommand mySqlCmd = mySqlCnn.CreateCommand();
                mySqlCmd.CommandText = "SELECT * FROM projet_membre ORDER BY Nom";

                mySqlDataReader = mySqlCmd.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    membres.Add(CreateFromReader(mySqlDataReader));
                }
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return membres.ToArray();
        }

        public Membre? Get(int id)
        {
            Membre? membre = null;
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                MySqlCommand mySqlCmd = mySqlCnn.CreateCommand();
                mySqlCmd.CommandText = "SELECT * FROM projet_membre WHERE Id = @Id";
                mySqlCmd.Parameters.AddWithValue("@Id", id);

                mySqlDataReader = mySqlCmd.ExecuteReader();
                if (mySqlDataReader.Read())
                {
                    membre = CreateFromReader(mySqlDataReader);
                }
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return membre;
        }

        public void Save(Membre membre)
        {
            MySqlConnection? mySqlCnn = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                using (MySqlCommand mySqlCmd = mySqlCnn.CreateCommand())
                {
                    if (membre.Id == 0)
                    {
                        // Nouvel membre
                        mySqlCmd.CommandText =
                            "INSERT INTO projet_membre(Nom, Courriel, Telephone, DateCreation, ApiKey) " +
                            "VALUES (@Nom, @Courriel, @Telephone, @DateCreation, @ApiKey)";
                    }
                    else
                    {
                        // Mise à jour d'un membre existant
                        mySqlCmd.CommandText =
                            "UPDATE projet_membre " +
                            "SET Nom=@Nom, Courriel=@Courriel, Telephone=@Telephone, DateCreation=@DateCreation, ApiKey=@ApiKey " +
                            "WHERE Id=@Id";

                        mySqlCmd.Parameters.AddWithValue("@Id", membre.Id);
                    }

                    // Ajout des paramètres communs
                    mySqlCmd.Parameters.AddWithValue("@Nom", membre.Nom.Trim());
                    mySqlCmd.Parameters.AddWithValue("@Courriel", membre.Courriel.Trim());
                    mySqlCmd.Parameters.AddWithValue("@Telephone", membre.Telephone.Trim());
                    mySqlCmd.Parameters.AddWithValue("@DateCreation", membre.DateCreation);
                    mySqlCmd.Parameters.AddWithValue("@ApiKey", membre.ApiKey.Trim());

                    // Exécution de la commande
                    mySqlCmd.ExecuteNonQuery();

                    if (membre.Id == 0)
                    {
                        // Affectation du nouvel Id si c'est un nouveau membre
                        membre.Id = (int)mySqlCmd.LastInsertedId;

                        using (MySqlCommand quotaCmd = mySqlCnn.CreateCommand())
                        {
                            quotaCmd.CommandText =
                                "INSERT INTO projet_quota_api (MembreId, DateMAJ, MaxRequetes, RequetesUtilisees) " +
                                "VALUES (@MembreId, @DerniereUtilisation, @MaxRequetes, @RequetesUtilisees)";
                            quotaCmd.Parameters.AddWithValue("@MembreId", membre.Id);
                            quotaCmd.Parameters.AddWithValue("@DerniereUtilisation", DateTime.Now);
                            quotaCmd.Parameters.AddWithValue("@MaxRequetes", 20);
                            quotaCmd.Parameters.AddWithValue("@RequetesUtilisees", 0); 

                            quotaCmd.ExecuteNonQuery();
                        }

                    }
                }
            }
            finally
            {
                if (mySqlCnn != null)
                {
                    mySqlCnn.Close();
                }
            }
        }

        public void Delete(int id)
        {
            MySqlConnection? mySqlCnn = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                using (MySqlCommand mySqlCmd = mySqlCnn.CreateCommand())
                {
                    // Suppression d'un membre par son Id
                    mySqlCmd.CommandText = "DELETE FROM projet_membre WHERE Id=@Id";
                    mySqlCmd.Parameters.AddWithValue("@Id", id);
                    mySqlCmd.ExecuteNonQuery();
                }
            }
            finally
            {
                if (mySqlCnn != null)
                {
                    mySqlCnn.Close();
                }
            }
        }

        public Membre? GetMembreByApiKey(string apiKey)
        {
            Membre? membre = null;
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                MySqlCommand mySqlCmd = mySqlCnn.CreateCommand();
                mySqlCmd.CommandText = "SELECT * FROM projet_membre WHERE Apikey = @ApiKey";
                mySqlCmd.Parameters.AddWithValue("@ApiKey", apiKey);

                mySqlDataReader = mySqlCmd.ExecuteReader();
                if (mySqlDataReader.Read())
                {
                    membre = CreateFromReader(mySqlDataReader);
                }
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return membre;
        }
    }
}
