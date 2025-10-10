using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventera.Migrations
{
    /// <inheritdoc />
    public partial class AddsFilename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Filename",
                table: "AstronomicalEvent",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Filename",
                table: "AstronomicalEvent");
        }
    }
}
