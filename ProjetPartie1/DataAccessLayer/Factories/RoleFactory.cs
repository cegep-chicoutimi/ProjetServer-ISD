using MySql.Data.MySqlClient;
using Projet.DataAccessLayer;

namespace Projet.DataAccessLayer.Factories
{
    public class RoleFactory
    {
        private List<string> CreateFromRead(MySqlDataReader mySqlDataReader)
        {
            List<string> roles = new List<string>();

            while (mySqlDataReader.Read())
            {
                roles.Add(mySqlDataReader["Role"].ToString() ?? string.Empty); // Recupere le nom du role
            }
            return roles;
        }

        public List<string> GetRolesFromApiKey(string apikey)
        {
            MySqlConnection mySqlCnn = null;
            MySqlDataReader mySqlDataReader = null;
            List<string> roles = new List<string>();

            try
            {
                mySqlCnn = new MySqlConnection(DAL.ConnectionString);
                mySqlCnn.Open();

                MySqlCommand mySqlCmd = mySqlCnn.CreateCommand();
                mySqlCmd.CommandText = @" 
                    SELECT mr. Role
                    FROM projet_membre_roles mr
                    JOIN projet_membre m ON mr. MembreId = m. Id
                    WHERE m. Apikey = @Apikey";
                mySqlCmd.Parameters.AddWithValue("@Apikey", apikey);

                mySqlDataReader = mySqlCmd.ExecuteReader();
                roles = CreateFromRead(mySqlDataReader);
            }

            finally
            {
                mySqlDataReader?.Close();
                mySqlCnn?.Close();
            }

            return roles;
        }
    }
}
