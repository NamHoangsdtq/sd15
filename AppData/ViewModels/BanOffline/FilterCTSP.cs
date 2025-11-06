using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.BanOffline
{
    public class FilterCTSP
    {
        public Guid IdSanPham { get; set; }
        public List<Guid> lstIdMS { get; set; }
        public List<Guid> lstIdKC { get; set; }
    }
}
