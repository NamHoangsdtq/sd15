using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.BanOffline
{
    public class HoaDonViewModelBanHang
    {
        public Guid Id { get; set; }
        public string MaHD { get; set; }
        public Guid? IdKhachHang { get; set; }
        public string? TenKhachHang { get; set; }
        public string? GhiChu { get; set; } 
        public List<HoaDonChiTietViewModel> lstHDCT { get; set; }
    }
}
