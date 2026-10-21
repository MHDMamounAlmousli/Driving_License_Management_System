using System;
using System.Data.SqlClient;
using System.Data;
namespace DataAccessLayer
{
    public class UsersData
    {
        public static bool GetUserInfoByID(int ID, ref int personID, ref string userName,ref string Password, ref bool isActive)
        {
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select UserID,PersonID,UserName,IsActive,Password from Users where UserID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", ID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    personID = Convert.ToInt32(reader["PersonID"]);
                    userName = reader["UserName"].ToString();
                    Password = reader["Password"].ToString();
                    isActive = Convert.ToBoolean(reader["IsActive"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user info: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return true;
        }
        public static bool GetUserInfoByPersonID(int ID, ref int UserID, ref string userName,ref string Password, ref bool isActive)
        {
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select UserID,PersonID,UserName,IsActive,Password from Users where PersonID = @ID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", ID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    UserID = Convert.ToInt32(reader["UserID"]);
                    userName = reader["UserName"].ToString();
                    Password = reader["Password"].ToString();
                    isActive = Convert.ToBoolean(reader["IsActive"]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving user info: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return true;
        }
        public static bool GetUserByUserNameOrPassword(string password,string username,ref int UserID,ref int PersonID,ref bool IsActive)
        {
            bool found = false;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "select * from Users where password = @password and Username = @username; ";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@username", username);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
               if(reader.Read())
               {
                    found = true;
                    UserID = (int)reader["UserID"];
                    PersonID = (int)reader["PersonID"];
                    IsActive = (bool)reader["IsActive"];

               }
            }
            catch(Exception ex)
            {
                throw new Exception("Wrong in GetUserByUserNameOrPassword method " + ex.Message);
            }finally
            {
                connection.Close();
            }
            return found;
        }
        public static int AddUser(string userName, string password, int personID, bool isActive)
        {
            int newUserID = -1;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "insert into Users (UserName,Password,PersonID,IsActive) values (@UserName,@Password,@PersonID,@IsActive)" +
                "select Scope_identity()";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", userName);
            command.Parameters.AddWithValue("@Password", password);
            command.Parameters.AddWithValue("@PersonID", personID);
            command.Parameters.AddWithValue("@IsActive", isActive);
            try
            {
                connection.Open();
                object rowsAffected = command.ExecuteScalar();
                if (rowsAffected != null && int.TryParse(rowsAffected.ToString(), out int UserID))
                    newUserID = UserID;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding user: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return newUserID;
        }
        public static bool UpdateUser(int UserID, int PersonID, string UserName,
      string Password, bool IsActive)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string query = @"Update  Users  
                            set PersonID = @PersonID,
                                UserName = @UserName,
                                Password = @Password,
                                IsActive = @IsActive
                                where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@UserID", UserID);


            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }
        public static bool DeleteUser(int UserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = @"Delete from Users where UserID = @UserID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }
            finally
            {
                connection.Close();
            }
            return (rowsAffected > 0);
        }
        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = @"SELECT  Users.UserID, Users.PersonID,
                            FullName = People.FirstName + ' ' + People.SecondName + ' ' + ISNULL( People.ThirdName,'') +' ' + People.LastName,
                             Users.UserName, Users.IsActive
                             FROM  Users INNER JOIN
                                    People ON Users.PersonID = People.PersonID";
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
                throw new Exception("Error retrieving users: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return dt;
        }
        public static bool ChangePassword(int userID,string NewPassword)
        {
            int rowsAffected = -1;
            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);
            string query = "Update Users set Password = @NewPassword where UserID = @userID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@userID", userID);
            command.Parameters.AddWithValue("@NewPassword", NewPassword);
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Did Not Change Password " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return rowsAffected > 0;
        }

        public static bool IsUserExist(int UserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string query = "SELECT Found=1 FROM Users WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool IsUserExist(string UserName)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string query = "SELECT Found=1 FROM Users WHERE UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool IsUserExistForPersonID(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(ConnectionString.connectionString);

            string query = "SELECT Found=1 FROM Users WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }
}
