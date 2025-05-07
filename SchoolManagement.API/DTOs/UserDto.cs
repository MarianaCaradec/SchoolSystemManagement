using SchoolManagement.API.Models;

namespace SchoolManagement.API.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public User.UserRole Role { get; set; }
        public Teacher? Teacher { get; set; }
        public Student? Student { get; set; }
    }
}
