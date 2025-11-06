using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class DanhGiaViewModel
    {
        public Guid ID { get; set; }
        public string BinhLuan { get; set; }
        public int? Sao { get; set; }
        public int TrangThai { get; set; }
        public string ChatLieu { get; set; }
        public string MauSac { get; set; }
        public string KichCo { get; set; }
        public string TenKH { get; set; }
        public DateTime? NgayDanhGia { get; set; }
    }
}
