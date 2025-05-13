namespace SchoolManagement.API.DTOs
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public long MobileNumber { get; set; }
        public int UserId { get; set; }
        public List<SubjectDto> Subjects { get; set; } = new();
        public List<ClassTeacherDto> Classes { get; set; } = new();
        public List<AttendanceDto> Attendances { get; set; } = new();
    }
}
