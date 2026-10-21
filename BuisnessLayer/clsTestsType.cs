using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer
{
    public class clsTestsType
    {
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };
        public enTestType TestTypeID { set; get; }
        public string TestTypeTitle { set; get; }
        public string TestTypeDescription { set; get; }
        public int Fees { get; set; }
        public clsTestsType()
        {
            this.TestTypeID = enTestType.VisionTest;
            this.TestTypeTitle = "";
            this.TestTypeDescription = "";
            this.Fees = 0;
        }
        private clsTestsType(enTestType testTypeID, string testTypeTitle, string testTypeDescription,int fess)
        {
            this.TestTypeID = testTypeID; 
            this.TestTypeTitle = testTypeTitle;
            this.TestTypeDescription = testTypeDescription;
            this.Fees = fess;
        }
        public static clsTestsType Find(enTestType testTypeID)
        {
            string TestTypeTitle = "", TestTypeDescription = "";
            int fees = 0;
            bool found = TestsTypeData.GetTestTypeByID((int)testTypeID, ref TestTypeTitle, ref TestTypeDescription, ref fees);
            if (found)
                return new clsTestsType(testTypeID, TestTypeTitle, TestTypeDescription, fees);
            return null;
        }
        private bool Update()
        {
            return TestsTypeData.Update((int)this.TestTypeID,this.TestTypeTitle,this.TestTypeDescription,this.Fees);
        }
        public bool Save()
        {
            if (Update())
                return true;
            return false;
        }
        public static DataTable GetAllTypes() => TestsTypeData.GetAllTypes();
    }
}
