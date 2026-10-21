using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer
{
    public class clsApplicationType
    {
        public enum enApplicationType
        {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7,None = 8
        }
        public enApplicationType ApplicationTypeID{ get; }
        public string ApplicationTitle{ set; get; }
        public int Fees{ set; get; }
        public clsApplicationType()
        {
            this.ApplicationTypeID = enApplicationType.None;
            this.ApplicationTitle = "";
            this.Fees = 0;
        }
        private clsApplicationType(enApplicationType applicationTypeID,string ApplicationTitle,int Fees)
        {
            this.ApplicationTypeID = applicationTypeID;
            this.ApplicationTitle = ApplicationTitle;
            this.Fees = Fees;
        }
        private bool _Update()
        {
            return ApplicationTypesData.Update((int)this.ApplicationTypeID, this.ApplicationTitle, this.Fees);
        }
        public static clsApplicationType Find(enApplicationType applicationTypeID)
        {
            string description = "";
            int fees = 0;
            bool found = ApplicationTypesData.GetApplicationTypeByID((int)applicationTypeID, ref description, ref fees);
            if(found)
                return new clsApplicationType(applicationTypeID,description,fees);
            return null;
        }
        public static DataTable GetAllTypes() => ApplicationTypesData.GetAllTypes();
        public bool Save()
        {
            if (_Update())
                return true;
            return false;
        }
    }
}
