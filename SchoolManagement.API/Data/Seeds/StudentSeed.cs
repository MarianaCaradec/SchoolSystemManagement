using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Data.Seeds
{
    public class StudentSeed : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasData(
                new Student
                {
                    Id = 1,
                    Name = "Sofía",
                    Surname = "Pérez",
                    BirthDate = new DateOnly(2005, 6, 15),
                    Address = "123 Calle Falsa, Paraná",
                    MobileNumber = 5493415123456,
                    UserId = 1,
                    ClassId = 1
                },
                new Student
                {
                    Id = 2,
                    Name = "Lucas",
                    Surname = "Ramírez",
                    BirthDate = new DateOnly(2006, 10, 22),
                    Address = "456 Avenida Siempre Viva, Paraná",
                    MobileNumber = 5493415234567,
                    UserId = 2,
                    ClassId = 2 
                },
                new Student
                {
                    Id = 3,
                    Name = "Camila",
                    Surname = "González",
                    BirthDate = new DateOnly(2004, 3, 8),
                    Address = "789 Boulevard Independencia, Paraná",
                    MobileNumber = 5493415345678,
                    UserId = 3,
                    ClassId = 3
                }
            );
        }
    }
}
