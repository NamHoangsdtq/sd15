using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class SeedingDataAnh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Anh",
                columns: new[] { "ID", "DuongDan", "IDMauSac", "IDSanPham", "TrangThai" },
                values: new object[,]
                {
                    { new Guid("3da49d96-68bf-4618-8574-01ece60aea75"), "1953198247766_ef28dde5aa_o_211f9ee337bc4c1fb7c5933b8a84a38a_compact242657303.webp", new Guid("fc23a8df-f7cf-4863-85c7-3781ac1873f7"), new Guid("e3515cb9-f3af-4fb3-a100-50ecf0aa81d3"), 1 },
                    { new Guid("a539976a-21e2-4930-bd5d-9c3dd6562679"), "1953197790942_bf4bc7352a_o_9c76cdf68bfc41c2936861828ab4003c_master242657308.webp", new Guid("579090a3-3f65-4cec-a71e-a756e9ce2f85"), new Guid("e3515cb9-f3af-4fb3-a100-50ecf0aa81d3"), 1 }
                });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Anh",
                keyColumn: "ID",
                keyValue: new Guid("3da49d96-68bf-4618-8574-01ece60aea75"));

            migrationBuilder.DeleteData(
                table: "Anh",
                keyColumn: "ID",
                keyValue: new Guid("a539976a-21e2-4930-bd5d-9c3dd6562679"));

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("0346fbdd-4286-42b4-8f6e-9f0b899efc21"),
                column: "NgayTao",
                value: new DateTime(2024, 5, 15, 8, 23, 12, 178, DateTimeKind.Local).AddTicks(8883));

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("15fef17d-088b-4b3b-a5a6-bd8760296809"),
                column: "NgayTao",
                value: new DateTime(2024, 5, 15, 8, 23, 12, 178, DateTimeKind.Local).AddTicks(8886));

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("41d68bd1-81a2-402a-bcfe-7f112549cf2e"),
                column: "NgayTao",
                value: new DateTime(2024, 5, 15, 8, 23, 12, 178, DateTimeKind.Local).AddTicks(8879));

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("ca7a8e83-4be8-427d-a469-dd7fb29a8f3f"),
                column: "NgayTao",
                value: new DateTime(2024, 5, 15, 8, 23, 12, 178, DateTimeKind.Local).AddTicks(8891));

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("e4e59bc3-b004-4ab9-b2bd-5109bf62be8d"),
                column: "NgayTao",
                value: new DateTime(2024, 5, 15, 8, 23, 12, 178, DateTimeKind.Local).AddTicks(8875));

            migrationBuilder.UpdateData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("eae74b1d-75cd-45bb-aecd-1f8d5dc7e171"),
                column: "NgayTao",
                value: new DateTime(2024, 5, 15, 8, 23, 12, 178, DateTimeKind.Local).AddTicks(8860));
        }
    }
}
