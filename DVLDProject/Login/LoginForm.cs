using BuisnessLayer;
using DVLDProject.GlobalClasses;
using System;
using System.Windows.Forms;

namespace DVLDProject.Login
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
        private void RememberMe()
        {
            if(checkBox1.Checked)
            {
                Properties.Settings.Default.UserName = textBox1.Text.Trim();
                Properties.Settings.Default.Password = clsSecurity.Encrypt(textBox2.Text.Trim());
                Properties.Settings.Default.IsRememberMe = true;
            }
            else
            {
                Properties.Settings.Default.UserName = "";
                Properties.Settings.Default.Password = "";
                Properties.Settings.Default.IsRememberMe = false;
            }
            Properties.Settings.Default.Save();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            clsUser user = clsUser.FindUserByUsername_Password(textBox1.Text.Trim(), textBox2.Text.Trim());
            if (user != null)
            {
                if (!user.IsActive)
                {
                    MessageBox.Show("Your account is deactivated. Please contact admin.", "Deactivated", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CurrentUser.currentuser = user;
                RememberMe();
                Form1 frm = new Form1();
                frm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid Username or Password!", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.IsRememberMe)
            {
                textBox1.Text = Properties.Settings.Default.UserName;
                textBox2.Text = clsSecurity.Decrypt(Properties.Settings.Default.Password);
                checkBox1.Checked = true;
            }
            else
            {
                textBox1.Clear();
                textBox2.Clear();
                checkBox1.Checked = false;
            }
        }
    }
}
