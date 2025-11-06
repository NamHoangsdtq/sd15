using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class SuaDBThemNgayNhanHangHoaDon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NgayNhanHang",
                table: "HoaDon",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayNhanHang",
                table: "HoaDon");
        }
    }
}
