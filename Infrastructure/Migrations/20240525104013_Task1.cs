using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Task1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Task_GroupId",
                table: "Task",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Groups_GroupId",
                table: "Task",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Task_Groups_GroupId",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Task_GroupId",
                table: "Task");
        }
    }
}
