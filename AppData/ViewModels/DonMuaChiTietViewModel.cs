using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class DonMuaChiTietViewModel
    {
        public Guid ID { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public string? TenNguoiNhan { get; set; }
        public string? SDT { get; set; }
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
        public int TienShip { get; set; }
        public string? PhuongThucThanhToan { get; set; }
        public int TrangThaiGiaoHang { get; set; }
        public int Diem { get; set; }
        public int TrangThaiLichSuTichDiem { get; set; }
        public Guid IDCTHD { get; set; }
        public int DonGia { get; set; }
        public int SoLuong { get; set; }
        public string TenKichCo { get; set; }
        public string TenMau { get; set; }
        public string TenSanPham { get; set;}
        public string DuongDan { get; set; }
        public int TrangThaiDanhGia { get; set; }
        public int? GiaTri { get; set; }
        public int? HinhThucGiamGia { get; set; }
        public int TiLeTieuDiem { get; set; }
        public int TiLeTichDiem { get; set; }
        public int LoaiHoaDon { get; set; }
        public List<LichSuTichDiem>? lichSuTichDiems { get; set; }
    }
}
