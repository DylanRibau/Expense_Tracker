using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Expense_Tracker.Data.Migrations
{
    public partial class Changingthesheetstabletohaveamonthandyeacolumninstadofhavingadatecolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Sheets");

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Sheets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Sheets",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "Sheets");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Sheets");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Sheets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
