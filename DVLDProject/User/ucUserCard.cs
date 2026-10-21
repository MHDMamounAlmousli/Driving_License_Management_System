using BuisnessLayer;
using System;
using System.Windows.Forms;

namespace DVLDProject.User
{
    public partial class ucUserCard : UserControl
    {
        private clsUser user;
        public ucUserCard()
        {
            InitializeComponent();
        }
        private void fillUserCard()
        {
            uctrPersonCard1.LoadPersonInfo(user.PersonID);
            label4.Text = user.UserID.ToString();
            label5.Text = user.UserName;
            label6.Text = (user.IsActive) ? "Yes" : "No";
        }
        public void LoadInfo(int UserID)
        {
            user = clsUser.Find(UserID);
            if(user !=null)
            {
                fillUserCard();
            }
            else
            {
                MessageBox.Show("User " + user.UserID + "not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RefreshInfo();
            }
        }
        private void RefreshInfo()
        {
            label4.Text = "[????]";
            label5.Text = "[????]";
            label6.Text = "[????]";
        }
        private void ucUserCard_Load(object sender, EventArgs e)
        {

        }

    }
}
