using BuisnessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLDProject.Application.ApplicationType
{
    public partial class UpdateApplicationType : Form
    {
        int typeid = -1;
        private clsApplicationType Type;
        public UpdateApplicationType(int id)
        {
            InitializeComponent();
            typeid = id;
        }
        private void LoadData()
        {
            Type = clsApplicationType.Find((clsApplicationType.enApplicationType)typeid);
            if (Type == null)
                return;
            label5.Text = ((int)Type.ApplicationTypeID).ToString();
            textBox1.Text = Type.ApplicationTitle.ToString();
            textBox2.Text = Type.Fees.ToString();
        }
        private void UpdateApplicationType_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Type.ApplicationTitle = textBox1.Text;
            Type.Fees = Convert.ToInt16(textBox2.Text);
            if(Type.Save())
            {
                MessageBox.Show("Saved Successfully","Saved",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error in Saving", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
