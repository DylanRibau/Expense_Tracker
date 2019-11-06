using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Expense_Tracker.Data.Migrations
{
    public partial class FixinUserConnectiotabletoaddUseIdfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                table: "UserConnections");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserConnections",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserConnections");

            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "UserConnections",
                nullable: true);
        }
    }
}
