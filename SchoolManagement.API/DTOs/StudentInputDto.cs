using SchoolManagement.API.Models;

namespace SchoolManagement.API.DTOs
{
    public class StudentInputDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Address { get; set; }
        public long MobileNumber { get; set; }
        public int UserId { get; set; }
        public int ClassId { get; set; }
    }
}
