using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer
{
    public class clsTestAppointment
    {
        enum enMode { add = 1, edit = 2 }
        private enMode mode = enMode.add;

        public int TestAppointmentID { get; set; }
        public int LocalDrivingLicenseID { get; set; }
        public int TestTypeID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int Fees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int? RetakeTestApplicationID { get; set; }
        public clsApplication RetakeTestInfo { get; set; }
        public clsTestAppointment()
        {
            TestAppointmentID = -1;
            LocalDrivingLicenseID = -1;
            TestTypeID = -1;
            AppointmentDate = DateTime.Now;
            Fees = 0;
            CreatedByUserID = 0;
            IsLocked = false;
            RetakeTestApplicationID = null;
        }
        public clsTestAppointment(int testAppointmentID, int localDrivingLicenseID, int testTypeID, DateTime appointmentDate, int fees, int createdByUserID, bool isLocked, int? retakeTestApplicationID)
        {
            TestAppointmentID = testAppointmentID;
            LocalDrivingLicenseID = localDrivingLicenseID;
            TestTypeID = testTypeID;
            AppointmentDate = appointmentDate;
            Fees = fees;
            CreatedByUserID = createdByUserID;
            IsLocked = isLocked;
            RetakeTestApplicationID = retakeTestApplicationID;
            mode = enMode.edit;
            RetakeTestInfo = clsApplication.Find(RetakeTestApplicationID ?? -1);
        }
        private bool addAppointment()
        {
            this.TestAppointmentID = TestAppointmentData.addTestAppointment(this.TestTypeID, this.LocalDrivingLicenseID, this.AppointmentDate, this.Fees, this.CreatedByUserID,this.IsLocked ,this.RetakeTestApplicationID);
            return this.TestAppointmentID != -1;
        }
        private bool editAppointment()
        {
            return TestAppointmentData.UpdateTestAppointment(this.TestAppointmentID, this.TestTypeID, this.LocalDrivingLicenseID, this.AppointmentDate, this.Fees, this.CreatedByUserID, this.IsLocked, this.RetakeTestApplicationID);
        }
        public bool Save()
        {
            switch(mode)
            {
                case enMode.add:
                    if (addAppointment())
                    {
                        mode = enMode.edit;
                        return true;
                    }
                    return false;
                case enMode.edit:
                    return editAppointment();
                default:
                    return false;
            }
        }
        public static clsTestAppointment Find(int TestAppointmentID)
        {
            int TestTypeID = 1; int LocalDrivingLicenseApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now; int PaidFees = 0;
            int CreatedByUserID = -1;
            bool IsLocked = false;
            int? RetakeTestApplicationID = -1;

            if (TestAppointmentData.GetAppointmentByID(TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID,ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))

                return new clsTestAppointment(TestAppointmentID,LocalDrivingLicenseApplicationID,TestTypeID,
             AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
                return null;

        }
        public static DataTable GetAppointmentPerTestType(int locallicenseid, clsTestsType.enTestType TestType) => TestAppointmentData.GetApplicationTestAppointmentsPerTestType(locallicenseid, (int)TestType);
    }
}
