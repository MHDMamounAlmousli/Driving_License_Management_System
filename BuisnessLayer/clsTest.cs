using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer
{
    public class clsTest
    {
        public enum enTestResult { Pass = 1, Fail = 0}
        public enum enMode { add = 1,update =2 }
        private enMode mode = enMode.add;
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public enTestResult TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }

        public clsTest()
        {
            TestID = -1;
            TestAppointmentID = -1;
            TestResult = enTestResult.Fail;
            Notes = string.Empty;
            CreatedByUserID = -1;
        }
        public clsTest(int testid,int testAppointmentID,enTestResult testResult,string notes,int createdbyuserid)
        {
            TestID = testid;
            TestAppointmentID = testAppointmentID;
            TestResult = testResult;
            Notes = notes;
            CreatedByUserID = createdbyuserid;
            mode = enMode.update;
        }
        private bool AddTest()
        {
            this.TestID = TestData.AddTest(this.TestAppointmentID, (byte)this.TestResult, this.Notes, this.CreatedByUserID);
            return this.TestID > 0;
        }
        private bool UpdateTest()
        {
            return TestData.UpdateTest(this.TestID, this.TestAppointmentID, (byte)this.TestResult, this.Notes, this.CreatedByUserID);
        }
        public bool Save()
        {
            switch(mode)
            {
                case enMode.add:
                      if(AddTest())
                      {
                        mode = enMode.update;
                        return true;
                      }
                    return false;
                case enMode.update:
                    return UpdateTest();
                default: return false;
                    
            }
        }
        public static bool IsTestExists(int testID) => TestData.TestExists(testID);
        public static bool IsTestExistsByAppointmentid(int testAppointmentID) => TestData.TestExistsByAppointmentID(testAppointmentID);
        public static clsTest Find(int testID)
        {
            int testAppointmentID = -1,createdByUserID = -1;
            string notes = "";
            byte testResult = 0;
            bool Found = TestData.GetTestByID(testID, ref testAppointmentID, ref testResult, ref notes, ref createdByUserID);
            if(Found)
            {
                return new clsTest(testID, testAppointmentID, (enTestResult)testResult, notes, createdByUserID);
            }
                return null;
        }
        public static clsTest FindByAppointmentID(int testAppointmentID)
        {
            int testId = -1, createdByUserID = -1;
            string notes = "";
            byte testResult = 0;
            bool Found = TestData.GetTestByAppointmentID(testAppointmentID, ref testId, ref testResult, ref notes, ref createdByUserID);
            if (Found)
            {
                return new clsTest(testId, testAppointmentID, (enTestResult)testResult, notes, createdByUserID);
            }
            return null;
        }
        public static clsTest GetLastTestPerTestType(int locallicense,clsTestsType.enTestType testTypeID)
        {
            int testID = -1;
            int testAppointmentID = -1;
            byte testResult = 0;
            string notes = "";
            int createdByUserID = -1;

            bool found = TestData.GetLastTestPerTestType(
                locallicense,
                (byte)testTypeID,
                ref testID,
                ref testAppointmentID,
                ref testResult,
                ref notes,
                ref createdByUserID);

            if (!found)
                return null;

            return new clsTest(
                testID,
                testAppointmentID,
                (clsTest.enTestResult)testResult,
                notes,
                createdByUserID);
        }
        public static int GetTestid(int appointmentID) => TestData.GetTestID(appointmentID);
    }
}
