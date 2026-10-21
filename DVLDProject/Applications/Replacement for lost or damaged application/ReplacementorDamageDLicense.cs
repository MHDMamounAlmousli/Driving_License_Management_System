using BuisnessLayer;
using DVLDProject.GlobalClasses;
using DVLDProject.License;
using System;
using System.Windows.Forms;

namespace DVLDProject.Applications.Replacement_for_lost_or_damaged_application
{
    public partial class ReplacementorDamageDLicense : Form
    {
        private clsLicense _selectedLicense;
        private clsLicense _newLicense;
        private clsLicense.enIssueReason _getIssueReason()
        {
            return rbDamagedLicense.Checked
                ? clsLicense.enIssueReason.DamagedReplacement
                : clsLicense.enIssueReason.LostReplacement;
        }
        private clsApplicationType.enApplicationType _getApplicationType()
        {
            return rbDamagedLicense.Checked
                ? clsApplicationType.enApplicationType.ReplaceDamagedDrivingLicense
                : clsApplicationType.enApplicationType.ReplaceLostDrivingLicense;
        }
        private void FillApplicationInfo()
        {
            lblTitle.Text = rbDamagedLicense.Checked ? "Replacement for Damaged License" : "Replacement for Lost License";
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedByUser.Text = CurrentUser.currentuser.UserName;

            clsApplicationType appType = clsApplicationType.Find(_getApplicationType());
            lblApplicationFees.Text = appType != null ? appType.Fees.ToString() : "0";

            if (_selectedLicense != null)
            {
                lblOldLicenseID.Text = _selectedLicense.LicenseID.ToString();
            }
        }
        private void ucLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            btnIssueReplacement.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
            lblOldLicenseID.Text = "???";

            _selectedLicense = ucLicenseInfoWithFilter1.SelectedLicense;

            if (_selectedLicense == null) return;

            llShowLicenseHistory.Enabled = true;
            FillApplicationInfo();

            if (!_selectedLicense.IsActive)
            {
                MessageBox.Show("Selected License is not Active.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_selectedLicense.isDetained)
            {
                MessageBox.Show("Selected License is Detained. Release it first to complete the Replacement Application.","Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnIssueReplacement.Enabled = true;
        }
        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if (_selectedLicense == null) return;

            if (MessageBox.Show("Are you sure you want to issue a replacement for this license?", "Confirm",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            _newLicense = _selectedLicense.Replace(_getIssueReason(), CurrentUser.currentuser.UserID);

            if (_newLicense == null)
            {
                MessageBox.Show("Failed to issue replacement license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblApplicationID.Text = _newLicense.ApplicationID.ToString();
            lblRreplacedLicenseID.Text = _newLicense.LicenseID.ToString();

            MessageBox.Show($"License Replaced Successfully with ID = {_newLicense.LicenseID}","License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssueReplacement.Enabled = false;
            ucLicenseInfoWithFilter1.Enabled = false;
            gbReplacementFor.Enabled = false;
            llShowLicenseInfo.Enabled = true;
        }

        private void ReplacementorDamageDLicense_Load(object sender, EventArgs e)
        {
            FillApplicationInfo();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowLicenseHistory frm = new ShowLicenseHistory(_selectedLicense.ApplicationInfo.ApplicantPersonID);
            frm.ShowDialog();
        }
        public ReplacementorDamageDLicense()
        {
            InitializeComponent();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowLicesenInfo frm = new ShowLicesenInfo(_newLicense.LicenseID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            FillApplicationInfo();
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            FillApplicationInfo();
        }

    }
}
