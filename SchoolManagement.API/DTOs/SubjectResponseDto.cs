using SchoolManagement.API.Models;

namespace SchoolManagement.API.DTOs
{
    public class SubjectResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<TeacherResponseDto> Teachers { get; set; } = new();
        public List<Grade> Grades { get; set; } = new();
    }
}
