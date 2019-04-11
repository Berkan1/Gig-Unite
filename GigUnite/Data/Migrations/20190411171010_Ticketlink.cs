using Microsoft.EntityFrameworkCore.Migrations;

namespace GigUnite.Data.Migrations
{
    public partial class Ticketlink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Profile");

            migrationBuilder.AddColumn<string>(
                name: "TicketLink",
                table: "Gig",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketLink",
                table: "Gig");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Profile",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }
    }
}
