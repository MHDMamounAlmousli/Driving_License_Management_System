using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace DataAccessLayer
{
    public class DetainLicenseData
    {
        public static bool GetDetainLicenseByID(int DetainID,ref int LicenseID,ref DateTime DetainDate,ref int Fees,ref int CreatedByUserID,ref bool IsReleased,ref DateTime? ReleaseDate,ref int? ReleasedByUserID,ref int? ReleaseApplicationID)
        {
            bool found = false;

            SqlConnection connection =new SqlConnection(ConnectionString.connectionString);

            string query = @"SELECT * FROM DetainedLicenses WHERE DetainID = @DetainID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", DetainID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    found = true;

                    LicenseID = Convert.ToInt32(reader["LicenseID"]);
                    DetainDate = Convert.ToDateTime(reader["DetainDate"]);
                    Fees = Convert.ToInt32(reader["FineFees"]);
                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    IsReleased = Convert.ToBoolean(reader["IsReleased"]);

                    ReleaseDate = reader["ReleaseDate"] == DBNull.Value ? (DateTime?)null: Convert.ToDateTime(reader["ReleaseDate"]);

                    ReleasedByUserID = reader["ReleasedByUserID"] == DBNull.Value ? (int?)null: Convert.ToInt32(reader["ReleasedByUserID"]);

                    ReleaseApplicationID = reader["ReleaseApplicationID"] == DBNull.Value? (int?)null: Convert.ToInt32(reader["ReleaseApplicationID"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetDetainLicenseByID " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return found;
        }
        public static bool GetDetainLicenseByLicenseID(int LicenseID, ref int DetainID, ref DateTime DetainDate, ref int Fees, ref int CreatedByUserID, ref bool IsReleased, ref DateTime? ReleaseDate, ref int? ReleasedByUserID, ref int? ReleaseApplicationID)
        {
            bool found = false;

            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string query = @"SELECT * FROM DetainedLicenses WHERE LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    found = true;

                    DetainID = Convert.ToInt32(reader["DetainID"]);
                    DetainDate = Convert.ToDateTime(reader["DetainDate"]);
                    Fees = Convert.ToInt32(reader["FineFees"]);
                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    IsReleased = Convert.ToBoolean(reader["IsReleased"]);

                    ReleaseDate = reader["ReleaseDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["ReleaseDate"]);

                    ReleasedByUserID = reader["ReleasedByUserID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["ReleasedByUserID"]);

                    ReleaseApplicationID = reader["ReleaseApplicationID"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["ReleaseApplicationID"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetDetainLicenseByLicenseID " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return found;
        }
        public static int AddDetainLicense(int licenseID,DateTime detainDate,int fees,int createdByUserID,bool isReleased,DateTime? releaseDate, int? releasedByUserID,int? releaseApplicationID)
        {
            int newDetainID = -1;

            SqlConnection connection =new SqlConnection(ConnectionString.connectionString);

            string query = @"
        INSERT INTO DetainedLicenses(LicenseID,DetainDate,FineFees,CreatedByUserID,IsReleased,ReleaseDate,ReleasedByUserID,ReleaseApplicationID)
        VALUES(@LicenseID,@DetainDate,@Fees,@CreatedByUserID,@IsReleased,@ReleaseDate,@ReleasedByUserID,@ReleaseApplicationID);
        SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", licenseID);
            command.Parameters.AddWithValue("@DetainDate", detainDate);
            command.Parameters.AddWithValue("@Fees", fees);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
            command.Parameters.AddWithValue("@IsReleased", isReleased);

            command.Parameters.AddWithValue( "@ReleaseDate",releaseDate.HasValue? (object)releaseDate.Value: DBNull.Value);

            command.Parameters.AddWithValue("@ReleasedByUserID",releasedByUserID.HasValue? (object)releasedByUserID.Value: DBNull.Value);

            command.Parameters.AddWithValue("@ReleaseApplicationID",releaseApplicationID.HasValue? (object)releaseApplicationID.Value: DBNull.Value);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int detainID))
                {
                    newDetainID = detainID;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AddDetainLicense " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return newDetainID;
        }
        public static bool UpdateDetainLicense(int detainID,int licenseID,DateTime detainDate,int fees,int createdByUserID,bool isReleased,DateTime? releaseDate,int? releasedByUserID,int? releaseApplicationID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string query = @"UPDATE DetainedLicenses
        SET
            LicenseID = @LicenseID,
            DetainDate = @DetainDate,
            FineFees = @Fees,
            CreatedByUserID = @CreatedByUserID,
            IsReleased = @IsReleased,
            ReleaseDate = @ReleaseDate,
            ReleasedByUserID = @ReleasedByUserID,
            ReleaseApplicationID = @ReleaseApplicationID
        WHERE DetainID = @DetainID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", detainID);
            command.Parameters.AddWithValue("@LicenseID", licenseID);
            command.Parameters.AddWithValue("@DetainDate", detainDate);
            command.Parameters.AddWithValue("@Fees", fees);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
            command.Parameters.AddWithValue("@IsReleased", isReleased);

            command.Parameters.AddWithValue( "@ReleaseDate",releaseDate.HasValue? (object)releaseDate.Value: DBNull.Value);

            command.Parameters.AddWithValue("@ReleasedByUserID",releasedByUserID.HasValue? (object)releasedByUserID.Value: DBNull.Value);

            command.Parameters.AddWithValue( "@ReleaseApplicationID",releaseApplicationID.HasValue? (object)releaseApplicationID.Value: DBNull.Value);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdateDetainLicense " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }
        public static bool IsDetain(int licenseID)
        {
            bool isDetained = false;

            SqlConnection connection =new SqlConnection(ConnectionString.connectionString);

            string query = @"SELECT 1 FROM DetainedLicenses WHERE LicenseID = @LicenseID AND IsReleased = 0";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", licenseID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                isDetained = result != null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in IsDetain " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return isDetained;
        }
        public static bool IsRelease(int licenseID)
        {
            bool isReleased = false;

            SqlConnection connection =new SqlConnection(ConnectionString.connectionString);

            string query = @"SELECT 1 FROM DetainedLicenses WHERE LicenseID = @LicenseID AND IsReleased = 1";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", licenseID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                isReleased = result != null;
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Error in IsRelease " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return isReleased;
        }
        public static DataTable GetAllDetainLicense()
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection =new SqlConnection(ConnectionString.connectionString);

            string query = "SELECT * FROM DetainedLicenses_View";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                    dataTable.Load(reader);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAllDetainLicense " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dataTable;
        }
        public static bool ReleaseDetainedLicense(int DetainID,int ReleasedByUserID, int ReleaseApplicationID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string query = @"UPDATE dbo.DetainedLicenses
                              SET IsReleased = 1, 
                              ReleaseDate = @ReleaseDate, 
                              ReleaseApplicationID = @ReleaseApplicationID   
                              WHERE DetainID=@DetainID;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DetainID", DetainID);
            command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
            command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Error in ReleaseDetainedLicense " + ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }
    }
}
