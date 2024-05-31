using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Group2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Member_Groups_GroupId2",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_GroupId1",
                table: "Member");

            migrationBuilder.DropIndex(
                name: "IX_Member_GroupId2",
                table: "Member");

            migrationBuilder.DropColumn(
                name: "GroupId2",
                table: "Member");

            migrationBuilder.CreateIndex(
                name: "IX_Member_GroupId1",
                table: "Member",
                column: "GroupId1",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Member_GroupId1",
                table: "Member");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId2",
                table: "Member",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Member_GroupId1",
                table: "Member",
                column: "GroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_Member_GroupId2",
                table: "Member",
                column: "GroupId2",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Member_Groups_GroupId2",
                table: "Member",
                column: "GroupId2",
                principalTable: "Groups",
                principalColumn: "Id");
        }
    }
}
