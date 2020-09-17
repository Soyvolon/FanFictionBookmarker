using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FanfictionBookmarker.Migrations
{
    public partial class RemoveDefaultFolder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_BookmarkFolder_DefaultFolderInternalKey",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_DefaultFolderInternalKey",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DefaultFolderInternalKey",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DefaultFolderInternalKey",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_DefaultFolderInternalKey",
                table: "AspNetUsers",
                column: "DefaultFolderInternalKey");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_BookmarkFolder_DefaultFolderInternalKey",
                table: "AspNetUsers",
                column: "DefaultFolderInternalKey",
                principalTable: "BookmarkFolder",
                principalColumn: "InternalKey",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
