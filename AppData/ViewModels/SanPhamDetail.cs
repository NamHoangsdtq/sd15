using AppData.Models;
using AppData.ViewModels.SanPham;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class SanPhamDetail
    {
        public Guid ID { get; set; }
        public string Ten { get; set; }
        public string MoTa { get; set; }
        public int TrangThai { get; set; }
        public string LoaiSP { get; set; }
        public List<string> ListImage { get; set; }// Toàn bộ Bt
        public List<ThuocTinhRequest> ThuocTinhs { get; set; }
        public List<ChiTietSanPhamViewModel> BienThes { get; set; }
    }
}
