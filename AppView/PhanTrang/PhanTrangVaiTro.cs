using AppData.Models;
using AppData.ViewModels;

namespace AppView.PhanTrang
{
    public class PhanTrangVaiTro
    {
        public IEnumerable<VaiTro> listvts { get; set; } = new List<VaiTro>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
