using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class LichSuTichDiemView
    {
        public Guid Id { get; set; }
        public Guid IDKhachHang { get; set; }
        public Guid IDHoaDon { get; set; }
        public string MaHD { get; set; }
        public Guid IDQuyDoiDiem { get; set; }
        public DateTime? NgayTichOrTieuDiem { get; set; }
        public string TenKhachHang { get; set; }
        public string SDT { get; set; }     
        public int SoDiemTichOrTieu { get; set; }
        public int? DiemTichKH { get; set; }   
        public int TrangThai { get; set; }     
        
    }
}
