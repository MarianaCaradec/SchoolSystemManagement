using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Data.Seeds
{
    public class GradeSeed : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.HasData(
                new Grade
                {
                    Id = 1,
                    Value = 85,
                    Date = new DateOnly(2025, 4, 22),
                    StudentId = 1,
                    SubjectId = 1
                },
                new Grade
                {
                    Id = 2,
                    Value = 90,
                    Date = new DateOnly(2025, 4, 20),
                    StudentId = 2, 
                    SubjectId = 2  
                },
                new Grade
                {
                    Id = 3,
                    Value = 75,
                    Date = new DateOnly(2025, 4, 18),
                    StudentId = 3,
                    SubjectId = 3 
                }
            );
        }
    }
}
