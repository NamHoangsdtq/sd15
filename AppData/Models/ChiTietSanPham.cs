using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class ChiTietSanPham
    {
        public Guid ID { get; set; }
        public string? Ma { get; set; }
        public int SoLuong { get; set; }
        public int GiaBan { get; set; }
        public DateTime NgayTao { get; set; }
        public int TrangThai { get; set; }
        // 1 - mặc định
        // 2 - ko mặc định
        // 0 - xóa
        public Guid IDSanPham { get; set; }
        public Guid? IDKhuyenMai { get; set; }
        public Guid IDMauSac { get; set; }
        public Guid IDKichCo { get; set; }
        public virtual KichCo KichCo { get;set; }
        public virtual MauSac MauSac { get;set; }
        public virtual KhuyenMai? KhuyenMai { get;set; }
        public virtual SanPham SanPham { get;set; }
        public virtual IEnumerable<ChiTietHoaDon> ChiTietHoaDons { get;set; }
        public virtual IEnumerable<ChiTietGioHang> ChiTietGioHangs { get;set; }
    }
}
