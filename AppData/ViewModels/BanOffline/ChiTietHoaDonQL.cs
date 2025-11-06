using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.BanOffline
{
    public class ChiTietHoaDonQL
    {
        public Guid Id { get; set; }
        public string MaHD { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public DateTime? NgayNhanHang { get; set; }
        public string? NhanVien { get; set; }
        public string KhachHang { get; set; }
        public string? NguoiNhan { get; set; }
        public string? SĐT { get; set; }
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
        public int? TienShip { get; set; }
        public int TrangThai { get; set; }
        public string PTTT { get; set; }
        public int? TruTieuDiem { get; set; }
        public string GhiChu { get; set; }
        public int? KhachCanTra { get; set; }
        public int? TienKhachTra { get; set; }
        public List<HoaDonChiTietViewModel> listsp { get; set; }
        public List<LichSuTichDiem> lstlstd { get; set; }
        public Voucher voucher { get; set; }
        public int LoaiHD { get; set; }
    }
}
