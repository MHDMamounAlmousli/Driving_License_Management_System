using BuisnessLayer;
using DVLDProject.GlobalClasses;
using System;
using System.Windows.Forms;

namespace DVLDProject.License
{
    public partial class IssueDriverLicenseFT : Form
    {
        private int localLicenseID;
        clsLocalDrivingLicense locallicense;
        public IssueDriverLicenseFT(int localLicenseID)
        {
            InitializeComponent();
            this.localLicenseID = localLicenseID;
        }
        private void IssueDriverLicenseFT_Load(object sender, EventArgs e)
        {
            ucDrivingLicenseInfo1.LoadDrivingLicenseInfo(localLicenseID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            locallicense = clsLocalDrivingLicense.FindLocalDrivingLicense(localLicenseID);
            int LicenseID = locallicense.IssueLicenseForTheFirstTime(textBox1.Text.Trim(), CurrentUser.currentuser.UserID);
            if(LicenseID != -1)
            {
                MessageBox.Show("License issued successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to issue license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
