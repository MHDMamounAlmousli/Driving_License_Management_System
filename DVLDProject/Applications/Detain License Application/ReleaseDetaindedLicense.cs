using BuisnessLayer;
using DVLDProject.GlobalClasses;
using DVLDProject.License;
using System;
using System.Windows.Forms;

namespace DVLDProject.Applications.Detain_License_Application
{
    public partial class ReleaseDetaindedLicense : Form
    {
        private int _detainID = -1;
        private clsLicense _selectedLicense;
        private clsDetainLicense _detainLicense;
        public ReleaseDetaindedLicense(int DetainID = -1)
        {
            InitializeComponent();
            _detainID = DetainID;
        }
        private void FillDetainInfo()
        {
            if (_detainLicense == null) return;
            int applicationFees = clsApplicationType.Find(clsApplicationType.enApplicationType.ReleaseDetainedDrivingLicsense).Fees;
            lblDetainID.Text = _detainLicense.DetainID.ToString();
            lblDetainDate.Text = _detainLicense.DetainDate.ToShortDateString();
            lblFineFees.Text = _detainLicense.Fees.ToString();
            lblApplicationFees.Text = applicationFees.ToString();
            lblTotalFees.Text = (_detainLicense.Fees + applicationFees).ToString();
            lblCreatedByUser.Text = CurrentUser.currentuser.UserName;
        }
        private void ucLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            btnRelease.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
            _selectedLicense = ucLicenseInfoWithFilter1.SelectedLicense;
            _detainLicense = clsDetainLicense.FindByLicenseID(_selectedLicense.LicenseID);
            lblLicenseID.Text = _selectedLicense.LicenseID.ToString();
            llShowLicenseHistory.Enabled = true;
            if(!_selectedLicense.isDetained)
            {
                MessageBox.Show("Selected License is not Detained!", "Not Allow", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            btnRelease.Enabled = true;
            FillDetainInfo();
        }
        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (_selectedLicense == null || _detainLicense == null) return;

            if (MessageBox.Show("Are you sure you want to release this detained license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            int applicationID = -1;
            if(!_selectedLicense.Release(CurrentUser.currentuser.UserID,ref applicationID))
            {
                MessageBox.Show("Failed to release the detained license.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Released Successfully", "Released", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lblApplicationID.Text = applicationID.ToString();
            llShowLicenseInfo.Enabled = true;
            ucLicenseInfoWithFilter1.Enabled = false;
            btnRelease.Enabled = false;
        }
        private void ReleaseDetaindedLicense_Load(object sender, EventArgs e)
        {
            if (_detainID != -1)
            {
                _detainLicense = clsDetainLicense.Find(_detainID);
                if (_detainLicense != null)
                {
                    ucLicenseInfoWithFilter1.LoadLicenseInfo(_detainLicense.LicenseID);
                    ucLicenseInfoWithFilter1.Enabled = false;
                }
            }
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowLicenseHistory frm = new ShowLicenseHistory(_selectedLicense.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowLicesenInfo frm = new ShowLicesenInfo(_selectedLicense.LicenseID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
