using SchoolManagement.API.Models;

namespace SchoolManagement.API.DTOs
{
    public class SubjectInputDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<TeacherResponseDto>? Teachers { get; set; } = new();
        public List<GradeDto>? Grades { get; set; } = new();
    }
}
