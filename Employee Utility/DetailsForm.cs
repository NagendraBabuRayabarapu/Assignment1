using Employee_Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace utility
{
    public partial class DetailsForm : Form
    {
        private int Id = -1;
        public DetailsForm()
        {
            InitializeComponent();
        }

        public DetailsForm(int id)
        {
            InitializeComponent();
            this.Id = id;
            this.buttonSubmit.Text = "Update";
            GetEmployeeData(id);
        }

        private async void GetEmployeeData(int id)
        {
            string getEmployeeURL = ConfigurationManager.AppSettings["GetEmp"];


            string jsonResponse = await Processor.MakeHttpCall(HttpMethod.Get, getEmployeeURL + Id.ToString(), null);


            if (string.IsNullOrEmpty(jsonResponse))
            {
                MessageBox.Show("Employee is not present", "INFO", MessageBoxButtons.OK);
            }
            else
            {
                List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(jsonResponse);
                this.textBoxFirstName.Text = employees[0].FirstName;
                this.textBoxLastName.Text = employees[0].LastName;
                this.textBoxSalary.Text = employees[0].Salary.ToString();
                this.comboBoxGender.SelectedItem = employees[0].Gender.ToString();

            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            ClearAllTextBoxes();
        }

        private void ClearAllTextBoxes()
        {
            // Loop through all controls on the form
            foreach (Control control in this.Controls)
            {
                if (control is GroupBox groupBox)
                {
                    foreach (Control innerControl in groupBox.Controls)
                    {
                        if (innerControl is TextBox innerTextBox)
                        {
                            // Set the TextBox text to an empty string
                            innerTextBox.Text = string.Empty;
                        }
                    }
                } 
                // Check if the control is a TextBox
                if (control is TextBox textBox)
                {
                    // Set the TextBox text to an empty string
                    textBox.Text = string.Empty;
                }
            }
        }

        private async void buttonSubmit_Click(object sender, EventArgs e)
        {
            Employee emp = new Employee
            {
                FirstName = textBoxFirstName.Text,
                LastName = textBoxLastName.Text,
                Gender = comboBoxGender.SelectedItem == null? "" : comboBoxGender.SelectedItem.ToString(),
                Salary = Convert.ToInt32(textBoxSalary.Text)
            };

            if (buttonSubmit.Text == "Update")
            {
                emp.ID = this.Id;
                string updateEmployeeURL = ConfigurationManager.AppSettings["UpdateEmp"];

                string requestJson = JsonConvert.SerializeObject(emp);

                string jsonResponse = await Processor.MakeHttpCall(HttpMethod.Put, updateEmployeeURL+Id.ToString(), requestJson);


                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    MessageBox.Show("Employee Updated Successfully", "INFO", MessageBoxButtons.OK);
                }
            }
            else
            {
                string addEmployeeURL = ConfigurationManager.AppSettings["AddEmp"];

                string requestJson = JsonConvert.SerializeObject(emp);

                string jsonResponse = await Processor.MakeHttpCall(HttpMethod.Post, addEmployeeURL, requestJson);

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    MessageBox.Show("Employee Added Successfully", "INFO", MessageBoxButtons.OK);
                }
            }

            this.Close();
        }
    }
}
