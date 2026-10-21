using BuisnessLayer;
using DVLDProject.License.International_License;
using DVLDProject.People;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLDProject.Applications.Internation_License_Application
{
    public partial class ListInternationalLicenses : Form
    {
        private DataTable dtInternational;
        private void RefreshData()
        {
            dtInternational = clsInternationalLicense.GetAllLicenses();
            dgvInternationalLicenses.DataSource = dtInternational;
            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.RowCount.ToString();
        }
        public ListInternationalLicenses()
        {
            InitializeComponent();
        }

        private void btnNewApplication_Click(object sender, EventArgs e)
        {
            IssueInternationalLicense frm = new IssueInternationalLicense();
            frm.ShowDialog();
            RefreshData();
        }

        private void ListInternationalLicenses_Load(object sender, EventArgs e)
        {
            RefreshData();
            cbFilterBy.SelectedIndex = 0;
            if (dgvInternationalLicenses.Rows.Count > 0)
            {
                dgvInternationalLicenses.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicenses.Columns[0].Width = 130;

                dgvInternationalLicenses.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenses.Columns[1].Width = 150;

                dgvInternationalLicenses.Columns[2].HeaderText = "Driver ID";
                dgvInternationalLicenses.Columns[2].Width = 130;

                dgvInternationalLicenses.Columns[3].HeaderText = "L.License ID";
                dgvInternationalLicenses.Columns[3].Width = 130;

                dgvInternationalLicenses.Columns[4].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[4].Width = 180;

                dgvInternationalLicenses.Columns[5].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns[5].Width = 180;

                dgvInternationalLicenses.Columns[6].HeaderText = "Is Active";
                dgvInternationalLicenses.Columns[6].Width = 120;

            }
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsInternationalLicense license = clsInternationalLicense.Find((int)dgvInternationalLicenses.CurrentRow.Cells[0].Value);
            ShowPersonInfo frm = new ShowPersonInfo(license.DriverInfo.PersonInfo.PersonID);
            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowInternationalLicenseInfo frm = new ShowInternationalLicenseInfo((int)dgvInternationalLicenses.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsInternationalLicense license = clsInternationalLicense.Find((int)dgvInternationalLicenses.CurrentRow.Cells[0].Value);
            ShowLicenseHistory frm = new ShowLicenseHistory(license.DriverInfo.PersonInfo.PersonID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterColumn = "";
            switch (cbFilterBy.Text)
            {
                case "International License ID":
                    filterColumn = "InternationalLicenseID";
                    break;
                case "Application ID":
                    filterColumn = "ApplicationID";
                    break;

                case "Driver ID":
                    filterColumn = "DriverID";
                    break;

                case "Local License ID":
                    filterColumn = "IssuedUsingLocalLicenseID";
                    break;

                case "Is Active":
                    filterColumn = "IsActive";
                    break;


                default:
                    filterColumn = "None";
                    break;
            }
            if (txtFilterValue.Text.Trim() == "" || filterColumn == "None")
            {
                dtInternational.DefaultView.RowFilter = "";
                lblInternationalLicensesRecords.Text = dgvInternationalLicenses.Rows.Count.ToString();
                return;
            }
            dtInternational.DefaultView.RowFilter = string.Format("[{0}] = {1}", filterColumn, txtFilterValue.Text.Trim());

            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.RowCount.ToString();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None" && cbFilterBy.Text != "Is Active");
            cbIsReleased.Visible = (cbFilterBy.Text == "Is Active");
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbIsReleased.SelectedIndex)
            {
                case 0:
                    dtInternational.DefaultView.RowFilter = string.Format("[IsActive] = false");
                    break;
                case 1:
                    dtInternational.DefaultView.RowFilter = string.Format("[IsActive] = true");
                    break;
                default:
                    dtInternational.DefaultView.RowFilter = "";
                    break;
            }
            lblInternationalLicensesRecords.Text = dgvInternationalLicenses.RowCount.ToString();
        }
    }
}
