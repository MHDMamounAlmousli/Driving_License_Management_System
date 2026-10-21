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

namespace DVLDProject.Tests.TestsType
{
    public partial class ListTestsType : Form
    {
        private DataTable dtTestTypes;
        public ListTestsType()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void RefreshData()
        {
            dtTestTypes = clsTestsType.GetAllTypes();
            dataGridView1.DataSource = dtTestTypes;
            label3.Text = dtTestTypes.Rows.Count.ToString();
        }
        private void EditTestsType_Load(object sender, EventArgs e)
        {
            RefreshData();

            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[0].Width = 120;

            dataGridView1.Columns[1].HeaderText = "Title";
            dataGridView1.Columns[1].Width = 200;

            dataGridView1.Columns[2].HeaderText = "Description";
            dataGridView1.Columns[2].Width = 400;

            dataGridView1.Columns[3].HeaderText = "Fees";
            dataGridView1.Columns[3].Width = 100;
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditTestsType frm = new EditTestsType((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            RefreshData();
        }
    }
}
