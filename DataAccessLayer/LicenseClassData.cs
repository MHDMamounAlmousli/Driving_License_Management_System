using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class LicenseClassData
    {
        public static DataTable GetAllLicenseClass()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select * from LicenseClasses";
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    dt.Load(reader);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAllLicenseClass " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }
        public static bool GetLicenseClassByID(int id,ref string className,ref string description,ref int allowedAge,ref int validtyLength,ref int fees)
        {
            bool result = false;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select * from LicenseClasses where LicenseClassID = @LicenseClassID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseClassID", id);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    result = true;
                    className = (string)reader["ClassName"];
                    description = (string)reader["ClassDescription"];
                    allowedAge = Convert.ToInt32(reader["MinimumAllowedAge"]);
                    validtyLength = Convert.ToInt32(reader["DefaultValidityLength"]);
                    fees = Convert.ToInt32(reader["ClassFees"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetLicenseClassByID " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return result;
        }
        public static bool GetLicenseClassByClassName(string className,ref int id, ref string description, ref int allowedAge, ref int validtyLength, ref int fees)
        {
            bool result = false;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select * from LicenseClasses where ClassName = @className";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@className", className);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    result = true;
                    id = Convert.ToInt32(reader["LicenseClassID"]);
                    description = (string)reader["ClassDescription"];
                    allowedAge = Convert.ToInt32(reader["MinimumAllowedAge"]);
                    validtyLength = Convert.ToInt32(reader["DefaultValidityLength"]);
                    fees = Convert.ToInt32(reader["ClassFees"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetLicenseClassByClassName " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return result;
        }
    }
}
