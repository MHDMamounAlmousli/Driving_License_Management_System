using System;
using System.Windows.Forms;

namespace DVLDProject.Applications.Internation_License_Application
{
    public partial class ShowInternationalLicenseInfo : Form
    {
        private int internationalID = -1;
        public ShowInternationalLicenseInfo(int InternationalLicenseID)
        {
            InitializeComponent();
            this.internationalID = InternationalLicenseID;
        }

        private void ShowInternationalLicenseInfo_Load(object sender, EventArgs e)
        {
            ucInternationalLicenseInfo1.LoadInternationLicenseInfo(internationalID);
        }
    }
}
