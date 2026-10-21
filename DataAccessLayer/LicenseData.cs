using System;
using System.Data.SqlClient;
namespace DataAccessLayer
{
    public class LicenseData
    {
        public static bool GetLicenseID(int LicenseID,ref int ApplicationID,ref int DriverID,ref int LicenseClass,ref DateTime IssueDate,ref DateTime ExpirationDate,ref string Notes,ref int Fees,ref bool isActive,ref int IssueReason,ref int CreatedByUserID)
        {
            bool found = false;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = @"SELECT * FROM Licenses WHERE LicenseID = @LicenseID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    found = true;
                    ApplicationID = Convert.ToInt32(reader["ApplicationID"]);
                    DriverID = Convert.ToInt32(reader["DriverID"]);
                    LicenseClass = Convert.ToInt32(reader["LicenseClass"]);
                    IssueDate = Convert.ToDateTime(reader["IssueDate"]);
                    ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]);
                    Notes = reader["Notes"] == DBNull.Value ? null: reader["Notes"].ToString();
                    Fees = Convert.ToInt32(reader["PaidFees"]);
                    isActive = Convert.ToBoolean(reader["IsActive"]);
                    IssueReason = Convert.ToInt32(reader["IssueReason"]);
                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetLicenseID " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return found;
        }
        public static int AddLicense(int applicationID,int driverID,int licenseClassID,DateTime issueDate,DateTime expirationDate,string notes,int fees,bool isActive,int issueReason,int createdByUserID)
        {
            int newLicenseID = -1;

            SqlConnection connection =new SqlConnection(ConnectionString.connectionString);
            string query = @"INSERT INTO Licenses( ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID)
            VALUES(@ApplicationID,@DriverID,@LicenseClassID,@IssueDate,@ExpirationDate,@Notes,@PaidFees,@IsActive,@IssueReason,@CreatedByUserID);
            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", applicationID);
            command.Parameters.AddWithValue("@DriverID", driverID);
            command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
            command.Parameters.AddWithValue("@IssueDate", issueDate);
            command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
            command.Parameters.AddWithValue("@Notes",string.IsNullOrWhiteSpace(notes)? (object)DBNull.Value: notes);
            command.Parameters.AddWithValue("@PaidFees", fees);
            command.Parameters.AddWithValue("@IsActive", isActive);
            command.Parameters.AddWithValue("@IssueReason", issueReason);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int licenseID))
                {
                    newLicenseID = licenseID;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AddLicense " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return newLicenseID;
        }
        public static bool UpdateLicense(int licenseID,int applicationID,int driverID,int licenseClassID,DateTime issueDate,DateTime expirationDate,string notes,int fees,bool isActive,int issueReason,int createdByUserID)
        {
            int rowsAffected = 0;
            SqlConnection connection =new SqlConnection(ConnectionString.connectionString);

            string query = @"UPDATE Licenses
        SET
            ApplicationID = @ApplicationID,
            DriverID = @DriverID,
            LicenseClass = @LicenseClassID,
            IssueDate = @IssueDate,
            ExpirationDate = @ExpirationDate,
            Notes = @Notes,
            PaidFees = @PaidFees,
            IsActive = @IsActive,
            IssueReason = @IssueReason,
            CreatedByUserID = @CreatedByUserID
        WHERE LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", licenseID);
            command.Parameters.AddWithValue("@ApplicationID", applicationID);
            command.Parameters.AddWithValue("@DriverID", driverID);
            command.Parameters.AddWithValue("@LicenseClassID", licenseClassID);
            command.Parameters.AddWithValue("@IssueDate", issueDate);
            command.Parameters.AddWithValue("@ExpirationDate", expirationDate);

            command.Parameters.AddWithValue("@Notes",string.IsNullOrWhiteSpace(notes)? (object)DBNull.Value: notes);

            command.Parameters.AddWithValue("@PaidFees", fees);
            command.Parameters.AddWithValue("@IsActive", isActive);
            command.Parameters.AddWithValue("@IssueReason", issueReason);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdateLicense " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }
        public static bool DeactivateLicense(int LicenseID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string query = @"UPDATE Licenses
                           SET 
                              IsActive = 0
                             
                         WHERE LicenseID=@LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);


            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Error in DeactivateLicense " + ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

    }
}

