using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class TeacherSeedsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Address", "BirthDate", "Email", "MobileNumber", "Name", "Password", "Surname" },
                values: new object[,]
                {
                    { 1, "123 Calle Falsa, Buenos Aires", new DateOnly(2000, 5, 20), "juan.gonzalez@example.com", 541112345678L, "Juan", "hashedpassword1", "González" },
                    { 2, "456 Avenida Siempre Viva, Córdoba", new DateOnly(2001, 8, 15), "maria.fernandez@example.com", 541198765432L, "María", "hashedpassword2", "Fernández" },
                    { 3, "789 Boulevard Independencia, Mendoza", new DateOnly(1999, 12, 10), "carlos.martinez@example.com", 541165432987L, "Carlos", "hashedpassword3", "Martínez" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
