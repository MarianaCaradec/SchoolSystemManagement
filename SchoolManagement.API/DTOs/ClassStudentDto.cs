namespace SchoolManagement.API.DTOs
{
    public class ClassStudentDto : ClassDto
    {
        public List<TeacherResponseDto> Teachers { get; set; } = new();
    }
}
