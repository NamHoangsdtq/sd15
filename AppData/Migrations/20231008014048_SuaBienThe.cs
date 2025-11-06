using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class SuaBienThe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BienThe_KhuyenMai_IDKhuyenMai",
                table: "BienThe");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgaySinh",
                table: "KhachHang",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "BinhLuan",
                table: "DanhGia",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<Guid>(
                name: "IDKhuyenMai",
                table: "BienThe",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_BienThe_KhuyenMai_IDKhuyenMai",
                table: "BienThe",
                column: "IDKhuyenMai",
                principalTable: "KhuyenMai",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BienThe_KhuyenMai_IDKhuyenMai",
                table: "BienThe");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgaySinh",
                table: "KhachHang",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BinhLuan",
                table: "DanhGia",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<Guid>(
                name: "IDKhuyenMai",
                table: "BienThe",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BienThe_KhuyenMai_IDKhuyenMai",
                table: "BienThe",
                column: "IDKhuyenMai",
                principalTable: "KhuyenMai",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
