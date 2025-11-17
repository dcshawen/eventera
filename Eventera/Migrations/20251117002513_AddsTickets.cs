using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eventera.Migrations
{
    /// <inheritdoc />
    public partial class AddsTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AstronomicalEventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Ticket_AstronomicalEvent_AstronomicalEventId",
                        column: x => x.AstronomicalEventId,
                        principalTable: "AstronomicalEvent",
                        principalColumn: "AstronomicalEventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_AstronomicalEventId",
                table: "Ticket",
                column: "AstronomicalEventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ticket");
        }
    }
}
