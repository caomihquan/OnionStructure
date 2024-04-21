using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onion.Datas.Migrations
{
    /// <inheritdoc />
    public partial class update0408 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleID",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Rooms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "Rooms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LevelCode",
                table: "Rooms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaximumMember",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RoomLanguage",
                columns: table => new
                {
                    LanguageCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LanguageName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomLanguage", x => x.LanguageCode);
                });

            migrationBuilder.CreateTable(
                name: "RoomLevel",
                columns: table => new
                {
                    LevelCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LevelName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomLevel", x => x.LevelCode);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    RoleID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.RoleID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_LanguageCode",
                table: "Rooms",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_LevelCode",
                table: "Rooms",
                column: "LevelCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomLanguage_LanguageCode",
                table: "Rooms",
                column: "LanguageCode",
                principalTable: "RoomLanguage",
                principalColumn: "LanguageCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_RoomLevel_LevelCode",
                table: "Rooms",
                column: "LevelCode",
                principalTable: "RoomLevel",
                principalColumn: "LevelCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserRole_RoleID",
                table: "Users",
                column: "RoleID",
                principalTable: "UserRole",
                principalColumn: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomLanguage_LanguageCode",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_RoomLevel_LevelCode",
                table: "Rooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserRole_RoleID",
                table: "Users");

            migrationBuilder.DropTable(
                name: "RoomLanguage");

            migrationBuilder.DropTable(
                name: "RoomLevel");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_LanguageCode",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_LevelCode",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "LevelCode",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "MaximumMember",
                table: "Rooms");
        }
    }
}
