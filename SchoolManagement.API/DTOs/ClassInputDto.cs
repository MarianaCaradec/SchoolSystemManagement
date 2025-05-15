namespace SchoolManagement.API.DTOs
{
    public class ClassInputDto
    {
        public int Id { get; set; }
        public string Course { get; set; }
        public string Divition { get; set; }
        public int Capacity { get; set; }
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
    }
}
