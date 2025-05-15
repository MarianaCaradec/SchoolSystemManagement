namespace SchoolManagement.API.DTOs
{
    public class ClassResponseDto
    {
        public int Id { get; set; }
        public string Course { get; set; }
        public string Divition { get; set; }
        public int Capacity { get; set; }
        public List<TeacherResponseDto> Teachers { get; set; } = new(); 
        public List<StudentDto> Students { get; set; } = new();
    }
}
