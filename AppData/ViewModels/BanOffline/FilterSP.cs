using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.BanOffline
{
    public class FilterSP
    {
        public List<Guid> lstDM { get; set; }
        public int khoangGia { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }   
    }
}
