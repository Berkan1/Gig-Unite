using Microsoft.EntityFrameworkCore.Migrations;

namespace GigUnite.Data.Migrations
{
    public partial class Optin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Optin",
                table: "Profile",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Optin",
                table: "Profile");
        }
    }
}
