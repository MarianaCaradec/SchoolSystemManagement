namespace SchoolManagement.API.Models
{
    public class Grade
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public DateOnly Date { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; } //Uno a muchos
        public int SubjectId { get; set; }
        public Subject Subject { get; set; } //Uno a muchos
    }
}
