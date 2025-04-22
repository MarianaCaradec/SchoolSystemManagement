using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Data.Seeds
{
    public class ClassSeed : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.HasData(
                new Class
                {
                    Id = 1,
                    Course = "4th Grade",
                    Divition = "A",
                    Capacity = 30
                },
                new Class
                {
                    Id = 2,
                    Course = "5th Grade",
                    Divition = "B",
                    Capacity = 25
                },
                new Class
                {
                    Id = 3,
                    Course = "6th Grade",
                    Divition = "C",
                    Capacity = 28
                }
            );
        }
    }
}
