using BuisnessLayer;
using DVLDProject.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLDProject.License.Controls
{
    public partial class ucDriverLicense : UserControl
    {
        private clsLicense license;
        public ucDriverLicense()
        {
            InitializeComponent();
        }
        public clsLicense SelectedLicense => license;
        public void ResetData()
        {
            lblClass.Text = "[????]";
            lblFullName.Text = "[????]";
            lblLicenseID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblGendor.Text = "[????]";
            lblIssueDate.Text = "[????]";
            lblIssueReason.Text = "[????]";
            lblNotes.Text = "[????]";
            lblIsActive.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblDriverID.Text = "[????]";
            lblExpirationDate.Text = "[????]";
            lblIsDetained.Text = "[????]";

            pbGendor.Image = Resources.Man_32;
            pbPersonImage.Image = Resources.Male_512;
        }
        public void LoadLicenseInfo(int licenseID)
        {
            license = clsLicense.Find(licenseID);
            if (license != null)
            {
                var personInfo = license.ApplicationInfo.PersonInfo;
                lblClass.Text = license.LicenseClassInfo.ClassName;
                lblFullName.Text = personInfo.FullName;
                lblLicenseID.Text = license.LicenseID.ToString();
                lblNationalNo.Text = personInfo.NationalNo;
                lblGendor.Text = personInfo.Gendor == 0? "Male":"Female";
                lblIssueDate.Text = license.IssueDate.ToShortDateString();
                lblIssueReason.Text = license.IssueReason.ToString();
                lblNotes.Text = string.IsNullOrEmpty(license.Notes)?"Nothing": license.Notes;
                lblIsActive.Text = license.IsActive ? "Yes" : "No";
                lblDateOfBirth.Text = personInfo.DateOfBirth.ToShortDateString();
                lblDriverID.Text = license.DriverID.ToString();
                lblExpirationDate.Text = license.ExpirationDate.ToShortDateString();
                lblIsDetained.Text = license.isDetained?"Yes":"No";
                pbGendor.Image = personInfo.Gendor == 0 ? Resources.Man_32 : Resources.Woman_32;
                if (!string.IsNullOrEmpty(personInfo.ImagePath))
                {
                    pbPersonImage.ImageLocation = personInfo.ImagePath;
                }
                else
                {
                    pbPersonImage.Image = personInfo.Gendor == 0 ? Resources.Male_512 : Resources.Female_512;
                }
                lblIsDetained.Text = clsDetainLicense.isDetain(license.LicenseID) ? "Yes" : "No";
            }
        }
    }
}
