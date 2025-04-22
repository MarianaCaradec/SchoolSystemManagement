using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Data.Seeds
{
    public class SubjectSeed : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.HasData(
                new Subject { Id = 1, Title = "Mathematics" },
                new Subject { Id = 2, Title = "Physics" },
                new Subject { Id = 3, Title = "History" }
                );
        }
    }
}
