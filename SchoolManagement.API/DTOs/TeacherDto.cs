using SchoolManagement.API.Models;

namespace SchoolManagement.API.DTOs
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public long MobileNumber { get; set; }
        public List<SubjectDto> Subjects { get; set; }
        public List<ClassDto> Classes { get; set; } = new();
    }
}
