using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.SanPham
{
    public class SanPhamTimKiemNangCao
    {
        public string KeyWord { get; set; }
        public List<Guid> IdLoaiSP { get; set; }
        public int GiaMin { get; set; }
        public int GiaMax { get; set; }
    }
}
