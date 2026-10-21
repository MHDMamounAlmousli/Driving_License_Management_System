using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
namespace DataAccessLayer
{
    public class DataCountries
    {
        public static bool GetCountryNameByID(int CountryID,ref string CountryName)
        {
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select CountryName from Countries where CountryID = @CountryID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryID", CountryID); 
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    CountryName = reader["CountryName"].ToString();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return false;
        }
        public static bool GetCountryIDByName(string CountryName, ref int CountryID)
        {
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select CountryID from Countries where CountryName = @CountryName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", CountryName);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    CountryID = Convert.ToInt32(reader["CountryID"]);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return false;
        }
        public static DataTable GetAllCountries()
        {
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select * from Countries";
            SqlCommand command = new SqlCommand(query, connection);
            DataTable dtCountries = new DataTable();
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    dtCountries.Load(reader);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving person information: from GetAllCountries  " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dtCountries;
        }
    }
}
