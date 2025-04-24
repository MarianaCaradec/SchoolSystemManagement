namespace SchoolManagement.API.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Address { get; set; }
        public long MobileNumber { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ClassId { get; set; }
        public Class Class { get; set; }
        public List<Attendance> Attendances { get; set; } = new();
        public List<Grade> Grades { get; set; } = new();
    }
}
