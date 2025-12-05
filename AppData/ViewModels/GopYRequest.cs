using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class GopYRequest
    {
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Họ tên phải từ 2 - 50 ký tự")]
        public string HoTen { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^(0[0-9]{9})$", ErrorMessage = "Số điện thoại phải gồm 10 số và bắt đầu bằng 0")]
        public string SoDienThoai { get; set; }
        [Required(ErrorMessage = "Nội dung không được để trống")]
        [MinLength(10, ErrorMessage = "Nội dung tối thiểu 10 ký tự")]
        public string NoiDung { get; set; }

    }
}
