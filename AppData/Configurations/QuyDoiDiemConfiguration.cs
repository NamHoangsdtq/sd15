using AppData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AppData.Configurations
{
    public class QuyDoiDiemConfiguration : IEntityTypeConfiguration<QuyDoiDiem>
    {
        public void Configure(EntityTypeBuilder<QuyDoiDiem> builder)
        {
            builder.ToTable("QuyDoiDiem");
            builder.HasKey(x => x.ID);
            //builder.Property(x => x.SoDiem).HasColumnType("int");
            builder.Property(x => x.TiLeTichDiem).HasColumnType("int");
            builder.Property(x => x.TiLeTieuDiem).HasColumnType("int");
            builder.Property(x => x.TrangThai).HasColumnType("int");
            //builder.HasData(new QuyDoiDiem() { ID = new Guid("16BD37C4-CEF0-4E92-9BB5-511C43D99037"),TiLeTichDiem= 0,TiLeTieuDiem= 0 ,TrangThai=1});
        }
    }
}
