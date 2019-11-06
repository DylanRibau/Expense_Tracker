using Microsoft.EntityFrameworkCore.Migrations;

namespace Expense_Tracker.Data.Migrations
{
    public partial class ChangeUserConnectiomodeUserUser2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_AspNetUsers_UserId",
                table: "UserConnections");

            migrationBuilder.DropIndex(
                name: "IX_UserConnections_UserId",
                table: "UserConnections");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserConnections",
                newName: "User");

            migrationBuilder.AlterColumn<string>(
                name: "User2Id",
                table: "UserConnections",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "User",
                table: "UserConnections",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserConnections_AspNetUsers_User2Id",
                table: "UserConnections");

            migrationBuilder.DropIndex(
                name: "IX_UserConnections_User2Id",
                table: "UserConnections");

            migrationBuilder.RenameColumn(
                name: "User",
                table: "UserConnections",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "User2Id",
                table: "UserConnections",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserConnections",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

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
    }
}
