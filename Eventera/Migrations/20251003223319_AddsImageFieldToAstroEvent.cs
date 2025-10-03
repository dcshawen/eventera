using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventera.Migrations
{
    /// <inheritdoc />
    public partial class AddsImageFieldToAstroEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "imageUrl",
                table: "AstronomicalEvent",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imageUrl",
                table: "AstronomicalEvent");
        }
    }
}
