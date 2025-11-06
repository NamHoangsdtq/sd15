using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class Anh
    {
        public Guid ID { get; set; }
        public string? DuongDan { get; set; }
        public int TrangThai { get; set; }
        public Guid? IDMauSac { get; set; }
        public Guid? IDSanPham { get; set; }
        //public virtual MauSac? MauSac { get; set; }
        public virtual SanPham? SanPham { get; set; }
    }
}
