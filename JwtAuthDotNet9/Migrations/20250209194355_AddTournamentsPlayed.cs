using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JwtAuthDotNet9.Migrations
{
    /// <inheritdoc />
    public partial class AddTournamentsPlayed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TournamentsPlayed",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TournamentsPlayed",
                table: "Users");
        }
    }
}
