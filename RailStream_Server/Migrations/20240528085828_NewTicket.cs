using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailStream_Server.Migrations
{
    /// <inheritdoc />
    public partial class NewTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WagonNumber",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WagonNumber1",
                table: "Tickets",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_WagonNumber1",
                table: "Tickets",
                column: "WagonNumber1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Wagons_WagonNumber1",
                table: "Tickets",
                column: "WagonNumber1",
                principalTable: "Wagons",
                principalColumn: "WagonNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Wagons_WagonNumber1",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_WagonNumber1",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "WagonNumber",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "WagonNumber1",
                table: "Tickets");
        }
    }
}
