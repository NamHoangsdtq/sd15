using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class SuaMoTaSanPham : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MoTa",
                table: "SanPham",
                type: "nvarchar(300)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MoTa",
                table: "SanPham",
                type: "nvarchar(300)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldNullable: true);
        }
    }
}
