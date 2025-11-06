using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class ChiTietHoaDonDanhGiaViewModel
    {
        public Guid ID { get; set; } 
        public Guid IDHoaDon { get; set; }
        public string TenSanPham { get; set; }
        public string TenMau { get; set; }
        public string TenKichThuoc { get; set; }
        public string DuongDan { get; set; }

    }
}
