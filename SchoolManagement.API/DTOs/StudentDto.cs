using SchoolManagement.API.Models;

namespace SchoolManagement.API.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public ClassDto Class { get; set; }
    }
}
