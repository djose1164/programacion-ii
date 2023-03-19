using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

using EntityLayer;

namespace DataLayer
{
    public class D_Record
    {
        public bool insert(E_Record record)
        {
            var cmd = prepareCmd("insertRecord", record);
            bool success = false;
            try
            {
                conn.Open();
                success = cmd.ExecuteNonQuery() != 0;
            }
            finally
            {
                conn.Close();
            }

            return success;
        }

        public bool update(E_Record record)
        {
            var cmd = prepareCmd("updateRecord", record);
            bool success = false;
            try
            {
                conn.Open();
                success = cmd.ExecuteNonQuery() != 0;
            }
            finally
            {
                conn.Close();
            }

            return success;
        }

        public E_Record find(E_Record record)
        {
            var cmd = new SqlCommand("findRecordByEmail", conn);
            cmd.Parameters.Add("@correo_electronico", System.Data.SqlDbType.VarChar, 128).Value = record.Email;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            E_Record fetched = null;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    fetched = new E_Record();
                    fetched.Name = reader["nombre"].ToString();
                    fetched.Surname= reader["apellido"].ToString();
                    fetched.Birthday = reader.GetDateTime(2).ToString("dd/MM/yyyy");
                    fetched.Address = reader["direccion"].ToString();
                    fetched.Genre = reader["genero"].ToString();
                    fetched.CivilState = reader["estado_civil"].ToString();
                    fetched.Phone = reader["movil"].ToString();
                    fetched.Telephone = reader["telefono"].ToString();
                    fetched.Email = reader["correo_electronico"].ToString();
                }
            }
            finally
            {
                conn.Close();
            }

            return fetched;
        }

        public bool delete(E_Record record)
        {
            string query = "DELETE FROM agenda WHERE correo_electronico = '"+record.Email+"'";
            var cmd = new SqlCommand(query, conn);
            bool success = false;
            try
            {
                conn.Open();
                success = cmd.ExecuteNonQuery() != 0;
            }
            finally 
            {
                conn.Close();
            }
            return success;
        }

        private SqlCommand prepareCmd(string command, E_Record record)
        {
            var cmd = new SqlCommand(command, conn);

            cmd.Parameters.Add("@nombre", System.Data.SqlDbType.VarChar).Value = record.Name;
            cmd.Parameters.Add("@apellido", System.Data.SqlDbType.VarChar).Value = record.Surname;
            cmd.Parameters.Add("@direccion", System.Data.SqlDbType.VarChar).Value = record.Address;
            cmd.Parameters.Add("@fecha_nacimiento", System.Data.SqlDbType.Date).Value = record.Birthday;
            cmd.Parameters.Add("@estado_civil", System.Data.SqlDbType.VarChar).Value = record.CivilState;
            cmd.Parameters.Add("@movil", System.Data.SqlDbType.VarChar).Value = record.Phone;
            cmd.Parameters.Add("@telefono", System.Data.SqlDbType.VarChar).Value = record.Telephone;
            cmd.Parameters.Add("@correo_electronico", System.Data.SqlDbType.VarChar).Value = record.Email;
            cmd.Parameters.Add("@genero", System.Data.SqlDbType.VarChar).Value = record.Genre;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            return cmd;
        }

        private static string uri = ConfigurationManager.ConnectionStrings["connSql"].ConnectionString;
        private SqlConnection conn = new SqlConnection(uri);
    }
}
