using BuisnessLayer;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace DVLDProject.User
{
    public partial class ChangePassword : Form
    {
        private int userID;
        private clsUser user;
        public ChangePassword(int UserID)
        {
            InitializeComponent();
            userID = UserID;
            user = clsUser.Find(userID);
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChangePassword_Load(object sender, EventArgs e)
        {
            if (user == null)
            {
                MessageBox.Show("User not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            ucUserCard1.LoadInfo(userID);
        }

        //private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
        //    {
        //        e.Handled = true;
        //    }
        //}

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(currentPass.Text))
            {
                errorProvider1.SetError(currentPass, "Fill the Field with your Password");
            }
            else if (currentPass.Text.Trim() != user.Password.Trim())
            {
                errorProvider1.SetError(currentPass, "Curront Password Is Wrong");
            }
            else
            {
                errorProvider1.SetError(currentPass, "");
            }
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "Fill the Field with your New Password");
            }
            else
            {
                errorProvider1.SetError(textBox2, "");
            }
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                errorProvider1.SetError(textBox3, "Please confirm your password");
            }
            else if (textBox3.Text.Trim() != textBox2.Text.Trim())
            {
                errorProvider1.SetError(textBox3, "Password does not match the New Password");
            }
            else
            {
                errorProvider1.SetError(textBox3, "");
            }
        }
        private void SavePassword()
        {
            if (clsUser.ChangePassword(userID, textBox2.Text))
            {
                MessageBox.Show("Password Changed Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed to change password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                MessageBox.Show("Make sure handle the Errors", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SavePassword();
        }
    }
}
