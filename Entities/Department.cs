using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Entities
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
