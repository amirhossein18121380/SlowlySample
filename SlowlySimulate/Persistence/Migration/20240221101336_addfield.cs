using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlowlySimulate.Persistence.Migration
{
    /// <inheritdoc />
    public partial class addfield : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExcluded",
                table: "UserTopics",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExcluded",
                table: "UserTopics");
        }
    }
}
