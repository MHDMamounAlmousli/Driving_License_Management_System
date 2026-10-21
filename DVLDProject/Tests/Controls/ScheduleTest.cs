using BuisnessLayer;
using DVLDProject.GlobalClasses;
using DVLDProject.Properties;
using System;
using System.Windows.Forms;

namespace DVLDProject.Tests.Controls
{
    public partial class ScheduleTest : UserControl
    {
        public enum enMode {  add = 1, edit = 2}
        private enMode mode = enMode.add;
        private int testAppointmentID = -1;
        private int localLicensID = -1;
        private clsLocalDrivingLicense localLicesne;
        private clsTestsType.enTestType testType = clsTestsType.enTestType.VisionTest;
        private clsTestAppointment testAppointment;
        private clsTestsType testTypeInfo;
        public ScheduleTest()
        {
            InitializeComponent();
        }
        private void ResetRetakeTestInfo()
        {
            lblRetakeAppFees.Text = "0";
            lblRetakeTestAppID.Text = "N/A";
            lblTotalFees.Text = lblFees.Text;
        }
        private void FillLocalLicenseInfo()
        {
            lblLocalDrivingLicenseAppID.Text = localLicesne.LocalDrivingLicenseId.ToString();
            lblDrivingClass.Text = localLicesne.LicenseClassInfo.ClassName;
            lblFullName.Text = localLicesne.PersonFullName;
            dtpTestDate.Text = DateTime.Now.ToString();
            lblTrial.Text = clsLocalDrivingLicense.GetTrialsPerTestType(localLicesne.LocalDrivingLicenseId, testType).ToString();
            lblFees.Text = testTypeInfo.Fees.ToString();
        }
        private void LoadRetakeTestInfo()
        {
            clsApplicationType retakeApplicationType = clsApplicationType.Find(clsApplicationType.enApplicationType.RetakeTest);


            int retakeFees = retakeApplicationType.Fees;
            int testFees = testTypeInfo.Fees;

            lblRetakeAppFees.Text = retakeApplicationType.Fees.ToString();
            lblTotalFees.Text = (retakeFees + testFees).ToString();

            if (mode == enMode.add)
            {
                lblRetakeTestAppID.Text = "N/A";
                return;
            }
            //clsTestAppointment appointment =clsTestAppointment.Find(testAppointmentID);

            if (testAppointment != null && testAppointment.RetakeTestApplicationID != -1)
            {
                lblRetakeTestAppID.Text = testAppointment.RetakeTestApplicationID.ToString();
            }
            else
            {
                lblRetakeTestAppID.Text = "N/A";
            }
        }
        private void HandleTypeTest()
        {
            switch (testType)
            {

                case clsTestsType.enTestType.VisionTest:
                    {
                        gbTestType.Text = "Vision Test";
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;
                    }

                case clsTestsType.enTestType.WrittenTest:
                    {
                        gbTestType.Text = "Written Test";
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;
                    }
                case clsTestsType.enTestType.StreetTest:
                    {
                        gbTestType.Text = "Street Test";
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;


                    }

            }
        }
        private void HandleAdd()
        {
            int trails = clsLocalDrivingLicense.GetTrialsPerTestType(localLicensID, testType);
            if(trails > 0)
            {
                lblTitle.Text = "Schedule Retake Test";
                gbRetakeTestInfo.Enabled = true;
                LoadRetakeTestInfo();
                return;
            }
                lblTitle.Text = "Schedule Test";
                gbRetakeTestInfo.Enabled = false ;
            ResetRetakeTestInfo();
          
        }
        private void HandleEdit()
        {
            //clsTestAppointment appointment = clsTestAppointment.Find(testAppointmentID);

            int trials =clsLocalDrivingLicense.GetTrialsPerTestType(testAppointment.LocalDrivingLicenseID, testType);

            if (testAppointment.IsLocked)
            {
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
                lblUserMessage.Visible = true;
                gbRetakeTestInfo.Enabled = true;
                lblUserMessage.Text ="Person already sat for the test, appointment locked";
                LoadRetakeTestInfo();
                return;
            }
            if (testAppointment.AppointmentDate > DateTime.Now)
                dtpTestDate.MinDate = DateTime.Now;
            else
                dtpTestDate.MinDate = testAppointment.AppointmentDate;

            dtpTestDate.Value = testAppointment.AppointmentDate;
            dtpTestDate.Enabled = true;
            btnSave.Enabled = true;
            lblUserMessage.Visible = false;
            if (trials > 0)
            {
                lblTitle.Text = "Schedule Retake Test";
                gbRetakeTestInfo.Enabled = true;
                LoadRetakeTestInfo();
            }
            else
            {
                lblTitle.Text = "Schedule Test";
                gbRetakeTestInfo.Enabled = false;
                ResetRetakeTestInfo();
            }
        }
        public void LoadTestInfo(clsTestsType.enTestType testtype, int localLicense, int testAppointmentID = -1)
        {
            this.localLicensID = localLicense;
            this.testAppointmentID = testAppointmentID;
            this.testType = testtype;

            mode = testAppointmentID == -1 ? enMode.add : enMode.edit;

            testTypeInfo = clsTestsType.Find(testType);
            HandleTypeTest();
            localLicesne = clsLocalDrivingLicense.FindLocalDrivingLicense(localLicensID);
            if (localLicesne != null)
            {
                FillLocalLicenseInfo();
                if (mode == enMode.edit)
                {
                    testAppointment = clsTestAppointment.Find(testAppointmentID);
                    if (testAppointment != null)
                    {
                        dtpTestDate.Text = testAppointment.AppointmentDate.ToString();
                        if (testAppointment.RetakeTestApplicationID != -1)
                            lblRetakeTestAppID.Text = testAppointment.RetakeTestApplicationID.ToString();
                        else
                            lblRetakeTestAppID.Text = "N/A";
                    }else
                    {
                        MessageBox.Show("Appointment not found.");
                        return;
                    }
                        HandleEdit();
                }
                else
                    HandleAdd();

            }
        }
        private void Save()
        {
            int? retaketestApplicationID = null;
            int trails = clsLocalDrivingLicense.GetTrialsPerTestType(localLicensID, testType);
            if (mode == enMode.add)
            {
                if (trails > 0)
                {

                    clsApplication retakeTestApplication = new clsApplication();
                    retakeTestApplication.ApplicantPersonID = localLicesne.ApplicantPersonID;
                    retakeTestApplication.ApplicationTypeID = clsApplicationType.enApplicationType.RetakeTest;
                    retakeTestApplication.ApplicationDate = DateTime.Now;
                    retakeTestApplication.ApplicationStatus = clsApplication.enStatus.New;
                    retakeTestApplication.Fees = clsApplicationType.Find(clsApplicationType.enApplicationType.RetakeTest).Fees;
                    retakeTestApplication.CreatedByUserID = CurrentUser.currentuser.UserID;
                    if (!retakeTestApplication.Save())
                    {
                        MessageBox.Show("Failed to create retake application.");
                        return;
                    }

                    retaketestApplicationID = retakeTestApplication.ApplicationID;
                    lblRetakeTestAppID.Text = retaketestApplicationID.ToString();
                }
               testAppointment = new clsTestAppointment();

               testAppointment.TestTypeID = (int)testType;
               testAppointment.LocalDrivingLicenseID = localLicensID;
               testAppointment.AppointmentDate = dtpTestDate.Value;
               testAppointment.Fees = testTypeInfo.Fees;
               testAppointment.CreatedByUserID = CurrentUser.currentuser.UserID;
               testAppointment.RetakeTestApplicationID = retaketestApplicationID;
               testAppointment.IsLocked = false;
                if(testAppointment.Save())
                {
                    mode = enMode.edit;
                    MessageBox.Show("Appointment saved successfully.");
                    return;
                }
            }
            else
            {
                testAppointment.AppointmentDate = dtpTestDate.Value;
                if (testAppointment.Save())
                {
                    MessageBox.Show("Appointment date updated successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to update appointment date.");
                }
                return;
            }

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }
    }
}
