using BuisnessLayer;
using DVLDProject.GlobalClasses;
using System;
using System.Data;
using System.Windows.Forms;

namespace DVLDProject.Applications.Local_Driving_License
{
    public partial class addEditLocalLicense : Form
    {
        private enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;
        private int LocalDrivingID = -1;
        private clsLocalDrivingLicense localLicense;
        private int selectedperson = -1;
        public addEditLocalLicense(int locallicense)
        {
            InitializeComponent();
            if(locallicense == -1)
            {
                _Mode = enMode.AddNew;
                this.Text = "AddNew Application"; 
            }
            else
            {
                LocalDrivingID = locallicense;
                _Mode = enMode.Update;
                this.Text = "Edit";
            }
        }
        private void FillLicenseType()
        {
            DataTable dt = clsLicenseClass.GetAllLicenseClass();
            foreach (DataRow row in dt.Rows)
                cbLicenseClass.Items.Add(row["ClassName"].ToString());
        }
        private void RefreshData()
        {
            FillLicenseType();
            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                lblCreatedByUser.Text = CurrentUser.currentuser.UserName;
                lblFees.Text = clsApplicationType.Find(clsApplicationType.enApplicationType.NewDrivingLicense).Fees.ToString();
                button2.Enabled = false;
                ApplicationInfo.Enabled = false;
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                cbLicenseClass.SelectedIndex = 2;
                lblLocalDrivingLicebseApplicationID.Text = "[???]";
                localLicense = new clsLocalDrivingLicense();
            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";

                ApplicationInfo.Enabled = true;
                button2.Enabled = true;
            }
        }
        private void LoadData()
        {
            button2.Enabled = true;
            ApplicationInfo.Enabled = true;
            localLicense = clsLocalDrivingLicense.FindLocalDrivingLicense(LocalDrivingID);
            personCardWithFilters1.FilterEnabled = false;
            if(localLicense == null)
            {
                MessageBox.Show("No LocalDivingLicense with ID = " + LocalDrivingID, "LocalDivingLicense Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }
            lblLocalDrivingLicebseApplicationID .Text = localLicense.LocalDrivingLicenseId.ToString();
            lblApplicationDate.Text = localLicense.ApplicationDate.ToString("dd / MM / yyyy");
            lblFees.Text = localLicense.Fees.ToString();
            lblCreatedByUser.Text = localLicense.CreatedByUserInfo.UserName;
            cbLicenseClass.SelectedIndex = localLicense.LicenseClassId -1 ;
            personCardWithFilters1.LoadPersonInfo(localLicense.ApplicantPersonID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int selectedLicenseClassID = Convert.ToInt32(cbLicenseClass.SelectedIndex + 1);
            if(_Mode == enMode.AddNew)
            {
                int activeApplicationID = clsLocalDrivingLicense.GetActiveApplicationID(selectedperson, 1, selectedLicenseClassID);
                if(activeApplicationID != -1)
                {
                    clsApplication activeApp = clsApplication.Find(activeApplicationID);

                    if (activeApp.ApplicationStatus == clsApplication.enStatus.New)
                    {
                        MessageBox.Show($"Choose another License Class. The selected person already has an active application for this class (ID = {activeApplicationID}).","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (activeApp.ApplicationStatus == clsApplication.enStatus.Completed)
                    {
                        MessageBox.Show($"The selected person already has a license for this class (Application ID = {activeApplicationID}).","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return; 
                }
            }
            localLicense.ApplicantPersonID = personCardWithFilters1.PersonID;
            localLicense.ApplicationTypeID = clsApplicationType.enApplicationType.NewDrivingLicense;
            localLicense.ApplicationDate = DateTime.Now;
            localLicense.LicenseClassId = cbLicenseClass.SelectedIndex + 1;
            localLicense.CreatedByUserID = CurrentUser.currentuser.UserID;
            localLicense.ApplicationStatus = clsApplication.enStatus.New;
            localLicense.LastStatusDate = DateTime.Now;
            localLicense.Fees = Convert.ToInt32(lblFees.Text);

            if(localLicense.Save())
            {
                lblLocalDrivingLicebseApplicationID.Text = localLicense.LocalDrivingLicenseId.ToString();
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void addEditLocalLicense_Load(object sender, EventArgs e)
        {

            RefreshData();
            if (_Mode == enMode.Update)
                LoadData();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update || personCardWithFilters1.PersonID != -1)
            {
                button2.Enabled = true;
                ApplicationInfo.Enabled = true;
                tabControl1.SelectedTab = tabControl1.TabPages["ApplicationInfo"];
                return;
            }
            else

            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addEditLocalLicense_Activated(object sender, EventArgs e)
        {
            personCardWithFilters1.FocusFilter();
        }

        private void personCardWithFilters1_OnPersonSelected(int obj)
        {
            selectedperson = obj;
        }
    }
}
