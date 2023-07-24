using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
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

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            string query = "SELECT * FROM Employees";
            return await ExecuteQueryAsync<Employee>(query, CommandType.Text, null, MapEmployee);
        }

        public async Task<List<Employee>> GetEmployeeByIdAsync(int id)
        {
            string query = "SELECT * FROM Employees WHERE ID = @ID";
            var parameters = new List<SqlParameter> { new SqlParameter("@ID", id) };
            return await ExecuteQueryAsync<Employee>(query, CommandType.Text, parameters, MapEmployee);
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            string spName = "sp_CreateEmp";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@FirstName", employee.FirstName),
                new SqlParameter("@LastName", employee.LastName),
                new SqlParameter("@Gender", employee.Gender),
                new SqlParameter("@Salary", employee.Salary)
            };

            await ExecuteNonQueryAsync(spName, CommandType.StoredProcedure, parameters);
        }

        public async Task<int> UpdateEmployeeAsync(int id, Employee updatedEmployee)
        {
            string spName = "sp_UpdateEmp";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@ID", id),
                new SqlParameter("@FirstName", updatedEmployee.FirstName),
                new SqlParameter("@LastName", updatedEmployee.LastName),
                new SqlParameter("@Gender", updatedEmployee.Gender),
                new SqlParameter("@Salary", updatedEmployee.Salary)
            };

            return await ExecuteNonQueryAsync(spName, CommandType.StoredProcedure, parameters);
        }

        public async Task<int> DeleteEmployeeAsync(int id)
        {
            string query = "DELETE FROM Employees WHERE ID = @ID";
            var parameters = new List<SqlParameter> { new SqlParameter("@ID", id) };
            return await ExecuteNonQueryAsync(query, CommandType.Text, parameters);
        }

        private async Task<List<T>> ExecuteQueryAsync<T>(string query, CommandType commandType, List<SqlParameter> parameters, Func<SqlDataReader, T> mapper)
        {
            List<T> result = new List<T>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = commandType;

                if (parameters != null && parameters.Count > 0)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                await connection.OpenAsync();

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        T item = mapper(reader);
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        private async Task<int> ExecuteNonQueryAsync(string query, CommandType commandType, List<SqlParameter> parameters)
        {
            int rowsAffected = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = commandType;

                if (parameters != null && parameters.Count > 0)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                await connection.OpenAsync();
                rowsAffected = await command.ExecuteNonQueryAsync();
            }

            return rowsAffected;
        }

        private Employee MapEmployee(SqlDataReader reader)
        {
            return new Employee
            {
                ID = (int)reader["ID"],
                FirstName = (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                Gender = (string)reader["Gender"],
                Salary = (int)reader["Salary"]
            };
        }
    }
}
