using BuisnessLayer;
using DVLDProject.Properties;
using System;
using System.Windows.Forms;

namespace DVLDProject.License.International_License
{
    public partial class ucInternationalLicenseInfo : UserControl
    {

        public ucInternationalLicenseInfo()
        {
            InitializeComponent();
        }
        public void LoadInternationLicenseInfo(int internationalid)
        {
            clsInternationalLicense License = clsInternationalLicense.Find(internationalid);
            if(License != null)
            {
                lblFullName.Text = License.DriverInfo.PersonInfo.FullName;
                lblInternationalLicenseID.Text = License.InternationalLicenseID.ToString();
                lblLocalLicenseID.Text = License.LicenseID.ToString();
                lblNationalNo.Text = License.DriverInfo.PersonInfo.NationalNo;
                lblGendor.Text = License.DriverInfo.PersonInfo.Gendor == 0 ? "Male" : "Female";
                lblIssueDate.Text = License.IssueDate.ToShortDateString();
                lblApplicationID.Text = License.ApplicationID.ToString();
                lblIsActive.Text = License.IsActive ? "Yes" : "No";
                lblDateOfBirth.Text = License.DriverInfo.PersonInfo.DateOfBirth.ToShortDateString();
                lblDriverID.Text = License.DriverID.ToString();
                lblExpirationDate.Text = License.ExpirationDate.ToShortDateString();
                pbGendor.Image = License.DriverInfo.PersonInfo.Gendor == 0 ? Resources.Man_32 : Resources.Woman_32;
                if(!string.IsNullOrEmpty(License.DriverInfo.PersonInfo.ImagePath))
                pbPersonImage.ImageLocation = License.DriverInfo.PersonInfo.ImagePath;
                else
                pbPersonImage.Image = License.DriverInfo.PersonInfo.Gendor == 0 ? Resources.Male_512: Resources.Woman_32;
            }
        }
        public void ResetDate()
        {
            lblFullName.Text = "[????]";
            lblInternationalLicenseID.Text = "[????]";
            lblLocalLicenseID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblGendor.Text = "[????]";
            lblIssueDate.Text = "[????]";
            lblApplicationID.Text = "[????]";
            lblIsActive.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblDriverID.Text = "[????]";
            lblExpirationDate.Text = "[????]";
            pbGendor.Image = Resources.Man_32;
            pbPersonImage.Image = Resources.Male_512;
        }

    }
}
