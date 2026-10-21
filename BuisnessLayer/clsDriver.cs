using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer
{
    public class clsDriver
    {
        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public int CreatedByUserID { get; set; }
        public DateTime CreatedDate { get; set; }
        public clsPerson PersonInfo { get; set; }
        public clsUser CreatedByUserInfo { get; set; }
        public clsDriver()
        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedDate = DateTime.Now;
            this.CreatedByUserID = -1;
        }
        public clsDriver(int driverID, int personID, int createdByUserID, DateTime createdDate)
        {
            this.DriverID = driverID;
            this.PersonID = personID;
            this.CreatedByUserID = createdByUserID;
            this.CreatedDate = createdDate;
            this.PersonInfo = clsPerson.Find(personID);
            this.CreatedByUserInfo = clsUser.Find(createdByUserID);
        }
        private bool AddDriver()
        {
            this.DriverID = DriverData.AddDriver(this.PersonID, this.CreatedByUserID, this.CreatedDate);
            return this.DriverID != -1;
        }
        public static clsDriver Find(int driverID)
        {
            int personID = -1;
            int createdByUserID = -1;
            DateTime createdDate = DateTime.Now;
            bool found = DriverData.GetDriverByID(driverID, ref personID, ref createdByUserID, ref createdDate);
            if (found)
                return new clsDriver(driverID, personID, createdByUserID, createdDate);
            return null;
        }
        public static clsDriver FindByPersonID(int PersonID)
        {
            int DriverID = -1;
            int createdByUserID = -1;
            DateTime createdDate = DateTime.Now;
            bool found = DriverData.GetDriverByPersonID(PersonID, ref DriverID, ref createdByUserID, ref createdDate);
            if (found)
                return new clsDriver(DriverID, PersonID, createdByUserID, createdDate);
            return null;
        }
        public static bool IsExist(int driverID) => DriverData.IsDriverExistByID(driverID);
        public static DataTable getAllDrivers() => DriverData.GetAllDrivers();
        public bool Save()
        {
            if (AddDriver())
                return true;
            return false;
        }
        public static bool IsExistByPersonID(int personID) => DriverData.IsDriverExistByPersonID(personID);
        public static DataTable GetLicensePerDriver(int driverID) => DriverData.GetLicense(driverID);
        public static DataTable GetLicensePerDriverByPersonID(int personID) => DriverData.GetLicenseByPersonID(personID);
    }
}
