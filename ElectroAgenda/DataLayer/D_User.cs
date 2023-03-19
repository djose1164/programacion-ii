using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

using EntityLayer;

namespace DataLayer
{
    public class D_User
    {
        public bool register(E_User user)
        {
            // El usuario ya existe en la base de datos?
            var cmd = new SqlCommand("isUsernameAvailable", conn);
            cmd.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 32).Value = user.Username;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            bool isAvailable = false;
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                    isAvailable = reader.GetString(0) == "true";
                reader.Close();
                if (isAvailable)
                {
                     cmd = new SqlCommand(
                        "INSERT INTO users(username, password) VALUES('"+user.Username+"','"+user.Password+"')", conn);
                    cmd.ExecuteNonQuery();
                }
                
            }
            finally
            {
                conn.Close();
            }
            return isAvailable;
        }

        public bool validate(E_User user)
        {
            var cmd = new SqlCommand("verifyUser", conn);
            cmd.Parameters.Add("@username", System.Data.SqlDbType.NVarChar, 32).Value = user.Username;
            cmd.Parameters.Add("@password", System.Data.SqlDbType.NVarChar, 32).Value = user.Password;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            bool isValid = false;
            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                    isValid = reader.GetString(0) == "true";
            }
            finally
            {
                conn.Close();
            }
            return isValid;
        }

        private static string uri = ConfigurationManager.ConnectionStrings["connSql"].ConnectionString;
        private SqlConnection conn = new SqlConnection(uri);
    }
}
