using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventera.Migrations
{
    /// <inheritdoc />
    public partial class AddsImageFieldToAstroEvent2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "imageUrl",
                table: "AstronomicalEvent",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "AstronomicalEvent",
                newName: "imageUrl");
        }
    }
}
