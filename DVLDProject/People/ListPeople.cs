using System;
using System.Data;
using System.Windows.Forms;
using BuisnessLayer;
using DVLDProject.People;
using Microsoft.SqlServer.Server;
namespace DVLDProject
{
    public partial class ListPeople : Form
    {
        private DataTable _dtPeople;
        public ListPeople()
        {
            InitializeComponent();
        }
        private void RefreshListInfo()
        {
            DataTable _dtAllPeople = clsPerson.GetAllPeople();
             _dtPeople = _dtAllPeople.DefaultView.ToTable(false, "PersonID", "NationalNo",
                                                      "FirstName", "SecondName", "ThirdName", "LastName",
                                                      "Gendor", "DateOfBirth", "CountryName",
                                                      "Phone", "Email");
            dataGridView1.DataSource = _dtPeople;
            label4.Text = (dataGridView1.RowCount).ToString();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowPersonInfo frm = new ShowPersonInfo((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            RefreshListInfo();
        }

        private void ListPeople_Load(object sender, EventArgs e)
        {
            RefreshListInfo();
            if(dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns[0].HeaderText = "Person ID";
                dataGridView1.Columns[0].Width = 110;

                dataGridView1.Columns[1].HeaderText = "National No.";
                dataGridView1.Columns[1].Width = 120;

                dataGridView1.Columns[2].HeaderText = "First Name";
                dataGridView1.Columns[2].Width = 120;

                dataGridView1.Columns[3].HeaderText = "Second Name";
                dataGridView1.Columns[3].Width = 120;

                dataGridView1.Columns[4].HeaderText = "Third Name";
                dataGridView1.Columns[4].Width = 120;

                dataGridView1.Columns[5].HeaderText = "Last Name";
                dataGridView1.Columns[5].Width = 120;

                dataGridView1.Columns[6].HeaderText = "Gendor";
                dataGridView1.Columns[6].Width = 120;

                dataGridView1.Columns[7].HeaderText = "Date Of Birth";
                dataGridView1.Columns[7].Width = 140;

                dataGridView1.Columns[9].HeaderText = "Phone";
                dataGridView1.Columns[9].Width = 120;


                dataGridView1.Columns[10].HeaderText = "Email";
                dataGridView1.Columns[10].Width = 170;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btAddPeople_Click(object sender, EventArgs e)
        {
            AddNewPerson frm = new AddNewPerson(-1);
            frm.ShowDialog();
            RefreshListInfo();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewPerson frm = new AddNewPerson(-1);
            frm.ShowDialog();
            RefreshListInfo();
        }

        private void editPersonRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
                return;
            if (dataGridView1.CurrentRow.Cells["PersonID"].Value == null)
                return;
            int PersonID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["PersonID"].Value);
            AddNewPerson frm = new AddNewPerson(PersonID);
            frm.ShowDialog();
            RefreshListInfo();
        }

        private void deletePersonRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you wann delete Person [" + dataGridView1.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (clsPerson.DeletePerson((int)dataGridView1.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Person Deleted Successfully", "Delete Person", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshListInfo();
                }
                else
                {
                    MessageBox.Show("Error Deleting Person", "Delete Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPersonInfo frm = new ShowPersonInfo((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
        private void filterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Visible = (filterType.Text != "None");

            textBox1.Text = "";
            if (_dtPeople != null)
            {
                _dtPeople.DefaultView.RowFilter = ""; 
                label4.Text = dataGridView1.RowCount.ToString();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (filterType.Text == "Person ID")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (_dtPeople == null) return;

            string filterColumn = "";
            switch (filterType.Text)
            {
                case "Person ID":
                    filterColumn = "PersonID";
                    break;
                case "National No":
                    filterColumn = "NationalNo";
                    break;
                case "First Name":
                    filterColumn = "FirstName";
                    break;
                case "Second Name":
                    filterColumn = "SecondName";
                    break;
                case "Third Name":
                    filterColumn = "ThirdName";
                    break;
                case "Last Name":
                    filterColumn = "LastName";
                    break;
                case "Nationality":
                    filterColumn = "CountryName";
                    break;
                case "Gendor":
                    filterColumn = "Gendor";
                    break;
                case "Phone":
                    filterColumn = "Phone";
                    break;
                case "Email":
                    filterColumn = "Email";
                    break;
                default:
                    filterColumn = "None";
                    break;
            }
            if (string.IsNullOrEmpty(textBox1.Text) || filterColumn == "None")
            {
                _dtPeople.DefaultView.RowFilter = "";
                label4.Text = dataGridView1.RowCount.ToString();
                return;
            }
            if (filterColumn == "PersonID")
            {
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}", filterColumn, textBox1.Text);
            }
            else
            {
                _dtPeople.DefaultView.RowFilter = string.Format("[{0}] like '%{1}%'", filterColumn, textBox1.Text);
            }


            label4.Text = dataGridView1.RowCount.ToString();
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
    }
}
