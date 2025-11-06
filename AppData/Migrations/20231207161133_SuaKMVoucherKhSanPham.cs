using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class SuaKMVoucherKhSanPham : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TongDanhGia",
                table: "SanPham");

            migrationBuilder.DropColumn(
                name: "TongSoSao",
                table: "SanPham");

            migrationBuilder.AlterColumn<string>(
                name: "MoTa",
                table: "Voucher",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<string>(
                name: "MoTa",
                table: "KhuyenMai",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MoTa",
                table: "Voucher",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

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
                name: "MoTa",
                table: "KhuyenMai",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);
        }
    }
}
