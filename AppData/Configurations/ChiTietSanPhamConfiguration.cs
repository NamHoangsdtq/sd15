using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppData.Configurations
{
    internal class ChiTietSanPhamConfiguration : IEntityTypeConfiguration<ChiTietSanPham>
    {
        public void Configure(EntityTypeBuilder<ChiTietSanPham> builder)
        {
            builder.ToTable("ChiTietSanPham");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Ma).HasColumnType("nvarchar(100)");
            builder.Property(x => x.SoLuong).HasColumnType("int");
            builder.Property(x => x.GiaBan).HasColumnType("int");
            builder.Property(x => x.NgayTao).HasColumnType("datetime");
            builder.Property(x => x.TrangThai).HasColumnType("int");
            builder.HasOne(x => x.MauSac).WithMany(x => x.ChiTietSanPhams).HasForeignKey(x => x.IDMauSac);
            builder.HasOne(x => x.KichCo).WithMany(x => x.ChiTietSanPhams).HasForeignKey(x => x.IDKichCo);
            builder.HasOne(x => x.SanPham).WithMany(x => x.ChiTietSanPhams).HasForeignKey(x => x.IDSanPham);
            builder.HasOne(x => x.KhuyenMai).WithMany(x => x.ChiTietSanPhams).HasForeignKey(x => x.IDKhuyenMai);
            //builder.HasData(new ChiTietSanPham()
            //{
            //    ID = new Guid("EAE74B1D-75CD-45BB-AECD-1F8D5DC7E171"),
            //    SoLuong = 100,
            //    GiaBan = 100000,
            //    NgayTao = DateTime.Now,
            //    TrangThai = 1,
            //    IDSanPham = new Guid("E3515CB9-F3AF-4FB3-A100-50ECF0AA81D3"),
            //    IDMauSac = new Guid("579090A3-3F65-4CEC-A71E-A756E9CE2F85"),
            //    IDKichCo = new Guid("C2A86244-1BB5-416A-BDE3-0FFEC26B4DB4"),
            //    Ma = "SP1DOS"
            //});
            //builder.HasData(new ChiTietSanPham()
            //{
            //    ID = new Guid("E4E59BC3-B004-4AB9-B2BD-5109BF62BE8D"),
            //    SoLuong = 100,
            //    GiaBan = 100000,
            //    NgayTao = DateTime.Now,
            //    TrangThai = 2,
            //    IDSanPham = new Guid("E3515CB9-F3AF-4FB3-A100-50ECF0AA81D3"),
            //    IDMauSac = new Guid("579090A3-3F65-4CEC-A71E-A756E9CE2F85"),
            //    IDKichCo = new Guid("EA11A0F2-0FFC-4FEF-8674-03B035E46FEA"),
            //    Ma = "SP1DOL"
            //});
            //builder.HasData(new ChiTietSanPham()
            //{
            //    ID = new Guid("41D68BD1-81A2-402A-BCFE-7F112549CF2E"),
            //    SoLuong = 100,
            //    GiaBan = 100000,
            //    NgayTao = DateTime.Now,
            //    TrangThai = 2,
            //    IDSanPham = new Guid("E3515CB9-F3AF-4FB3-A100-50ECF0AA81D3"),
            //    IDMauSac = new Guid("FC23A8DF-F7CF-4863-85C7-3781AC1873F7"),
            //    IDKichCo = new Guid("EA11A0F2-0FFC-4FEF-8674-03B035E46FEA"),
            //    Ma = "SP1DENL"
            //});
            //builder.HasData(new ChiTietSanPham()
            //{
            //    ID = new Guid("0346FBDD-4286-42B4-8F6E-9F0B899EFC21"),
            //    SoLuong = 100,
            //    GiaBan = 100000,
            //    NgayTao = DateTime.Now,
            //    TrangThai = 2,
            //    IDSanPham = new Guid("E3515CB9-F3AF-4FB3-A100-50ECF0AA81D3"),
            //    IDMauSac = new Guid("579090A3-3F65-4CEC-A71E-A756E9CE2F85"),
            //    IDKichCo = new Guid("4FAD0BB8-4A56-4751-A6A4-A6387E30702E"),
            //    Ma = "SP1DOM"
            //});
            //builder.HasData(new ChiTietSanPham()
            //{
            //    ID = new Guid("15FEF17D-088B-4B3B-A5A6-BD8760296809"),
            //    SoLuong = 100,
            //    GiaBan = 100000,
            //    NgayTao = DateTime.Now,
            //    TrangThai = 2,
            //    IDSanPham = new Guid("E3515CB9-F3AF-4FB3-A100-50ECF0AA81D3"),
            //    IDMauSac = new Guid("FC23A8DF-F7CF-4863-85C7-3781AC1873F7"),
            //    IDKichCo = new Guid("4FAD0BB8-4A56-4751-A6A4-A6387E30702E"),
            //    Ma = "SP1DENM"
            //});
            //builder.HasData(new ChiTietSanPham()
            //{
            //    ID = new Guid("CA7A8E83-4BE8-427D-A469-DD7FB29A8F3F"),
            //    SoLuong = 100,
            //    GiaBan = 100000,
            //    NgayTao = DateTime.Now,
            //    TrangThai = 2,
            //    IDSanPham = new Guid("E3515CB9-F3AF-4FB3-A100-50ECF0AA81D3"),
            //    IDMauSac = new Guid("FC23A8DF-F7CF-4863-85C7-3781AC1873F7"),
            //    IDKichCo = new Guid("C2A86244-1BB5-416A-BDE3-0FFEC26B4DB4"),
            //    Ma = "SP1DENS"
            //});
        }
    }
}
