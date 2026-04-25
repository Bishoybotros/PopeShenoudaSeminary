using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopeShenoudaSeminary.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGradeIdFromStudentSubjectGrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjectGrades_Grades_GradeId",
                table: "StudentSubjectGrades");

            migrationBuilder.DropIndex(
                name: "IX_StudentSubjectGrades_GradeId",
                table: "StudentSubjectGrades");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "StudentSubjectGrades");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "StudentSubjectGrades",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubjectGrades_GradeId",
                table: "StudentSubjectGrades",
                column: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjectGrades_Grades_GradeId",
                table: "StudentSubjectGrades",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id");
        }
    }
}
