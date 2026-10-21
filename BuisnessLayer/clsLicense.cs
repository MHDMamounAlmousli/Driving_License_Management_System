using DataAccessLayer;
using System;
namespace BuisnessLayer
{
    public class clsLicense
    {
        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };
        public enum enMode { add = 1, update = 2 }
        public enMode mode = enMode.add;
        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public int LiceseClass { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public int Fees { get; set; }
        public bool IsActive { get; set; }
        public enIssueReason IssueReason { get; set; }
        public int CreatedByUserID { get; set; }
        public clsLicenseClass LicenseClassInfo { get; set; }
        public clsUser CreatedByUserInfo { get; set; }
        public clsDriver DriverInfo { get; set; }
        public clsApplication ApplicationInfo { get; set; }
        public clsDetainLicense DetainedInfo { set; get; }
        public bool isDetained
        {
            get
            {
               return clsDetainLicense.isDetain(this.LicenseID);
            }
        }
        public bool isExpired()
        {
            return (this.ExpirationDate < DateTime.Now);
        }

        private bool AddLicense()
        {
            this.LicenseID = LicenseData.AddLicense(ApplicationID, DriverID, LiceseClass, IssueDate, ExpirationDate, Notes, Fees, IsActive, (int)IssueReason, CreatedByUserID);
            return this.LicenseID != -1;
        }
        private bool UpdateLicense()
        {
            return LicenseData.UpdateLicense(this.LicenseID, this.ApplicationID, this.DriverID, this.LiceseClass, this.IssueDate, this.ExpirationDate, this.Notes, this.Fees, this.IsActive, (int)this.IssueReason, this.CreatedByUserID);
        }
        public clsLicense(int licenseID, int applicationID, int driverID, int liceseClass, DateTime issueDate, DateTime expirationDate, string notes, int fees, bool isActive, enIssueReason issueReason, int createdByUserID)
        {
            LicenseID = licenseID;
            ApplicationID = applicationID;
            DriverID = driverID;
            LiceseClass = liceseClass;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            Notes = notes;
            Fees = fees;
            IsActive = isActive;
            IssueReason = issueReason;
            CreatedByUserID = createdByUserID;
            LicenseClassInfo = clsLicenseClass.Find(liceseClass);
            CreatedByUserInfo = clsUser.Find(CreatedByUserID);
            ApplicationInfo = clsApplication.Find(ApplicationID);
            DriverInfo = clsDriver.Find(DriverID);
            DetainedInfo = clsDetainLicense.FindByLicenseID(licenseID);
            mode = enMode.update;
        }

        public clsLicense()
        {
            LicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            LiceseClass = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            Notes = "";
            Fees = 0;
            IsActive = false;
            IssueReason = enIssueReason.FirstTime;
            CreatedByUserID = -1;
        }
        public static clsLicense Find(int LicenseID)
        {
            int ApplicationID = -1; int DriverID = -1; int LicenseClass = -1;
            DateTime IssueDate = DateTime.Now; DateTime ExpirationDate = DateTime.Now;
            string Notes = "";
            int PaidFees = 0; bool IsActive = true; int CreatedByUserID = 1;
            int IssueReason = 1;
            if (LicenseData.GetLicenseID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass, ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))

                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, (enIssueReason)IssueReason, CreatedByUserID);
            else
                return null;

        }
        public bool Save()
        {
            switch (mode)
            {
                case enMode.add:
                if (AddLicense())
                {
                    mode = enMode.update;
                    return true;
                }
                    return false;
                case enMode.update:
                    return UpdateLicense();
                default:
                    return false;
            }
        }
        public int Detain(int fees,int createdByUserID)
        {
            clsDetainLicense detain = new clsDetainLicense();
            detain.LicenseID = this.LicenseID;
            detain.DetainDate = DateTime.Now;
            detain.Fees = fees;
            detain.CreatedByUserID = createdByUserID;
            detain.IsReleased = false;
            detain.ReleaseDate = null;
            detain.ReleasedByUserID = null;
            detain.ReleaseApplicationID = null;
            if (!detain.Save())
                return -1;
            return detain.DetainID;
        }
        public bool Release(int releasedByUserID, ref int applicationID)
        {
            clsApplication application = new clsApplication();
            application.ApplicantPersonID = this.DriverInfo.PersonID;
            application.ApplicationDate = DateTime.Now;
            application.ApplicationTypeID = clsApplicationType.enApplicationType.ReleaseDetainedDrivingLicsense;
            application.ApplicationStatus = clsApplication.enStatus.Completed;
            application.LastStatusDate = DateTime.Now;
            application.Fees = clsApplicationType.Find(clsApplicationType.enApplicationType.ReleaseDetainedDrivingLicsense).Fees;
            application.CreatedByUserID = releasedByUserID;
            if (!application.Save()) return false;
            applicationID = application.ApplicationID;

            return this.DetainedInfo.ReleasLicense(releasedByUserID, application.ApplicationID);
        }
        public bool DeactivateCurrentLicense() => LicenseData.DeactivateLicense(this.LicenseID);
        public clsLicense RenewLicense(string notes,int createdByUserID)
        {
            clsApplication application = new clsApplication();
            application.ApplicantPersonID = this.DriverInfo.PersonID;
            application.ApplicationDate = DateTime.Now;
            application.ApplicationTypeID = clsApplicationType.enApplicationType.RenewDrivingLicense;
            application.ApplicationStatus = clsApplication.enStatus.Completed;
            application.LastStatusDate = DateTime.Now;
            application.Fees = clsApplicationType.Find(clsApplicationType.enApplicationType.RenewDrivingLicense).Fees;
            application.CreatedByUserID = createdByUserID;
            if (!application.Save())
                return null;
            clsLicense License = new clsLicense();
            License.ApplicationID = application.ApplicationID;
            License.DriverID = this.DriverID;
            License.LiceseClass = this.LiceseClass;
            License.IssueDate = DateTime.Now;
            int DefultValidityLength = this.LicenseClassInfo.ValidityLength;
            License.ExpirationDate = DateTime.Now.AddYears(DefultValidityLength);
            License.Notes = notes;
            License.Fees = this.LicenseClassInfo.Fess;
            License.IsActive = true;
            License.IssueReason = enIssueReason.Renew;
            License.CreatedByUserID = createdByUserID;
            if (!License.Save())
                return null;
            DeactivateCurrentLicense();
            return License;
        }
        public clsLicense Replace(enIssueReason IssueReason,int createdByUserID)
        {
            clsApplication application = new clsApplication();
            application.ApplicantPersonID = this.DriverInfo.PersonID;
            application.ApplicationDate = DateTime.Now;
            application.ApplicationTypeID = (IssueReason == enIssueReason.DamagedReplacement) ? clsApplicationType.enApplicationType.ReplaceDamagedDrivingLicense :clsApplicationType.enApplicationType.ReplaceLostDrivingLicense;
            application.ApplicationStatus = clsApplication.enStatus.Completed;
            application.LastStatusDate = DateTime.Now;
            application.Fees = clsApplicationType.Find(application.ApplicationTypeID).Fees;
            application.CreatedByUserID = createdByUserID;
            if (!application.Save())
            {
                return null;
            }
            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LiceseClass = this.LiceseClass;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = this.ExpirationDate;
            NewLicense.Notes = this.Notes;
            NewLicense.Fees = 0;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = IssueReason;
            NewLicense.CreatedByUserID = CreatedByUserID;
            if (!NewLicense.Save())
                return null;
            DeactivateCurrentLicense();
            return NewLicense;
        }
        public clsInternationalLicense IssueInternationLicense(int createdByUserID)
        {
            clsInternationalLicense internationalLicense = new clsInternationalLicense();
            internationalLicense.ApplicantPersonID = this.DriverInfo.PersonID;
            internationalLicense.ApplicationDate = DateTime.Now;
            internationalLicense.ApplicationTypeID = clsApplicationType.enApplicationType.NewInternationalLicense;
            internationalLicense.ApplicationStatus = clsApplication.enStatus.Completed;
            internationalLicense.LastStatusDate = DateTime.Now;
            internationalLicense.Fees = clsApplicationType.Find(clsApplicationType.enApplicationType.NewInternationalLicense).Fees;
            internationalLicense.CreatedByUserID = createdByUserID;

            internationalLicense.DriverID = this.DriverID;
            internationalLicense.LicenseID = this.LicenseID;
            internationalLicense.IssueDate = DateTime.Now;
            internationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
            internationalLicense.IsActive = true;

            if (internationalLicense.Save())
                return internationalLicense;

            return null;
        }

    }
}
