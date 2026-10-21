using System;
using System.Data.SqlClient;
using System.Data;
namespace DataAccessLayer
{
    public class ApplicationTypesData
    {
        public static bool GetApplicationTypeByID(int id, ref string description, ref int fees)
        {
            bool found = false;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select * from ApplicationTypes  where ApplicationTypeID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    description = reader["ApplicationTypeTitle"].ToString();
                    fees = Convert.ToInt16(reader["ApplicationFees"]);
                    found = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetApplicationTypeByID " + ex.Message);
            }
            finally { connection.Close(); }
            return found;
        }
        public static bool Update(int id, string description, int fees)
        {
            int rowsAffected = -1;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "update ApplicationTypes set ApplicationTypeTitle = @ApplicationTypeTitle, ApplicationFees = @ApplicationFees  where ApplicationTypeID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);
            command.Parameters.AddWithValue("@ApplicationTypeTitle", description);
            command.Parameters.AddWithValue("@ApplicationFees", fees);
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Update Method "+ex.Message);
            }
            finally { connection.Close(); }
            return rowsAffected > 0;
        }
        public static DataTable GetAllTypes()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select * from ApplicationTypes";
            SqlCommand command = new SqlCommand(query,connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    dt.Load(reader);
            }catch(Exception ex)
            {
                throw new Exception("Error retrieving ApplicationTypes: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }
    }
}
