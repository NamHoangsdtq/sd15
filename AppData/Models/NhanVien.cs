using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class NhanVien
    {
        public Guid ID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Tên.")]
        [StringLength(20, ErrorMessage = "Tên không được vượt quá 20 kí tự ")]
        public string Ten { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email.")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 kí tự ")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Password.")]
        public string PassWord { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại.")]
        public string? SDT { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Địa chỉ.")]
        [StringLength(250, ErrorMessage = "Địa chỉ không được vượt quá 250 kí tự ")]
        public string? DiaChi { get; set; }
        public int? TrangThai { get; set; }
        public Guid IDVaiTro { get; set; }
        public virtual IEnumerable<HoaDon>? HoaDons { get; set; }
        public virtual VaiTro? VaiTro { get; set; }
    }
}
