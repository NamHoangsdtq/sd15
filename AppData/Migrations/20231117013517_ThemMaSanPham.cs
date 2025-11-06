using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class ThemMaSanPham : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Ma",
                table: "SanPham",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ma",
                table: "ChiTietSanPham",
                type: "nvarchar(10)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ma",
                table: "SanPham");

            migrationBuilder.DropColumn(
                name: "Ma",
                table: "ChiTietSanPham");
        }
    }
}
