using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Models;
using Microsoft.AspNetCore.Identity;

namespace SchoolManagement.API.Data.Seeds
{
    public class UserSeed : IEntityTypeConfiguration<User>
    {
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = 1,
                    Email = "pepitoaurelio@test.com",
                    Password = HashPassword("student111"),
                    Role = User.UserRole.Student,
                },
                new User
                {
                    Id = 2,
                    Email = "martamaria@test.com",
                    Password = HashPassword("student222"),
                    Role = User.UserRole.Student,
                },
                new User
                {
                    Id = 3,
                    Email = "rosalopez@test.com",
                    Password = HashPassword("student333"),
                    Role = User.UserRole.Student,
                },
                new User
                {
                    Id = 4,
                    Email = "roberto@test.com",
                    Password = HashPassword("teacher444"),
                    Role = User.UserRole.Teacher,
                },
                new User
                {
                    Id = 5,
                    Email = "tatamartino@test.com",
                    Password = HashPassword("teacher555"),
                    Role = User.UserRole.Teacher,
                },
                new User
                {
                    Id = 6,
                    Email = "roccototo@test.com",
                    Password = HashPassword("teacher666"),
                    Role = User.UserRole.Teacher,
                },
                new User
                {
                    Id = 7,
                    Email = "admin@gmail.com",
                    Password = HashPassword("admin777"),
                    Role = User.UserRole.Admin,
                }
            );
        }

        private string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }
    }
}
