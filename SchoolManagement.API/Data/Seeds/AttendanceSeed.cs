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
                    Present = true
                },
                new Attendance
                {
                    Id = 2,
                    Date = new DateOnly(2025, 4, 21),
                    Present = false
                },
                new Attendance
                {
                    Id = 3,
                    Date = new DateOnly(2025, 4, 20),
                    Present = true
                }
            );
        }
    }
}
