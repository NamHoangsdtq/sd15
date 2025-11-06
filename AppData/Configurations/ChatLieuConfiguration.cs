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
    internal class ChatLieuConfiguration : IEntityTypeConfiguration<ChatLieu>
    {
        public void Configure(EntityTypeBuilder<ChatLieu> builder)
        {
            builder.ToTable("ChatLieu");
            builder.HasKey(x => x.ID);
            builder.Property(x => x.Ten).HasColumnType("nvarchar(20)");
            builder.Property(x => x.TrangThai).HasColumnType("int");
            //builder.HasData(new ChatLieu() { ID = new Guid("273C786D-D0B6-4CBE-8531-6DC997532659"), Ten = "Vải", TrangThai = 1 });
        }
    }
}
