using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlowlySimulate.Persistence.Migration
{
    /// <inheritdoc />
    public partial class setting : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppLang",
                table: "SlowlyUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "ConfirmBeforeSendingTheLetter",
                table: "SlowlyUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DarkMode",
                table: "SlowlyUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "JoinedDate",
                table: "SlowlyUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "LastOnline",
                table: "SlowlyUsers",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<bool>(
                name: "PushNotification",
                table: "SlowlyUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppLang",
                table: "SlowlyUsers");

            migrationBuilder.DropColumn(
                name: "ConfirmBeforeSendingTheLetter",
                table: "SlowlyUsers");

            migrationBuilder.DropColumn(
                name: "DarkMode",
                table: "SlowlyUsers");

            migrationBuilder.DropColumn(
                name: "JoinedDate",
                table: "SlowlyUsers");

            migrationBuilder.DropColumn(
                name: "LastOnline",
                table: "SlowlyUsers");

            migrationBuilder.DropColumn(
                name: "PushNotification",
                table: "SlowlyUsers");
        }
    }
}
