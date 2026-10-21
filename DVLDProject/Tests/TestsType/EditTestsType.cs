using BuisnessLayer;
using System;
using System.Windows.Forms;

namespace DVLDProject.Tests.TestsType
{
    public partial class EditTestsType : Form
    {
        private int testID;
        private clsTestsType testType;
        public EditTestsType(int testID)
        {
            InitializeComponent();
            this.testID = testID;
        }
        private void LoadData()
        {
            testType = clsTestsType.Find((clsTestsType.enTestType) testID);
            if (testType == null)
                return;
            label5.Text = ((int)testType.TestTypeID).ToString();
            textBox1.Text = testType.TestTypeTitle.ToString();
            textBox2.Text = testType.TestTypeDescription.ToString();
            textBox3.Text = testType.Fees.ToString();
        }

        private void EditTestsType_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            testType.TestTypeTitle = textBox1.Text;
            testType.TestTypeDescription = textBox2.Text;
            testType.Fees = Convert.ToInt16(textBox3.Text);
            if(testType.Save())
                MessageBox.Show("Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Error in Saving", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
