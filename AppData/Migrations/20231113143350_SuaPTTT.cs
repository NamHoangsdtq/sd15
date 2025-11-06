using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class SuaPTTT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietPTTT");

            migrationBuilder.DropTable(
                name: "PhuongThucThanhToan");

            migrationBuilder.AlterColumn<string>(
                name: "PhuongThucThanhToan",
                table: "HoaDon",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayDanhGia",
                table: "DanhGia",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NgayDanhGia",
                table: "DanhGia");

            migrationBuilder.AlterColumn<string>(
                name: "PhuongThucThanhToan",
                table: "HoaDon",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PhuongThucThanhToan",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhuongThucThanhToan", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietPTTT",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDPTTT = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoTien = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietPTTT", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChiTietPTTT_HoaDon_IDHoaDon",
                        column: x => x.IDHoaDon,
                        principalTable: "HoaDon",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietPTTT_PhuongThucThanhToan_IDPTTT",
                        column: x => x.IDPTTT,
                        principalTable: "PhuongThucThanhToan",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPTTT_IDHoaDon",
                table: "ChiTietPTTT",
                column: "IDHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPTTT_IDPTTT",
                table: "ChiTietPTTT",
                column: "IDPTTT");
        }
    }
}
