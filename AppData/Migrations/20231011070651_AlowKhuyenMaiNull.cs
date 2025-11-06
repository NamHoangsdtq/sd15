using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class AlowKhuyenMaiNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BienThe_KhuyenMai_IDKhuyenMai",
                table: "BienThe");

            migrationBuilder.AlterColumn<Guid>(
                name: "IDKhuyenMai",
                table: "BienThe",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_BienThe_KhuyenMai_IDKhuyenMai",
                table: "BienThe",
                column: "IDKhuyenMai",
                principalTable: "KhuyenMai",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BienThe_KhuyenMai_IDKhuyenMai",
                table: "BienThe");

            migrationBuilder.AlterColumn<Guid>(
                name: "IDKhuyenMai",
                table: "BienThe",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BienThe_KhuyenMai_IDKhuyenMai",
                table: "BienThe",
                column: "IDKhuyenMai",
                principalTable: "KhuyenMai",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
