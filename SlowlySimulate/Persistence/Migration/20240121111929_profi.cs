using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlowlySimulate.Persistence.Migration
{
    /// <inheritdoc />
    public partial class profi : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "SlowlyUsers",
                newName: "AboutMe");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "SlowlyUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LetterLength",
                table: "SlowlyUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReplyTime",
                table: "SlowlyUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SlowlyId",
                table: "SlowlyUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "SlowlyUsers");

            migrationBuilder.DropColumn(
                name: "LetterLength",
                table: "SlowlyUsers");

            migrationBuilder.DropColumn(
                name: "ReplyTime",
                table: "SlowlyUsers");

            migrationBuilder.DropColumn(
                name: "SlowlyId",
                table: "SlowlyUsers");

            migrationBuilder.RenameColumn(
                name: "AboutMe",
                table: "SlowlyUsers",
                newName: "Bio");
        }
    }
}
