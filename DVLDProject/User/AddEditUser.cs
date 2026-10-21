using BuisnessLayer;
using System.Windows.Forms;
namespace DVLDProject.User
{
    public partial class AddEditUser : Form
    {
        private enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;
        private int _UserID = -1;
        private clsUser _User;
        public AddEditUser(int UserID)
        {
            InitializeComponent();
            if (UserID == -1)
                _Mode = enMode.AddNew;
            else
            {
                _UserID = UserID;
                _Mode = enMode.Update;
            }
        }
        private void LoadData()
        {
            personCardWithFilters1.FilterEnabled = false;
            _User = clsUser.Find(_UserID);
            if (_User == null)
            {
                MessageBox.Show("No User with ID = " + _UserID, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }
            personCardWithFilters1.LoadPersonInfo(_User.PersonID);
            LUserID.Text = _UserID.ToString();
            textBox3.Text = _User.UserName;
            textBox1.Text = _User.Password;
            textBox2.Text = _User.Password;
            checkBox1.Checked = _User.IsActive;

        }
        private void RefreshData()
        {
            if (_Mode == enMode.AddNew)
            {
                this.Text = "AddNew User";
                label1.Text = "AddNew User";
                _User = new clsUser();
                button2.Enabled = false;
                tbLoginInfo.Enabled = false;
            }
            else
            {
                this.Text = "Edit";
                label1.Text = "Update User";
                button2.Enabled = true ;
                tbLoginInfo.Enabled = true;
                return;
            }
            LUserID.Text = "???";
            textBox3.Clear();
            textBox1.Clear();
            textBox2.Clear();
            checkBox1.Checked = false;

        }
        private void SaveData()
        {
            if(!ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _User.PersonID = personCardWithFilters1.PersonID;
            _User.Password = textBox1.Text.Trim();
            _User.IsActive = checkBox1.Checked;
            _User.UserName = textBox3.Text.Trim();
            if(_User.Save())
            {
                this.Text = "Edit";
                label1.Text = "Update User";
                label2.Text = _User.UserID.ToString();
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            SaveData();
        }

        //private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
        //    {
        //        e.Handled = true;
        //    }
        //}

        private void textBox2_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                errorProvider1.SetError(textBox2, "this field is required");
            }
            else if (textBox2.Text.Trim() != textBox1.Text.Trim())
            {
                errorProvider1.SetError(textBox2, "Password does not match the New Password");
            }
            else
            {
                errorProvider1.SetError(textBox2, "");
            }
        }

        private void textBox3_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox3.Text.Trim()))
            {
                errorProvider1.SetError(textBox3, "This field is required.");
                return;
            }
            if (_Mode == enMode.AddNew || textBox3.Text.Trim() != _User.UserName)
            {
                if (clsUser.UserExsitByUserName(textBox3.Text.Trim())) 
                {
                    errorProvider1.SetError(textBox3, "Username already exists.");
                    return;
                }
            }
                errorProvider1.SetError(textBox3, "");
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                button2.Enabled = true;
                tbLoginInfo.Enabled = true;
                tabControl1.SelectedTab = tabControl1.TabPages["tbLoginInfo"];
                return;
            }
            if (personCardWithFilters1.PersonID != -1)
            {
                if (clsUser.UserExsitByPersonID(personCardWithFilters1.PersonID))
                {
                    MessageBox.Show("Selected Person already has a user, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    tbLoginInfo.Enabled = true;
                    button2.Enabled = true;
                    tabControl1.SelectedTab = tabControl1.TabPages["tbLoginInfo"];
                }
            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                RefreshData();
            }
        }

        private void AddEditUser_Load(object sender, System.EventArgs e)
        {
            RefreshData();
            if (_Mode == enMode.Update)
                LoadData();
        }

        private void textBox1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, "this field is required");
            }
            else
            {
                errorProvider1.SetError(textBox1, "");
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
