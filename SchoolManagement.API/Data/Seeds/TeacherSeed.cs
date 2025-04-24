using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Data.Seeds
{
    public class TeacherSeed : IEntityTypeConfiguration<Teacher>
    {
        public void Configure(EntityTypeBuilder<Teacher> builder)
        {
            builder.HasData(

            new Teacher
            {
                Id = 1,
                Name = "Juan",
                Surname = "González",
                BirthDate = new DateOnly(2000, 5, 20),
                Address = "123 Calle Falsa, Buenos Aires",
                MobileNumber = 541112345678,
                UserId = 4,
            },
            new Teacher
            {
                Id = 2,
                Name = "María",
                Surname = "Fernández",
                BirthDate = new DateOnly(2001, 8, 15),
                Address = "456 Avenida Siempre Viva, Córdoba",
                MobileNumber = 541198765432,
                UserId = 5,
            },
            new Teacher
            {
                Id = 3,
                Name = "Carlos",
                Surname = "Martínez",
                BirthDate = new DateOnly(1999, 12, 10),
                Address = "789 Boulevard Independencia, Mendoza",
                MobileNumber = 541165432987,
                UserId = 6,
            }
            );
        }
    }
}
