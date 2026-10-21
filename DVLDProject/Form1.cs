using BuisnessLayer;
using DVLDProject.Account_Settings;
using DVLDProject.Application.ApplicationType;
using DVLDProject.Applications.Detain_License_Application;
using DVLDProject.Applications.Internation_License_Application;
using DVLDProject.Applications.Local_Driving_License;
using DVLDProject.Applications.ReNew_License_Application;
using DVLDProject.Applications.Replacement_for_lost_or_damaged_application;
using DVLDProject.Driver;
using DVLDProject.GlobalClasses;
using DVLDProject.License.International_License;
using DVLDProject.Login;
using DVLDProject.Tests.TestsType;
using DVLDProject.User;
using System;
using System.Windows.Forms;

namespace DVLDProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {


        }

        private void People_Click(object sender, EventArgs e)
        {
            ListPeople frm = new ListPeople();
            frm.ShowDialog();
        }

        private void Users_Click(object sender, EventArgs e)
        {
            ListUsers frm = new ListUsers();
            frm.ShowDialog();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentUserInfo frm = new CurrentUserInfo();
            frm.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePassword frm = new ChangePassword(CurrentUser.currentuser.UserID);
            frm.ShowDialog();
        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentUser.currentuser = null;
            this.Close();
            LoginForm frm = new LoginForm();
            frm.Show();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (CurrentUser.currentuser != null)
                System.Windows.Forms.Application.Exit();
        }

        private void ManageApplicationTypes_Click(object sender, EventArgs e)
        {
            ListApplicationTypes frm = new ListApplicationTypes();
            frm.ShowDialog();
        }

        private void ManageTestTypes_Click(object sender, EventArgs e)
        {
            ListTestsType fr = new ListTestsType();
            fr.ShowDialog();
        }

        private void localLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addEditLocalLicense frm = new addEditLocalLicense(-1);
            frm.ShowDialog();

        }

        private void localDrivingLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListLocalLicense frm = new ListLocalLicense();
            frm.ShowDialog();
        }

        private void Drivers_Click(object sender, EventArgs e)
        {
            DriverList frm = new DriverList();
            frm.ShowDialog();
        }

        private void internationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IssueInternationalLicense frm = new IssueInternationalLicense();
            frm.ShowDialog();
        }

        private void internationalLicenseApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListInternationalLicenses frm = new ListInternationalLicenses();
            frm.ShowDialog();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenewLicenseApplication frm = new RenewLicenseApplication();
            frm.ShowDialog();
        }

        private void replacementForLostOrDamagedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReplacementorDamageDLicense frm = new ReplacementorDamageDLicense();
            frm.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DetainLicense frm = new DetainLicense();
            frm.ShowDialog();
        }

        private void manageDetainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListDetainLicenses frm = new ListDetainLicenses();
            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReleaseDetaindedLicense frm = new ReleaseDetaindedLicense();
            frm.ShowDialog();
        }

        private void retakeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListLocalLicense frm = new ListLocalLicense();
            frm.ShowDialog();
        }

        private void releaseDetainedDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReleaseDetaindedLicense frm = new ReleaseDetaindedLicense();
            frm.ShowDialog();
        }
    }
}
