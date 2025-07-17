using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs.Employee
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int DepartmentId { get; set; }

        [Required]
        public int DesignationId { get; set; }

        public int? UserId { get; set; } // Used for authorization check
        public string? DepartmentName { get; set; }
        public string? DesignationTitle { get; set; }
    }
}
