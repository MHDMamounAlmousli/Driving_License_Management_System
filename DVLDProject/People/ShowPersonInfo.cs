using System;
using System.Windows.Forms;

namespace DVLDProject.People
{
    public partial class ShowPersonInfo : Form
    {
        private int _PersonID = -1;
        public ShowPersonInfo(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ShowPersonInfo_Load(object sender, EventArgs e)
        {
            uctrPersonCard1.LoadPersonInfo(_PersonID);
        }
    }
}
