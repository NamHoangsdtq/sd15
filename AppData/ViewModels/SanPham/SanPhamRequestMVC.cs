using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.SanPham
{
    public class SanPhamRequestMVC
    {
        public string Ten { get; set; }
        public string LoaiSP { get; set; }
        public string Mota { get; set; }
        public List<string> ThuocTinhs { get; set; }
        public List<GiaTriRequestMVC> GiaTris { get; set; }
    }
}
