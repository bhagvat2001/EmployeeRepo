using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.DTOs.Designation;
using EmployeeManagementSystem.Entities;

namespace EmployeeManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DesignationsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DesignationsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var designations = await _context.Designations
                    .Select(d => new DesignationResponseDto
                    {
                        Id = d.Id,
                        Title = d.Title
                    })
                    .ToListAsync();

                return Ok(designations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving designations: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(DesignationDto desigDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var designation = new Designation
                {
                    Title = desigDto.Title
                };

                _context.Designations.Add(designation);
                await _context.SaveChangesAsync();

                var response = new DesignationResponseDto
                {
                    Id = designation.Id,
                    Title = designation.Title
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating designation: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DesignationDto updatedDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var designation = await _context.Designations
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (designation == null)
                    return NotFound();

                designation.Title = updatedDto.Title;

                await _context.SaveChangesAsync();

                var response = new DesignationResponseDto
                {
                    Id = designation.Id,
                    Title = designation.Title
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error updating designation: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var designation = await _context.Designations
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (designation == null)
                    return NotFound();

                _context.Designations.Remove(designation);
                await _context.SaveChangesAsync();

                return Ok("Designation deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting designation: {ex.Message}");
            }
        }
    }
}
