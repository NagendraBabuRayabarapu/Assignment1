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
        private readonly EmployeeService _employeeService;

        private readonly string _connectionString;

        public EmployeesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("EmployeeDBConnection");
            _employeeService = new EmployeeService(_connectionString);

        }

        [HttpGet]
        public IActionResult Gets()
        {
            List<Employee> employees = _employeeService.GetAllEmployees();

            if (employees.Count == 0)
            {
                return NotFound();
            }

            return Ok(employees);
        }

        [HttpGet("GetEmployee")]
        public IActionResult GetEmployee(int id)
        {
            Employee employee = _employeeService.GetEmployeeById(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost("Add")]
        public IActionResult AddEmployee([FromBody] Employee employee)
        {
            _employeeService.AddEmployee(employee);
            return Ok("SUCCESS");
        }

        [HttpPut("Update")]
        public IActionResult UpdateEmployee(int id, [FromBody] Employee updatedEmployee)
        {
            bool success = _employeeService.UpdateEmployee(id, updatedEmployee);

            if (!success)
            {
                return NotFound("Record not exists or not modified");
            }

            return Ok($"SUCCESS 1 row modified");
        }

        [HttpDelete("Delete")]
        public IActionResult DeleteEmployee(int id)
        {
            bool success = _employeeService.DeleteEmployee(id);

            if (!success)
            {
                return NotFound("Record not exists or not deleted");
            }

            return Ok($"SUCCESS 1 row deleted");
        }
    }
}
