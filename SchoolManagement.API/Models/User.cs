namespace SchoolManagement.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // "Admin", "Teacher", "Student"
        public Teacher? Teacher { get; set; } //Uno a uno opcional
        public Student? Student { get; set; } //Uno a uno opcional
    }
}
