using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Task_PlannedAt_Field_Change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PlannedAt",
                table: "Tasks",
                newName: "PlannedStartHour");

            migrationBuilder.AddColumn<DateTime>(
                name: "PlannedEndHour",
                table: "Tasks",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlannedEndHour",
                table: "Tasks");

            migrationBuilder.RenameColumn(
                name: "PlannedStartHour",
                table: "Tasks",
                newName: "PlannedAt");
        }
    }
}
