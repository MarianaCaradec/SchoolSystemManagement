namespace SchoolManagement.API.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Teacher> Teachers { get; set; } = new(); //Muchos a muchos
        public List<Grade> Grades { get; set; } = new();
    }
}
