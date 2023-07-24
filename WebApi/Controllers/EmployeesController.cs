using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Gets()
        {
            List<Employee> employees = await _employeeService.GetAllEmployeesAsync();

            if (employees.Count == 0)
            {
                return NotFound();
            }

            return Ok(employees);
        }

        [HttpGet("GetEmployee")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            List<Employee> employee = await _employeeService.GetEmployeeByIdAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            await _employeeService.AddEmployeeAsync(employee);
            return Ok("SUCCESS");
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] Employee updatedEmployee)
        {
            bool success = await _employeeService.UpdateEmployeeAsync(id, updatedEmployee);

            if (!success)
            {
                return NotFound("Record not exists or not modified");
            }

            return Ok($"SUCCESS 1 row modified");
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            bool success = await _employeeService.DeleteEmployeeAsync(id);

            if (!success)
            {
                return NotFound("Record not exists or not deleted");
            }

            return Ok($"SUCCESS 1 row deleted");
        }
    }

}
