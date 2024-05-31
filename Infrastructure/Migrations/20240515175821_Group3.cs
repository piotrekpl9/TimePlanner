using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Group3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_Groups_GroupId1",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_GroupId1",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "GroupId1",
                table: "Member");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupId1",
                table: "Member",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Member_GroupId1",
                table: "Member",
                column: "GroupId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Groups_GroupId1",
                table: "Member",
                column: "GroupId1",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
