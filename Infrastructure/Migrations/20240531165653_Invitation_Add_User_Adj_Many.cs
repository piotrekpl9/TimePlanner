using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Invitation_Add_User_Adj_Many : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invitation_UserId",
                table: "Invitation");

            migrationBuilder.CreateIndex(
                name: "IX_Invitation_UserId",
                table: "Invitation",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Invitation_UserId",
                table: "Invitation");

            migrationBuilder.CreateIndex(
                name: "IX_Invitation_UserId",
                table: "Invitation",
                column: "UserId",
                unique: true);
        }
    }
}
