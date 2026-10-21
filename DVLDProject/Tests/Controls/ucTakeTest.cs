using BuisnessLayer;
using DVLDProject.Properties;
using System.Windows.Forms;

namespace DVLDProject.Tests.Controls
{
    public partial class ucTakeTest : UserControl
    {
        private clsLocalDrivingLicense localLicense;
        private clsTestAppointment appointment;
        public ucTakeTest()
        {
            InitializeComponent();
        }
        public void HandleImage(clsTestsType.enTestType testType)
        {
            switch (testType)
            {
                case clsTestsType.enTestType.VisionTest:
                    pbTestTypeImage.Image = Resources.Vision_512;
                    break;
                case clsTestsType.enTestType.WrittenTest:
                    pbTestTypeImage.Image = Resources.Written_Test_512;
                    break;
                case clsTestsType.enTestType.StreetTest:
                    pbTestTypeImage.Image = Resources.driving_test_512;
                    break;
            }

        }
        public void LoadScheduledTest(int locallicense,int appointmentID,clsTestsType.enTestType testType)
        {
            HandleImage(testType);
            localLicense = clsLocalDrivingLicense.FindLocalDrivingLicense(locallicense);
            appointment = clsTestAppointment.Find(appointmentID);
            lblLocalDrivingLicenseAppID.Text = localLicense.LocalDrivingLicenseId.ToString();
            lblDrivingClass.Text = localLicense.LicenseClassInfo.ClassName;
            lblFullName.Text = localLicense.PersonFullName;
            lblTrial.Text = clsLocalDrivingLicense.GetTrialsPerTestType(locallicense, testType).ToString();
            lblDate.Text = appointment.AppointmentDate.ToString("dd/MM/yyyy");
            lblFees.Text = clsTestsType.Find(testType).Fees.ToString();
            lblTestID.Text = clsTest.GetTestid(appointmentID).ToString();
            if (lblTestID.Text == "-1")
                lblTestID.Text = "Not Taken Yet";
        }
    }
}
