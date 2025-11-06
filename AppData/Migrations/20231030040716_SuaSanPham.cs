using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class SuaSanPham : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietGioHang_BienThe_IDBienThe",
                table: "ChiTietGioHang");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDon_BienThe_IDBienThe",
                table: "ChiTietHoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGia_BienThe_IDBienThe",
                table: "DanhGia");

            migrationBuilder.DropForeignKey(
                name: "FK_DanhGia_KhachHang_IDKhachHang",
                table: "DanhGia");

            migrationBuilder.DropTable(
                name: "BienTheAnh");

            migrationBuilder.DropTable(
                name: "ChiTietBienThe");

            migrationBuilder.DropTable(
                name: "ThuocTinhSanPham");

            migrationBuilder.DropTable(
                name: "BienThe");

            migrationBuilder.DropTable(
                name: "GiaTri");

            migrationBuilder.DropTable(
                name: "ThuocTinh");

            migrationBuilder.DropIndex(
                name: "IX_DanhGia_IDBienThe",
                table: "DanhGia");

            migrationBuilder.DropIndex(
                name: "IX_DanhGia_IDKhachHang",
                table: "DanhGia");

            migrationBuilder.DropColumn(
                name: "IDBienThe",
                table: "DanhGia");

            migrationBuilder.DropColumn(
                name: "IDKhachHang",
                table: "DanhGia");

            migrationBuilder.DropColumn(
                name: "Ten",
                table: "Anh");

            migrationBuilder.RenameColumn(
                name: "IDBienThe",
                table: "ChiTietHoaDon",
                newName: "IDCTSP");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietHoaDon_IDBienThe",
                table: "ChiTietHoaDon",
                newName: "IX_ChiTietHoaDon_IDCTSP");

            migrationBuilder.RenameColumn(
                name: "IDBienThe",
                table: "ChiTietGioHang",
                newName: "IDCTSP");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietGioHang_IDBienThe",
                table: "ChiTietGioHang",
                newName: "IX_ChiTietGioHang_IDCTSP");

            migrationBuilder.AddColumn<Guid>(
                name: "IDChatLieu",
                table: "SanPham",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "TongDanhGia",
                table: "SanPham",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TongSoSao",
                table: "SanPham",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "PhuongThucThanhToan",
                table: "HoaDon",
                type: "nvarchar(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "DiaChi",
                table: "HoaDon",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AddColumn<string>(
                name: "DuongDan",
                table: "Anh",
                type: "varchar(100)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "IDMauSac",
                table: "Anh",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IDSanPham",
                table: "Anh",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ChatLieu",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatLieu", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "KichCo",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KichCo", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MauSac",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    Ma = table.Column<string>(type: "varchar(10)", nullable: true),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MauSac", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietSanPham",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    GiaBan = table.Column<int>(type: "int", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false),
                    IDSanPham = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDKhuyenMai = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IDMauSac = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDKichCo = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietSanPham", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChiTietSanPham_KhuyenMai_IDKhuyenMai",
                        column: x => x.IDKhuyenMai,
                        principalTable: "KhuyenMai",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_ChiTietSanPham_KichCo_IDKichCo",
                        column: x => x.IDKichCo,
                        principalTable: "KichCo",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietSanPham_MauSac_IDMauSac",
                        column: x => x.IDMauSac,
                        principalTable: "MauSac",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietSanPham_SanPham_IDSanPham",
                        column: x => x.IDSanPham,
                        principalTable: "SanPham",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_IDChatLieu",
                table: "SanPham",
                column: "IDChatLieu");

            migrationBuilder.CreateIndex(
                name: "IX_Anh_IDMauSac",
                table: "Anh",
                column: "IDMauSac");

            migrationBuilder.CreateIndex(
                name: "IX_Anh_IDSanPham",
                table: "Anh",
                column: "IDSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietSanPham_IDKhuyenMai",
                table: "ChiTietSanPham",
                column: "IDKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietSanPham_IDKichCo",
                table: "ChiTietSanPham",
                column: "IDKichCo");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietSanPham_IDMauSac",
                table: "ChiTietSanPham",
                column: "IDMauSac");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietSanPham_IDSanPham",
                table: "ChiTietSanPham",
                column: "IDSanPham");

            migrationBuilder.AddForeignKey(
                name: "FK_Anh_MauSac_IDMauSac",
                table: "Anh",
                column: "IDMauSac",
                principalTable: "MauSac",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Anh_SanPham_IDSanPham",
                table: "Anh",
                column: "IDSanPham",
                principalTable: "SanPham",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietGioHang_ChiTietSanPham_IDCTSP",
                table: "ChiTietGioHang",
                column: "IDCTSP",
                principalTable: "ChiTietSanPham",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDon_ChiTietSanPham_IDCTSP",
                table: "ChiTietHoaDon",
                column: "IDCTSP",
                principalTable: "ChiTietSanPham",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDon_DanhGia_ID",
                table: "ChiTietHoaDon",
                column: "ID",
                principalTable: "DanhGia",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SanPham_ChatLieu_IDChatLieu",
                table: "SanPham",
                column: "IDChatLieu",
                principalTable: "ChatLieu",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anh_MauSac_IDMauSac",
                table: "Anh");

            migrationBuilder.DropForeignKey(
                name: "FK_Anh_SanPham_IDSanPham",
                table: "Anh");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietGioHang_ChiTietSanPham_IDCTSP",
                table: "ChiTietGioHang");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDon_ChiTietSanPham_IDCTSP",
                table: "ChiTietHoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietHoaDon_DanhGia_ID",
                table: "ChiTietHoaDon");

            migrationBuilder.DropForeignKey(
                name: "FK_SanPham_ChatLieu_IDChatLieu",
                table: "SanPham");

            migrationBuilder.DropTable(
                name: "ChatLieu");

            migrationBuilder.DropTable(
                name: "ChiTietSanPham");

            migrationBuilder.DropTable(
                name: "KichCo");

            migrationBuilder.DropTable(
                name: "MauSac");

            migrationBuilder.DropIndex(
                name: "IX_SanPham_IDChatLieu",
                table: "SanPham");

            migrationBuilder.DropIndex(
                name: "IX_Anh_IDMauSac",
                table: "Anh");

            migrationBuilder.DropIndex(
                name: "IX_Anh_IDSanPham",
                table: "Anh");

            migrationBuilder.DropColumn(
                name: "IDChatLieu",
                table: "SanPham");

            migrationBuilder.DropColumn(
                name: "TongDanhGia",
                table: "SanPham");

            migrationBuilder.DropColumn(
                name: "TongSoSao",
                table: "SanPham");

            migrationBuilder.DropColumn(
                name: "DuongDan",
                table: "Anh");

            migrationBuilder.DropColumn(
                name: "IDMauSac",
                table: "Anh");

            migrationBuilder.DropColumn(
                name: "IDSanPham",
                table: "Anh");

            migrationBuilder.RenameColumn(
                name: "IDCTSP",
                table: "ChiTietHoaDon",
                newName: "IDBienThe");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietHoaDon_IDCTSP",
                table: "ChiTietHoaDon",
                newName: "IX_ChiTietHoaDon_IDBienThe");

            migrationBuilder.RenameColumn(
                name: "IDCTSP",
                table: "ChiTietGioHang",
                newName: "IDBienThe");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietGioHang_IDCTSP",
                table: "ChiTietGioHang",
                newName: "IX_ChiTietGioHang_IDBienThe");

            migrationBuilder.AlterColumn<string>(
                name: "PhuongThucThanhToan",
                table: "HoaDon",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChi",
                table: "HoaDon",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IDBienThe",
                table: "DanhGia",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "IDKhachHang",
                table: "DanhGia",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Ten",
                table: "Anh",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BienThe",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDKhuyenMai = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IDSanPham = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GiaBan = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "Datetime", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BienThe", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BienThe_KhuyenMai_IDKhuyenMai",
                        column: x => x.IDKhuyenMai,
                        principalTable: "KhuyenMai",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_BienThe_SanPham_IDSanPham",
                        column: x => x.IDSanPham,
                        principalTable: "SanPham",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThuocTinh",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "Datetime", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuocTinh", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BienTheAnh",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdAnh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdBienThe = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BienTheAnh", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BienTheAnh_Anh_IdAnh",
                        column: x => x.IdAnh,
                        principalTable: "Anh",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BienTheAnh_BienThe_IdBienThe",
                        column: x => x.IdBienThe,
                        principalTable: "BienThe",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GiaTri",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdThuocTinh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ten = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiaTri", x => x.ID);
                    table.ForeignKey(
                        name: "FK_GiaTri_ThuocTinh_IdThuocTinh",
                        column: x => x.IdThuocTinh,
                        principalTable: "ThuocTinh",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThuocTinhSanPham",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDSanPham = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDThuocTinh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayLuu = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThuocTinhSanPham", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ThuocTinhSanPham_SanPham_IDSanPham",
                        column: x => x.IDSanPham,
                        principalTable: "SanPham",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ThuocTinhSanPham_ThuocTinh_IDThuocTinh",
                        column: x => x.IDThuocTinh,
                        principalTable: "ThuocTinh",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietBienThe",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDBienThe = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IDGiaTri = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayLuu = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TrangThai = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietBienThe", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ChiTietBienThe_BienThe_IDBienThe",
                        column: x => x.IDBienThe,
                        principalTable: "BienThe",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietBienThe_GiaTri_IDGiaTri",
                        column: x => x.IDGiaTri,
                        principalTable: "GiaTri",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_IDBienThe",
                table: "DanhGia",
                column: "IDBienThe");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGia_IDKhachHang",
                table: "DanhGia",
                column: "IDKhachHang");

            migrationBuilder.CreateIndex(
                name: "IX_BienThe_IDKhuyenMai",
                table: "BienThe",
                column: "IDKhuyenMai");

            migrationBuilder.CreateIndex(
                name: "IX_BienThe_IDSanPham",
                table: "BienThe",
                column: "IDSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_BienTheAnh_IdAnh",
                table: "BienTheAnh",
                column: "IdAnh");

            migrationBuilder.CreateIndex(
                name: "IX_BienTheAnh_IdBienThe",
                table: "BienTheAnh",
                column: "IdBienThe");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietBienThe_IDBienThe",
                table: "ChiTietBienThe",
                column: "IDBienThe");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietBienThe_IDGiaTri",
                table: "ChiTietBienThe",
                column: "IDGiaTri");

            migrationBuilder.CreateIndex(
                name: "IX_GiaTri_IdThuocTinh",
                table: "GiaTri",
                column: "IdThuocTinh");

            migrationBuilder.CreateIndex(
                name: "IX_ThuocTinhSanPham_IDSanPham",
                table: "ThuocTinhSanPham",
                column: "IDSanPham");

            migrationBuilder.CreateIndex(
                name: "IX_ThuocTinhSanPham_IDThuocTinh",
                table: "ThuocTinhSanPham",
                column: "IDThuocTinh");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietGioHang_BienThe_IDBienThe",
                table: "ChiTietGioHang",
                column: "IDBienThe",
                principalTable: "BienThe",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietHoaDon_BienThe_IDBienThe",
                table: "ChiTietHoaDon",
                column: "IDBienThe",
                principalTable: "BienThe",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGia_BienThe_IDBienThe",
                table: "DanhGia",
                column: "IDBienThe",
                principalTable: "BienThe",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DanhGia_KhachHang_IDKhachHang",
                table: "DanhGia",
                column: "IDKhachHang",
                principalTable: "KhachHang",
                principalColumn: "IDKhachHang",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
