using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class InternationalLicenseData
    {
        public static bool GetInternationalLicense(int licenseID,ref int ApplicationID , ref int DriverID,ref int LocalLicenseID,ref DateTime IssueDate,ref DateTime ExpirationDate,ref bool IsActive,ref int CreatedByUserID)
        {
            bool Found = false;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select * from InternationalLicenses where InternationalLicenseID = @licenseID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@licenseID", licenseID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    ApplicationID = Convert.ToInt32(reader["ApplicationID"]);
                    DriverID = Convert.ToInt32(reader["DriverID"]);
                    LocalLicenseID = Convert.ToInt32(reader["IssuedUsingLocalLicenseID"]);
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    IsActive = (bool)reader["IsActive"];
                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    Found = true;
                }
            }catch(Exception ex)
            {
                throw new Exception("Error in GetInternationalLicense " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return Found;
        }
        public static int AddInternationalLicense(int applicationID,int driverID,int localLicenseID,DateTime issueDate,DateTime expirationDate,bool isActive,int createdByUserID)
        {
            int newInternationalLicenseID = -1;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string query = @"update InternationalLicenses
                            set IsActive = 0
                            where DriverID = @DriverID;
                            INSERT INTO InternationalLicenses(ApplicationID,DriverID,IssuedUsingLocalLicenseID,IssueDate,ExpirationDate,IsActive,CreatedByUserID)
                            VALUES(@ApplicationID,@DriverID,@LocalLicenseID,@IssueDate,@ExpirationDate,@IsActive,@CreatedByUserID);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", applicationID);
            command.Parameters.AddWithValue("@DriverID", driverID);
            command.Parameters.AddWithValue("@LocalLicenseID", localLicenseID);
            command.Parameters.AddWithValue("@IssueDate", issueDate);
            command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
            command.Parameters.AddWithValue("@IsActive", isActive);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null &&int.TryParse(result.ToString(), out int ID))
                {
                    newInternationalLicenseID = ID;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AddInternationalLicense " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return newInternationalLicenseID;
        }
        public static DataTable GetAllInternationalLicenses()
        {
            DataTable dt = new DataTable();

            SqlConnection connection =new SqlConnection(ConnectionString.connectionString);

            string query = @"SELECT InternationalLicenseID,ApplicationID,DriverID,IssuedUsingLocalLicenseID,IssueDate,ExpirationDate,IsActive FROM InternationalLicenses";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                dt.Load(reader);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAllInternationalLicenses " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }
        public static DataTable GetInternationalLicensePerDriver(int DriverID)
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string query = @"SELECT InternationalLicenseID,ApplicationID,IssuedUsingLocalLicenseID,IssueDate,ExpirationDate,IsActive FROM InternationalLicenses where DriverID = @DriverID ";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if(reader.HasRows)
                    dt.Load(reader);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetInternationalLicensePerDriver " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }
        public static int IsDriverHasActiveInternationalLicense(int DriverID)
        {
            int LicenseID = -1;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "SELECT InternationalLicenseID FROM InternationalLicenses where DriverID = @DriverID and IsActive = 1";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int id))
                    LicenseID = id;
            }catch(Exception ex)
            {
                throw new Exception("Error in IsDriverHasActiveInternationalLicense " + ex.Message);
            }
            finally { connection.Close(); }
            return LicenseID;
        }
    }
}
