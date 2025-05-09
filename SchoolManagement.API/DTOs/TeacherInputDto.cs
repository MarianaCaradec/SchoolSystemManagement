namespace SchoolManagement.API.DTOs
{
    public class TeacherInputDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Address { get; set; }
        public long MobileNumber { get; set; }
        public int UserId { get; set; }
    }
}
