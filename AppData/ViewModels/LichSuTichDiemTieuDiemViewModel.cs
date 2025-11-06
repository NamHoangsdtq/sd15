using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class LichSuTichDiemTieuDiemViewModel
    {
        public Guid? IdNguoiDung { get; set; }
        public string? MaHD { get; set; }
        public Guid IDLichSu { get; set; }
        public int Diem { get; set; }
        public int TrangThaiLSTD { get; set; }
        public Guid? IDHoaDon { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public DateTime? NgayNhanHang { get; set; }
        public string? Ngaytao1 { get; set; }
        public string? Ngaythanhtoan1 { get; set; }
        public string? Ngaynhanhang1 { get; set; }
        public string? TenNguoiNhan { get; set; }
        public int TrangThaiGiaoHang { get; set; }
        public int LoaiHoaDon { get; set; }
    }
}
