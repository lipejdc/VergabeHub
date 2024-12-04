using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VergabeHub.Migrations
{
    /// <inheritdoc />
    public partial class NoticePlatformNotice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NoticePlatformNotices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoticeId = table.Column<int>(type: "int", nullable: false),
                    NoticePlatformId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoticePlatformNotices", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoticePlatformNotices");
        }
    }
}
