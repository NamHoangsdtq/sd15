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
    internal class MauSacConfiguration : IEntityTypeConfiguration<MauSac>
    {
        public void Configure(EntityTypeBuilder<MauSac> builder)
        {
            builder.ToTable("MauSac");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Ten).HasColumnType("nvarchar(10)").IsRequired();
            builder.Property(x => x.Ma).HasColumnType("varchar(10)");
            builder.Property(x => x.TrangThai).HasColumnType("int").IsRequired();
            //builder.HasData(new MauSac() { ID = new Guid("FC23A8DF-F7CF-4863-85C7-3781AC1873F7"), Ten = "Đen", Ma = "#000000", TrangThai = 1 });
            //builder.HasData(new MauSac() { ID = new Guid("579090A3-3F65-4CEC-A71E-A756E9CE2F85"), Ten = "Đỏ", Ma = "#e00000", TrangThai = 1 });
        }
    }
}
