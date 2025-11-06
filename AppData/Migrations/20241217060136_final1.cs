using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class final1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anh_MauSac_IDMauSac",
                table: "Anh");

            migrationBuilder.DropIndex(
                name: "IX_Anh_IDMauSac",
                table: "Anh");

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("0346fbdd-4286-42b4-8f6e-9f0b899efc21"),
                column: "NgayTao",
                value: new DateTime(2024, 12, 17, 13, 1, 35, 783, DateTimeKind.Local).AddTicks(7320));

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("15fef17d-088b-4b3b-a5a6-bd8760296809"),
                column: "NgayTao",
                value: new DateTime(2024, 12, 17, 13, 1, 35, 783, DateTimeKind.Local).AddTicks(7324));

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("41d68bd1-81a2-402a-bcfe-7f112549cf2e"),
                column: "NgayTao",
                value: new DateTime(2024, 12, 17, 13, 1, 35, 783, DateTimeKind.Local).AddTicks(7315));

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("ca7a8e83-4be8-427d-a469-dd7fb29a8f3f"),
                column: "NgayTao",
                value: new DateTime(2024, 12, 17, 13, 1, 35, 783, DateTimeKind.Local).AddTicks(7329));

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("e4e59bc3-b004-4ab9-b2bd-5109bf62be8d"),
                column: "NgayTao",
                value: new DateTime(2024, 12, 17, 13, 1, 35, 783, DateTimeKind.Local).AddTicks(7310));

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("eae74b1d-75cd-45bb-aecd-1f8d5dc7e171"),
                column: "NgayTao",
                value: new DateTime(2024, 12, 17, 13, 1, 35, 783, DateTimeKind.Local).AddTicks(7288));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("0346fbdd-4286-42b4-8f6e-9f0b899efc21"),
                column: "NgayTao",
                value: new DateTime(2024, 5, 15, 8, 31, 34, 899, DateTimeKind.Local).AddTicks(5295));

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("15fef17d-088b-4b3b-a5a6-bd8760296809"),
                column: "NgayTao",
                value: new DateTime(2024, 5, 15, 8, 31, 34, 899, DateTimeKind.Local).AddTicks(5298));

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("41d68bd1-81a2-402a-bcfe-7f112549cf2e"),
                column: "NgayTao",
                value: new DateTime(2024, 5, 15, 8, 31, 34, 899, DateTimeKind.Local).AddTicks(5292));

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("ca7a8e83-4be8-427d-a469-dd7fb29a8f3f"),
                column: "NgayTao",
                value: new DateTime(2024, 5, 15, 8, 31, 34, 899, DateTimeKind.Local).AddTicks(5302));

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("e4e59bc3-b004-4ab9-b2bd-5109bf62be8d"),
                column: "NgayTao",
                value: new DateTime(2024, 5, 15, 8, 31, 34, 899, DateTimeKind.Local).AddTicks(5287));

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("eae74b1d-75cd-45bb-aecd-1f8d5dc7e171"),
                column: "NgayTao",
                value: new DateTime(2024, 5, 15, 8, 31, 34, 899, DateTimeKind.Local).AddTicks(5271));

            migrationBuilder.CreateIndex(
                name: "IX_Anh_IDMauSac",
                table: "Anh",
                column: "IDMauSac");

            migrationBuilder.AddForeignKey(
                name: "FK_Anh_MauSac_IDMauSac",
                table: "Anh",
                column: "IDMauSac",
                principalTable: "MauSac",
                principalColumn: "ID");
        }
    }
}
