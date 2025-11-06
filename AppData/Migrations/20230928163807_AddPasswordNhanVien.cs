using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class AddPasswordNhanVien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PassWord",
                table: "NhanVien",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassWord",
                table: "NhanVien");
        }
    }
}
