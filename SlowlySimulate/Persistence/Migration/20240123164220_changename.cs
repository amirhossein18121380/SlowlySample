using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlowlySimulate.Persistence.Migration
{
    /// <inheritdoc />
    public partial class changename : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowAddMeById",
                table: "SlowlyUsers");

            migrationBuilder.RenameColumn(
                name: "SlowlyId",
                table: "MatchingPreferences",
                newName: "AllowAddMeById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllowAddMeById",
                table: "MatchingPreferences",
                newName: "SlowlyId");

            migrationBuilder.AddColumn<bool>(
                name: "AllowAddMeById",
                table: "SlowlyUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
