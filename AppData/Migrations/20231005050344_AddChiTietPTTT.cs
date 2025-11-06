using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class AddChiTietPTTT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anh_BienThe_IDBienThe",
                table: "Anh");

            migrationBuilder.DropTable(
                name: "ChiTietKhuyenMai");

            migrationBuilder.DropColumn(
                name: "IDVaiTro",
                table: "KhachHang");

            migrationBuilder.DropColumn(
                name: "Anh",
                table: "BienThe");

            migrationBuilder.RenameColumn(
                name: "IDBienThe",
                table: "Anh",
                newName: "IDChiTietBienThe");

            migrationBuilder.RenameIndex(
                name: "IX_Anh_IDBienThe",
                table: "Anh",
                newName: "IX_Anh_IDChiTietBienThe");

            migrationBuilder.AddColumn<Guid>(
                name: "IDKhuyenMai",
                table: "BienThe",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
                    SoTien = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    IDPTTT = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDHoaDon = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                name: "IX_BienThe_IDKhuyenMai",
                table: "BienThe",
                column: "IDKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPTTT_IDHoaDon",
                table: "ChiTietPTTT",
                column: "IDHoaDon");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietPTTT_IDPTTT",
                table: "ChiTietPTTT",
                column: "IDPTTT");

            migrationBuilder.AddForeignKey(
                name: "FK_Anh_ChiTietBienThe_IDChiTietBienThe",
                table: "Anh",
                column: "IDChiTietBienThe",
                principalTable: "ChiTietBienThe",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BienThe_KhuyenMai_IDKhuyenMai",
                table: "BienThe",
                column: "IDKhuyenMai",
                principalTable: "KhuyenMai",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anh_ChiTietBienThe_IDChiTietBienThe",
                table: "Anh");

            migrationBuilder.DropForeignKey(
                name: "FK_BienThe_KhuyenMai_IDKhuyenMai",
                table: "BienThe");

            migrationBuilder.DropTable(
                name: "ChiTietPTTT");

            migrationBuilder.DropTable(
                name: "PhuongThucThanhToan");

            migrationBuilder.DropIndex(
                name: "IX_BienThe_IDKhuyenMai",
                table: "BienThe");

            migrationBuilder.DropColumn(
                name: "IDKhuyenMai",
                table: "BienThe");

            migrationBuilder.RenameColumn(
                name: "IDChiTietBienThe",
                table: "Anh",
                newName: "IDBienThe");

            migrationBuilder.RenameIndex(
                name: "IX_Anh_IDChiTietBienThe",
                table: "Anh",
                newName: "IX_Anh_IDBienThe");

            migrationBuilder.AddColumn<Guid>(
                name: "IDVaiTro",
                table: "KhachHang",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Anh",
                table: "BienThe",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ChiTietKhuyenMai",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDBienThe = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDKhuyenMai = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietKhuyenMai", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChiTietKhuyenMai_BienThe_IDBienThe",
                        column: x => x.IDBienThe,
                        principalTable: "BienThe",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietKhuyenMai_KhuyenMai_IDKhuyenMai",
                        column: x => x.IDKhuyenMai,
                        principalTable: "KhuyenMai",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietKhuyenMai_IDBienThe",
                table: "ChiTietKhuyenMai",
                column: "IDBienThe");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietKhuyenMai_IDKhuyenMai",
                table: "ChiTietKhuyenMai",
                column: "IDKhuyenMai");

            migrationBuilder.AddForeignKey(
                name: "FK_Anh_BienThe_IDBienThe",
                table: "Anh",
                column: "IDBienThe",
                principalTable: "BienThe",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
