using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Models
{
    public class GopY
    {
        public int Id { get; set; }

        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string NoiDung { get; set; }

        public DateTime NgayTao { get; set; }

        // cho Admin quản lý
        public bool IsRead { get; set; }   // đã đọc hay chưa
    }
}
