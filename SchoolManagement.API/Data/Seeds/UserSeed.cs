using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Models;

namespace SchoolManagement.API.Data.Seeds
{
    public class UserSeed : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = 1,
                    Email = "pepitoaurelio@test.com",
                    Password = "hashedpassword645",
                    Role = "Student"
                },
                new User
                {
                  Id = 2,
                  Email = "martamaria@test.com",
                  Password = "hashedpassword547",
                  Role = "Student"
                },
                new User
                {
                    Id = 3,
                    Email = "rosalopez@test.com",
                    Password = "hashedpassword687",
                    Role = "Student"
                },
                new User
                {
                    Id = 4,
                    Email = "roberto@test.com",
                    Password = "hashedpassword79889",
                    Role = "Teacher"
                },
                new User
                {
                    Id = 5,
                    Email = "tatamartino@test.com",
                    Password = "hashedpassword98347",
                    Role = "Teacher"
                },
                new User
                {
                    Id = 6,
                    Email = "roccototo@test.com",
                    Password = "hashedpassword8437",
                    Role = "Teacher"
                }
                );
        }
    }
}
