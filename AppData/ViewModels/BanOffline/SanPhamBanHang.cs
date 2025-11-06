using AppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.BanOffline
{
    public class SanPhamBanHang
    {
        public Guid Id { get; set; }
        public string MaSP { get; set; }
        public string Anh { get; set; }
        public string Ten { get; set; }
        public int? GiaBan { get; set; }
        public Guid IdLsp { get; set; }
        public int? GiaGoc { get; set; }

    }
}
