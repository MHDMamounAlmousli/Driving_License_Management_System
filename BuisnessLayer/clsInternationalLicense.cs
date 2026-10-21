using DataAccessLayer;
using System;
using System.Data;
namespace BuisnessLayer
{
    public class clsInternationalLicense : clsApplication
    {
        private bool addLicense()
        {
            this.InternationalLicenseID = InternationalLicenseData.AddInternationalLicense(this.ApplicationID, this.DriverID, this.LicenseID, this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);
            return this.InternationalLicenseID != -1;
        }
        public int InternationalLicenseID { get; set; }
        public int DriverID { get; set; }
        public int LicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public clsDriver DriverInfo { get; set; }
        public clsLicense LicenseInfo { get; set; }
        public clsUser UserInfo { get; set; }
        public clsInternationalLicense()
        {
            this.InternationalLicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.IsActive = false;
            this.CreatedByUserID = -1;
        }
        public clsInternationalLicense(int internationalLicenseID, int applicationID, int applicantPersonID,DateTime applicationDate, enStatus applicationStatus, DateTime lastStatusDate,int fees, int createdByUserID, int driverID, int licenseID,DateTime issueDate, DateTime expirationDate, bool isActive)
            : base(applicationID, applicantPersonID, applicationDate,clsApplicationType.enApplicationType.NewInternationalLicense,applicationStatus, lastStatusDate, fees, createdByUserID)
        {
            this.InternationalLicenseID = internationalLicenseID;
            this.ApplicationID = applicationID;
            this.DriverID = driverID;
            this.LicenseID = licenseID;
            this.IssueDate = issueDate;
            this.ExpirationDate = expirationDate;
            this.IsActive = isActive;
            this.CreatedByUserID = createdByUserID;

            this.DriverInfo = clsDriver.Find(this.DriverID);
            this.LicenseInfo = clsLicense.Find(this.LicenseID);
            this.UserInfo = clsUser.Find(this.CreatedByUserID);
        }
        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            int applicationID = -1, driverID = -1, licenseID = -1, createdByUserID = -1;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            bool isActive = false;
            bool found = InternationalLicenseData.GetInternationalLicense(InternationalLicenseID,ref applicationID,ref driverID,ref licenseID,ref issueDate,ref expirationDate,ref isActive,ref createdByUserID);
            if (found)
            {
                clsApplication application = clsApplication.Find(applicationID);
                if (application != null)
                {
                    return new clsInternationalLicense( InternationalLicenseID,application.ApplicationID,application.ApplicantPersonID,application.ApplicationDate,application.ApplicationStatus,application.LastStatusDate,application.Fees,createdByUserID,driverID,licenseID,issueDate,expirationDate,isActive);
                }
            }

            return null;
        }
        public bool Save()
        {
            if (!base.Save())
                return false;
            if (addLicense())
                return true;
            return false;
        }
        public static DataTable GetAllLicenses() => InternationalLicenseData.GetAllInternationalLicenses();
        public static DataTable GetAllLicensesPerDriver(int DriverID) => InternationalLicenseData.GetInternationalLicensePerDriver(DriverID);
        public static int HasActiveInternationalLicense(int DriverID) => InternationalLicenseData.IsDriverHasActiveInternationalLicense(DriverID);
    }
}
