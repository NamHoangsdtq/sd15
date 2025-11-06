using AppData.Models;
using AppData.ViewModels.SanPham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class ChiTietSanPhamViewModel
    {
        public Guid ID { get; set; }
        public string Ten { get; set; }
        public int SoLuong { get; set; }
        public int GiaBan { get; set; } 
        public int GiaGoc { get; set; }
        public int TrangThai { get; set; }
        public string? Anh { get; set; }
        public string? MauSac { get; set; }
        public string KichCo { get; set; }
        public string? MaCTSP { get; set; }
        public int? GiaTriKM { get; set; }
        public int? TrangThaiKM { get; set; }
    }
}
