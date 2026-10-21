using BuisnessLayer;
using DVLDProject.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLDProject.Applications.Controls
{
    public partial class ApplicationBasicInfo : UserControl
    {
        private clsApplication _ApplicationInfo;
        public ApplicationBasicInfo()
        {
            InitializeComponent();
        }
        public void ResetApplicationInfo()
        {
            lblApplicationID.Text = "[????]";
            lblStatus.Text = "[????]";
            lblType.Text = "[????]";
            lblFees.Text = "[????]";
            lblApplicant.Text = "[????]";
            lblDate.Text = "[????]";
            lblStatusDate.Text = "[????]";
            lblCreatedByUser.Text = "[????]";
            llViewPersonInfo.Enabled = false;
            llViewPersonInfo.DisabledLinkColor = Color.Gray;
        }
        public void LoadApplicationInfo(int applicationID)
        {
            _ApplicationInfo = clsApplication.Find(applicationID);
            if (_ApplicationInfo == null)
            {
                ResetApplicationInfo();
                MessageBox.Show("No Application with ApplicationID = " + _ApplicationInfo.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FillApplicationInfo();
        }
        private void FillApplicationInfo()
        {
                lblApplicationID.Text = _ApplicationInfo.ApplicationID.ToString();
                lblStatus.Text = _ApplicationInfo.ApplicationStatus.ToString();
                lblFees.Text = _ApplicationInfo.Fees.ToString();
                lblType.Text = _ApplicationInfo.ApplicationTypeInfo.ApplicationTitle;
                lblApplicant.Text = _ApplicationInfo.PersonInfo.FullName;
                lblDate.Text = _ApplicationInfo.ApplicationDate.ToShortDateString();
                lblStatusDate.Text = _ApplicationInfo.LastStatusDate.ToShortDateString();
                lblCreatedByUser.Text = _ApplicationInfo.CreatedByUserInfo.UserName;
                llViewPersonInfo.ActiveLinkColor = Color.Blue;
                llViewPersonInfo.Enabled = true;
        }

        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowPersonInfo frm = new ShowPersonInfo(_ApplicationInfo.ApplicantPersonID);
            frm.ShowDialog();
            FillApplicationInfo();
        }
    }
}
