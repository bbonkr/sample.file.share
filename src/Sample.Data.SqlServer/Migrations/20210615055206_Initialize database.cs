using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sample.Data.SqlServer.Migrations
{
    public partial class Initializedatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false, comment: "Identifier"),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "User account name"),
                    DisplayName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "display name"),
                    Email = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false, comment: "Email address")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false, comment: "Identifier"),
                    Name = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "File name"),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Content type"),
                    Size = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L, comment: "File size"),
                    Uri = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false, comment: "File origin access uri;"),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValue: new DateTimeOffset(new DateTime(2021, 6, 15, 5, 52, 6, 647, DateTimeKind.Unspecified).AddTicks(4671), new TimeSpan(0, 0, 0, 0, 0)), comment: "File uploaded at"),
                    CreatedBy = table.Column<string>(type: "char(36)", nullable: false, comment: "Creator")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_AppUsers_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Access",
                columns: table => new
                {
                    Id = table.Column<string>(type: "char(36)", nullable: false, comment: "Identifier"),
                    UserId = table.Column<string>(type: "char(36)", nullable: false, comment: "User identifier"),
                    FileId = table.Column<string>(type: "char(36)", nullable: false, comment: "File identifier"),
                    Token = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "Access token '/api/file/{token}'"),
                    ExpiresOn = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true, comment: "Expires access")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Access", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Access_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Access_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Access_FileId",
                table: "Access",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Access_Token",
                table: "Access",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Access_UserId",
                table: "Access",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_CreatedBy",
                table: "Files",
                column: "CreatedBy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Access");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "AppUsers");
        }
    }
}
