using BuisnessLayer;
using DVLDProject.License;
using DVLDProject.People;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLDProject.Applications.Detain_License_Application
{
    public partial class ListDetainLicenses : Form
    {
        private DataTable dtDetainLicenses;
        private void RefreshData()
        {
            dtDetainLicenses = clsDetainLicense.GetAllDetainse();
            dgvDetainedLicenses.DataSource = dtDetainLicenses;
            lblTotalRecords.Text = dgvDetainedLicenses.RowCount.ToString();
        }
        public ListDetainLicenses()
        {
            InitializeComponent();
        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            ReleaseDetaindedLicense frm = new ReleaseDetaindedLicense();
            frm.ShowDialog();
            RefreshData();
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            DetainLicense frm = new DetainLicense();
            frm.ShowDialog();
            RefreshData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch (cbFilterBy.Text)
            {
                case "Detain ID":
                    FilterColumn = "DetainID";
                    break;
                case "Is Released":
                        FilterColumn = "IsReleased";
                        break;
                case "National No.":
                    FilterColumn = "NationalNo";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Release Application ID":
                    FilterColumn = "ReleaseApplicationID";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                dtDetainLicenses.DefaultView.RowFilter = "";
                lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
                return;
            }
            if (FilterColumn == "DetainID" || FilterColumn == "ReleaseApplicationID")
                dtDetainLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                dtDetainLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '%{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblTotalRecords.Text = dgvDetainedLicenses.RowCount.ToString();
        }

        private void ListDetainLicenses_Load(object sender, EventArgs e)
        {
            RefreshData();
            cbFilterBy.SelectedIndex = 0;
            cbIsReleased.SelectedIndex = 2;
            if (dgvDetainedLicenses.Rows.Count > 0)
            {
                dgvDetainedLicenses.Columns[0].HeaderText = "D.ID";
                dgvDetainedLicenses.Columns[0].Width = 80;

                dgvDetainedLicenses.Columns[1].HeaderText = "L.ID";
                dgvDetainedLicenses.Columns[1].Width = 80;

                dgvDetainedLicenses.Columns[2].HeaderText = "D.Date";
                dgvDetainedLicenses.Columns[2].Width = 130;

                dgvDetainedLicenses.Columns[3].HeaderText = "Is Released";
                dgvDetainedLicenses.Columns[3].Width = 80;

                dgvDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                dgvDetainedLicenses.Columns[4].Width = 80;

                dgvDetainedLicenses.Columns[5].HeaderText = "Release Date";
                dgvDetainedLicenses.Columns[5].Width = 130;

                dgvDetainedLicenses.Columns[6].HeaderText = "N.No.";
                dgvDetainedLicenses.Columns[6].Width = 80;

                dgvDetainedLicenses.Columns[7].HeaderText = "Full Name";
                dgvDetainedLicenses.Columns[7].Width = 200;

                dgvDetainedLicenses.Columns[8].HeaderText = "Rlease App.ID";
                dgvDetainedLicenses.Columns[8].Width = 80;

            }
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Detain ID" || cbFilterBy.Text == "Release Application ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLicense license = clsLicense.Find((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value);
            ShowPersonInfo frm = new ShowPersonInfo(license.ApplicationInfo.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLicesenInfo frm = new ShowLicesenInfo(((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value));
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLicense license = clsLicense.Find((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value);
            ShowLicenseHistory frm = new ShowLicenseHistory(license.ApplicationInfo.ApplicantPersonID);
            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReleaseDetaindedLicense frm = new ReleaseDetaindedLicense((int)dgvDetainedLicenses.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            switch(cbIsReleased.SelectedIndex)
            {
                case 0:
                    dtDetainLicenses.DefaultView.RowFilter = string.Format("[IsReleased] = false");
                    break;
                case 1:
                    dtDetainLicenses.DefaultView.RowFilter = string.Format("[IsReleased] = true");
                    break;
                default:
                    dtDetainLicenses.DefaultView.RowFilter = "";
                    break;
            }
            lblTotalRecords.Text = dgvDetainedLicenses.RowCount.ToString();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "Is Released" && cbFilterBy.Text != "None");
            cbIsReleased.Visible = cbFilterBy.Text == "Is Released";
        }

        private void dgvDetainedLicenses_SelectionChanged(object sender, EventArgs e)
        {
            bool isReleased = Convert.ToBoolean(dgvDetainedLicenses.CurrentRow.Cells[3].Value);
            releaseDetainedLicenseToolStripMenuItem.Enabled = !isReleased;
        }
    }
}
