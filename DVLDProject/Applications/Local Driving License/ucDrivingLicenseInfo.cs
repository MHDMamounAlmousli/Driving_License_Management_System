using BuisnessLayer;
using DVLDProject.License;
using System.Drawing;
using System.Windows.Forms;

namespace DVLDProject.Applications.Local_Driving_License
{
    public partial class ucDrivingLicenseInfo : UserControl
    {
        private int localLicenseID = -1; 
        private clsLocalDrivingLicense ldl;
        private int licenseID;
        public ucDrivingLicenseInfo()
        {
            InitializeComponent();
        }
        public void LoadDrivingLicenseInfo(int ldlID)
        {
            localLicenseID = ldlID;
            ldl = clsLocalDrivingLicense.FindLocalDrivingLicense(localLicenseID);
            if (ldl == null)
            {
                ResetData();
                MessageBox.Show("No Local Driving License Application with ID = " + ldlID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                LoadData();
        }
        private void LoadData()
        {
            licenseID = clsLocalDrivingLicense.getActiveLicenseID(localLicenseID);
            bool ActiveLicense = licenseID != -1;
            llShowLicenceInfo.Enabled = ActiveLicense;
            llShowLicenceInfo.ActiveLinkColor = ActiveLicense ? Color.Blue : Color.Gray;
            lblLocalDrivingLicenseApplicationID.Text = ldl.ApplicationID.ToString();
            lblAppliedFor.Text = ldl.LicenseClassInfo.ClassName;
            applicationBasicInfo1.LoadApplicationInfo(ldl.ApplicationID);
            int countPassedTests = clsLocalDrivingLicense.getAllPassedTest(ldl.LocalDrivingLicenseId);
            lblPassedTests.Text = $"{countPassedTests}/3";
        }
        private void ResetData()
        {
            lblLocalDrivingLicenseApplicationID.Text = "[????]";
            lblAppliedFor.Text = "[????]";
            lblPassedTests.Text = "0";
            llShowLicenceInfo.ActiveLinkColor = Color.Gray;
            llShowLicenceInfo.Enabled = false;
        }

        private void llShowLicenceInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowLicesenInfo frm = new ShowLicesenInfo(licenseID);
                frm.ShowDialog();
        }
    }
}
