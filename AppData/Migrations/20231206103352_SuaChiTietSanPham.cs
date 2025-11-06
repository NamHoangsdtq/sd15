using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class SuaChiTietSanPham : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Ma",
                table: "ChiTietSanPham",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Ma",
                table: "ChiTietSanPham",
                type: "nvarchar(10)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);
        }
    }
}
