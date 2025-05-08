using SchoolManagement.API.Models;

namespace SchoolManagement.API.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public User.UserRole Role { get; set; }
        public TeacherDto? Teacher { get; set; }
        public StudentDto? Student { get; set; }
    }
}
