using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppData.Migrations
{
    public partial class AddAnhBienThe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Anh_ChiTietBienThe_IDChiTietBienThe",
                table: "Anh");

            migrationBuilder.DropIndex(
                name: "IX_Anh_IDChiTietBienThe",
                table: "Anh");

            migrationBuilder.DropColumn(
                name: "IDChiTietBienThe",
                table: "Anh");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgaySinh",
                table: "KhachHang",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AlterColumn<string>(
                name: "BinhLuan",
                table: "DanhGia",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.CreateTable(
                name: "BienTheAnh",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdAnh = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdBienThe = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BienTheAnh", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BienTheAnh_Anh_IdAnh",
                        column: x => x.IdAnh,
                        principalTable: "Anh",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BienTheAnh_BienThe_IdBienThe",
                        column: x => x.IdBienThe,
                        principalTable: "BienThe",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BienTheAnh_IdAnh",
                table: "BienTheAnh",
                column: "IdAnh");

            migrationBuilder.CreateIndex(
                name: "IX_BienTheAnh_IdBienThe",
                table: "BienTheAnh",
                column: "IdBienThe");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BienTheAnh");

            migrationBuilder.AlterColumn<DateTime>(
                name: "NgaySinh",
                table: "KhachHang",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BinhLuan",
                table: "DanhGia",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AddColumn<Guid>(
                name: "IDChiTietBienThe",
                table: "Anh",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Anh_IDChiTietBienThe",
                table: "Anh",
                column: "IDChiTietBienThe");

            migrationBuilder.AddForeignKey(
                name: "FK_Anh_ChiTietBienThe_IDChiTietBienThe",
                table: "Anh",
                column: "IDChiTietBienThe",
                principalTable: "ChiTietBienThe",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
