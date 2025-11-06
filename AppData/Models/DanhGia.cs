using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class DanhGia
    {
        public Guid ID { get; set; }
        [MaxLength(250, ErrorMessage = "Tối đa 250 ký tự")]
        public string? BinhLuan { get; set; }
        [Range(1, 5, ErrorMessage = "Số sao từ 1 đến 5")]
        public int? Sao { get; set; }
        public DateTime? NgayDanhGia { get; set; }
        public int TrangThai { get; set; }
        public virtual ChiTietHoaDon ChiTietHoaDon { get; set; }
    }
}
