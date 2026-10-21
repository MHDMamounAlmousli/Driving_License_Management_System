using System;
using System.Windows.Forms;
namespace DVLDProject.License
{
    public partial class ShowLicesenInfo : Form
    {
        private int LicenseID = -1;
        public ShowLicesenInfo(int LicenseID)
        {
            InitializeComponent();
            this.LicenseID = LicenseID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowLicesenInfo_Load(object sender, EventArgs e)
        {
            ucDriverLicense1.LoadLicenseInfo(LicenseID);
        }
    }
}
