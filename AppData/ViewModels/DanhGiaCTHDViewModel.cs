using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels
{
    public class DanhGiaCTHDViewModel
    {
        public Guid idCTHD { get; set; }
        public Guid idHD { get; set; }  
        public int soSao { get; set; }
        public string? danhgia { get; set; }
    }
}
