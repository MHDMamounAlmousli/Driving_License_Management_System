using DataAccessLayer;
using System;
using System.Data;
using System.Data.SqlClient;
namespace BuisnessLayer
{
    public class clsApplication
    {
        protected enum enMode { addnew , update}
        protected enMode Mode = enMode.addnew;
        public enum enStatus { Cancel = 2 , New = 1,Completed = 3 }
      
        private bool _AddNewApplication()
        {
            this.ApplicationID = ApplicationData.AddNewApplication(
                this.ApplicantPersonID, this.ApplicationDate,(int) this.ApplicationTypeID,
                (byte)this.ApplicationStatus, this.LastStatusDate, this.Fees, this.CreatedByUserID);

            return (this.ApplicationID != -1);
        }

        private bool _UpdateApplication()
        {

            return ApplicationData.UpdateApplication(
                this.ApplicationID, this.ApplicantPersonID, this.ApplicationDate,
               (int) this.ApplicationTypeID, (byte)this.ApplicationStatus,
                this.LastStatusDate, this.Fees, this.CreatedByUserID);
        }

        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }
        public DateTime ApplicationDate { get; set; }
        public clsApplicationType.enApplicationType ApplicationTypeID { get; set; }
        public enStatus ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public int Fees { get; set; }
        public int CreatedByUserID {  get; set; }
        public clsPerson PersonInfo { get; set; }
        public clsApplicationType ApplicationTypeInfo { get; set; }
        public clsUser CreatedByUserInfo { get; set; }
        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = clsApplicationType.enApplicationType.None;
            this.ApplicationStatus = enStatus.New;
            this.LastStatusDate = DateTime.Now;
            this.Fees = 0;
            this.CreatedByUserID =-1;
            this.Mode = enMode.addnew;
        }
        public clsApplication(int applicationID, int applicantPersonID, DateTime applicationDate, clsApplicationType.enApplicationType applicationTypeID, enStatus applicationStatus, DateTime lastStatusDate, int fees, int createdByUserID)
        {
            ApplicationID = applicationID;
            ApplicantPersonID = applicantPersonID;
            ApplicationDate = applicationDate;
            ApplicationTypeID = applicationTypeID;
            ApplicationStatus = applicationStatus;
            LastStatusDate = lastStatusDate;
            Fees = fees;
            CreatedByUserID = createdByUserID;
            ApplicationTypeInfo = clsApplicationType.Find(ApplicationTypeID);
            PersonInfo = clsPerson.Find(ApplicantPersonID);
            CreatedByUserInfo = clsUser.Find(CreatedByUserID);
            Mode = enMode.update;
        }
        public static clsApplication Find(int applicationID)
        {
            int applicationTypeID = -1, applicationPersonID = -1, fees = 0, CreatedByUserID = -1;byte ApplicationStatus = 0;
            DateTime ApplicationDate = DateTime.Now, LastStatusDate = DateTime.Now;
            bool found = ApplicationData.GetApplicationByID(applicationID, ref applicationPersonID,ref ApplicationDate ,ref applicationTypeID, ref ApplicationStatus, ref LastStatusDate, ref fees, ref CreatedByUserID);
            if (found)
                return new clsApplication(applicationID, applicationPersonID, ApplicationDate, (clsApplicationType.enApplicationType)applicationTypeID, (enStatus)ApplicationStatus, LastStatusDate, fees, CreatedByUserID);
            return null;
        }
        public static bool IsApplicationExist(int applicationID) => ApplicationData.IsApplicationExist(applicationID);
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.addnew:
                    if (_AddNewApplication())
                    {
                        Mode = enMode.update;
                        return true;
                    }
                    return false;

                case enMode.update:
                    return _UpdateApplication();
            }
            return false;
        }
        public bool UpdateStatus(enStatus status)
        {
            this.ApplicationStatus = status;
            return ApplicationData.UpdateStatus(this.ApplicationID, (byte)status);
        }
        public bool Delete(int applicationID) => ApplicationData.DeleteApplication(applicationID);
        public static bool PersonHasActiveApplicationLicenseClass(int PersonID, int ApplicationTypeID, int LicenseClassID) => ApplicationData.DoesPersonHaveActiveApplicationForLicenseClass(PersonID, ApplicationTypeID, LicenseClassID);
        public static int  GetActiveApplicationID(int personID, int applicationTypeID, int licenseClassID) => ApplicationData.GetActiveApplicationIDForLicenseClass(personID, applicationTypeID, licenseClassID);
    }
}
