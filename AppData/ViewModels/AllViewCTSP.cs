using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class AllViewCTSP
    {
        public Guid ID { get; set; }
        public string? MaCTSP { get; set; }
        public string TenSanPham { get; set; }
        public string TenAnh { get; set; }
        public Guid? IdKhuyenMai { get; set; }
        public string TenMauSac { get; set; }
        public string? MaMauSac { get; set; }
        public string TenKichCo { get; set; }
        public int SoLuong { get; set; }
        public int GiaGoc { get; set; }
        public int GiaKhuyenMai { get; set; }
        public DateTime NgayTao { get; set; }
        public int TrangThai { get; set; }
        
        
    }
}
