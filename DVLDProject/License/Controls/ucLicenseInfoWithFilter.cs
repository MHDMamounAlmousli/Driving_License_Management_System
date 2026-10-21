using BuisnessLayer;
using System;
using System.Windows.Forms;

namespace DVLDProject.License.Controls
{
    public partial class ucLicenseInfoWithFilter : UserControl
    {
        public ucLicenseInfoWithFilter()
        {
            InitializeComponent();
        }
        public event Action<int> OnLicenseSelected;
        private bool _EnableFilter = true;
            public bool EnableFilter
            {
                get { return _EnableFilter; }
                set
                {
                    _EnableFilter = value;
                    gbFilters.Enabled = _EnableFilter;
                }
            }
        public clsLicense SelectedLicense => ucDriverLicense1.SelectedLicense;
        public void LoadLicenseInfo(int licenseID)
        {
            txtLicenseID.Text = licenseID.ToString();
            btnFind_Click(null, null);
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;
            int licenseID = Convert.ToInt32(txtLicenseID.Text);
            clsLicense License = clsLicense.Find(licenseID);
            if (License is null)
            {
                ucDriverLicense1.ResetData();
                MessageBox.Show("License was not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OnLicenseSelected?.Invoke(-1);
            }
            else
            {
                ucDriverLicense1.LoadLicenseInfo(licenseID);
                OnLicenseSelected?.Invoke(licenseID);
            }
        }

        private void txtLicenseID_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLicenseID.Text))
            {
                errorProvider1.SetError(txtLicenseID, "Enter LicenseID");
                txtLicenseID.Focus();
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtLicenseID, "");
                e.Cancel = false;
            }
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            if (e.KeyChar == (char)13)
            {
                btnFind.PerformClick();
            }
        }
    }
}
