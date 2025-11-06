using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configurations
{
    public class LoaiSPConfiguration : IEntityTypeConfiguration<LoaiSP>
    {
        public void Configure(EntityTypeBuilder<LoaiSP> builder)
        {
            builder.ToTable("LoaiSP");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Ten).HasColumnType("nvarchar(30)").IsRequired();
            builder.Property(x => x.TrangThai).HasColumnType("int");
            builder.HasOne(x => x.LoaiSPCha).WithMany().HasForeignKey(x => x.IDLoaiSPCha);
            //builder.HasData(new LoaiSP() { ID = new Guid("A3C72719-E496-4C09-9A8A-024E34704AE2"), Ten = "Áo", TrangThai = 1});
            //builder.HasData(new LoaiSP() { ID = new Guid("B51CF153-D69C-4397-9147-B3D70131BE8D"), Ten = "Áo thun", TrangThai = 1, IDLoaiSPCha = new Guid("A3C72719-E496-4C09-9A8A-024E34704AE2") });
        }
    }
}
