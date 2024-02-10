using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBotSchool.Migrations
{
    /// <inheritdoc />
    public partial class Inittt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "Users",
                newName: "IsDeleteReminder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleteReminder",
                table: "Users",
                newName: "IsDelete");
        }
    }
}
