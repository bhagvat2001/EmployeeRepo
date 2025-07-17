using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs.Designation
{
    public class DesignationDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }
    }
}
