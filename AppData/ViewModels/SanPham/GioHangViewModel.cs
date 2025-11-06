using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.SanPham
{
    public class GioHangViewModel
    {
        public List<GioHangRequest> GioHangs { get; set; }
        public long TongTien { get; set; }
    }
}
