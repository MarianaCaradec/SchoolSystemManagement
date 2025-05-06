using SchoolManagement.API.Models;

namespace SchoolManagement.API.DTOs
{
    public class AuthDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public User.UserRole Role { get; set; }
    }
}
