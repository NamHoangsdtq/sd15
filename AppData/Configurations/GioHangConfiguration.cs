using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configurations
{
    public class GioHangConfiguration : IEntityTypeConfiguration<GioHang>
    {
        public void Configure(EntityTypeBuilder<GioHang> builder)
        {
            builder.ToTable("GioHang");
            builder.HasKey(x => x.IDKhachHang);
            builder.Property(x => x.NgayTao).HasColumnType("datetime");
            builder.HasOne(x => x.KhachHang).WithOne(x => x.GioHang).HasForeignKey<KhachHang>();
        }
    }
}
