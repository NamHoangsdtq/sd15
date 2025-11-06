using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.SanPham
{
    public class BienTheRequest
    {
        public Guid ID { get; set; }
        public int SoLuong { get; set; }
        public int GiaBan { get; set; }
        public int TrangThai { get; set; }
        public Guid IDSanPham { get; set; }
        public Guid? IDKhuyenMai { get; set; }
        public List<Guid> ListIdGiaTri { get; set; }
        public List<string>? LstImagePath { get; set; }

    }
}
