using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DriverData
    {
        public static bool GetDriverByID(int driverID,ref int PersonID,ref int CreatedByUserID,ref DateTime CreatedDate)
        {
            bool found = false;

            SqlConnection connection =new SqlConnection(ConnectionString.connectionString);

            string query = @"SELECT PersonID,CreatedByUserID,CreatedDate FROM Drivers WHERE DriverID = @DriverID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", driverID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    found = true;

                    PersonID = Convert.ToInt32(reader["PersonID"]);
                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetDriverByID " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return found;
        }
        public static bool GetDriverByPersonID(int PersonID, ref int driverID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool found = false;

            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string query = @"SELECT DriverID,CreatedByUserID,CreatedDate FROM Drivers WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    found = true;

                    driverID = Convert.ToInt32(reader["DriverID"]);
                    CreatedByUserID = Convert.ToInt32(reader["CreatedByUserID"]);
                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetDriverByPersonID " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return found;
        }
        public static int AddDriver(int personID,int createdByUserID,DateTime createdDate)
        {
            int newDriverID = -1;
            SqlConnection connection =new SqlConnection(ConnectionString.connectionString);

            string query = @"INSERT INTO Drivers(PersonID,CreatedByUserID,CreatedDate)
                             VALUES(@PersonID,@CreatedByUserID,@CreatedDate);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", personID);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserID);
            command.Parameters.AddWithValue("@CreatedDate", createdDate);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int driverID))
                {
                    newDriverID = driverID;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in AddDriver " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return newDriverID;
        }

        public static bool IsDriverExistByID(int driverID)
        {
            bool isFound = false;

            SqlConnection connection =
                new SqlConnection(ConnectionString.connectionString);

            string query = @"SELECT 1 FROM Drivers WHERE DriverID = @DriverID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", driverID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                isFound = result != null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in IsDriverExistByID " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        public static bool IsDriverExistByPersonID(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection =
                new SqlConnection(ConnectionString.connectionString);

            string query = @"SELECT 1 FROM Drivers WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                isFound = result != null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in IsDriverExistByPersonID " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static DataTable GetAllDrivers()
        {
            DataTable dataTable = new DataTable();

            SqlConnection connection =new SqlConnection(ConnectionString.connectionString);

            string query = "SELECT * FROM Drivers_View";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                dataTable.Load(reader);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetAllDrivers " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dataTable;
        }
        public static DataTable GetLicense(int DriverID)
        {
            DataTable dataTable = new DataTable();
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = @"SELECT Licenses.LicenseID, Licenses.ApplicationID, LicenseClasses.ClassName, Licenses.IssueDate, Licenses.ExpirationDate, Licenses.IsActive
FROM     Drivers INNER JOIN
                  Licenses ON Drivers.DriverID = Licenses.DriverID INNER JOIN
                  LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                            where Drivers.DriverID = @DriverID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    dataTable.Load(reader);
            }catch(Exception ex)
            {
                throw new Exception("Error in GetLicense " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dataTable;
        }

        public static DataTable GetLicenseByPersonID(int PersonID)
        {
            DataTable dataTable = new DataTable();
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = @"SELECT Licenses.LicenseID, Licenses.ApplicationID, LicenseClasses.ClassName, Licenses.IssueDate, Licenses.ExpirationDate, Licenses.IsActive
                  FROM     Drivers INNER JOIN
                  Licenses ON Drivers.DriverID = Licenses.DriverID INNER JOIN
                  LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                            where Drivers.PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    dataTable.Load(reader);
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetLicenseByPersonID " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dataTable;
        }
    }

}
