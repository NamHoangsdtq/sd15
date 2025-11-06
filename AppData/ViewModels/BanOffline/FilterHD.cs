using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.ViewModels.BanOffline
{
    public class FilterHD
    {
        public List<int> lstTT { get; set; }
        public List<int> loaiHD  { get; set; }
        public int loaitk { get; set; }
        public string keyWord { get; set; }
        public string ngaybd { get; set; }
        public string ngaykt { get; set; }
        public int page { get; set; }
        public int pageSize { get; set; }
    }
}
