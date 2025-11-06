using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class SuaDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "QuyDoiDiem",
                columns: new[] { "ID", "TiLeTichDiem", "TiLeTieuDiem", "TrangThai" },
                values: new object[] { new Guid("16bd37c4-cef0-4e92-9bb5-511c43d99037"), 0, 0, 1 });

            migrationBuilder.InsertData(
                table: "VaiTro",
                columns: new[] { "ID", "Ten", "TrangThai" },
                values: new object[] { new Guid("952c1a5d-74ff-4daf-ba88-135c5440809c"), "Nhân viên", 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "QuyDoiDiem",
                keyColumn: "ID",
                keyValue: new Guid("16bd37c4-cef0-4e92-9bb5-511c43d99037"));

            migrationBuilder.DeleteData(
                table: "VaiTro",
                keyColumn: "ID",
                keyValue: new Guid("952c1a5d-74ff-4daf-ba88-135c5440809c"));
        }
    }
}
