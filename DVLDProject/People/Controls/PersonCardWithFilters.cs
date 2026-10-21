using System;
using System.ComponentModel;
using System.Windows.Forms;
namespace DVLDProject.People
{
    public partial class PersonCardWithFilters : UserControl
    {
        public event Action<int> OnPersonSelected;
        //protected virtual void PersonSelected(int PersonID)
        //{
        //    OnPersonSelected?.Invoke(PersonID);
        //}
        public PersonCardWithFilters()
        {
            InitializeComponent();
        }

        private void PersonCardWithFilters_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
        }

        private bool _showAddPerson = true;
        private bool _FilterEnabled = true;
        public bool ShowAddPerson
        {
            get { return _showAddPerson; }
            set
            {
                _showAddPerson = value;
                btnAddNewPerson.Visible = _showAddPerson;
            }
        }
        public bool FilterEnabled
        {
            get { return _FilterEnabled; }
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }
        public int PersonID => uctrPersonCard1.PersonID;
        public clsPerson SelectedPersonInfo => uctrPersonCard1.SelectedPerson;
        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            AddNewPerson frm = new AddNewPerson(-1);
            frm.DataBack += Frm_DataBack;
            frm.ShowDialog();

        }
        private void Frm_DataBack(int personID)
        {
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = personID.ToString();
            FindPerson();
        }
        public void LoadPersonInfo(int PersonID)
        {
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
            FindPerson();
        }
        private void FindPerson()
        {
            switch (cbFilterBy.SelectedIndex)
            {
                case 0:
                    uctrPersonCard1.LoadPersonInfo(txtFilterValue.Text);
                    break;

                case 1:
                    uctrPersonCard1.LoadPersonInfo(int.Parse(txtFilterValue.Text));
                    break;
                default:
                    break;
            }
            if (OnPersonSelected != null && FilterEnabled)
                OnPersonSelected(uctrPersonCard1.PersonID);
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FindPerson();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btnFind.PerformClick();
            }
            else if (cbFilterBy.SelectedIndex == 1)
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }
        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Clear();
            txtFilterValue.Focus();
        }

        private void btnFind_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFilterValue.Text))
            {
                errorProvider1.SetError(txtFilterValue, "Empty Field make sure to fill it");
            }
            else
            {
                errorProvider1.SetError(txtFilterValue, null);
            }
        }
        public void FocusFilter()
        {
            txtFilterValue.Focus();
        }
    }
}
