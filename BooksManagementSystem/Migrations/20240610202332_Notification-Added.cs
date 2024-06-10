using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BooksManagementSystem.Migrations
{
    public partial class NotificationAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NotificationMessage",
                table: "Borrowings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NotificationType",
                table: "Borrowings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationMessage",
                table: "Borrowings");

            migrationBuilder.DropColumn(
                name: "NotificationType",
                table: "Borrowings");
        }
    }
}
