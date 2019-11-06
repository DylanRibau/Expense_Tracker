using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Expense_Tracker.Data.Migrations
{
    public partial class Addintantiatorforthecolletionofsheetrecordsinthesheetobject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sheet",
                table: "SheetRecords");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Sheet",
                table: "SheetRecords",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
