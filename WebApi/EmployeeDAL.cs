using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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
            string query = "SELECT * FROM Employees";
            return ExecuteQuery<Employee>(query, CommandType.Text, null, MapEmployee);
        }

        public List<Employee> GetEmployeeById(int id)
        {
            string query = "SELECT * FROM Employees WHERE ID = @ID";
            var parameters = new List<SqlParameter> { new SqlParameter("@ID", id) };
            return ExecuteQuery<Employee>(query, CommandType.Text, parameters, MapEmployee);
        }

        public void AddEmployee(Employee employee)
        {
            string spName = "sp_CreateEmp";
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@FirstName", employee.FirstName),
                new SqlParameter("@LastName", employee.LastName),
                new SqlParameter("@Gender", employee.Gender),
                new SqlParameter("@Salary", employee.Salary)
            };

            ExecuteNonQuery(spName, CommandType.StoredProcedure, parameters);
        }

        public int UpdateEmployee(int id, Employee updatedEmployee)
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

            return ExecuteNonQuery(spName, CommandType.StoredProcedure, parameters);
        }

        public int DeleteEmployee(int id)
        {
            string query = "DELETE FROM Employees WHERE ID = @ID";
            var parameters = new List<SqlParameter> { new SqlParameter("@ID", id) };
            return ExecuteNonQuery(query, CommandType.Text, parameters);
        }

        private List<T> ExecuteQuery<T>(string query, CommandType commandType, List<SqlParameter> parameters, Func<SqlDataReader, T> mapper)
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

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        T item = mapper(reader);
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        private int ExecuteNonQuery(string query, CommandType commandType, List<SqlParameter> parameters)
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

                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
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
