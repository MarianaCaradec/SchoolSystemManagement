using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class GradeSeedsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
