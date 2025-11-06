using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.BanOffline
{
    public class HomeProductViewModel
    {
        public Guid Id { get; set; }
        public string Ten { get; set; }
        public string Anh { get; set; }
        public int? GiaBan { get; set; }
        public int? GiaGoc { get; set; }
        public int? KhuyenMai { get; set; }
        public int SLBan { get; set; }
        public Guid? IdCTSP { get; set; }
        public DateTime? NgayTao { get; set; }
        public double? SoSao { get; set; }
    }
}
