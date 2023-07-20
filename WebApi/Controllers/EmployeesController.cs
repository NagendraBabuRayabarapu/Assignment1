using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Text;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Gets()
        {
            List<Employee> employees = new List<Employee>();
            string connectionString = "Server = NagendraPC; Database = EmployeeDB; Trusted_Connection = True; Encrypt=False";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employees";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Employee employee = new Employee();
                    employee.ID = (int)reader["ID"];
                    employee.FirstName = (string)reader["FirstName"];
                    employee.LastName = (string)reader["LastName"];
                    employee.Gender = (string)reader["Gender"];
                    employee.Salary = (int)reader["Salary"];

                    employees.Add(employee);
                }

                reader.Close();
            }

            if (employees.Count == 0)
            {
                return NotFound();
            }

            return Ok(employees);
        }


        [HttpGet("GetEmployee")]
        public IActionResult GetEmployee(int id)
        {
            List<Employee> employees = new List<Employee>();
            string connectionString = "Server = NagendraPC; Database = EmployeeDB; Trusted_Connection = True; Encrypt=False";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT * FROM Employees WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Employee employee = new Employee();
                    employee.ID = (int)reader["ID"];
                    employee.FirstName = (string)reader["FirstName"];
                    employee.LastName = (string)reader["LastName"];
                    employee.Gender = (string)reader["Gender"];
                    employee.Salary = (int)reader["Salary"];
                    employees.Add(employee);
                }

                reader.Close();
            }

            return Ok(employees);
        }

        [HttpPost("Add")]
        public IActionResult AddEmployee([FromBody] Employee employee)
        {
            string connectionString = "Server = NagendraPC; Database = EmployeeDB; Trusted_Connection = True; Encrypt=False";//from cofig file

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_CreateEmp", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@Gender", employee.Gender);
                command.Parameters.AddWithValue("@Salary", employee.Salary);

                connection.Open();
                command.ExecuteNonQuery();
            }

            return Ok("SUCCESS");
        }

        [HttpPut("Update")]
        public IActionResult UpdateEmployee(int id, [FromBody] Employee updatedEmployee)
        {
            string connectionString = "Server = NagendraPC; Database = EmployeeDB; Trusted_Connection = True; Encrypt=False";
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("sp_UpdateEmp", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@FirstName", updatedEmployee.FirstName);
                command.Parameters.AddWithValue("@LastName", updatedEmployee.LastName);
                command.Parameters.AddWithValue("@Gender", updatedEmployee.Gender);
                command.Parameters.AddWithValue("@Salary", updatedEmployee.Salary);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            if (rowsAffected == 0)
            {
                return NotFound("Record not exists or not modified");
            }

            return Ok($"SUCCESS {rowsAffected} rows modified");
        }


        [HttpDelete("Delete")]
        public IActionResult DeleteEmployee(int id)
        {
            string connectionString = "Server = NagendraPC; Database = EmployeeDB; Trusted_Connection = True; Encrypt=False";
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM Employees WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            if (rowsAffected == 0)
            {
                return NotFound("Record not exists or not deleted");
            }
            return Ok($"SUCCESS {rowsAffected} rows deleted");

        }

    }
}
