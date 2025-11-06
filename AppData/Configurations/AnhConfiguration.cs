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
    internal class AnhConfiguration : IEntityTypeConfiguration<Anh>
    {
        public void Configure(EntityTypeBuilder<Anh> builder)
        {
            builder.ToTable("Anh");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.DuongDan).HasColumnType("varchar(100)").IsRequired();
            builder.Property(x => x.TrangThai).HasColumnType("int").IsRequired();
            //builder.HasOne(x => x.MauSac).WithMany(x => x.Anhs).HasForeignKey(x => x.IDMauSac);
            builder.HasOne(x => x.SanPham).WithMany(x => x.Anhs).HasForeignKey(x => x.IDSanPham);
            //builder.HasData(new Anh() { ID = new Guid("3DA49D96-68BF-4618-8574-01ECE60AEA75"), TrangThai = 1, DuongDan = "1953198247766_ef28dde5aa_o_211f9ee337bc4c1fb7c5933b8a84a38a_compact242657303.webp", IDMauSac = new Guid("FC23A8DF-F7CF-4863-85C7-3781AC1873F7"), IDSanPham = new Guid("E3515CB9-F3AF-4FB3-A100-50ECF0AA81D3") });
            //builder.HasData(new Anh() { ID = new Guid("A539976A-21E2-4930-BD5D-9C3DD6562679"), TrangThai = 1, DuongDan = "1953197790942_bf4bc7352a_o_9c76cdf68bfc41c2936861828ab4003c_master242657308.webp", IDMauSac = new Guid("579090A3-3F65-4CEC-A71E-A756E9CE2F85"), IDSanPham = new Guid("E3515CB9-F3AF-4FB3-A100-50ECF0AA81D3") });
        }
    }
}
