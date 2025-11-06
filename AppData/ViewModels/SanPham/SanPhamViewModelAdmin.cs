using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.SanPham
{
    public class SanPhamViewModelAdmin
    {
        public Guid ID { get; set; }
        public string Ten { get; set; }
        public string Ma { get; set; }
        public string? Anh { get; set; }
        public int GiaBan { get; set; }
        public int GiaGoc { get; set; }
        public int SoLuong { get; set; }
        public string ChatLieu { get; set; }
        public string? LoaiSPCha { get; set; }
        public string? LoaiSPCon { get; set; }
        public int TrangThai { get; set; }
        public Guid? IDKhuyenMai { get; set; }
    }
}
