using Microsoft.EntityFrameworkCore.Migrations;

namespace FanfictionBookmarker.Migrations
{
    public partial class AddIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "FanficBookmark",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Index",
                table: "BookmarkFolder",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Index",
                table: "FanficBookmark");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "BookmarkFolder");
        }
    }
}
