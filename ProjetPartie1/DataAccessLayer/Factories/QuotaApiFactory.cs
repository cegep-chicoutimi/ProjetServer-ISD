using MySql.Data.MySqlClient;
using Projet.DataAccessLayer;
using Projet.Models;

namespace Projet.DataAccessLayer.Factories
{
    public class QuotaApiFactory
    {

        private QuotaApi CreateFromReader(MySqlDataReader mySqlDataReader)
        {
            int id = (int)mySqlDataReader["Id"];
            int membreId = (int)mySqlDataReader["MembreId"];
            DateTime dateMAJ = (DateTime)mySqlDataReader["DateMAJ"];
            int maxRequetes = (int)mySqlDataReader["MaxRequetes"];
            int requetesUtilisees = (int)mySqlDataReader["RequetesUtilisees"];

            return new QuotaApi(id, membreId, dateMAJ, maxRequetes, requetesUtilisees);
        }
        public QuotaApi CreateEmpty()
        {
            return new QuotaApi(0, 0, DateTime.MinValue, 0, 0);
        }

        public QuotaApi? GetByMemBreId(int membreId)
        {
            QuotaApi? quotaApi = null;
            MySqlConnection? mySqlCnn = null;
            MySqlDataReader? mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                MySqlCommand mySqlCmd = mySqlCnn.CreateCommand();
                mySqlCmd.CommandText = "SELECT * FROM projet_quota_api WHERE MembreId = @MembreId";
                mySqlCmd.Parameters.AddWithValue("@MembreId", membreId);

                mySqlDataReader = mySqlCmd.ExecuteReader();
                if (mySqlDataReader.Read())
                {
                    quotaApi = CreateFromReader(mySqlDataReader);
                }
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return quotaApi;
        }

        public void Save(QuotaApi quotaApi)
        {
            MySqlConnection? mySqlCnn = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                using (MySqlCommand mySqlCmd = mySqlCnn.CreateCommand())
                {
                    // Mise à jour d'un livre existant
                    mySqlCmd.CommandText = "UPDATE projet_quota_api " +
                                           "SET DateMAJ=@DateMAJ, RequetesUtilisees=@RequetesUtilisees " +
                                           "WHERE Id=@Id";

                    mySqlCmd.Parameters.AddWithValue("@Id", quotaApi.Id);

                    // Ajout des paramètres communs
                    mySqlCmd.Parameters.AddWithValue("@DateMAJ", quotaApi.DateMAJ);
                    mySqlCmd.Parameters.AddWithValue("@RequetesUtilisees", quotaApi.RequeteUtilisees);

                    // Exécution de la requête
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
    }
}
