using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
