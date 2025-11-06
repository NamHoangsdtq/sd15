using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class SeedingDataSP : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ChatLieu",
                columns: new[] { "ID", "Ten", "TrangThai" },
                values: new object[] { new Guid("273c786d-d0b6-4cbe-8531-6dc997532659"), "Vải", 1 });

            migrationBuilder.InsertData(
                table: "KichCo",
                columns: new[] { "ID", "Ten", "TrangThai" },
                values: new object[,]
                {
                    { new Guid("4fad0bb8-4a56-4751-a6a4-a6387e30702e"), "L", 1 },
                    { new Guid("c2a86244-1bb5-416a-bde3-0ffec26b4db4"), "M", 1 },
                    { new Guid("ea11a0f2-0ffc-4fef-8674-03b035e46fea"), "S", 1 }
                });

            migrationBuilder.InsertData(
                table: "LoaiSP",
                columns: new[] { "ID", "IDLoaiSPCha", "Ten", "TrangThai" },
                values: new object[] { new Guid("a3c72719-e496-4c09-9a8a-024e34704ae2"), null, "Áo", 1 });

            migrationBuilder.InsertData(
                table: "MauSac",
                columns: new[] { "ID", "Ma", "Ten", "TrangThai" },
                values: new object[,]
                {
                    { new Guid("579090a3-3f65-4cec-a71e-a756e9ce2f85"), "#e00000", "Đỏ", 1 },
                    { new Guid("fc23a8df-f7cf-4863-85c7-3781ac1873f7"), "#000000", "Đen", 1 }
                });

            migrationBuilder.InsertData(
                table: "LoaiSP",
                columns: new[] { "ID", "IDLoaiSPCha", "Ten", "TrangThai" },
                values: new object[] { new Guid("b51cf153-d69c-4397-9147-b3d70131be8d"), new Guid("a3c72719-e496-4c09-9a8a-024e34704ae2"), "Áo thun", 1 });

            migrationBuilder.InsertData(
                table: "SanPham",
                columns: new[] { "ID", "IDChatLieu", "IDLoaiSP", "Ma", "MoTa", "Ten", "TrangThai" },
                values: new object[] { new Guid("e3515cb9-f3af-4fb3-a100-50ecf0aa81d3"), new Guid("273c786d-d0b6-4cbe-8531-6dc997532659"), new Guid("b51cf153-d69c-4397-9147-b3d70131be8d"), "SP1", "ko", "Áo thun mùa hè", 1 });

            migrationBuilder.InsertData(
                table: "ChiTietSanPham",
                columns: new[] { "ID", "GiaBan", "IDKhuyenMai", "IDKichCo", "IDMauSac", "IDSanPham", "Ma", "NgayTao", "SoLuong", "TrangThai" },
                values: new object[,]
                {
                    { new Guid("0346fbdd-4286-42b4-8f6e-9f0b899efc21"), 100000, null, new Guid("4fad0bb8-4a56-4751-a6a4-a6387e30702e"), new Guid("579090a3-3f65-4cec-a71e-a756e9ce2f85"), new Guid("e3515cb9-f3af-4fb3-a100-50ecf0aa81d3"), "SP1DOM", new DateTime(2024, 5, 15, 8, 23, 12, 178, DateTimeKind.Local).AddTicks(8883), 100, 2 },
                    { new Guid("15fef17d-088b-4b3b-a5a6-bd8760296809"), 100000, null, new Guid("4fad0bb8-4a56-4751-a6a4-a6387e30702e"), new Guid("fc23a8df-f7cf-4863-85c7-3781ac1873f7"), new Guid("e3515cb9-f3af-4fb3-a100-50ecf0aa81d3"), "SP1DENM", new DateTime(2024, 5, 15, 8, 23, 12, 178, DateTimeKind.Local).AddTicks(8886), 100, 2 },
                    { new Guid("41d68bd1-81a2-402a-bcfe-7f112549cf2e"), 100000, null, new Guid("ea11a0f2-0ffc-4fef-8674-03b035e46fea"), new Guid("fc23a8df-f7cf-4863-85c7-3781ac1873f7"), new Guid("e3515cb9-f3af-4fb3-a100-50ecf0aa81d3"), "SP1DENL", new DateTime(2024, 5, 15, 8, 23, 12, 178, DateTimeKind.Local).AddTicks(8879), 100, 2 },
                    { new Guid("ca7a8e83-4be8-427d-a469-dd7fb29a8f3f"), 100000, null, new Guid("c2a86244-1bb5-416a-bde3-0ffec26b4db4"), new Guid("fc23a8df-f7cf-4863-85c7-3781ac1873f7"), new Guid("e3515cb9-f3af-4fb3-a100-50ecf0aa81d3"), "SP1DENS", new DateTime(2024, 5, 15, 8, 23, 12, 178, DateTimeKind.Local).AddTicks(8891), 100, 2 },
                    { new Guid("e4e59bc3-b004-4ab9-b2bd-5109bf62be8d"), 100000, null, new Guid("ea11a0f2-0ffc-4fef-8674-03b035e46fea"), new Guid("579090a3-3f65-4cec-a71e-a756e9ce2f85"), new Guid("e3515cb9-f3af-4fb3-a100-50ecf0aa81d3"), "SP1DOL", new DateTime(2024, 5, 15, 8, 23, 12, 178, DateTimeKind.Local).AddTicks(8875), 100, 2 },
                    { new Guid("eae74b1d-75cd-45bb-aecd-1f8d5dc7e171"), 100000, null, new Guid("c2a86244-1bb5-416a-bde3-0ffec26b4db4"), new Guid("579090a3-3f65-4cec-a71e-a756e9ce2f85"), new Guid("e3515cb9-f3af-4fb3-a100-50ecf0aa81d3"), "SP1DOS", new DateTime(2024, 5, 15, 8, 23, 12, 178, DateTimeKind.Local).AddTicks(8860), 100, 1 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("0346fbdd-4286-42b4-8f6e-9f0b899efc21"));

            migrationBuilder.DeleteData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("15fef17d-088b-4b3b-a5a6-bd8760296809"));

            migrationBuilder.DeleteData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("41d68bd1-81a2-402a-bcfe-7f112549cf2e"));

            migrationBuilder.DeleteData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("ca7a8e83-4be8-427d-a469-dd7fb29a8f3f"));

            migrationBuilder.DeleteData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("e4e59bc3-b004-4ab9-b2bd-5109bf62be8d"));

            migrationBuilder.DeleteData(
                table: "ChiTietSanPham",
                keyColumn: "ID",
                keyValue: new Guid("eae74b1d-75cd-45bb-aecd-1f8d5dc7e171"));

            migrationBuilder.DeleteData(
                table: "KichCo",
                keyColumn: "ID",
                keyValue: new Guid("4fad0bb8-4a56-4751-a6a4-a6387e30702e"));

            migrationBuilder.DeleteData(
                table: "KichCo",
                keyColumn: "ID",
                keyValue: new Guid("c2a86244-1bb5-416a-bde3-0ffec26b4db4"));

            migrationBuilder.DeleteData(
                table: "KichCo",
                keyColumn: "ID",
                keyValue: new Guid("ea11a0f2-0ffc-4fef-8674-03b035e46fea"));

            migrationBuilder.DeleteData(
                table: "MauSac",
                keyColumn: "ID",
                keyValue: new Guid("579090a3-3f65-4cec-a71e-a756e9ce2f85"));

            migrationBuilder.DeleteData(
                table: "MauSac",
                keyColumn: "ID",
                keyValue: new Guid("fc23a8df-f7cf-4863-85c7-3781ac1873f7"));

            migrationBuilder.DeleteData(
                table: "SanPham",
                keyColumn: "ID",
                keyValue: new Guid("e3515cb9-f3af-4fb3-a100-50ecf0aa81d3"));

            migrationBuilder.DeleteData(
                table: "ChatLieu",
                keyColumn: "ID",
                keyValue: new Guid("273c786d-d0b6-4cbe-8531-6dc997532659"));

            migrationBuilder.DeleteData(
                table: "LoaiSP",
                keyColumn: "ID",
                keyValue: new Guid("b51cf153-d69c-4397-9147-b3d70131be8d"));

            migrationBuilder.DeleteData(
                table: "LoaiSP",
                keyColumn: "ID",
                keyValue: new Guid("a3c72719-e496-4c09-9a8a-024e34704ae2"));
        }
    }
}
