namespace SchoolManagement.API.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Address { get; set; }
        public long MobileNumber { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } 
        public List<Subject> Subjects { get; set; } = new();
        public List<Class> Classes { get; set;  } = new();
        public List<Attendance> Attendances { get; set; } = new();
    }
}
