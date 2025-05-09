using SchoolManagement.API.Models;

namespace SchoolManagement.API.DTOs
{
    public class AttendanceDto
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public bool Present { get; set; }
        public int? StudentId { get; set; }
        public Student Student { get; set; } //Muchos a uno
        public int? TeacherId { get; set; }
        public Teacher Teacher { get; set; } //Muchos a uno
    }
}
