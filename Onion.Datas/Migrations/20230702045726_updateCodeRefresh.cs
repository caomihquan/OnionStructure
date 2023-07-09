using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Onion.Datas.Migrations
{
    /// <inheritdoc />
    public partial class updateCodeRefresh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CodeRefreshToken",
                table: "UserTokens",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeRefreshToken",
                table: "UserTokens");
        }
    }
}
