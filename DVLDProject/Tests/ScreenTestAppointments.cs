using BuisnessLayer;
using DVLDProject.Properties;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLDProject.Tests
{
    public partial class ScreenTestAppointments : Form
    {
        private DataTable dtLicenseTestAppointments;
        private int LocalLicenseID;
        private clsTestsType.enTestType TestType = clsTestsType.enTestType.VisionTest;

        public ScreenTestAppointments(int locallicense,clsTestsType.enTestType testType)
        {
            InitializeComponent();
            LocalLicenseID = locallicense;
            TestType = testType;
        }
        private void ChangingScreenPattern()
        {
            switch(TestType)
            {
                case clsTestsType.enTestType.VisionTest:
                    {
                        this.Name = "Vision Test Appointment";
                        pbTestTypeImage.Image = Resources.Vision_512;
                        lblTitle.Text = "Vision Test Appointments";
                        break;
                    }
                case clsTestsType.enTestType.WrittenTest:
                    {
                        this.Name = "Written Test Appointment";
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        lblTitle.Text = "Written Test Appointments";
                        break;
                    }
                case clsTestsType.enTestType.StreetTest:
                    {
                        this.Name = "Street Test Appointment";
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        lblTitle.Text = "Street Test Appointments";
                        break;
                    }
            }
        }

        private void ScreenTestAppointments_Load(object sender, EventArgs e)
        {
            ChangingScreenPattern();
            ucDrivingLicenseInfo1.LoadDrivingLicenseInfo(LocalLicenseID);
            dtLicenseTestAppointments = clsTestAppointment.GetAppointmentPerTestType(LocalLicenseID, TestType);
            dgvLicenseTestAppointments.DataSource = dtLicenseTestAppointments;
            lblRecordsCount.Text = dgvLicenseTestAppointments.RowCount.ToString();
            if (dgvLicenseTestAppointments.Rows.Count > 0)
            {
                dgvLicenseTestAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvLicenseTestAppointments.Columns[0].Width = 150;

                dgvLicenseTestAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvLicenseTestAppointments.Columns[1].Width = 200;

                dgvLicenseTestAppointments.Columns[2].HeaderText = "Paid Fees";
                dgvLicenseTestAppointments.Columns[2].Width = 150;

                dgvLicenseTestAppointments.Columns[3].HeaderText = "Is Locked";
                dgvLicenseTestAppointments.Columns[3].Width = 100;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            if (clsLocalDrivingLicense.IsHasActiveAppointment(LocalLicenseID, TestType))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            clsTest lasttest = clsLocalDrivingLicense.ResultLastAppointment(LocalLicenseID, TestType);
            if (lasttest is null)
            {
                ScheduleTestType frm = new ScheduleTestType(LocalLicenseID, TestType);
                frm.ShowDialog();
                ScreenTestAppointments_Load(null, null);

                return;
            }
            if(lasttest.TestResult == clsTest.enTestResult.Pass)
            {
                MessageBox.Show("This person already passed this test before, you can only retake failed test","Not Allowed", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            ScheduleTestType frmRetake =new ScheduleTestType(LocalLicenseID, TestType);
            frmRetake.ShowDialog();
            ScreenTestAppointments_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScheduleTestType frm = new ScheduleTestType(LocalLicenseID, TestType, (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            ScreenTestAppointments_Load(null, null);
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScreenTakeTest frm = new ScreenTakeTest(LocalLicenseID, (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value, TestType);
            frm.ShowDialog();
            ScreenTestAppointments_Load(null, null);
        }
    }
}
