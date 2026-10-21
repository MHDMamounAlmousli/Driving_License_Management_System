using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer
{
    public class clsLocalDrivingLicense : clsApplication
    {
        public enum enMode { addnew = 0, update = 1 };
        public enMode Mode = enMode.addnew;
        public int LocalDrivingLicenseId { get; set; } 
        public int LicenseClassId { get; set; }
        public clsLicenseClass LicenseClassInfo { get; set; }
        public string PersonFullName => base.PersonInfo.FullName;
            
        private bool _AddNew()
        {
            this.LocalDrivingLicenseId = LocalLicenseData.AddNewLocalDrivingLicenseApplication(ApplicationID,this.LicenseClassId);
            return this.LocalDrivingLicenseId != -1;
        }
        private bool _Update()
        {
            return LocalLicenseData.UpdateLocalDrivingLicenseApplication(this.LocalDrivingLicenseId,this.ApplicationID,this.LicenseClassId);
        }
   
        public clsLocalDrivingLicense()
        {
            this.LocalDrivingLicenseId = -1;
            this.LicenseClassId = -1;
            this.Mode = enMode.addnew;
        }
        private clsLocalDrivingLicense(int localDrivingLicenseApplicationID,int applicationID,int applicantPersonID, DateTime applicationDate,clsApplicationType.enApplicationType applicationTypeID,enStatus applicationStatus,DateTime lastStatusDate,int paidFees,int createdByUserID,int licenseClassID) : base(applicationID, applicantPersonID, applicationDate, applicationTypeID,applicationStatus, lastStatusDate, paidFees, createdByUserID)
        {
            this.LocalDrivingLicenseId = localDrivingLicenseApplicationID;
            this.LicenseClassId = licenseClassID;
            this.LicenseClassInfo = clsLicenseClass.Find(licenseClassID);

            this.Mode = enMode.update;
        }
        public static clsLocalDrivingLicense FindLocalDrivingLicense(int id)
        {
            int applicationID = -1;
            int licenseClassID = -1;
            bool isFound = LocalLicenseData.GetLocalDrivingLicenseByID(id,ref applicationID,ref licenseClassID);
            if(isFound )
            {
                clsApplication application = clsApplication.Find(applicationID);
                if(application != null )
                {
                    return new clsLocalDrivingLicense(id, applicationID, application.ApplicantPersonID, application.ApplicationDate, application.ApplicationTypeID, application.ApplicationStatus, application.LastStatusDate, application.Fees, application.CreatedByUserID, licenseClassID);
                }
            }
            return null;
        }
        public static clsLocalDrivingLicense FindLocalDrivingLicenseApplicationID(int applicationID)
        {
            int localDrivingLicenseID = -1;
            int licenseClassID = -1;
            bool isFound = LocalLicenseData.GetLocalDrivingLicenseByApplicationID(
                applicationID,
                ref localDrivingLicenseID,
                ref licenseClassID
            );

            if (isFound)
            {
                clsApplication application = clsApplication.Find(applicationID);
                if (application != null)
                {
                    return new clsLocalDrivingLicense(
                        localDrivingLicenseID,
                        applicationID,
                        application.ApplicantPersonID,
                        application.ApplicationDate,
                        application.ApplicationTypeID,
                        application.ApplicationStatus,
                        application.LastStatusDate,
                        application.Fees,
                        application.CreatedByUserID,
                        licenseClassID
                    );
                }
            }

            return null;
        }
        
        public static DataTable GetAllLocalInfo() => LocalLicenseData.GetAllLocalDrivingLicenseInfo();
        public bool Save()
        {
            if (!base.Save())
                return false;
            else
            {
                switch(Mode)
                {
                    case enMode.addnew:
                        if( _AddNew())
                        {
                            Mode = enMode.update;
                            return true;
                        }
                        return false;
                    case enMode.update:
                        return _Update();
                    default: return false;

                }
            }
        }
        public bool Delete()
        {
            if (!LocalLicenseData.DeleteLocalDrivingLicenseApplication(LocalDrivingLicenseId))
                return false;
            return base.Delete(ApplicationID);
        }
        public static bool isPassedTestType(int locallicense, clsTestsType.enTestType testType) => LocalLicenseData.IsPassedTestType(locallicense, (int)testType);
        public static int getAllPassedTest(int locallicense) => LocalLicenseData.GetAllPassedTest(locallicense);
        public static bool IsHasActiveAppointment(int locallicense, clsTestsType.enTestType testType) => LocalLicenseData.IsLicenseHasActiveAppointment(locallicense, (int)testType);
        public static clsTest ResultLastAppointment(int locallicense, clsTestsType.enTestType testType) => clsTest.GetLastTestPerTestType(locallicense, testType);
        public static int GetTrialsPerTestType(int locallicense, clsTestsType.enTestType testTYpe) => TestAppointmentData.TotalTrialsPerTest(locallicense, (byte)testTYpe);
        public static int getActiveLicenseID(int localLicenseID) => LocalLicenseData.GetActiveLicenseID(localLicenseID);
        public int IssueLicenseForTheFirstTime(string Notes, int currentUserID)
        {
            int DriverID = -1;
            clsLicense license = new clsLicense();
            clsLocalDrivingLicense localLicense = FindLocalDrivingLicense(LocalDrivingLicenseId);
            clsDriver newDriver = clsDriver.FindByPersonID(localLicense.ApplicantPersonID);
            if(newDriver is null)
            {
                newDriver = new clsDriver();
                newDriver.PersonID = localLicense.ApplicantPersonID;
                newDriver.CreatedByUserID = currentUserID;
                newDriver.CreatedDate = DateTime.Now;
                if(newDriver.Save())
                {
                    DriverID = newDriver.DriverID;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                DriverID = newDriver.DriverID;
            }
            license.ApplicationID = localLicense.ApplicationID;
            license.DriverID = DriverID;
            license.LiceseClass = localLicense.LicenseClassId;
            license.IssueDate = DateTime.Now;
            license.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.ValidityLength);
            license.Notes = Notes;
            license.Fees = this.LicenseClassInfo.Fess;
            license.IsActive = true;
            license.IssueReason = clsLicense.enIssueReason.FirstTime;
            license.CreatedByUserID = currentUserID;
            if (license.Save())
            {
                this.UpdateStatus(clsApplication.enStatus.Completed);
                return license.LicenseID;
            }
            else
                return -1;
        }
    }
}
