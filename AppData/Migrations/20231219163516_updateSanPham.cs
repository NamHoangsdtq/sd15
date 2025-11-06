using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class updateSanPham : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MoTa",
                table: "SanPham",
                type: "nvarchar(300)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "NhanVien",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DiaChi",
                table: "NhanVien",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "Ten",
                table: "MauSac",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)");

            migrationBuilder.AlterColumn<string>(
                name: "Ten",
                table: "LoaiSP",
                type: "nvarchar(30)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "KhachHang",
                type: "varchar(250)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(250)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MoTa",
                table: "SanPham",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)");

            migrationBuilder.AlterColumn<int>(
                name: "TrangThai",
                table: "NhanVien",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DiaChi",
                table: "NhanVien",
                type: "nvarchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Ten",
                table: "MauSac",
                type: "nvarchar(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Ten",
                table: "LoaiSP",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "KhachHang",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(250)",
                oldNullable: true);
        }
    }
}
