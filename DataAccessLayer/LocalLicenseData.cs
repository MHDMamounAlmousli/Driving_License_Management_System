using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccessLayer
{
    public class LocalLicenseData
    {
        public static bool GetLocalDrivingLicenseByID(int id, ref int ApplicationID, ref int LicenseClassID)
        {
            bool found = false;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select * from LocalDrivingLicenseApplications where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", id);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    found = true;
                    ApplicationID = (int)reader["ApplicationID"];
                    LicenseClassID = (int)reader["LicenseClassID"];
                }
            } catch (Exception ex)
            {
                throw new Exception("Error in GetLocalDrivingLicenseByID " + ex.Message);
            }
            finally { connection.Close(); }
            return found;
        }
        public static bool GetLocalDrivingLicenseByApplicationID(int id, ref int LDLID, ref int LicenseClassID)
        {
            bool found = false;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select * from LocalDrivingLicenseApplications where ApplicationID = @ApplicationID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", id);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    found = true;
                    LDLID = (int)reader["LocalDrivingLicenseApplicationID"];
                    LicenseClassID = (int)reader["LicenseClassID"];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetLocalDrivingLicenseByApplicationID " + ex.Message);
            }
            finally { connection.Close(); }
            return found;
        }
        public static bool GetLocalDrivingLicenseByLicenseClassID(int id, ref int LDLID, ref int ApplicationID)
        {
            bool found = false;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select * from LocalDrivingLicenseApplications where LicenseClassID = @LicenseClassID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseClassID", id);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    found = true;
                    LDLID = (int)reader["LocalDrivingLicenseApplicationID"];
                    ApplicationID = (int)reader["ApplicationID"];
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetLocalDrivingLicenseByLicenseClassID " + ex.Message);
            }
            finally { connection.Close(); }
            return found;
        }
        public static int AddNewLocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            int LocalDrivingLicenseApplicationID = -1;

            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string query = @"INSERT INTO LocalDrivingLicenseApplications ( 
                            ApplicationID,LicenseClassID)
                             VALUES (@ApplicationID,@LicenseClassID);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LocalDrivingLicenseApplicationID = insertedID;
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Error in AddNewLocalDrivingLicenseApplication " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return LocalDrivingLicenseApplicationID;
        }
        public static bool UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string query = @"Update  LocalDrivingLicenseApplications  
                            set ApplicationID = @ApplicationID,
                                LicenseClassID = @LicenseClassID
                            where LocalDrivingLicenseApplicationID=@LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);


            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdateLocalDrivingLicenseApplication " + ex.Message);
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllLocalDrivingLicenseInfo()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = @"SELECT * FROM LocalDrivingLicenseApplications_View";
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAllLocalDrivingLicenseInfo " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }
        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string query = @"Delete LocalDrivingLicenseApplications 
                                where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Error in DeleteLocalDrivingLicenseApplication " + ex.Message);
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

        }

        public static bool IsPassedTestType(int localDrivingLicenseID, int testTypeID)
        {
            bool found = false;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
                string query = @"
            SELECT 1
            FROM LocalDrivingLicenseApplications
            INNER JOIN TestAppointments
                ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID =
                   TestAppointments.LocalDrivingLicenseApplicationID
            INNER JOIN Tests
                ON TestAppointments.TestAppointmentID =
                   Tests.TestAppointmentID
            WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
              AND TestAppointments.TestTypeID = @TestTypeID
              AND Tests.TestResult = 1;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID",localDrivingLicenseID);
            command.Parameters.AddWithValue("@TestTypeID", testTypeID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                found = reader.HasRows;
            }catch(Exception ex)
            {
                throw new Exception("Error in IsPassedTestType " + ex.Message);
            }
            finally { connection.Close(); }
            return found;
        }
        public static int GetAllPassedTest(int localDrivingLicenseID)
        {
            int count = 0;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = @"SELECT COUNT(*)
                     FROM LocalDrivingLicenseApplications
                     INNER JOIN TestAppointments
                         ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID =
                            TestAppointments.LocalDrivingLicenseApplicationID
                     INNER JOIN Tests
                         ON TestAppointments.TestAppointmentID =
                            Tests.TestAppointmentID
                     WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID =
                            @LocalDrivingLicenseID
                       AND Tests.TestResult = 1;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseID", localDrivingLicenseID);
            try
            {
                connection.Open();

                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int res))
                    count = res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAllPassedTest " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return count;
        }
        public static bool IsLicenseHasActiveAppointment(int localDrivingLicenseID,int testTypeID)
        {
            bool found = false;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "SELECT 1 FROM  LocalDrivingLicenseApplications INNER JOIN  TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID where TestAppointments.TestTypeID = @TestTypeID and TestAppointments.IsLocked = 0 and LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue(@"TestTypeID", testTypeID);
            command.Parameters.AddWithValue(@"LocalDrivingLicenseApplicationID", localDrivingLicenseID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                found = reader.HasRows;
            }catch(Exception ex) { throw new Exception("Error in IsLicenseHasActiveAppointment " + ex.Message); }
            finally { connection.Close(); }
            return found;
        }
        public static int GetActiveLicenseID(int localLicenseID)
        {
            int licenseID = -1;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = @"SELECT TOP 1 L.LicenseID
                                FROM Licenses L
                                INNER JOIN Applications A
                                    ON L.ApplicationID = A.ApplicationID
                                INNER JOIN LocalDrivingLicenseApplications LDL
                                    ON A.ApplicationID = LDL.ApplicationID
                                WHERE LDL.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                                  AND L.IsActive = 1;";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localLicenseID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int ID))
                    licenseID = ID;
            }catch(Exception ex)
            {
                Console.WriteLine("Error in GetActiveLicenseID " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return licenseID;
        }

    }
}