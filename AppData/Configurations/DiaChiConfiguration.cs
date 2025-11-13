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
    internal class DiaChiConfiguration : IEntityTypeConfiguration<DiaChi>
    {
        public void Configure(EntityTypeBuilder<DiaChi> builder)
        {
            builder.ToTable("DiaChi");
            builder.HasKey(d => d.ID);

            builder.Property(d => d.TenNguoiNhan)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(d => d.SoDienThoai)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(d => d.DiaChiCuThe)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(d => d.TinhThanh)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.QuanHuyen)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.PhuongXa)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(d => d.IsDefault)
                .IsRequired();

            // === THÊM 3 CẤU HÌNH MỚI CHO CÁC CỘT GHN ===
            // (IsRequired(false) nghĩa là cho phép NULL)
            builder.Property(d => d.ProvinceID)
                .IsRequired(false);

            builder.Property(d => d.DistrictID)
                .IsRequired(false);

            builder.Property(d => d.WardCode)
                .IsRequired(false)
                .HasMaxLength(50); // Đặt độ dài cho WardCode (là string)
            // === KẾT THÚC PHẦN THÊM ===

            builder.HasOne(d => d.KhachHang)
                .WithMany(kh => kh.DiaChis)
                .HasForeignKey(d => d.IDKhachHang)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}