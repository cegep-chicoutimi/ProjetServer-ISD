using MySql.Data.MySqlClient;
using ProjetPartie1.Models;

namespace Projet.DataAccessLayer.Factories
{
    public class CatégorieFactory
    {
        private Catégorie CreateFromReader(MySqlDataReader mySqlDataReader)
        {
            int id = (int)mySqlDataReader["Id"];
            string nom = mySqlDataReader["Nom"].ToString() ?? string.Empty;

            return new Catégorie(id, nom);
        }

        public Catégorie CreateEmpty()
        {
            return new Catégorie(0, string.Empty);
        }

        public Catégorie[] GetAll()
        {
            List<Catégorie> categories = new List<Catégorie>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                MySqlCommand mySqlCmd = mySqlCnn.CreateCommand();
                mySqlCmd.CommandText = "SELECT * FROM projet_categorie";

                mySqlDataReader = mySqlCmd.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    categories.Add(CreateFromReader(mySqlDataReader));
                }
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return categories.ToArray();
        }

        public Catégorie? Get(int id)
        {
            Catégorie? categorie = null;
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                MySqlCommand mySqlCmd = mySqlCnn.CreateCommand();
                mySqlCmd.CommandText = "SELECT * FROM projet_categorie WHERE Id = @Id";
                mySqlCmd.Parameters.AddWithValue("@Id", id);

                mySqlDataReader = mySqlCmd.ExecuteReader();
                if (mySqlDataReader.Read())
                {
                    categorie = CreateFromReader(mySqlDataReader);
                }
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return categorie;
        }

        public void Save(Catégorie categorie)
        {
            MySqlConnection? mySqlCnn = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                using (MySqlCommand mySqlCmd = mySqlCnn.CreateCommand())
                {
                    if (categorie.Id == 0)
                    {
                        // Nouveau produit avec Id == 0
                        mySqlCmd.CommandText = "INSERT INTO projet_categorie(Nom) VALUES (@Nom)";
                    }
                    else
                    {
                        // Mise à jour d'un produit existant
                        mySqlCmd.CommandText = "UPDATE projet_categorie SET Nom=@Nom WHERE Id=@Id";
                        mySqlCmd.Parameters.AddWithValue("@Id", categorie.Id);
                    }

                    mySqlCmd.Parameters.AddWithValue("@Nom", categorie.Nom.Trim());
                    mySqlCmd.ExecuteNonQuery();

                    if (categorie.Id == 0)
                    {
                        // Si c'était un nouvel enregistrement, récupérer l'ID généré
                        categorie.Id = (int)mySqlCmd.LastInsertedId;
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
                    // Suppression d'une catégorie par ID
                    mySqlCmd.CommandText = "DELETE FROM projet_categorie WHERE Id=@Id";
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

        public Catégorie? GetByLivreId(int livreId)
        {
            Catégorie? categorie = null;
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                MySqlCommand mySqlCmd = mySqlCnn.CreateCommand();
                mySqlCmd.CommandText = "SELECT * FROM projet_categorie WHERE Id = @Id";
                mySqlCmd.Parameters.AddWithValue("@Id", livreId);

                mySqlDataReader = mySqlCmd.ExecuteReader();
                if (mySqlDataReader.Read())
                {
                    categorie = CreateFromReader(mySqlDataReader);
                }
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return categorie;
        }
    }
}