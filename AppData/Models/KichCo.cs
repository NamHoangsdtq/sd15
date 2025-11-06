using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class KichCo
    {
        public Guid ID { get; set; }
        [StringLength(10, ErrorMessage = "Tên kích cỡ không được vượt quá 10 kí tự ")]
        public string? Ten { get; set; }
        public int? TrangThai { get; set; }
        public virtual IEnumerable<ChiTietSanPham> ChiTietSanPhams { get; set; }
    }
}
