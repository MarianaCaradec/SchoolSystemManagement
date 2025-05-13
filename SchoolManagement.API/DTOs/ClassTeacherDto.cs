namespace SchoolManagement.API.DTOs
{
    public class ClassTeacherDto : ClassDto
    {
        public List<StudentResponseDto> Students { get; set; } = new();
    }
}
