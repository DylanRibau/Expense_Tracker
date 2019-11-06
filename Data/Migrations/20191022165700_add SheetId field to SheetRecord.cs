using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Expense_Tracker.Data.Migrations
{
    public partial class addSheetIdfieldtoSheetRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SheetRecords_Sheets_SheetId",
                table: "SheetRecords");

            migrationBuilder.AlterColumn<Guid>(
                name: "SheetId",
                table: "SheetRecords",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SheetRecords_Sheets_SheetId",
                table: "SheetRecords",
                column: "SheetId",
                principalTable: "Sheets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SheetRecords_Sheets_SheetId",
                table: "SheetRecords");

            migrationBuilder.AlterColumn<Guid>(
                name: "SheetId",
                table: "SheetRecords",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_SheetRecords_Sheets_SheetId",
                table: "SheetRecords",
                column: "SheetId",
                principalTable: "Sheets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
