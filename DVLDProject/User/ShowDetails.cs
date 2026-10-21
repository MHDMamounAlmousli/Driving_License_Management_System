using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDProject.User
{
    public partial class ShowDetails : Form
    {
        private int UserID;
        public ShowDetails(int UserID)
        {
            InitializeComponent();
            this.UserID = UserID;
        }

        private void ShowDetails_Load(object sender, EventArgs e)
        {
            ucUserCard1.LoadInfo(UserID);
        }
    }
}
