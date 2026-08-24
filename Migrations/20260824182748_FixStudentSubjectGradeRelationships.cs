using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopeShenoudaSeminary.Migrations
{
    /// <inheritdoc />
    public partial class FixStudentSubjectGradeRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGrades_Users_StudentId",
                table: "StudentGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjectGrades_Subjects_SubjectId",
                table: "StudentSubjectGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjectGrades_Users_StudentId",
                table: "StudentSubjectGrades");

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
                name: "FK_StudentGrades_Users_StudentId",
                table: "StudentGrades",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjectGrades_Grades_GradeId",
                table: "StudentSubjectGrades",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjectGrades_Subjects_SubjectId",
                table: "StudentSubjectGrades",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjectGrades_Users_StudentId",
                table: "StudentSubjectGrades",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGrades_Users_StudentId",
                table: "StudentGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjectGrades_Grades_GradeId",
                table: "StudentSubjectGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjectGrades_Subjects_SubjectId",
                table: "StudentSubjectGrades");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjectGrades_Users_StudentId",
                table: "StudentSubjectGrades");

            migrationBuilder.DropIndex(
                name: "IX_StudentSubjectGrades_GradeId",
                table: "StudentSubjectGrades");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "StudentSubjectGrades");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGrades_Users_StudentId",
                table: "StudentGrades",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjectGrades_Subjects_SubjectId",
                table: "StudentSubjectGrades",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjectGrades_Users_StudentId",
                table: "StudentSubjectGrades",
                column: "StudentId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
