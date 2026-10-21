using DVLDProject.Properties;
using System;
using System.IO;
using System.Windows.Forms;
using DVLDProject.People;
namespace DVLDProject
{
    public partial class uctrPersonCard : UserControl
    {
        private int _PersonID = -1;
        public int PersonID => _PersonID;
        private clsPerson _Person;
        public clsPerson SelectedPerson => _Person;
        public uctrPersonCard()
        {
            InitializeComponent();
            ResetPersonInfo();
        }
        public void FillPersonInfo()
        {
            _PersonID = _Person.PersonID;
            PersonIDResult.Text = _Person.PersonID.ToString();
            NameResult.Text = _Person.FullName;
            NationalNoResult.Text = _Person.NationalNo;
            DateOfBirthResult.Text = _Person.DateOfBirth.ToString("dd/MM/yyyy");
            GendorResult.Text = (_Person.Gendor==0)?"Male":"Female";
            linkLabel1.ActiveLinkColor = System.Drawing.Color.Blue;
            linkLabel1.Enabled = true;
            if (_Person.Gendor == 0)
            {
                pictureBox3.Image = Resources.Man_32;
            }
            else
            {
                pictureBox3.Image = Resources.Woman_32;
            }
                AddressResult.Text = _Person.Address;
            PhoneResult.Text = _Person.Phone;
            EmailResult.Text = _Person.Email;
            CountryResult.Text = (_Person.CountryInfo != null) ? _Person.CountryInfo.CountryName : "????";
            _LoadPersonImage();
        }
        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);
            if (_Person != null)
                FillPersonInfo();
            else
            {
                MessageBox.Show("Person " +PersonID + " not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetPersonInfo();

            }
        }
        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);
            if (_Person != null)
                FillPersonInfo();
            else
            {
                MessageBox.Show("Person NationalNo: " + NationalNo + " not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ResetPersonInfo();

            }
        }
        private void ResetPersonInfo()
        {
            _PersonID = -1;
            PersonIDResult.Text = "????";
            NameResult.Text = "????";
            NationalNoResult.Text = "????";
            DateOfBirthResult.Text = "????";
            GendorResult.Text = "????";
            AddressResult.Text = "????";
            PhoneResult.Text = "????";
            EmailResult.Text = "????";
            CountryResult.Text = "????";
            linkLabel1.ActiveLinkColor = System.Drawing.Color.Gray;
            linkLabel1.Enabled = false;
            PbxPersonCard.Image = Resources.Male_512;
        }
        private void _LoadPersonImage()
        {
            if (_Person.Gendor == 0)
                PbxPersonCard.Image = Resources.Male_512;
            else
                PbxPersonCard.Image = Resources.Female_512;

            if (!string.IsNullOrWhiteSpace(_Person.ImagePath))
                if (File.Exists(_Person.ImagePath))
                    PbxPersonCard.ImageLocation = _Person.ImagePath;
                else
                    MessageBox.Show("Could not find this image: = " + _Person.ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_Person == null)
                return;

            AddNewPerson frm = new AddNewPerson(_PersonID);
            frm.ShowDialog();
            LoadPersonInfo(_PersonID);
        }
    }
}
