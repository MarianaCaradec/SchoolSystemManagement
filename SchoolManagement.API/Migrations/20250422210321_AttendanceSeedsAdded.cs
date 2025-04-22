using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class AttendanceSeedsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Attendances",
                columns: new[] { "Id", "Date", "Present" },
                values: new object[,]
                {
                    { 1, new DateOnly(2025, 4, 22), true },
                    { 2, new DateOnly(2025, 4, 21), false },
                    { 3, new DateOnly(2025, 4, 20), true }
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
        }
    }
}
