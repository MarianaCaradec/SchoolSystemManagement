﻿namespace SchoolManagement.API.DTOs
{
    public class TeacherResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public long MobileNumber { get; set; }
        public string Address { get; set; }
        public int UserId { get; set; }
    }
}
