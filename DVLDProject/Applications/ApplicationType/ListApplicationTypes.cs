using System;

using System.Data;

using System.Windows.Forms;
using BuisnessLayer;
namespace DVLDProject.Application.ApplicationType
{
    public partial class ListApplicationTypes : Form
    {
        private DataTable dtTypes ;
        public ListApplicationTypes()
        {
            InitializeComponent();
        }
        private void RefreshData()
        {
            dtTypes = clsApplicationType.GetAllTypes();
            dataGridView1.DataSource = dtTypes;
            label3.Text = dtTypes.Rows.Count.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ManageApplicationTypes_Load(object sender, EventArgs e)
        {
            RefreshData();
            if(dtTypes.Rows.Count >0)
            {
                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[0].Width = 110;

                dataGridView1.Columns[1].HeaderText = "Title";
                dataGridView1.Columns[1].Width = 400;

                dataGridView1.Columns[2].HeaderText = "Fees";
                dataGridView1.Columns[2].Width = 100;
            }
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateApplicationType frm = new UpdateApplicationType((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            RefreshData();
        }
    }
}
