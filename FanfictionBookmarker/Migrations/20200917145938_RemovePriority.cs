using Microsoft.EntityFrameworkCore.Migrations;

namespace FanfictionBookmarker.Migrations
{
    public partial class RemovePriority : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Priority",
                table: "FanficBookmark");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "BookmarkFolder");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "FanficBookmark",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Priority",
                table: "BookmarkFolder",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}