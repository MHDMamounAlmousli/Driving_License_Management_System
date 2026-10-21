using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class TestAppointmentData
    {
        public static bool GetAppointmentByID(int appointmentID, ref int testTypeID, ref int ldlID, ref DateTime appointmentdate, ref int fees, ref int createdbyuserid, ref bool islocked, ref int? retaketestapplicationid)
        {
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select * from TestAppointments where TestAppointmentID = @appointmentID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@appointmentID", appointmentID);
            bool found = false;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    testTypeID = Convert.ToInt32(reader["TestTypeID"]);
                    ldlID = Convert.ToInt32(reader["LocalDrivingLicenseApplicationID"]);
                    appointmentdate = Convert.ToDateTime(reader["AppointmentDate"]);
                    fees = Convert.ToInt32(reader["PaidFees"]);
                    createdbyuserid = Convert.ToInt32(reader["CreatedByUserID"]);
                    islocked = Convert.ToBoolean(reader["IsLocked"]);
                    if (reader["RetakeTestApplicationID"] != DBNull.Value)
                        retaketestapplicationid = Convert.ToInt32(reader["RetakeTestApplicationID"]);
                    else
                        retaketestapplicationid = null;
                        found = true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAppointmentByID " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return found;
        }

        public static bool GetAppointmentByLocaladLicenseID( int ldlID, ref int appointmentID, ref int testTypeID, ref DateTime appointmentdate, ref int fees, ref int createdbyuserid, ref bool islocked, ref int? retaketestapplicationid)
        {
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select * from TestAppointments where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", ldlID);
            bool found = false;
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    appointmentID = Convert.ToInt32(reader["TestAppointmentID"]);
                    testTypeID = Convert.ToInt32(reader["TestTypeID"]);
                    appointmentdate = Convert.ToDateTime(reader["AppointmentDate"]);
                    fees = Convert.ToInt32(reader["PaidFees"]);
                    createdbyuserid = Convert.ToInt32(reader["CreatedByUserID"]);
                    islocked = Convert.ToBoolean(reader["IsLocked"]);
                    if (reader["RetakeTestApplicationID"] != DBNull.Value)
                        retaketestapplicationid = Convert.ToInt32(reader["RetakeTestApplicationID"]);
                    else
                        retaketestapplicationid = null;
                    found = true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAppointmentByID " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return found;
        }

        public static int addTestAppointment(int testTypeID, int ldlID, DateTime appointmentdate, int fees, int createdbyuserid, bool islocked, int? retaketestapplicationid)
        {
            int newAppointmentID = -1;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "insert into TestAppointments (TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID) values (@testTypeID, @ldlID, @appointmentdate, @fees, @createdbyuserid, @islocked, @retaketestapplicationid); select SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@testTypeID", testTypeID);
            command.Parameters.AddWithValue("@ldlID", ldlID);
            command.Parameters.AddWithValue("@appointmentdate", appointmentdate);
            command.Parameters.AddWithValue("@fees", fees);
            command.Parameters.AddWithValue("@createdbyuserid", createdbyuserid);
            command.Parameters.AddWithValue("@islocked", islocked);
            if (retaketestapplicationid.HasValue)
                command.Parameters.AddWithValue("@retaketestapplicationid", retaketestapplicationid.Value);
            else
                command.Parameters.AddWithValue("@retaketestapplicationid", DBNull.Value);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int id))
                    newAppointmentID = id;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in addTestAppointment " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return newAppointmentID;
        }
        public static bool UpdateTestAppointment(int appointmentID, int testTypeID, int ldlID, DateTime appointmentdate, int fees, int createdbyuserid, bool islocked, int? retaketestapplicationid)
        {
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "update TestAppointments set TestTypeID = @testTypeID, LocalDrivingLicenseApplicationID = @ldlID, AppointmentDate = @appointmentdate, PaidFees = @fees, CreatedByUserID = @createdbyuserid, IsLocked = @islocked, RetakeTestApplicationID = @retaketestapplicationid where TestAppointmentID = @appointmentID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@appointmentID", appointmentID);
            command.Parameters.AddWithValue("@testTypeID", testTypeID);
            command.Parameters.AddWithValue("@ldlID", ldlID);
            command.Parameters.AddWithValue("@appointmentdate", appointmentdate);
            command.Parameters.AddWithValue("@fees", fees);
            command.Parameters.AddWithValue("@createdbyuserid", createdbyuserid);
            command.Parameters.AddWithValue("@islocked", islocked);
            if (retaketestapplicationid.HasValue)
                command.Parameters.AddWithValue("@retaketestapplicationid", retaketestapplicationid.Value);
            else
                command.Parameters.AddWithValue("@retaketestapplicationid", DBNull.Value);
            bool updated = false;
            try
            {
                connection.Open();
                updated = command.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in UpdateTestAppointment " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return updated;
        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = @"SELECT TestAppointmentID, AppointmentDate,PaidFees, IsLocked
                        FROM TestAppointments
                        WHERE  
                        (TestTypeID = @TestTypeID) 
                        AND (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)
                        order by TestAppointmentID desc;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    dt.Load(reader);
                reader.Close();
            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }
        public static int TotalTrialsPerTest(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            int count = 0;

            SqlConnection connection =new SqlConnection(ConnectionString.connectionString);

            string query = @"
        SELECT COUNT(Tests.TestID)
        FROM Tests
        INNER JOIN TestAppointments
            ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
        INNER JOIN LocalDrivingLicenseApplications
            ON TestAppointments.LocalDrivingLicenseApplicationID =
               LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
        WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID =
              @LocalDrivingLicenseApplicationID
          AND TestAppointments.TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID",LocalDrivingLicenseApplicationID);

            command.Parameters.AddWithValue( "@TestTypeID",TestTypeID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int res))
                    count = res;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in TotalTrialsPerTest " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return count;
        }

    }
}
