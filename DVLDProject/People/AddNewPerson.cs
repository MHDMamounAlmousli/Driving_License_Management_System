using BuisnessLayer;
using DVLDProject.Properties;
using System;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using DVLDProject.GlobalClasses;

namespace DVLDProject.People
{
    public partial class AddNewPerson : Form
    {
        private clsPerson _Person;
        private int _PersonID;
        public Action<int> DataBack;
        private enum enMode { AddNew = 1, Update = 2 }
        private enMode _Mode = enMode.AddNew;
        public AddNewPerson(int PersonID)
        {
            InitializeComponent();
            if(PersonID == -1)
            {
                _Mode = enMode.AddNew;
                this.Text = "AddNew Person";
            }
            else
            {
                _Mode = enMode.Update;
                _PersonID = PersonID;
                this.Text = "Update Person";
            }
        }
        private void fillCountry()
        {
            DataTable dt = clsCountries.GetAllCountries();
            foreach(DataRow row in dt.Rows)
            {
                cbCountry.Items.Add(row["CountryName"].ToString());
            }
        }
        private void _LoadData()
        {
            _Person = clsPerson.Find(_PersonID);
            if (_Person == null)
            {
                MessageBox.Show("Person not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtAddress.Text = _Person.Address;
            txtPhone.Text = _Person.Phone;
            cbCountry.SelectedIndex = cbCountry.FindStringExact(_Person.CountryInfo.CountryName);
            txtNationalNo.Text = _Person.NationalNo;
            txtEmail.Text = _Person.Email;
            label14.Text = _Person.PersonID.ToString();
            if (_Person.Gendor == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;
            dtpDateOfBirth.Value = _Person.DateOfBirth;
            if (!string.IsNullOrEmpty(_Person.ImagePath))
            {
                pbPersonImage.ImageLocation = _Person.ImagePath;
                llRemoveImage.Visible = true;
            }
        }
        private void _SaveData()
        {
            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            clsCountries country = clsCountries.Find(cbCountry.SelectedItem.ToString());
            _Person.NationalityCountryID = country.CountryID;
            _Person.NationalNo = txtNationalNo.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            if (rbMale.Checked)
                _Person.Gendor = 0;
            else
                _Person.Gendor = 1;
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            if (pbPersonImage.ImageLocation != null)
            {
                _Person.ImagePath = pbPersonImage.ImageLocation;
            }
            else
                _Person.ImagePath = "";
            if (_Person.Save())
            {
                _Mode = enMode.Update;
                DataBack?.Invoke(_Person.PersonID);
                label1.Text = "Update Person";
                MessageBox.Show("_Person saved successfully", "Success");
            }
            else
                MessageBox.Show("Error saving _Person", "Error");
        }

        private void AddNewPerson_Load(object sender, EventArgs e)
        {
            RefreshData();
            if(_Mode == enMode.Update)
            _LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
                return;
            if (!HandleImage())
                return;
            _SaveData();
        }
        private void RefreshData()
        {
            fillCountry();

            if (_Mode == enMode.AddNew)
            {
                this.Text = "AddNew Person";
                label1.Text = "Add New Person";
                _Person = new clsPerson();
            }else
            {
                this.Text = "Update Person";
                label1.Text = "Update Person";
                return;
            }
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;
            cbCountry.SelectedIndex = cbCountry.FindString("Syria");
            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";
            rbMale.Checked = true;
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";

        }
        private bool HandleImage()
        {
            if (_Person.ImagePath != pbPersonImage.ImageLocation)
            {
                if (_Person.ImagePath != "")
                {
                    try
                    {
                        File.Delete(_Person.ImagePath);
                    }
                    catch (IOException)
                    {
                        return false;
                    }
                }

                if (pbPersonImage.ImageLocation != null)
                {
                    string SourceImageFile = pbPersonImage.ImageLocation;

                    if (clsUtil.CopyImageToFolder(ref SourceImageFile))
                    {
                        pbPersonImage.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error copying image to folder");
                        return false;
                    }
                }

            }
                return true;
        }

        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text.Trim()))
            {
                e.Cancel = true; 
                errorProvider1.SetError(txtFirstName, "First Name is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFirstName, "");
            }
        }

        private void txtSecondName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSecondName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtSecondName, "Second Name is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtSecondName, "");
            }
        }

        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtLastName, "Last Name is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtLastName, "");
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is required!");
                return;
            }

            if (_Mode == enMode.AddNew || txtNationalNo.Text.Trim() != _Person.NationalNo)
            {
                if (clsPerson.IsExist(txtNationalNo.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtNationalNo, "National Number already exists!");
                    return;
                }
            }

            e.Cancel = false;
            errorProvider1.SetError(txtNationalNo, "");
        }
        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtAddress, "Address is required!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtAddress, "");
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (!txtEmail.Text.Trim().EndsWith("@gmail.com") ||txtEmail.Text.Trim().Length <= "@gmail.com".Length)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid email format (must end with @gmail.com)");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtEmail, "");
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pbPersonImage.Image = null;
            llRemoveImage.Visible = false;
            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.InitialDirectory = @"C:\Users\Mousl\Pictures";
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;
                pbPersonImage.Load(selectedFilePath);
                llRemoveImage.Visible = true;
            }
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Male_512;
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Female_512;
        }
    } 
}
