using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configurations
{
    public class SanPhamConfiguration : IEntityTypeConfiguration<SanPham>
    {
        public void Configure(EntityTypeBuilder<SanPham> builder)
        {
            builder.ToTable("SanPham");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Ten).HasColumnType("nvarchar(200)");
            builder.Property(x => x.Ma).HasColumnType("nvarchar(10)");
            builder.Property(x => x.MoTa).HasColumnType("nvarchar(300)");
            builder.Property(x => x.TrangThai).HasColumnType("int");
            builder.HasOne(x => x.LoaiSP).WithMany(x => x.SanPhams).HasForeignKey(x => x.IDLoaiSP);
            builder.HasOne(x => x.ChatLieu).WithMany(x => x.SanPhams).HasForeignKey(x => x.IDChatLieu);
            //builder.HasData(new SanPham() { ID = new Guid("E3515CB9-F3AF-4FB3-A100-50ECF0AA81D3"), Ten = "Áo thun mùa hè", MoTa = "ko", TrangThai = 1, IDLoaiSP = new Guid("B51CF153-D69C-4397-9147-B3D70131BE8D"), IDChatLieu = new Guid("273C786D-D0B6-4CBE-8531-6DC997532659"), Ma = "SP1" });
        }
    }
}
