using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    
    public class DiaChi
    {
       
        public Guid ID { get; set; }

        // Khóa ngoại liên kết đến bảng KhachHang
        public Guid IDKhachHang { get; set; }

        [Required(ErrorMessage = "Tên người nhận là bắt buộc")]
        [StringLength(255)]
        public string TenNguoiNhan { get; set; }

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [StringLength(20)]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Địa chỉ cụ thể là bắt buộc")]
        [StringLength(500)]
        public string DiaChiCuThe { get; set; } // Ví dụ: "Số 123, đường ABC"

        [Required(ErrorMessage = "Tỉnh/thành là bắt buộc")]
        [StringLength(100)]
        public string TinhThanh { get; set; }

        [Required(ErrorMessage = "Quận/huyện là bắt buộc")]
        [StringLength(100)]
        public string QuanHuyen { get; set; }

        [Required(ErrorMessage = "Phường/xã là bắt buộc")]
        [StringLength(100)]
        public string PhuongXa { get; set; }

        // Đánh dấu đây có phải địa chỉ mặc định hay không
        public bool IsDefault { get; set; }
        public int? ProvinceID { get; set; }
        public int? DistrictID { get; set; }
        public string? WardCode { get; set; }

        // --- Navigation Properties ---

        // Tạo mối quan hệ: Một địa chỉ chỉ thuộc về MỘT khách hàng
        [ForeignKey("IDKhachHang")]
        public virtual KhachHang? KhachHang { get; set; }
    }
}
