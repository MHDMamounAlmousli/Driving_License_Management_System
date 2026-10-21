using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class TestData
    {
        public static bool GetTestByID(int testID, ref int testappointmentID, ref byte testResult, ref string notes, ref int createdByUserID)
        {
            bool found = false;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "SELECT TestAppointmentID, TestResult, Notes, CreatedByUserID FROM Tests WHERE TestID = @TestID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", testID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    found = true;
                    testappointmentID = (int)reader["TestAppointmentID"];
                    testResult = Convert.ToByte(reader["TestResult"]);
                    notes = reader["Notes"] == DBNull.Value ? null : reader["Notes"].ToString();
                    createdByUserID = (int)reader["CreatedByUserID"];
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error in GetTestByID " + e.Message);
            }
            finally { connection.Close(); }
            return found;
        }
        public static bool GetLastTestPerTestType(int localDrivingLicenseApplicationID,byte testTypeID,ref int testID,ref int testAppointmentID,ref byte testResult,ref string notes,ref int createdByUserID)
        {
            bool Found = false;

            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string query = @"
        SELECT TOP 1
            Tests.TestID,
            Tests.TestAppointmentID,
            Tests.TestResult,
            Tests.Notes,
            Tests.CreatedByUserID
        FROM LocalDrivingLicenseApplications
        INNER JOIN TestAppointments
            ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID =
               TestAppointments.LocalDrivingLicenseApplicationID
        INNER JOIN Tests
            ON TestAppointments.TestAppointmentID =
               Tests.TestAppointmentID
        WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID =
              @LocalDrivingLicenseApplicationID
          AND TestAppointments.TestTypeID = @TestTypeID
        ORDER BY Tests.TestID DESC;";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue(
                "@LocalDrivingLicenseApplicationID",
                localDrivingLicenseApplicationID);

            command.Parameters.AddWithValue(
                "@TestTypeID",
                testTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Found = true;

                    testID = Convert.ToInt32(reader["TestID"]);
                    testAppointmentID = Convert.ToInt32(reader["TestAppointmentID"]);
                    testResult = Convert.ToByte(reader["TestResult"]);
                    notes = reader["Notes"] == DBNull.Value ? "" : reader["Notes"].ToString();
                    createdByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetLastTestPerTestType " +ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return Found;
        }
        public static bool GetTestByAppointmentID(int testappointmentID, ref int testID, ref byte testResult, ref string notes, ref int createdByUserID)
        {
            bool found = false;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "SELECT TestID, TestResult, Notes, CreatedByUserID FROM Tests WHERE TestAppointmentID = @TestAppointmentID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", testappointmentID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    found = true;
                    testID = (int)reader["TestID"];
                    testResult = (byte)reader["TestResult"];
                    notes = (string)reader["Notes"];
                    createdByUserID = (int)reader["CreatedByUserID"];
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error in GetTestByAppointmentID " + e.Message);
            }
            finally { connection.Close(); }
            return found;
        }
        public static int AddTest( int testappointmentID, byte testResult, string notes, int createdByUserID)
        {
            int newTestID = -1;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "INSERT INTO Tests ( TestAppointmentID, TestResult, Notes, CreatedByUserID) VALUES ( @TestAppointmentID, @TestResult, @Notes, @CreatedByUserID)select Scope_identity()";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", testappointmentID);
            command.Parameters.AddWithValue("@TestResult", testResult);
            command.Parameters.AddWithValue("@Notes", string.IsNullOrWhiteSpace(notes)? (object)DBNull.Value: notes);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int testid))
                    newTestID = testid;
            }
            catch (Exception e)
            {
                throw new Exception("Error in AddTest " + e.Message);
            }
            finally { connection.Close(); }
            return newTestID;
        }
        public static bool UpdateTest(int testID, int testappointmentID, byte testResult, string notes, int createdByUserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "UPDATE Tests SET TestAppointmentID = @TestAppointmentID, TestResult = @TestResult, Notes = @Notes, CreatedByUserID = @CreatedByUserID WHERE TestID = @TestID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", testID);
            command.Parameters.AddWithValue("@TestAppointmentID", testappointmentID);
            command.Parameters.AddWithValue("@TestResult", testResult);
            command.Parameters.AddWithValue("@Notes", notes ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Error in UpdateTest " + e.Message);
            }
            finally { connection.Close(); }
            return rowsAffected > 0;
        }
        public static bool DeleteTest(int testID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "DELETE FROM Tests WHERE TestID = @TestID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", testID);
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Error in DeleteTest " + e.Message);
            }
            finally { connection.Close(); }
            return rowsAffected > 0;
        }
        public static bool TestExists(int testID)
        {
            bool exists = false;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "SELECT 1 FROM Tests WHERE TestID = @TestID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", testID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                exists = reader.HasRows;
            }
            catch (Exception e)
            {
                throw new Exception("Error in TestExists " + e.Message);
            }
            finally { connection.Close(); }
            return exists;
        }
        public static bool TestExistsByAppointmentID(int testappointmentID)
        {
            bool exists = false;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "SELECT 1 FROM Tests WHERE TestAppointmentID = @TestAppointmentID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", testappointmentID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                exists = reader.HasRows;
            }
            catch (Exception e)
            {
                throw new Exception("Error in TestExistsByAppointmentID " + e.Message);
            }
            finally { connection.Close(); }
            return exists;
        }
        public static int GetTestID(int testappointmentID)
        {
            int testID = -1;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "SELECT TOP 1 TestID FROM Tests WHERE TestAppointmentID = @TestAppointmentID ORDER BY TestID DESC;";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestAppointmentID", testappointmentID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    testID = id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetTestID: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return testID;
        }

    }
}
