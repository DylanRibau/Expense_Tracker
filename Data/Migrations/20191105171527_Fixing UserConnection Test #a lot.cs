using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Expense_Tracker.Data.Migrations
{
    public partial class FixingUserConnectionTestalot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_AspNetUsers_User2Id",
                table: "UserConnections");

            migrationBuilder.DropIndex(
                name: "IX_UserConnections_User2Id",
                table: "UserConnections");

            migrationBuilder.AlterColumn<Guid>(
                name: "User2Id",
                table: "UserConnections",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserConnections",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserConnections_UserId",
                table: "UserConnections",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_AspNetUsers_UserId",
                table: "UserConnections",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_AspNetUsers_UserId",
                table: "UserConnections");

            migrationBuilder.DropIndex(
                name: "IX_UserConnections_UserId",
                table: "UserConnections");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserConnections");

            migrationBuilder.AlterColumn<string>(
                name: "User2Id",
                table: "UserConnections",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_UserConnections_User2Id",
                table: "UserConnections",
                column: "User2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserConnections_AspNetUsers_User2Id",
                table: "UserConnections",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
