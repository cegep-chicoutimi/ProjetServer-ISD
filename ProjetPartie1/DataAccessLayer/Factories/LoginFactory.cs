using MySql.Data.MySqlClient;
using Projet.DataAccessLayer;
using Projet.Models;

namespace Projet.DataAccessLayer.Factories
{
    public class LoginFactory
    {
        private string CreateFromReader(MySqlDataReader mySqlDataReader)
        {
            
            string apiKey = mySqlDataReader["ApiKey"].ToString();

            return apiKey;
        }
        
        public string Get(string username, string password)
        {
            string apikey = null;
            MySqlConnection mySqlCnn = null;
            MySqlDataReader mySqlDataReader = null;

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                MySqlCommand mySqlCmd = mySqlCnn.CreateCommand();
                mySqlCmd.CommandText = @"SELECT pm.ApiKey
                    FROM projet_login_membre plm
                    JOIN projet_membre pm ON plm.membreid = pm.Id
                    WHERE plm.username = @Username
                    AND plm.password = @Password;";
                mySqlCmd.Parameters.AddWithValue("@Username", username);
                mySqlCmd.Parameters.AddWithValue("@Password", password);

                mySqlDataReader = mySqlCmd.ExecuteReader();
                if (mySqlDataReader.Read())
                {
                    apikey = CreateFromReader(mySqlDataReader);
                }
            }
            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return apikey;
        }
    }
}
