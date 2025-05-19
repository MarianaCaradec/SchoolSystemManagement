namespace SchoolManagement.API.DTOs
{
    public class AttendanceResponseDto
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public bool Present { get; set; }
        public StudentInputDto? Student { get; set; }
        public TeacherInputDto? Teacher { get; set; }
    }
}
