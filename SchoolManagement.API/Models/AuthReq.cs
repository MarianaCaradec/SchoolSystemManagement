namespace SchoolManagement.API.Models
{
    public class AuthReq
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
    }
}
