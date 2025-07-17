using EmployeeManagementSystem.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystem.Entities
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public int DesignationId { get; set; }
        [ForeignKey("DesignationId")]
        public Designation Designation { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
