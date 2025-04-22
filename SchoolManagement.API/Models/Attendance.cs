namespace SchoolManagement.API.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public bool Present { get; set; }
        public List<Student> Students { get; set; } = new(); //Muchos a muchos
        public List<Teacher> Teachers { get; set; } = new(); //Muchos a muchos
    }
}
