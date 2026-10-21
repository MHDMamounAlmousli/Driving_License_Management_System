using DVLDProject.GlobalClasses;
using System;
using System.Windows.Forms;

namespace DVLDProject.Account_Settings
{
    public partial class CurrentUserInfo : Form
    {
        public CurrentUserInfo()
        {
            InitializeComponent();
        }

        private void CurrentUserInfo_Load(object sender, EventArgs e)
        {
            ucUserCard1.LoadInfo(CurrentUser.currentuser.UserID);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
