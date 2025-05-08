using SchoolManagement.API.Models;

namespace SchoolManagement.API.DTOs
{
    public class AuthDto
    {
        public string Email { get; set; }
        public User.UserRole Role { get; set; }
    }
}
