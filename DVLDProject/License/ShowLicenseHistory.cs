using BuisnessLayer;
using System;
using System.Windows.Forms;

namespace DVLDProject.Applications
{
    public partial class ShowLicenseHistory : Form
    {
        int PersonID;
        public ShowLicenseHistory(int PersonID)
        {
            InitializeComponent();
            this.PersonID = PersonID;
        }

        private void ShowLicenseHistory_Load(object sender, EventArgs e)
        {
            personCardWithFilters1.LoadPersonInfo(PersonID);
            personCardWithFilters1.FilterEnabled = false;
            ucDriverLicenseHistory1.LoadLocalLicenseInfoByPersonID(PersonID);
            ucDriverLicenseHistory1.LoadInternationsalLicenseInfoByPersonID(PersonID);
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();

        private void ucDriverLicenseHistory1_Load(object sender, EventArgs e)
        {

        }
    }
}
