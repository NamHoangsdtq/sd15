using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.SanPham
{
    public class ChiTietSanPhamRequest
    {
        public Guid IDChiTietSanPham { get; set; }
        public Guid? IDMauSac { get; set; }
        public Guid? IDKichCo { get; set; }
        public string? TenKichCo { get; set; }
        public string? TenMauSac { get; set; }
        public string? MaMau { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng không được âm")]
        public int? SoLuong { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Giá bán không được âm")]
        public int? GiaBan { get; set; }
    }
}
