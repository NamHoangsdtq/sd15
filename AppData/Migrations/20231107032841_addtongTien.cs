using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class addtongTien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoaiHD",
                table: "HoaDon",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MaHD",
                table: "HoaDon",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ThueVAT",
                table: "HoaDon",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TongTien",
                table: "HoaDon",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoaiHD",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "MaHD",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "ThueVAT",
                table: "HoaDon");

            migrationBuilder.DropColumn(
                name: "TongTien",
                table: "HoaDon");
        }
    }
}
