using System;
using System.Collections.Generic;
using System.Data;
using DataAccessLayer;
namespace BuisnessLayer
{
    public class clsUser
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int PersonID { get; set; }
        public bool IsActive { get; set; }
        public clsPerson PersonInfo { get; set; }
        private enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;
        private bool _AddNewUser()
        {
            this.UserID = UsersData.AddUser(UserName, Password,PersonID ,IsActive);
            return this.UserID != -1;
        }
        private bool _Update()
        {
            return UsersData.UpdateUser(UserID, PersonID, UserName, Password, IsActive);
        }
        public clsUser()
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = false;
            this.PersonInfo = new clsPerson();
            this.Mode = enMode.AddNew;
        }
        private clsUser(int UserID,int PersonID,string Password,string UserName,bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;
            this.PersonInfo = clsPerson.Find(PersonID);
            this.Mode = enMode.Update;
        }
        public static clsUser Find(int UserID)
        {
            int PersonID = -1;
            string UserName = "", Password = "";
            bool IsActive = false ,IsFound = UsersData.GetUserInfoByID(UserID,ref PersonID,ref UserName,ref Password,ref IsActive);
            if(IsFound)
            {
                return new clsUser(UserID, PersonID, Password, UserName, IsActive);
            }
            return null;
            
        }
        public static clsUser FindByPersonID(int PersonID)
        {
            int UserID = -1;
            string UserName = "", Password = "";
            bool IsActive = false, IsFound = UsersData.GetUserInfoByPersonID(PersonID, ref UserID, ref UserName,ref Password, ref IsActive);
            if (IsFound)
            {
                return new clsUser(UserID, PersonID, Password, UserName, IsActive);
            }
            return null;

        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewUser())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else return false;
                    }

                case enMode.Update:
                    return _Update();
                default:return false;
            }
        }
        public static DataTable GetAllUsers() => UsersData.GetAllUsers();
        public static bool UserExsitByID(int userID) => UsersData.IsUserExist(userID);
        public static bool UserExsitByPersonID(int PersonID) => UsersData.IsUserExistForPersonID(PersonID);
        public static bool UserExsitByUserName(string username) => UsersData.IsUserExist(username);
        public static bool DeleteUser(int UserID) => UsersData.DeleteUser(UserID);
        public static bool ChangePassword(int UserID, string Password) => UsersData.ChangePassword(UserID, Password);
        public static clsUser FindUserByUsername_Password(string username,string password)
        {
            int UserID = -1,PersonID = -1;
            bool IsActive = false,Found = UsersData.GetUserByUserNameOrPassword(password,username,ref UserID,ref PersonID,ref IsActive);
            if (Found)
                return new clsUser(UserID, PersonID, password, username, IsActive);
            return null;

        }

    }
}
