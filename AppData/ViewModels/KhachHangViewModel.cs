using AppData.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class KhachHangViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? SDT { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(8, ErrorMessage = "Mật khẩu phải ít nhất 8 ký tự")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và nhập lại mật khẩu không khớp")]
        public string? ConfirmPassword { get; set; }
        public int? DiemTich { get; set; }
        public int? TrangThai { get; set; }
    }
}
