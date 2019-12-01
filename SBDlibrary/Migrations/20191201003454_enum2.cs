using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SBDlibrary.Migrations
{
    public partial class enum2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "data_odbioru",
                table: "Rezerwacje",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "data_odbioru",
                table: "Rezerwacje");
        }
    }
}
