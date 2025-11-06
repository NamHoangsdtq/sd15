using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.BanOffline
{
    public class ChiTietSanPhamBanHang // Để hiện view
    {
        public Guid Id { get; set; }
        public string Ten { get; set; }
        public List<MauSac> lstMau { get; set; }
        public List<KichCo> lstKC { get; set; }
    }
}
