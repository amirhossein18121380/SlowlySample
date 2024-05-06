using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlowlySimulate.Persistence.Migration
{
    /// <inheritdoc />
    public partial class matchtable : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchingPreferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SlowlyUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SlowlyId = table.Column<bool>(type: "bit", nullable: false),
                    AutoMatch = table.Column<bool>(type: "bit", nullable: false),
                    ProfileSuggestions = table.Column<bool>(type: "bit", nullable: false),
                    EnableAgeFilter = table.Column<bool>(type: "bit", nullable: false),
                    AgeFrom = table.Column<int>(type: "int", nullable: true),
                    AgeTo = table.Column<int>(type: "int", nullable: true),
                    BeMale = table.Column<bool>(type: "bit", nullable: false),
                    BeFemale = table.Column<bool>(type: "bit", nullable: false),
                    BeNonBinary = table.Column<bool>(type: "bit", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchingPreferences", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchingPreferences");
        }
    }
}
