using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.WebApi.Migrations
{
    public partial class A02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SaveMessageId",
                table: "BlogPosts",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_SaveMessageId",
                table: "BlogPosts",
                column: "SaveMessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_SaveMessages_SaveMessageId",
                table: "BlogPosts",
                column: "SaveMessageId",
                principalTable: "SaveMessages",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_SaveMessages_SaveMessageId",
                table: "BlogPosts");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_SaveMessageId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "SaveMessageId",
                table: "BlogPosts");
        }
    }
}
