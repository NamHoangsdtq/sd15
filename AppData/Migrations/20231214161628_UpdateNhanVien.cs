using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class UpdateNhanVien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anh_SanPham_IDSanPham",
                table: "Anh");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "NhanVien",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)");

            migrationBuilder.AlterColumn<Guid>(
                name: "IDSanPham",
                table: "Anh",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Anh_SanPham_IDSanPham",
                table: "Anh",
                column: "IDSanPham",
                principalTable: "SanPham",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anh_SanPham_IDSanPham",
                table: "Anh");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "NhanVien",
                type: "nvarchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<Guid>(
                name: "IDSanPham",
                table: "Anh",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Anh_SanPham_IDSanPham",
                table: "Anh",
                column: "IDSanPham",
                principalTable: "SanPham",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
