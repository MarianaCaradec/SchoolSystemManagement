using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSeedAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "Capacity", "Course", "Divition" },
                values: new object[,]
                {
                    { 1, 30, "4th Grade", "A" },
                    { 2, 25, "5th Grade", "B" },
                    { 3, 28, "6th Grade", "C" }
                });

            migrationBuilder.InsertData(
                table: "Subjects",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { 1, "Mathematics" },
                    { 2, "Physics" },
                    { 3, "History" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Password", "Role" },
                values: new object[,]
                {
                    { 1, "pepitoaurelio@test.com", "hashedpassword645", "Student" },
                    { 2, "martamaria@test.com", "hashedpassword547", "Student" },
                    { 3, "rosalopez@test.com", "hashedpassword687", "Student" },
                    { 4, "roberto@test.com", "hashedpassword79889", "Teacher" },
                    { 5, "tatamartino@test.com", "hashedpassword98347", "Teacher" },
                    { 6, "roccototo@test.com", "hashedpassword8437", "Teacher" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "Address", "BirthDate", "ClassId", "MobileNumber", "Name", "Surname", "UserId" },
                values: new object[,]
                {
                    { 1, "123 Calle Falsa, Paraná", new DateOnly(2005, 6, 15), 1, 5493415123456L, "Sofía", "Pérez", 1 },
                    { 2, "456 Avenida Siempre Viva, Paraná", new DateOnly(2006, 10, 22), 2, 5493415234567L, "Lucas", "Ramírez", 2 },
                    { 3, "789 Boulevard Independencia, Paraná", new DateOnly(2004, 3, 8), 3, 5493415345678L, "Camila", "González", 3 }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "Id", "Address", "BirthDate", "MobileNumber", "Name", "Surname", "UserId" },
                values: new object[,]
                {
                    { 1, "123 Calle Falsa, Buenos Aires", new DateOnly(2000, 5, 20), 541112345678L, "Juan", "González", 4 },
                    { 2, "456 Avenida Siempre Viva, Córdoba", new DateOnly(2001, 8, 15), 541198765432L, "María", "Fernández", 5 },
                    { 3, "789 Boulevard Independencia, Mendoza", new DateOnly(1999, 12, 10), 541165432987L, "Carlos", "Martínez", 6 }
                });

            migrationBuilder.InsertData(
                table: "Attendances",
                columns: new[] { "Id", "Date", "Present", "StudentId", "TeacherId" },
                values: new object[,]
                {
                    { 1, new DateOnly(2025, 4, 22), true, 1, null },
                    { 2, new DateOnly(2025, 4, 21), false, 2, null },
                    { 3, new DateOnly(2025, 4, 20), true, 3, null },
                    { 4, new DateOnly(2025, 2, 20), true, null, 1 },
                    { 5, new DateOnly(2025, 4, 14), false, null, 2 },
                    { 6, new DateOnly(2025, 1, 9), true, null, 3 }
                });

            migrationBuilder.InsertData(
                table: "Grades",
                columns: new[] { "Id", "Date", "StudentId", "SubjectId", "Value" },
                values: new object[,]
                {
                    { 1, new DateOnly(2025, 4, 22), 1, 1, 85 },
                    { 2, new DateOnly(2025, 4, 20), 2, 2, 90 },
                    { 3, new DateOnly(2025, 4, 18), 3, 3, 75 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Grades",
                keyColumn: "Id",
                keyValue: 3);

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

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subjects",
                keyColumn: "Id",
                keyValue: 3);

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

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
