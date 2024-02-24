using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBotSchool.Migrations
{
    /// <inheritdoc />
    public partial class AddedScheduledRemindersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduledReminders",
                columns: table => new
                {
                    ReminderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledReminders", x => x.ReminderId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduledReminders");
        }
    }
}
