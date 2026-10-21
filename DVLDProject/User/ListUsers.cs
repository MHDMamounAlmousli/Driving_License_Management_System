using System;
using System.Data;
using System.Windows.Forms;
using BuisnessLayer;
using DVLDProject.GlobalClasses;
namespace DVLDProject.User
{
    public partial class ListUsers : Form
    {
        private DataTable dtUsers ;
        private void RefreshListUsers()
        {
            dtUsers = clsUser.GetAllUsers();
            dataGridView1.DataSource = dtUsers;
            label4.Text = dtUsers.Rows.Count.ToString();
        }
        public ListUsers()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ListUsers_Load(object sender, EventArgs e)
        {
            RefreshListUsers();
            cbFilter.SelectedIndex = 0;
            cbFIlterActive.SelectedIndex = 2;
            if(dataGridView1.Rows.Count >0)
            {
                dataGridView1.Columns[0].HeaderText = "User ID";
                dataGridView1.Columns[0].Width = 110;

                dataGridView1.Columns[1].HeaderText = "Person ID";
                dataGridView1.Columns[1].Width = 120;

                dataGridView1.Columns[2].HeaderText = "Full Name";
                dataGridView1.Columns[2].Width = 350;

                dataGridView1.Columns[3].HeaderText = "UserName";
                dataGridView1.Columns[3].Width = 120;

                dataGridView1.Columns[4].HeaderText = "Is Active";
                dataGridView1.Columns[4].Width = 100;
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string filterColumn = "";
            switch(cbFilter.SelectedIndex)
            {
                case 1:
                    filterColumn = "UserID";
                    break;
                case 2:
                    filterColumn = "UserName";
                    break;
                case 3:
                    filterColumn = "PersonID";
                    break;
                case 4:
                    filterColumn = "FullName";
                    break;
                case 5:
                    filterColumn = "IsActive";
                    break;
                default:
                    filterColumn = "None";
                    break;



            }
            if (string.IsNullOrEmpty(txtFilter.Text) || cbFilter.Text == "None")
            {
                dtUsers.DefaultView.RowFilter = "";
                label4.Text = dataGridView1.RowCount.ToString();
                return;
            }
            if (cbFilter.Text == "Person ID" || cbFilter.Text == "User ID")
            {
                dtUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", filterColumn, txtFilter.Text);
            }
            else
            {
                dtUsers.DefaultView.RowFilter = string.Format("[{0}] like  '%{1}%'", filterColumn, txtFilter.Text);
            }
            label4.Text = dataGridView1.RowCount.ToString();
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cbFilter.Text != "None" && cbFilter.Text != "Is Active");
            cbFIlterActive.Visible = (cbFilter.Text == "Is Active");
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilter.Text =="Person ID" || cbFilter.Text == "User ID")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }

        private void cbFIlterActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cbFIlterActive.SelectedIndex)
            {
                case 0:
                    dtUsers.DefaultView.RowFilter = string.Format("[IsActive] = false");
                    break;
                case 1:
                    dtUsers.DefaultView.RowFilter = string.Format("[IsActive] = true");
                    break;
                default:
                    dtUsers.DefaultView.RowFilter = "";
                    break;
            }
            label4.Text = dataGridView1.RowCount.ToString();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowDetails frm = new ShowDetails((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePassword frm = new ChangePassword((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddEditUser frm = new AddEditUser(-1);
            frm.ShowDialog();
            RefreshListUsers();
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEditUser frm = new AddEditUser(-1);
            frm.ShowDialog();
            RefreshListUsers();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEditUser frm = new AddEditUser((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
                RefreshListUsers();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserId = (int)dataGridView1.CurrentRow.Cells[0].Value;
            if (CurrentUser.currentuser.UserID == UserId)
            {
                MessageBox.Show("You Cannot Delete Your Own User Account!", "Delete User", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (clsUser.DeleteUser(UserId))
            {
                MessageBox.Show("User Deleted Successfully!", "Delete User", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshListUsers();
            }
            else
            {
                MessageBox.Show("Error Deleting User!", "Delete User", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Not Implemented Yet!", "Not Ready!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
