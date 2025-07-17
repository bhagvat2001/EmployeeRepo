using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entities;
using EmployeeManagementSystem.DTOs.Employee;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var employees = await (from e in _context.Employees
                                       join d in _context.Departments on e.DepartmentId equals d.Id
                                       join g in _context.Designations on e.DesignationId equals g.Id
                                       select new EmployeeDto
                                       {
                                           Id = e.Id,
                                           Name = e.Name,
                                           Email = e.Email,
                                           DepartmentId = e.DepartmentId,
                                           DesignationId = e.DesignationId,
                                           DepartmentName = d.Name,
                                           DesignationTitle = g.Title
                                       }).ToListAsync();

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving employees: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var employee = await _context.Employees
                    .Include(e => e.Department)
                    .Include(e => e.Designation)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (employee == null)
                    return NotFound();

                var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                if (currentUserRole == "Employee" && employee.UserId != currentUserId)
                    return Forbid();

                var dto = new EmployeeDto
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    DepartmentId = employee.DepartmentId,
                    DesignationId = employee.DesignationId,
                    DepartmentName = employee.Department?.Name,
                    DesignationTitle = employee.Designation?.Title,
                    UserId = employee.UserId
                };

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving employee: {ex.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Create([FromBody] EmployeeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var employee = new Employee
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    DepartmentId = dto.DepartmentId,
                    DesignationId = dto.DesignationId,
                    UserId = dto.UserId ?? 0
                };

                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();

                var response = new EmployeeResponseDto
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    DepartmentId = employee.DepartmentId,
                    DesignationId = employee.DesignationId,
                    DepartmentName = _context.Departments.FirstOrDefault(d => d.Id == employee.DepartmentId)?.Name,
                    DesignationTitle = _context.Designations.FirstOrDefault(d => d.Id == employee.DesignationId)?.Title,
                    UserId = employee.UserId
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating employee: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
                if (employee == null) return NotFound();

                employee.Name = dto.Name;
                employee.Email = dto.Email;
                employee.DepartmentId = dto.DepartmentId;
                employee.DesignationId = dto.DesignationId;

                await _context.SaveChangesAsync();

                dto.Id = employee.Id;
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating employee: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
                if (employee == null) return NotFound();

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();

                return Ok("Deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting employee: {ex.Message}");
            }
        }
    }
}
