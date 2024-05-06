using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SlowlySimulate.Persistence.Migration
{
    /// <inheritdoc />
    public partial class addfreind : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Friend",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
                    RequestedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestedToId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsStarredUser = table.Column<bool>(type: "bit", nullable: false),
                    IsHiddenUser = table.Column<bool>(type: "bit", nullable: false),
                    IsRemovedUser = table.Column<bool>(type: "bit", nullable: false),
                    IsReportedUser = table.Column<bool>(type: "bit", nullable: false),
                    RequestTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FriendRequestFlag = table.Column<int>(type: "int", nullable: false),
                    SlowlyUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    CreatedDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UpdatedDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friend", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friend_SlowlyUsers_RequestedById",
                        column: x => x.RequestedById,
                        principalTable: "SlowlyUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Friend_SlowlyUsers_RequestedToId",
                        column: x => x.RequestedToId,
                        principalTable: "SlowlyUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Friend_SlowlyUsers_SlowlyUserId",
                        column: x => x.SlowlyUserId,
                        principalTable: "SlowlyUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friend_RequestedById",
                table: "Friend",
                column: "RequestedById");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_RequestedToId",
                table: "Friend",
                column: "RequestedToId");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_SlowlyUserId",
                table: "Friend",
                column: "SlowlyUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friend");
        }
    }
}
