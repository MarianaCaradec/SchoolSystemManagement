using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SchoolManagement.API.Migrations
{
    /// <inheritdoc />
    public partial class ClassTeacherRelationSeeded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassTeacher_Classes_ClassesId",
                table: "ClassTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassTeacher_Teachers_TeachersId",
                table: "ClassTeacher");

            migrationBuilder.RenameColumn(
                name: "TeachersId",
                table: "ClassTeacher",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "ClassesId",
                table: "ClassTeacher",
                newName: "ClassId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassTeacher_TeachersId",
                table: "ClassTeacher",
                newName: "IX_ClassTeacher_TeacherId");

            migrationBuilder.InsertData(
                table: "ClassTeacher",
                columns: new[] { "ClassId", "TeacherId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ClassTeacher_Classes_ClassId",
                table: "ClassTeacher",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassTeacher_Teachers_TeacherId",
                table: "ClassTeacher",
                column: "TeacherId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassTeacher_Classes_ClassId",
                table: "ClassTeacher");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassTeacher_Teachers_TeacherId",
                table: "ClassTeacher");

            migrationBuilder.DeleteData(
                table: "ClassTeacher",
                keyColumns: new[] { "ClassId", "TeacherId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "ClassTeacher",
                keyColumns: new[] { "ClassId", "TeacherId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "ClassTeacher",
                keyColumns: new[] { "ClassId", "TeacherId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "ClassTeacher",
                newName: "TeachersId");

            migrationBuilder.RenameColumn(
                name: "ClassId",
                table: "ClassTeacher",
                newName: "ClassesId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassTeacher_TeacherId",
                table: "ClassTeacher",
                newName: "IX_ClassTeacher_TeachersId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClassTeacher_Classes_ClassesId",
                table: "ClassTeacher",
                column: "ClassesId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassTeacher_Teachers_TeachersId",
                table: "ClassTeacher",
                column: "TeachersId",
                principalTable: "Teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
