using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } // "Admin", "HR", "Employee"
    }
}
