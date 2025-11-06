using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.SanPham
{
    public class DonMuaSuccessViewModel
    {
        public string ID { get; set; }
        public string Ten { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public string PhuongThucThanhToan { get; set; }
        public int TongTien { get; set; }
        public int DiemTich { get; set; }
        public int DiemSuDung { get; set; }
        public string MaVoucher { get; set; }
        public bool Login { get; set; }
        public string GhiChu { get; set; }
        public List<GioHangRequest> GioHangs { get; set; }
    }
}
