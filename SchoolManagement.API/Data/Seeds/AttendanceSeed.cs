using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Data.Seeds
{
    public class AttendanceSeed : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasData(
                new Attendance
                {
                    Id = 1,
                    Date = new DateOnly(2025, 4, 22),
                    Present = true,
                    StudentId = 1
                },
                new Attendance
                {
                    Id = 2,
                    Date = new DateOnly(2025, 4, 21),
                    Present = false,
                    StudentId = 2
                },
                new Attendance
                {
                    Id = 3,
                    Date = new DateOnly(2025, 4, 20),
                    Present = true,
                    StudentId = 3
                },
                new Attendance
                {
                    Id = 4,
                    Date = new DateOnly(2025, 2, 20),
                    Present = true,
                    TeacherId = 1
                },
                new Attendance
                {
                    Id = 5,
                    Date = new DateOnly(2025, 4, 14),
                    Present = false,
                    TeacherId = 2
                },
                new Attendance
                {
                    Id = 6,
                    Date = new DateOnly(2025, 1, 9),
                    Present = true,
                    TeacherId = 3
                }
            );
        }
    }
}
