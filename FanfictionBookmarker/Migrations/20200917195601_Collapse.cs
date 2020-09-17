using Microsoft.EntityFrameworkCore.Migrations;

namespace FanfictionBookmarker.Migrations
{
    public partial class Collapse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Collapsed",
                table: "BookmarkFolder",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Collapsed",
                table: "BookmarkFolder");
        }
    }
}
