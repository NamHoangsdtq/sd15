using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.BanOffline
{
    public class HoaDonThanhToanViewModel
    {
        public Guid Id { get; set; }
        public string MaHD { get; set; }
        public string KhachHang { get; set; }
        public Guid? IdKhachHang { get; set; }
        public string NhanVien { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public int? TongSL { get; set; }
        public int? TongTien { get; set; }
        public int? DiemTichHD { get; set; }
        public int? DiemKH { get; set; }
    }
}
