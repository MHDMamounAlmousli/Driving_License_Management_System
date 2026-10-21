using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class TestsTypeData
    {
        public static bool GetTestTypeByID(int id,ref string testTitle,ref string testDescription,ref int fees)
        {
            bool found = false;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select * from TestTypes where TestTypeID = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    found = true;
                    testTitle = reader["TestTypeTitle"].ToString();
                    testDescription = reader["TestTypeDescription"].ToString();
                    fees = Convert.ToInt32(reader["TestTypeFees"]);
                }
            }catch(Exception ex)
            {
                throw new Exception("Error in GetTestTypeByID " + ex.Message);
            }
            finally { connection.Close(); }
            return found;
        }
        public static bool Update(int id, string testTitle, string testDescription, int fees)
        {
            int rowAffected = -1;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "update TestTypes set TestTypeTitle= @TestTypeTitle,TestTypeDescription = @TestTypeDescription,TestTypeFees = @TestTypeFees  where TestTypeID = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@TestTypeTitle", testTitle);
            command.Parameters.AddWithValue("@TestTypeDescription", testDescription);
            command.Parameters.AddWithValue("@TestTypeFees", fees);
            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Update " + ex.Message);
            }
            finally { connection.Close(); }
            return rowAffected > 0;
        }
        public static DataTable GetAllTypes()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select * from TestTypes";
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
                throw new Exception("Error retrieving TestTypes: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }
    }
}
