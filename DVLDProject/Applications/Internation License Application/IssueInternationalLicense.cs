using BuisnessLayer;
using DVLDProject.Applications;
using DVLDProject.Applications.Internation_License_Application;
using DVLDProject.GlobalClasses;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLDProject.License.International_License
{
    public partial class IssueInternationalLicense : Form
    {
        int internationalLicenseID = -1;
        clsInternationalLicense _InternationalLicense;   
        clsLicense _License;
        public IssueInternationalLicense()
        {
            InitializeComponent();
        }
        private void FillApplicationInfo()
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblExpirationDate.Text = DateTime.Now.AddYears(1).ToShortDateString();
            lblIssueDate.Text = lblApplicationDate.Text;
            lblFees.Text = clsApplicationType.Find(clsApplicationType.enApplicationType.NewInternationalLicense).Fees.ToString();
            lblCreatedByUser.Text = CurrentUser.currentuser.UserName;
        }
        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            _InternationalLicense = _License.IssueInternationLicense(CurrentUser.currentuser.UserID);

            if (_InternationalLicense != null)
            {
                internationalLicenseID = _InternationalLicense.InternationalLicenseID;

                MessageBox.Show("Issued Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblApplicationID.Text = _InternationalLicense.ApplicationID.ToString();
                lblInternationalLicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();

                llShowLicenseInfo.Enabled = true;
                btnIssueLicense.Enabled = false;
                ucLicenseInfoWithFilter1.Enabled = false;
            }
            else
            {
                MessageBox.Show("Failed to issue international license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowLicenseHistory frm = new ShowLicenseHistory(_License.ApplicationInfo.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowInternationalLicenseInfo frm = new ShowInternationalLicenseInfo(internationalLicenseID);
            frm.ShowDialog();
        }

        private void IssueInternationalLicense_Load(object sender, EventArgs e)
        {
            FillApplicationInfo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ucLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            btnIssueLicense.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
            int LicenseID = obj;
            lblLocalLicenseID.Text = LicenseID.ToString();
            llShowLicenseHistory.Enabled = LicenseID != -1;

            if (obj == -1)
            {
                llShowLicenseHistory.Enabled = false;
                lblLocalLicenseID.Text = "[???]";
                return;
            }
            _License = clsLicense.Find(LicenseID);
            if (_License.LicenseClassInfo.LicenseClassID != 3)
            {
                MessageBox.Show($"License with ID = {LicenseID} must be Class 3 to issue International License", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!_License.IsActive)
            {
                MessageBox.Show($"License with ID = {LicenseID} is not Acitve", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_License.ExpirationDate <= DateTime.Now)
            {
                MessageBox.Show($"License with ID = {LicenseID} is Expired", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (_License.isDetained)
            {
                MessageBox.Show($"License with ID = {LicenseID} is Detained", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int license = clsInternationalLicense.HasActiveInternationalLicense(_License.DriverID);
            if (license != -1)
            {
                internationalLicenseID = license;
                MessageBox.Show("Person already have an active international license with ID = " + license.ToString(), "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                llShowLicenseInfo.Enabled = true;
                btnIssueLicense.Enabled = false;
                return;
            }
            btnIssueLicense.Enabled = true;
        }
    }
}
