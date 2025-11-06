using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class DonMuaViewModel
    {
        public Guid? IdNguoiDung {  get; set; }
        public string? MaHD { get; set; }
        public Guid IDLichSu { get; set; }
        public int Diem { get; set; }
        public int TrangThaiLSTD { get; set; }
        public Guid? IDVoucher { get; set; }
        public int GiaTri { get; set; }
        public int HinhThucGiamGia { get; set; }
        public Guid? IDHoaDon { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime? NgayThanhToan { get; set; }
        public DateTime? NgayNhanHang { get; set; }
        public string? Ngaytao1 { get; set; }
        public string? Ngaythanhtoan1 { get; set; }
        public string? Ngaynhanhang1 { get; set; }
        public string? TenNguoiNhan { get; set; }
        public string? SDT { get; set; }
        public string? Email { get; set; }
        public string? DiaChi { get; set; }
        public int TienShip { get; set; }
        public int TrangThaiGiaoHang { get; set; }
        public int? TongTien { get; set; }
        public int TiLeTichDiem { get; set; }
        public int TiLeTieuDiem { get; set; }
        public int LoaiHoaDon { get; set; }
    }
}
