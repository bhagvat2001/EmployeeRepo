using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs.Department
{
    public class DepartmentDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}
