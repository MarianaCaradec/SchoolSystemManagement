namespace SchoolManagement.API.Models
{
    public class User
    {
        public enum UserRole
        {
            Admin,
            Teacher,
            Student
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public Teacher? Teacher { get; set; } //Uno a uno opcional
        public Student? Student { get; set; } //Uno a uno opcional
    }
}
