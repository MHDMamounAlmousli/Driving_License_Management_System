using BuisnessLayer;
using DVLDProject.GlobalClasses;
using DVLDProject.License;
using System;
using System.Windows.Forms;

namespace DVLDProject.Applications.Detain_License_Application
{
    public partial class DetainLicense : Form
    {
        private clsLicense _selectedLicense;
        private int _detainID = -1;
        public DetainLicense()
        {
            InitializeComponent();
        }
        private void FillDetainInfo()
        {
            lblDetainDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedByUser.Text = CurrentUser.currentuser.UserName;
        }
        private void ucLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            button1.Enabled = false;
            llShowLicenseHistory.Enabled = false;
            llShowLicenseInfo.Enabled = false;
            _selectedLicense = ucLicenseInfoWithFilter1.SelectedLicense;
            if (_selectedLicense is null) return;
            lblLicenseID.Text = _selectedLicense.LicenseID.ToString();
            llShowLicenseHistory.Enabled = true;
            if (!_selectedLicense.IsActive)
            {
                MessageBox.Show($"Selected License is not Active", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (_selectedLicense.isDetained)
            {
                MessageBox.Show($"Selected License is already Detained", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            button1.Enabled = true;
            txtFineFees.Focus();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren()) return;
            if (MessageBox.Show("Are you sure you want to detain this license?", "Confirm",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            _detainID = _selectedLicense.Detain(Convert.ToInt32(txtFineFees.Text.Trim()), CurrentUser.currentuser.UserID);
            if (_detainID == -1)
            {
                MessageBox.Show("Could not Save Detain License Info!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblDetainID.Text = _detainID.ToString();
            MessageBox.Show($"License Detained Successfully with ID = {_detainID}", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ucLicenseInfoWithFilter1.Enabled = false;
            button1.Enabled = false;
            txtFineFees.Enabled = false;
            llShowLicenseInfo.Enabled = true;
        }

        private void DetainLicense_Load(object sender, EventArgs e)
        {
            FillDetainInfo();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowLicenseHistory frm = new ShowLicenseHistory(_selectedLicense.ApplicationInfo.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowLicesenInfo frm = new ShowLicesenInfo(_selectedLicense.LicenseID);
            frm.ShowDialog();
        }

        private void txtFineFees_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtFineFees.Text))
            {
                errorProvider1.SetError(txtFineFees, "Error Enter Fees");
                txtFineFees.Focus();
            }
            else
            {
                errorProvider1.SetError(txtFineFees, "");
            }
        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
