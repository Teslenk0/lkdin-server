using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LKDin.Server.DataAccess.Migrations
{
    public partial class ConvertRowVersionIntoVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Users",
                type: "INTEGER",
                rowVersion: true,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Users");

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Users",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
