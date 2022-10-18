using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BlogApp.WebApi.Migrations
{
    public partial class A06 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "BlogPosts");

            migrationBuilder.AddColumn<long>(
                name: "PostTypeId",
                table: "BlogPosts",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "PostTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_PostTypeId",
                table: "BlogPosts",
                column: "PostTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_PostTypes_PostTypeId",
                table: "BlogPosts",
                column: "PostTypeId",
                principalTable: "PostTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_PostTypes_PostTypeId",
                table: "BlogPosts");

            migrationBuilder.DropTable(
                name: "PostTypes");

            migrationBuilder.DropIndex(
                name: "IX_BlogPosts_PostTypeId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "PostTypeId",
                table: "BlogPosts");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "BlogPosts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
