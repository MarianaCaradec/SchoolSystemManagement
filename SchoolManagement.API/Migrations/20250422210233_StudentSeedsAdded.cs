using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class StudentSeedsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address", "BirthDate", "ClassId", "Email", "MobileNumber", "Name", "Password", "Surname" },
                values: new object[,]
                {
                    { 1, "123 Calle Falsa, Paraná", new DateOnly(2005, 6, 15), 1, "sofia.perez@example.com", 5493415123456L, "Sofía", "hashedpassword1", "Pérez" },
                    { 2, "456 Avenida Siempre Viva, Paraná", new DateOnly(2006, 10, 22), 2, "lucas.ramirez@example.com", 5493415234567L, "Lucas", "hashedpassword2", "Ramírez" },
                    { 3, "789 Boulevard Independencia, Paraná", new DateOnly(2004, 3, 8), 3, "camila.gonzalez@example.com", 5493415345678L, "Camila", "hashedpassword3", "González" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
