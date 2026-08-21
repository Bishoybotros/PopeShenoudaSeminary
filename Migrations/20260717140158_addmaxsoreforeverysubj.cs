using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PopeShenoudaSeminary.Migrations
{
    /// <inheritdoc />
    public partial class addmaxsoreforeverysubj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxScore",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxScore",
                table: "Subjects");
        }
    }
}
