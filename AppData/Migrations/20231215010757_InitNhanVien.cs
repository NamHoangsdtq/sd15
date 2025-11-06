using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class InitNhanVien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "VaiTro",
                columns: new[] { "ID", "Ten", "TrangThai" },
                values: new object[] { new Guid("b4996b2d-a343-434b-bfe9-09f8efbb3852"), "Admin", 1 });

            migrationBuilder.InsertData(
                table: "NhanVien",
                columns: new[] { "ID", "DiaChi", "Email", "IDVaiTro", "PassWord", "SDT", "Ten", "TrangThai" },
                values: new object[] { new Guid("2ec27ab7-5f67-4ed5-ae67-52f9c9726ebf"), "Ha Noi", "5tingclothes@gmail.com", new Guid("b4996b2d-a343-434b-bfe9-09f8efbb3852"), "$2a$10$xRUcz2MNCsP9waNEzcVW5.Rk7sktZE.mp2BQVcJsP8A1uOvoSDgjO", "0988143310", "Admin", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NhanVien",
                keyColumn: "ID",
                keyValue: new Guid("2ec27ab7-5f67-4ed5-ae67-52f9c9726ebf"));

            migrationBuilder.DeleteData(
                table: "VaiTro",
                keyColumn: "ID",
                keyValue: new Guid("b4996b2d-a343-434b-bfe9-09f8efbb3852"));
        }
    }
}
