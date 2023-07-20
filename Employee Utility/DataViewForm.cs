using Employee_Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private int ID;
        public DataViewForm()
        {
            InitializeComponent();
            this.ID = -1;
        }

        public DataViewForm(int id)
        {
            InitializeComponent();
            this.ID = id;
        }

        private async void Form2_Load(object sender, EventArgs e)
        {
            string jsonResponse = "";

            if (this.ID == -1)
            {
                string url = "https://localhost:7261/api/Employees";//from config
                jsonResponse = await Processor.MakeHttpCall(HttpMethod.Get, url, null);
            }
            else
            {
                string url = $"https://localhost:7261/api/Employees/GetEmployee?id={this.ID}";
                jsonResponse = await Processor.MakeHttpCall(HttpMethod.Get, url, null);
            }

            if (string.IsNullOrEmpty(jsonResponse))
            {
                MessageBox.Show("Unsuccessful request", "ERROR", MessageBoxButtons.OK);
                return;
            }


            List<Employee> employees = JsonConvert.DeserializeObject<List<Employee>>(jsonResponse);

            if (employees.Count == 0)
            {
                MessageBox.Show("No Employees Found", "INFO", MessageBoxButtons.OK);
            }
            else
            {
                dataGridView1.DataSource = employees;
            }
            CustomizeDataGridView();
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
    }
}
