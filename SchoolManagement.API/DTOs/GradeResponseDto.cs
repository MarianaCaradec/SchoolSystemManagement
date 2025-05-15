namespace SchoolManagement.API.DTOs
{
    public class GradeResponseDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateOnly Date { get; set; }
        public StudentDto Student { get; set; }
        public SubjectDto Subject { get; set; }
    }
}
