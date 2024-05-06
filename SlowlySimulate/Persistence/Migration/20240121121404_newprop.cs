using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlowlySimulate.Persistence.Migration
{
    /// <inheritdoc />
    public partial class newprop : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AllowAddMeById",
                table: "SlowlyUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowAddMeById",
                table: "SlowlyUsers");
        }
    }
}
