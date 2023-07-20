using Newtonsoft.Json;
using utility;

namespace Employee_Utility
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void get_Click(object sender, EventArgs e)
        {
            DataViewForm form2 = new DataViewForm();
            form2.Show();
            ClearAllTextBoxes();
        }

        private async void add_ClickAsync(object sender, EventArgs e)
        {
            Employee emp = new Employee
            {
                FirstName = textBoxFirstName.Text ,
                LastName = textBoxLastName.Text,
                Gender = textBoxGender.Text,
                Salary = Convert.ToInt32(textBoxSalary.Text)
            };
            if (!string.IsNullOrEmpty(textBoxID.Text))
            {
                emp.ID = Convert.ToInt32(textBoxID.Text);
            }

            string url = "https://localhost:7261/api/Employees/Add";
            string requestJson = JsonConvert.SerializeObject(emp);     

            string jsonResponse = await Processor.MakeHttpCall(HttpMethod.Post, url, requestJson);

            if (!string.IsNullOrEmpty(jsonResponse))
            {
                MessageBox.Show("Employee Added Successfully", "INFO", MessageBoxButtons.OK);
            }
            ClearAllTextBoxes();

        }

        private void getById_Click(object sender, EventArgs e)
        {
            DataViewForm form2 = new DataViewForm(Convert.ToInt32(textBoxID.Text));
            form2.Show();
        }

        private async void update_Click(object sender, EventArgs e)
        {
            Employee emp = new Employee
            {
                ID = Convert.ToInt32(textBoxID.Text),
                FirstName = textBoxFirstName.Text,
                LastName = textBoxLastName.Text,
                Gender = textBoxGender.Text,
                Salary = Convert.ToInt32(textBoxSalary.Text)
            };

            string url = $"https://localhost:7261/api/Employees/Update?id={textBoxID.Text}";
            string requestJson = JsonConvert.SerializeObject(emp);

            string jsonResponse = await Processor.MakeHttpCall(HttpMethod.Put, url, requestJson);


            if (!string.IsNullOrEmpty(jsonResponse))
            {
                MessageBox.Show("Employee Updated Successfully", "INFO", MessageBoxButtons.OK);
            }
            ClearAllTextBoxes();

        }

        private async void delete_Click(object sender, EventArgs e)
        {

            string url = $"https://localhost:7261/api/Employees/Delete?id={textBoxID.Text}";

            string jsonResponse = await Processor.MakeHttpCall(HttpMethod.Delete, url, null);

            if (!string.IsNullOrEmpty(jsonResponse))
            {
                MessageBox.Show("Employee Deleted Successfully", "INFO", MessageBoxButtons.OK);
            }

            ClearAllTextBoxes();

        }

        private void ClearAllTextBoxes()
        {
            // Loop through all controls on the form
            foreach (Control control in this.Controls)
            {
                // Check if the control is a TextBox
                if (control is TextBox textBox)
                {
                    // Set the TextBox text to an empty string
                    textBox.Text = string.Empty;
                }
            }
        }
    }
}