using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.SanPham
{
    public class SanPhamViewModel
    {
        public Guid ID { get; set; }
        public string Ten { get; set; }
        public int TrangThai { get; set; }
        public int TrangThaiCTSP { get; set; }
        public string LoaiSP { get; set; }
        public Guid? IDMauSac { get; set; }
        public Guid? IDKichCo { get; set; }
        public Guid? IDChatLieu { get; set; }
        public DateTime? NgayTao { get; set; }
        public Guid? IdChiTietSanPham { get; set; } // Id của biến thể mặc định
        public int SoLuong { get; set; }
        public string Image { get; set; } // Của biến thể mặc định
        public int GiaBan { get; set; }//Của biến thể mặc định sau khi nhân vs khuyến mãi
        public int? GiaGoc { get; set; }//Của biến thể mặc định
        public Guid? IDKhuyenMai { get; set; }//Của biến thể mặc định
        public int? TrangThaiKM { get; set; }
        public int? GiaTriKM { get; set; }
        public double? soSao { get;set; }
    }
}
