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
using System.Windows.Forms;

namespace utility
{
    public partial class DataViewForm : Form
    {
        private List<Employee> _employees;
        private List<Employee> _filteredEmployees;

        public DataViewForm()
        {
            InitializeComponent();
        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private async Task LoadEmployees()
        {
            string getEmployeesURl = ConfigurationManager.AppSettings["GetAllEmp"];

            string jsonResponse = await Processor.MakeHttpCall(HttpMethod.Get, getEmployeesURl, null);
            _employees = JsonConvert.DeserializeObject<List<Employee>>(jsonResponse);
            if (_employees.Count == 0)
            {
                MessageBox.Show("No Employees Found", "INFO", MessageBoxButtons.OK);
            }
            else
            {
                dataGridView1.DataSource = _employees;
                CustomizeDataGridView();
            }
            _filteredEmployees = _employees;
        }

        private void CustomizeDataGridView()
        {
            // Optionally customize the appearance or behavior of the DataGridView
            // For example, you can set column headers, adjust column widths, etc.
            dataGridView1.Columns["ID"].HeaderText = "Employee ID";
            dataGridView1.Columns["FirstName"].HeaderText = "First Name";
            dataGridView1.Columns["LastName"].HeaderText = "Last Name";
            dataGridView1.Columns["Gender"].HeaderText = "Gender";
            dataGridView1.Columns["Salary"].HeaderText = "Salary";

            // Adjust column widths based on content or your preference
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void TextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBoxSearch.Text.Trim();

            bool isIdMatch = int.TryParse(searchText, out int searchId);
            
            _filteredEmployees = _employees.Where(emp =>
            {
                return isIdMatch && emp.ID == searchId || 
                       emp.FirstName.ToLower().Contains(searchText.ToLower()) || 
                       emp.LastName.ToLower().Contains(searchText.ToLower());
            }).ToList();

            dataGridView1.DataSource = _filteredEmployees;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            
            DetailsForm detailsForm = new DetailsForm();
            detailsForm.ShowDialog();
            LoadEmployees();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            string input = ShowInputBox("Enter ID:");
            if (!string.IsNullOrEmpty(input))
            {
                if (int.TryParse(input, out int id))
                {
                    DialogResult dialogResult = MessageBox.Show($"You entered ID: {id}. Do you want to edit?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
                    if (dialogResult == DialogResult.Yes)
                    {
                        DetailsForm detailsForm = new DetailsForm(id);
                        detailsForm.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid ID. Please enter a valid numeric ID.", "Error", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
                }
            }
            LoadEmployees();

        }

        private string ShowInputBox(string prompt)
        {
            Form promptForm = new Form
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "Enter ID",
                StartPosition = FormStartPosition.CenterScreen
            };

            Label lblPrompt = new Label() { Left = 50, Top = 20, Text = prompt };
            TextBox txtInput = new TextBox() { Left = 50, Top = 50, Width = 200 };
            Button btnOk = new Button() { Text = "OK", Left = 50, Width = 70, Top = 80 };
            Button btnCancel = new Button() { Text = "Cancel", Left = 150, Width = 70, Top = 80 };

            btnOk.Click += (sender, e) => { promptForm.DialogResult = DialogResult.OK; };
            btnCancel.Click += (sender, e) => { promptForm.DialogResult = DialogResult.Cancel; };

            promptForm.Controls.Add(lblPrompt);
            promptForm.Controls.Add(txtInput);
            promptForm.Controls.Add(btnOk);
            promptForm.Controls.Add(btnCancel);

            return promptForm.ShowDialog() == DialogResult.OK ? txtInput.Text : null;
        }

        private void Button1Refresh_Click(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            string input = ShowInputBox("Enter ID:");
            if (!string.IsNullOrEmpty(input))
            {
                if (int.TryParse(input, out int id))
                {
                    DialogResult dialogResult = MessageBox.Show($"You entered ID: {id}. Do you want to delete?", "Information", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        string deleteEmployeeURl = ConfigurationManager.AppSettings["DeleteEmp"];

                        string jsonResponse = await Processor.MakeHttpCall(HttpMethod.Delete, deleteEmployeeURl+id.ToString(), null);

                        if (!string.IsNullOrEmpty(jsonResponse))
                        {
                            MessageBox.Show("Employee Deleted Successfully", "INFO", MessageBoxButtons.OK);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid ID. Please enter a valid numeric ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            LoadEmployees();
        }
    }
}
