using WebApi.Models;

namespace WebApi
{
    public class EmployeeService
    {
        private readonly EmployeeDAL _employeeDAL;

        public EmployeeService(string connectionString)
        {
            _employeeDAL = new EmployeeDAL(connectionString);
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _employeeDAL.GetAllEmployeesAsync();
        }

        public async Task<List<Employee>> GetEmployeeByIdAsync(int id)
        {
            return await _employeeDAL.GetEmployeeByIdAsync(id);
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            await _employeeDAL.AddEmployeeAsync(employee);
        }

        public async Task<bool> UpdateEmployeeAsync(int id, Employee updatedEmployee)
        {
            int rowsAffected = await _employeeDAL.UpdateEmployeeAsync(id, updatedEmployee);
            return rowsAffected > 0;
        }


        public async Task<bool> DeleteEmployeeAsync(int id)
        {
            int rowsAffected = await _employeeDAL.DeleteEmployeeAsync(id);
            return rowsAffected > 0;
        }
    }

}
