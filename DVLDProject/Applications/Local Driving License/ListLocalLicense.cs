using BuisnessLayer;
using DVLDProject.License;
using DVLDProject.Tests;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLDProject.Applications.Local_Driving_License
{
    public partial class ListLocalLicense : Form
    {
        private DataTable dtLoclaLicense;
        private void RefreshData()
        {
            dtLoclaLicense = clsLocalDrivingLicense.GetAllLocalInfo();
            dgvLocalDrivingLicenseApplications.DataSource = dtLoclaLicense;
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
        }

        public ListLocalLicense()
        {
            InitializeComponent();
        }
        private bool HandleRecordOptions()
        {
            bool EnableTest = true;
            showApplicationDetailsToolStripMenuItem.Enabled = false;
            editApplicationToolStripMenuItem.Enabled = false;
            cancelApplicationToolStripMenuItem.Enabled = false;
            deleteApplicationToolStripMenuItem.Enabled = false;
            schduleTestsToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;
            showPersonLicenseHistoryToolStripMenuItem.Enabled = false;
            int localDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicense localDrivingLicense = clsLocalDrivingLicense.FindLocalDrivingLicense(localDrivingLicenseApplicationID);

            switch (localDrivingLicense.ApplicationStatus)
            {
                case clsApplication.enStatus.New:
                    showApplicationDetailsToolStripMenuItem.Enabled = true;
                    editApplicationToolStripMenuItem.Enabled = true;
                    cancelApplicationToolStripMenuItem.Enabled = true;
                    deleteApplicationToolStripMenuItem.Enabled = true;
                    schduleTestsToolStripMenuItem.Enabled = true;
                    showPersonLicenseHistoryToolStripMenuItem.Enabled = true;
                    break;

                case clsApplication.enStatus.Cancel:
                    showApplicationDetailsToolStripMenuItem.Enabled = true;
                    issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                    showPersonLicenseHistoryToolStripMenuItem.Enabled = true;
                    EnableTest = false;
                    break;

                case clsApplication.enStatus.Completed:
                    showApplicationDetailsToolStripMenuItem.Enabled = true;
                    showLicenseToolStripMenuItem.Enabled = true;
                    showPersonLicenseHistoryToolStripMenuItem.Enabled = true;
                    issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                    break;
            }
            return EnableTest;
        }
        private bool HandleTestTypes()
        {
            bool Enable = true;
            int localDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            int passedTests = clsLocalDrivingLicense.getAllPassedTest(localDrivingLicenseApplicationID);
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            visionTestToolStripMenuItem.Enabled = false;
            scheduleWrittenTestToolStripMenuItem.Enabled = false;
            scheduleStreetTestToolStripMenuItem.Enabled = false;
            if (passedTests == 0)
            {
                visionTestToolStripMenuItem.Enabled = true;
            }
            else if (passedTests == 1)
            {
                scheduleWrittenTestToolStripMenuItem.Enabled = true;
            }
            else if (passedTests == 2)
            {
                scheduleStreetTestToolStripMenuItem.Enabled = true;
            }
            else 
            {
                issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
                Enable = false;
            }
            return Enable;
        }
        private void dgvLocalDrivingLicenseApplications_SelectionChanged(object sender, EventArgs e)
        {
            bool flag = HandleTestTypes();
            bool flag1 = HandleRecordOptions();
            schduleTestsToolStripMenuItem.Enabled = flag && flag1;
        }
        private void Filter()
        {
            string FilterColumn = "";
            switch (cbFilterBy.SelectedIndex)
            {
                case 0:
                    FilterColumn = "None";
                    break;
                case 1:
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;
                case 2:
                    FilterColumn = "NationalNo";
                    break;
                case 3:
                    FilterColumn = "FullName";
                    break;
                case 4:
                    FilterColumn = "Status";
                    break;
                default: FilterColumn = "None"; break;
            }
            if (string.IsNullOrEmpty(txtFilterValue.Text) || cbFilterBy.Text == "None")
            {
                dtLoclaLicense.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.RowCount.ToString();
                return;
            }
            if(cbFilterBy.Text == "L.D.L.AppID" || cbFilterBy.Text == "National No.")
            {
                dtLoclaLicense.DefaultView.RowFilter = string.Format("[{0}] = {1}",FilterColumn,txtFilterValue.Text);
            }
            else
            {
                dtLoclaLicense.DefaultView.RowFilter = string.Format("[{0}]  like '%{1}%'", FilterColumn, txtFilterValue.Text);
            }
            lblRecordsCount.Text = dgvLocalDrivingLicenseApplications.Rows.Count.ToString();
            
        }
        private void ListLocalLicense_Load(object sender, EventArgs e)
        {
            RefreshData();
            cbFilterBy.SelectedIndex = 0;
            if (dgvLocalDrivingLicenseApplications.Rows.Count > 0)
            {

                dgvLocalDrivingLicenseApplications.Columns[0].HeaderText = "L.D.L.AppID";
                dgvLocalDrivingLicenseApplications.Columns[0].Width = 120;

                dgvLocalDrivingLicenseApplications.Columns[1].HeaderText = "Driving Class";
                dgvLocalDrivingLicenseApplications.Columns[1].Width = 220;

                dgvLocalDrivingLicenseApplications.Columns[2].HeaderText = "National No.";
                dgvLocalDrivingLicenseApplications.Columns[2].Width = 120;

                dgvLocalDrivingLicenseApplications.Columns[3].HeaderText = "Full Name";
                dgvLocalDrivingLicenseApplications.Columns[3].Width = 350;

                dgvLocalDrivingLicenseApplications.Columns[4].HeaderText = "Application Date";
                dgvLocalDrivingLicenseApplications.Columns[4].Width = 170;

                dgvLocalDrivingLicenseApplications.Columns[5].HeaderText = "Passed Tests";
                dgvLocalDrivingLicenseApplications.Columns[5].Width = 110;
            }

            cbFilterBy.SelectedIndex = 0;
        }


        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            Filter();
            
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            addEditLocalLicense frm = new addEditLocalLicense(-1);
            frm.ShowDialog();
            RefreshData();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "National No." || cbFilterBy.Text == "L.D.L.AppID")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addEditLocalLicense frm = new addEditLocalLicense((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            RefreshData();
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel this application?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            int localDrivingLicenseApplicationID = (int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicense localDrivingLicense = clsLocalDrivingLicense.FindLocalDrivingLicense(localDrivingLicenseApplicationID);

            if(localDrivingLicense.ApplicationStatus == clsApplication.enStatus.Cancel)
            {
                MessageBox.Show("Already Canceled", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (localDrivingLicense.UpdateStatus(clsApplication.enStatus.Cancel))
            {
                MessageBox.Show("Application Cancelled Successfully.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshData();
            }
            else
            {
                MessageBox.Show("Error: Could not cancel application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLocalLicenseInfo frm = new ShowLocalLicenseInfo((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            RefreshData();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Delete this application?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }
           clsLocalDrivingLicense localDrivingLicense = clsLocalDrivingLicense.FindLocalDrivingLicense((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            if (localDrivingLicense.Delete())
            {
                MessageBox.Show("Application Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshData();
            }
            else
            {
                MessageBox.Show("Error: Could not delete application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void visionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenTestAppointments frm = new ScreenTestAppointments((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value, clsTestsType.enTestType.VisionTest);
            frm.ShowDialog();
            RefreshData();
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenTestAppointments frm = new ScreenTestAppointments((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value, clsTestsType.enTestType.WrittenTest);
            frm.ShowDialog();
            RefreshData();
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenTestAppointments frm = new ScreenTestAppointments((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value, clsTestsType.enTestType.StreetTest);
            frm.ShowDialog();
            RefreshData();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLicesenInfo frm = new ShowLicesenInfo(clsLocalDrivingLicense.getActiveLicenseID((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value));
            frm.ShowDialog();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IssueDriverLicenseFT frm = new IssueDriverLicenseFT((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            RefreshData();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicense local = clsLocalDrivingLicense.FindLocalDrivingLicense((int)dgvLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            ShowLicenseHistory frm = new ShowLicenseHistory(local.ApplicantPersonID);
            frm.ShowDialog();
        }
    }
}
