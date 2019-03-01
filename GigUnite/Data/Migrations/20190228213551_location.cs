using Microsoft.EntityFrameworkCore.Migrations;

namespace GigUnite.Data.Migrations
{
    public partial class location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Gig");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Gig");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Gig",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Gig");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Gig",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Gig",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
