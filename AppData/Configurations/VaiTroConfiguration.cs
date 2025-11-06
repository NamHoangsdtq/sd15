using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configurations
{
    public class VaiTroConfiguration : IEntityTypeConfiguration<VaiTro>
    {
        public void Configure(EntityTypeBuilder<VaiTro> builder)
        {
            builder.ToTable("VaiTro");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Ten).HasColumnType("nvarchar(20)").IsRequired();
            builder.Property(x => x.TrangThai).HasColumnType("int");
            builder.HasData(new VaiTro() { ID = new Guid("B4996B2D-A343-434B-BFE9-09F8EFBB3852"), Ten = "Admin", TrangThai = 1 });
            builder.HasData(new VaiTro() { ID = new Guid("952c1a5d-74ff-4daf-ba88-135c5440809c"), Ten = "Nhân viên", TrangThai = 1 });
        }
    }
}
