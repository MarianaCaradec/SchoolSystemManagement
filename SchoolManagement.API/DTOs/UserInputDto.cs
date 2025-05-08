using SchoolManagement.API.Models;

namespace SchoolManagement.API.DTOs
{
    public class UserInputDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public User.UserRole Role { get; set; }
    }
}
