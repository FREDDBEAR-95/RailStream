using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RailStream_Server.Migrations
{
    /// <inheritdoc />
    public partial class RailStream : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Trains_TrainId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Routes_RouteId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Wagons_WagonNumber1",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wagons",
                table: "Wagons");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_WagonNumber1",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Routes_TrainId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "WagonNumber",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "WagonNumber1",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TrainId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "RoleDescription",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Sections",
                table: "Roles");

            migrationBuilder.RenameColumn(
                name: "RouteId",
                table: "Tickets",
                newName: "WagonId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_RouteId",
                table: "Tickets",
                newName: "IX_Tickets_WagonId");

            migrationBuilder.AlterColumn<string>(
                name: "WagonNumber",
                table: "Wagons",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Wagons",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<long>(
                name: "PhoneNumber",
                table: "Users",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "BirthDate",
                table: "Users",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReleaseDate",
                table: "Trains",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                table: "Trains",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TrainId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "TimeWays",
                table: "Routes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "DepartureTime",
                table: "Routes",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DepartureDate",
                table: "Routes",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "ArrivalTime",
                table: "Routes",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ArrivalDate",
                table: "Routes",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wagons",
                table: "Wagons",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Trains_RouteId",
                table: "Trains",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TrainId",
                table: "Tickets",
                column: "TrainId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Trains_TrainId",
                table: "Tickets",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "TrainId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Wagons_WagonId",
                table: "Tickets",
                column: "WagonId",
                principalTable: "Wagons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trains_Routes_RouteId",
                table: "Trains",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Trains_TrainId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Wagons_WagonId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Trains_Routes_RouteId",
                table: "Trains");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wagons",
                table: "Wagons");

            migrationBuilder.DropIndex(
                name: "IX_Trains_RouteId",
                table: "Trains");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TrainId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Wagons");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "Trains");

            migrationBuilder.DropColumn(
                name: "TrainId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ArrivalDate",
                table: "Routes");

            migrationBuilder.RenameColumn(
                name: "WagonId",
                table: "Tickets",
                newName: "RouteId");

            migrationBuilder.RenameIndex(
                name: "IX_Tickets_WagonId",
                table: "Tickets",
                newName: "IX_Tickets_RouteId");

            migrationBuilder.AlterColumn<string>(
                name: "WagonNumber",
                table: "Wagons",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "PhoneNumber",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "Users",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ReleaseDate",
                table: "Trains",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

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

            migrationBuilder.AlterColumn<DateTime>(
                name: "TimeWays",
                table: "Routes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DepartureTime",
                table: "Routes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DepartureDate",
                table: "Routes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ArrivalTime",
                table: "Routes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time");

            migrationBuilder.AddColumn<int>(
                name: "TrainId",
                table: "Routes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RoleDescription",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sections",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wagons",
                table: "Wagons",
                column: "WagonNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_WagonNumber1",
                table: "Tickets",
                column: "WagonNumber1");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_TrainId",
                table: "Routes",
                column: "TrainId");

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Trains_TrainId",
                table: "Routes",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "TrainId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Routes_RouteId",
                table: "Tickets",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Wagons_WagonNumber1",
                table: "Tickets",
                column: "WagonNumber1",
                principalTable: "Wagons",
                principalColumn: "WagonNumber");
        }
    }
}
