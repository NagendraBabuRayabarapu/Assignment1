using Microsoft.Data.SqlClient;
using System.Data;
using WebApi.Models;

namespace WebApi
{
    public class EmployeeDAL
    {
        private readonly string _connectionString;

        public EmployeeDAL(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
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

            return employees;
        }

        public Employee GetEmployeeById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
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

                    return employee;
                }

                reader.Close();
            }

            return null;
        }

        public void AddEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
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
        }

        public int UpdateEmployee(int id, Employee updatedEmployee)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
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

            return rowsAffected;
        }

        public int DeleteEmployee(int id)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand("DELETE FROM Employees WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }

            return rowsAffected;
        }
    }


}