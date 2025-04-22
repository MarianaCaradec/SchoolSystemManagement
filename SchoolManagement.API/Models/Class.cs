namespace SchoolManagement.API.Models
{
    public class Class
    {
        public int Id { get; set; }
        public string Course { get; set; }
        public string Divition { get; set; }
        public int Capacity { get; set; }
        public List<Teacher> Teachers { get; set; } = new(); //Muchos a muchos
        public List<Student> Students { get; set; } = new(); //Uno a muchos
    }
}
