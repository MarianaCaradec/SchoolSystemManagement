namespace SchoolManagement.API.DTOs
{
    public class ClassDto
    {
        public int Id { get; set; }
        public string Course { get; set; }
        public string Divition { get; set; }
        public List<TeacherDto> Teachers { get; set; } = new List<TeacherDto>();
        public List<StudentDto> Students { get; set; } = new List<StudentDto>();
    }
}
