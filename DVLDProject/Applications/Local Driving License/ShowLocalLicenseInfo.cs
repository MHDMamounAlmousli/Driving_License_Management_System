using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDProject.Applications.Local_Driving_License
{
    public partial class ShowLocalLicenseInfo : Form
    {
        private int LocalLicenseID;
        public ShowLocalLicenseInfo(int localLicenseID)
        {
            InitializeComponent();
            LocalLicenseID = localLicenseID;
        }

        private void ShowLocalLicenseInfo_Load(object sender, EventArgs e)
        {
            ucDrivingLicenseInfo1.LoadDrivingLicenseInfo(LocalLicenseID);
        }
    }
}
