using BuisnessLayer;
using DVLDProject.GlobalClasses;
using System;
using System.Windows.Forms;

namespace DVLDProject.Tests
{
    public partial class ScreenTakeTest : Form
    {
        private int localLicense = -1;
        private int appointment = -1;
        private clsTestsType.enTestType testType;
        private clsTest test;
        private clsTestAppointment testAppointment;
        public ScreenTakeTest(int localLicenseID,int appointmentID,clsTestsType.enTestType testType)
        {
            InitializeComponent();
            this.localLicense = localLicenseID;
            this.appointment = appointmentID;
            this.testType = testType;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            test = new clsTest
            {
                TestAppointmentID = appointment,
                TestResult = rbPass.Checked ? clsTest.enTestResult.Pass : clsTest.enTestResult.Fail,
                Notes = txtNotes.Text,
                CreatedByUserID = CurrentUser.currentuser.UserID
            };
            if (test.Save())
            {
                MessageBox.Show("Test Result Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                testAppointment = clsTestAppointment.Find(appointment);
                testAppointment.IsLocked = true;
                testAppointment.Save();
                return;
            }
        }
        private void ScreenTakeTest_Load(object sender, EventArgs e)
        {
            int testID = clsTest.GetTestid(appointment);
            LoadTestInfo(testID);
            if (testID != -1)
            {
                lblUserMessage.Visible = true;
                btnSave.Enabled = false;
                rbFail.Enabled = false;
                rbPass.Enabled = false;
                txtNotes.Enabled = false;

            }
            ucTakeTest1.LoadScheduledTest(localLicense, appointment, testType);
        }
        private void LoadTestInfo(int testID)
        {
            test = clsTest.Find(testID);

            if (test == null)
                return;

            rbPass.Checked = test.TestResult == clsTest.enTestResult.Pass;
            rbFail.Checked = test.TestResult == clsTest.enTestResult.Fail;

            txtNotes.Text = test.Notes ?? "";
        }
    }
}
