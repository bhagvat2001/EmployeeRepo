using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Entities
{
    public class Designation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
