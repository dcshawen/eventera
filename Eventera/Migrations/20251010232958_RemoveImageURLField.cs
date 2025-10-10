using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventera.Migrations
{
    /// <inheritdoc />
    public partial class RemoveImageURLField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "AstronomicalEvent");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "AstronomicalEvent",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
