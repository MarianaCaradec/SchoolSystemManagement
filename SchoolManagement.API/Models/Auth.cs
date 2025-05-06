using static SchoolManagement.API.Models.User;

namespace SchoolManagement.API.Models
{
    public class Auth
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public long? MobileNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
