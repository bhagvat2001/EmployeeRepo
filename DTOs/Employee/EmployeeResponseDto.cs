namespace EmployeeManagementSystem.DTOs.Employee
{
    public class EmployeeResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int DepartmentId { get; set; }

        public string DepartmentName { get; set; }

        public int DesignationId { get; set; }

        public string DesignationTitle { get; set; }

        public int? UserId { get; set; }
    }
}
