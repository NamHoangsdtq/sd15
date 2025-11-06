using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class xoaVatsdiem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoDiem",
                table: "QuyDoiDiem");

            migrationBuilder.DropColumn(
                name: "ThueVAT",
                table: "HoaDon");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "KhachHang",
                type: "varchar(250)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SoDiem",
                table: "QuyDoiDiem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "KhachHang",
                type: "varchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(250)");

            migrationBuilder.AddColumn<int>(
                name: "ThueVAT",
                table: "HoaDon",
                type: "int",
                nullable: true);
        }
    }
}
