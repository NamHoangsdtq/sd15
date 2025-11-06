using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Configurations
{
    internal class NhanVienConfiguration : IEntityTypeConfiguration<NhanVien>
    {
        public void Configure(EntityTypeBuilder<NhanVien> builder)
        {
            builder.ToTable("NhanVien");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Ten).HasColumnType("nvarchar(20)").IsRequired();
            builder.Property(x => x.Email).HasColumnType("nvarchar(100)").IsRequired();
            builder.Property(x => x.SDT).HasColumnType("nvarchar(20)").IsRequired();
            builder.Property(x => x.DiaChi).HasColumnType("nvarchar(20)").IsRequired();
            builder.Property(x => x.TrangThai).HasColumnType("int");
            builder.HasOne(x => x.VaiTro).WithMany(x => x.NhanViens).HasForeignKey(x => x.IDVaiTro);
            //builder.HasData(new NhanVien() { ID = new Guid("2EC27AB7-5F67-4ED5-AE67-52F9C9726EBF"), Ten = "Admin", Email = "admin@gmail.com", SDT = "0985143915", DiaChi = "Ha Noi", TrangThai = 1, IDVaiTro = new Guid("B4996B2D-A343-434B-BFE9-09F8EFBB3852"), PassWord = "$2a$10$SkimxxBIlrv/l33hTFvbkutV/.jF4rlwd9APgp1ZZjNEgVDYXvHa6" });
        }
    }
}
