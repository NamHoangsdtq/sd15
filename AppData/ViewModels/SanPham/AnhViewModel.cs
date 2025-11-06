using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.SanPham
{
    public class AnhViewModel
    {
        public Guid ID { get; set; }
        public string DuongDan { get; set; }
        public string TenMau { get; set; }
        public string MaMau { get; set; }
    }
}
