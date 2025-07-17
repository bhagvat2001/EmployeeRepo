using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Entities;
using EmployeeManagementSystem.DTOs.Department;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var departments = await _context.Departments
                    .Select(d => new DepartmentResponseDto
                    {
                        Id = d.Id,
                        Name = d.Name
                    }).ToListAsync();

                return Ok(departments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving departments: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var department = new Department
                {
                    Name = dto.Name
                };

                _context.Departments.Add(department);
                await _context.SaveChangesAsync();

                var response = new DepartmentResponseDto
                {
                    Id = department.Id,
                    Name = department.Name
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating department: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DepartmentDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
                if (department == null) return NotFound();

                department.Name = dto.Name;
                await _context.SaveChangesAsync();

                var response = new DepartmentResponseDto
                {
                    Id = department.Id,
                    Name = department.Name
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating department: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == id);
                if (department == null) return NotFound();

                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();

                return Ok("Department deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting department: {ex.Message}");
            }
        }
    }
}
