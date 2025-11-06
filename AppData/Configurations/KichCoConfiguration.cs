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
    internal class KichCoConfiguration : IEntityTypeConfiguration<KichCo>
    {
        public void Configure(EntityTypeBuilder<KichCo> builder)
        {
            builder.ToTable("KichCo");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Ten).HasColumnType("nvarchar(10)").IsRequired();
            builder.Property(x => x.TrangThai).HasColumnType("int").IsRequired();
            //builder.HasData(new KichCo() {ID = new Guid("EA11A0F2-0FFC-4FEF-8674-03B035E46FEA"),Ten = "S" , TrangThai  = 1 });
            //builder.HasData(new KichCo() {ID = new Guid("C2A86244-1BB5-416A-BDE3-0FFEC26B4DB4"),Ten = "M" , TrangThai  = 1 });
            //builder.HasData(new KichCo() {ID = new Guid("4FAD0BB8-4A56-4751-A6A4-A6387E30702E"),Ten = "L" , TrangThai  = 1 });
        }
    }
}
