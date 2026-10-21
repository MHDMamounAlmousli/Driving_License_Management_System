using BuisnessLayer;
using System;
using System.Windows.Forms;

namespace DVLDProject.Tests
{
    public partial class ScheduleTestType : Form
    {
        private int locallicenseID = -1;
        private clsTestsType.enTestType testType = clsTestsType.enTestType.VisionTest;
        private int testappointment = -1;
        public ScheduleTestType(int locallicense,clsTestsType.enTestType testtype,int testappointment = -1)
        {
            InitializeComponent();
            locallicenseID = locallicense;
            testType = testtype;
            this.testappointment = testappointment;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ScheduleTestType_Load(object sender, EventArgs e)
        {
            scheduleTest1.LoadTestInfo(testType, locallicenseID, testappointment);
        }
    }
}
