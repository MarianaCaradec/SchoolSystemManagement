namespace SchoolManagement.API.DTOs
{
    public class StudentResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Address { get; set; }
        public long MobileNumber { get; set; }
        public AuthDto? EmailRole { get; set; }
        public ClassStudentDto? Class { get; set; }
        public List<AttendanceDto>? Attendances { get; set; } = new();
        public List<GradeDto>? Grades { get; set; } = new();
    }
}
