using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccessLayer.Migrations
{
    public partial class StoryMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserImage_ImageId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "StoryData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContentType = table.Column<string>(nullable: true),
                    Data = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoryData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Story",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserProfileId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    StoryDataId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Story", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Story_StoryData_StoryDataId",
                        column: x => x.StoryDataId,
                        principalTable: "StoryData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Story_StoryDataId",
                table: "Story",
                column: "StoryDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserImage_ImageId",
                table: "AspNetUsers",
                column: "ImageId",
                principalTable: "UserImage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserImage_ImageId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Story");

            migrationBuilder.DropTable(
                name: "StoryData");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserImage_ImageId",
                table: "AspNetUsers",
                column: "ImageId",
                principalTable: "UserImage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
