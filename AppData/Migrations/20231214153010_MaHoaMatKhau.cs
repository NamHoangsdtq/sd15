using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class MaHoaMatKhau : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "KhachHang",
                type: "varchar(MAX)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(15)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "KhachHang",
                type: "varchar(15)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)");
        }
    }
}
