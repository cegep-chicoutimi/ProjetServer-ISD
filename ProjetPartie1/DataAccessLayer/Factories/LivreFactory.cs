using MySql.Data.MySqlClient;
using ProjetPartie1.Models;

namespace Projet.DataAccessLayer.Factories
{
    public class LivreFactory
    {
        private Livre CreateFromReader(MySqlDataReader mySqlDataReader)
        {
            int id = (int)mySqlDataReader["Id"];
            string isbn = mySqlDataReader["ISBN"].ToString() ?? string.Empty;
            string titre = mySqlDataReader["Titre"].ToString() ?? string.Empty;
            int nbPages = (int)mySqlDataReader["NbPages"];
            int auteurId = (int)mySqlDataReader["AuteurId"];
            int categorieId = (int)mySqlDataReader["CategorieId"];

            Livre livre =  new Livre(id, isbn, titre, nbPages, auteurId, categorieId);

            DAL dAL = new DAL();
            livre.Auteur = dAL.AuteurFactory.Get(livre.AuteurId);
            livre.Catégorie = dAL.CatégorieFactory.Get(livre.CategorieId);

            return livre;
        }

        public Livre CreateEmpty()
        {
            return new Livre(0, string.Empty, string.Empty, 0, 0, 0);
        }

        public Livre[] GetAll()
        {
            List<Livre> livres = new List<Livre>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                MySqlCommand mySqlCmd = mySqlCnn.CreateCommand();
                mySqlCmd.CommandText = "SELECT * FROM projet_livre";

                mySqlDataReader = mySqlCmd.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    livres.Add(CreateFromReader(mySqlDataReader));
                }
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return livres.ToArray();
        }

        public Livre? Get(int id)
        {
            Livre? livre = null;
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                MySqlCommand mySqlCmd = mySqlCnn.CreateCommand();
                mySqlCmd.CommandText = "SELECT * FROM projet_livre WHERE Id = @Id";
                mySqlCmd.Parameters.AddWithValue("@Id", id);

                mySqlDataReader = mySqlCmd.ExecuteReader();
                if (mySqlDataReader.Read())
                {
                    livre = CreateFromReader(mySqlDataReader);
                }
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return livre;
        }

        public void Save(Livre livre)
        {
            MySqlConnection? mySqlCnn = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                using (MySqlCommand mySqlCmd = mySqlCnn.CreateCommand())
                {
                    if (livre.Id == 0)
                    {
                        // Nouveau livre avec Id == 0
                        mySqlCmd.CommandText = "INSERT INTO projet_livre(ISBN, Titre, NbPages, AuteurId, CategorieId) " +
                                               "VALUES (@ISBN, @Titre, @NbPages, @AuteurId, @CategorieId)";
                    }
                    else
                    {
                        // Mise à jour d'un livre existant
                        mySqlCmd.CommandText = "UPDATE projet_livre " +
                                               "SET ISBN=@ISBN, Titre=@Titre, NbPages=@NbPages, AuteurId=@AuteurId, CategorieId=@CategorieId " +
                                               "WHERE Id=@Id";

                        mySqlCmd.Parameters.AddWithValue("@Id", livre.Id);
                    }

                    // Ajout des paramètres communs
                    mySqlCmd.Parameters.AddWithValue("@ISBN", livre.ISBN.Trim());
                    mySqlCmd.Parameters.AddWithValue("@Titre", livre.Titre.Trim());
                    mySqlCmd.Parameters.AddWithValue("@NbPages", livre.NbPages);
                    mySqlCmd.Parameters.AddWithValue("@AuteurId", livre.AuteurId);
                    mySqlCmd.Parameters.AddWithValue("@CategorieId", livre.CategorieId);

                    // Exécution de la requête
                    mySqlCmd.ExecuteNonQuery();

                    if (livre.Id == 0)
                    {
                        // Si c'était un nouveau livre (requête INSERT), on récupère l'ID généré
                        livre.Id = (int)mySqlCmd.LastInsertedId;
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
                    // Suppression d'un livre par ID
                    mySqlCmd.CommandText = "DELETE FROM projet_livre WHERE Id=@Id";
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

        public Livre[] GetAllByAutorId(int autorId)
        {
            List<Livre> livres = new List<Livre>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                MySqlCommand mySqlCmd = mySqlCnn.CreateCommand();
                mySqlCmd.CommandText = "SELECT * FROM projet_livre WHERE AuteurId = @Id";
                mySqlCmd.Parameters.AddWithValue("@Id", autorId);

                mySqlDataReader = mySqlCmd.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    livres.Add(CreateFromReader(mySqlDataReader));
                }
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return livres.ToArray();
        }

        public Livre[] GetAllByCategoryId(int CategorieId)
        {
            List<Livre> livres = new List<Livre>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                MySqlCommand mySqlCmd = mySqlCnn.CreateCommand();
                mySqlCmd.CommandText = "SELECT * FROM projet_livre WHERE CategorieId = @Id";
                mySqlCmd.Parameters.AddWithValue("@Id", CategorieId);

                mySqlDataReader = mySqlCmd.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    livres.Add(CreateFromReader(mySqlDataReader));
                }
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return livres.ToArray();
        }

        public Livre[] GetAllByEmrprunts(int action)
        {
            List<Livre> livres = new List<Livre>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                MySqlCommand mySqlCmd = mySqlCnn.CreateCommand();

                if (action == 1)
                    mySqlCmd.CommandText = "select l.Id, ISBN, Titre, NbPages, AuteurId, CategorieId from projet_livre l \r\nleft Join projet_emprunt e on e.LivreId = l.Id\r\nwhere e.LivreId is null";

                else
                    mySqlCmd.CommandText = "select l.Id, ISBN, Titre, NbPages, AuteurId, CategorieId from projet_livre l \r\nleft Join projet_emprunt e on e.LivreId = l.Id\r\nwhere e.LivreId != null";

                
                mySqlDataReader = mySqlCmd.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    livres.Add(CreateFromReader(mySqlDataReader));
                }
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return livres.ToArray();
        }

        public Livre[] GetAllByMembre(int action, int MembreId)
        {
            List<Livre> livres = new List<Livre>();
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                MySqlCommand mySqlCmd = mySqlCnn.CreateCommand();

                if (action == 1)
                    mySqlCmd.CommandText = "select l.Id, ISBN, Titre, NbPages, AuteurId, CategorieId from projet_livre l" +
                        "\r\nleft Join projet_emprunt e on e.LivreId = l.Id" +
                        "\r\nleft join projet_membre m on m.Id = e.MembreId" +
                        "\r\nwhere MembreId = @Id and DateRetour < current_date()";

                else
                    mySqlCmd.CommandText = "select l.Id, ISBN, Titre, NbPages, AuteurId, CategorieId from projet_livre l" +
                        "\r\nleft Join projet_emprunt e on e.LivreId = l.Id" +
                        "\r\nleft join projet_membre m on m.Id = e.MembreId" +
                        "\r\nwhere MembreId = @Id and DateRetour > current_date()";

                mySqlCmd.Parameters.AddWithValue("@Id", MembreId);
                mySqlDataReader = mySqlCmd.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    livres.Add(CreateFromReader(mySqlDataReader));
                }
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return livres.ToArray();
        }
    }
}
