using BuisnessLayer;
using DVLDProject.Applications;
using DVLDProject.People;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLDProject.Driver
{
    public partial class DriverList : Form
    {
        DataTable dtDrivers;
        private void RefreshData()
        {
            dtDrivers = clsDriver.getAllDrivers();
            dgvDrivers.DataSource = dtDrivers;
            lblRecordsCount.Text = dgvDrivers.RowCount.ToString();
        }
        public DriverList()
        {
            InitializeComponent();
        }

        private void DriverList_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            RefreshData();
            if (dgvDrivers.Rows.Count > 0)
            {
                dgvDrivers.Columns[0].HeaderText = "Driver ID";
                dgvDrivers.Columns[0].Width = 100;

                dgvDrivers.Columns[1].HeaderText = "Person ID";
                dgvDrivers.Columns[1].Width = 100;

                dgvDrivers.Columns[2].HeaderText = "National No.";
                dgvDrivers.Columns[2].Width = 100;

                dgvDrivers.Columns[3].HeaderText = "Full Name";
                dgvDrivers.Columns[3].Width = 280;

                dgvDrivers.Columns[4].HeaderText = "Date";
                dgvDrivers.Columns[4].Width = 160;

                dgvDrivers.Columns[5].HeaderText = "Active Licenses";
                dgvDrivers.Columns[5].Width = 120;
            }

        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string filterColumn = "";
            switch (cbFilterBy.Text)
            {
                case "Driver ID":
                    filterColumn = "DriverID";
                    break;

                case "Person ID":
                    filterColumn = "PersonID";
                    break;

                case "National No.":
                    filterColumn = "NationalNo";
                    break;


                case "Full Name":
                    filterColumn = "FullName";
                    break;

                default:
                    filterColumn = "None";
                    break;

            }
            if(txtFilterValue.Text.Trim() == "" || filterColumn == "None")
            {
                dtDrivers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvDrivers.Rows.Count.ToString();
                return;
            }
            if (filterColumn != "FullName" && filterColumn != "NationalNo")
                dtDrivers.DefaultView.RowFilter = string.Format("[{0}] = {1}", filterColumn, txtFilterValue.Text.Trim());
            else
                dtDrivers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", filterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = dgvDrivers.RowCount.ToString();
        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPersonInfo frm = new ShowPersonInfo((int)dgvDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Driver ID" || cbFilterBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");
            txtFilterValue.Text = "";
            txtFilterValue.Focus();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLicenseHistory frm = new ShowLicenseHistory((int)dgvDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
        }

        private void issueInternationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
