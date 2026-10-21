using BuisnessLayer;
using DVLDProject.Applications.Internation_License_Application;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLDProject.License.Controls
{
    public partial class ucDriverLicenseHistory : UserControl
    {
        private int PersonID;
        private clsDriver Driver;
        private DataTable dtLocalLicense;
        private DataTable dtInternationLicense;
        public ucDriverLicenseHistory()
        {
            InitializeComponent();
        }
        private void LoadLocalLicense()
        {
            dtLocalLicense = clsDriver.GetLicensePerDriverByPersonID(PersonID);
            dgvLocalLicensesHistory.DataSource = dtLocalLicense;
            lblLocalLicensesRecords.Text = dtLocalLicense.Rows.Count.ToString();
            if (dgvLocalLicensesHistory.Rows.Count > 0)
            {
                dgvLocalLicensesHistory.Columns[0].HeaderText = "Lic.ID";
                dgvLocalLicensesHistory.Columns[0].Width = 80;

                dgvLocalLicensesHistory.Columns[1].HeaderText = "App.ID";
                dgvLocalLicensesHistory.Columns[1].Width = 80;

                dgvLocalLicensesHistory.Columns[2].HeaderText = "Class Name";
                dgvLocalLicensesHistory.Columns[2].Width = 220;

                dgvLocalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicensesHistory.Columns[3].Width = 150;

                dgvLocalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicensesHistory.Columns[4].Width = 150;

                dgvLocalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvLocalLicensesHistory.Columns[5].Width = 80;

            }
        }
        private void LoadInternationalLicense()
        {
            dtInternationLicense = clsInternationalLicense.GetAllLicensesPerDriver(Driver.DriverID);
            dgvInternationalLicensesHistory.DataSource = dtInternationLicense;
            lblInternationalLicensesRecords.Text = dtInternationLicense.Rows.Count.ToString();
            if (dgvInternationalLicensesHistory.Rows.Count > 0)
            {
                dgvInternationalLicensesHistory.Columns[0].HeaderText = "int.License ID";
                dgvInternationalLicensesHistory.Columns[0].Width = 120;

                dgvInternationalLicensesHistory.Columns[1].HeaderText = "App.ID";
                dgvInternationalLicensesHistory.Columns[1].Width = 80;

                dgvInternationalLicensesHistory.Columns[2].HeaderText = "L.License ID";
                dgvInternationalLicensesHistory.Columns[2].Width = 100;

                dgvInternationalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicensesHistory.Columns[3].Width = 180;

                dgvInternationalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicensesHistory.Columns[4].Width = 180;

                dgvInternationalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicensesHistory.Columns[5].Width = 80;

            }
        }

        public void LoadLocalLicenseInfoByPersonID(int PersonID)
        {
            this.PersonID = PersonID;
            Driver = clsDriver.FindByPersonID(PersonID);
            if (Driver != null)
            {
                LoadLocalLicense();
            }
        }
        public void LoadInternationsalLicenseInfoByPersonID(int PersonID)
        {
            this.PersonID = PersonID;
            Driver = clsDriver.FindByPersonID(PersonID);
            if (Driver != null)
                LoadInternationalLicense();
        }
        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLicesenInfo frm = new ShowLicesenInfo((int)dgvLocalLicensesHistory.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void showLicenseInfoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowInternationalLicenseInfo frm = new ShowInternationalLicenseInfo((int)dgvInternationalLicensesHistory.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
    }
}
