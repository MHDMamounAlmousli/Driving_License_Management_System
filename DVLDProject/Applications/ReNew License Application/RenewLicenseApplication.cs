using BuisnessLayer;
using DVLDProject.GlobalClasses;
using DVLDProject.License;
using System;
using System.Windows.Forms;

namespace DVLDProject.Applications.ReNew_License_Application
{
    public partial class RenewLicenseApplication : Form
    {
        private clsLicense _newLicense;
        private clsLicense selectedLicense;
        private void FillApplicationInfo()
        {
            lblOldLicenseID.Text = selectedLicense.LicenseID.ToString();
            int applicationFees = clsApplicationType.Find(clsApplicationType.enApplicationType.RenewDrivingLicense).Fees;
            int licenseFees = selectedLicense.LicenseClassInfo.Fess;

            lblExpirationDate.Text = DateTime.Now.AddYears(selectedLicense.LicenseClassInfo.ValidityLength).ToShortDateString();
            lblApplicationFees.Text = applicationFees.ToString();
            lblLicenseFees.Text = licenseFees.ToString();
            lblTotalFees.Text = (applicationFees + licenseFees).ToString();
        }
        private void ucLicenseInfoWithFilter1_OnLicenseSelected(int licenseID)
        {
            btnRenewLicense.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            txtNotes.Enabled = false;
            lblOldLicenseID.Text = "???";
            selectedLicense = ucLicenseInfoWithFilter1.SelectedLicense;
            if (selectedLicense == null) return;
            llShowLicenseHistory.Enabled = true;
            FillApplicationInfo();
            if (!selectedLicense.isExpired())
            {
                MessageBox.Show($"Selected License is not yet expired, it will expire on {selectedLicense.ExpirationDate.ToShortDateString()}","Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (selectedLicense.isDetained)
            {
                MessageBox.Show("Selected License is Detained. Release it first to complete the Renew Application.","Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            txtNotes.Enabled = true;
            btnRenewLicense.Enabled = true;
        }

        private void btnRenewLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to renew this license?", "Confirm",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            _newLicense = selectedLicense.RenewLicense(txtNotes.Text.Trim(), CurrentUser.currentuser.UserID);
            if (_newLicense == null)
            {
                MessageBox.Show("Failed to renew the license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblApplicationID.Text = _newLicense.ApplicationID.ToString();
            lblRenewedLicenseID.Text = _newLicense.LicenseID.ToString();
            MessageBox.Show($"License Renewed Successfully with ID = {_newLicense.LicenseID}","License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
            btnRenewLicense.Enabled = false;
            ucLicenseInfoWithFilter1.Enabled = false;
            llShowLicenseInfo.Enabled = true;
        }

        public RenewLicenseApplication()
        {
            InitializeComponent();
        }
        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowLicesenInfo frm = new ShowLicesenInfo(_newLicense.LicenseID);
            frm.ShowDialog();
        }
        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowLicenseHistory frm = new ShowLicenseHistory(selectedLicense.ApplicationInfo.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RenewLicenseApplication_Load_1(object sender, EventArgs e)
        {
            lblCreatedByUser.Text = CurrentUser.currentuser.UserName;
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = lblApplicationDate.Text;
        }
    }
}
