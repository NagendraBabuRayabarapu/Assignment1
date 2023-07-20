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

        public List<Employee> GetAllEmployees()
        {
            return _employeeDAL.GetAllEmployees();
        }

        public Employee GetEmployeeById(int id)
        {
            return _employeeDAL.GetEmployeeById(id);
        }

        public void AddEmployee(Employee employee)
        {
            _employeeDAL.AddEmployee(employee);
        }

        public bool UpdateEmployee(int id, Employee updatedEmployee)
        {
            int rowsAffected = _employeeDAL.UpdateEmployee(id, updatedEmployee);
            return rowsAffected > 0;
        }

        public bool DeleteEmployee(int id)
        {
            int rowsAffected = _employeeDAL.DeleteEmployee(id);
            return rowsAffected > 0;
        }
    }

}
