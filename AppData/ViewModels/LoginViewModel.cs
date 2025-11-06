using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class LoginViewModel
    {
        public Guid Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Ten { get; set; }
        public string SDT { get; set; }
        public int? DiemTich { get; set; }
        public int? GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        //0 - Nhan Vien
        //1 - Khach Hang
        public int? vaiTro { get; set; }
        public bool IsAccountLocked { get; set; } // New property for locked account
        public string Message { get; set; }
    }
}
