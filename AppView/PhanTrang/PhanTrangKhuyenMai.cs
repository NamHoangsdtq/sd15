using AppData.Models;
using AppData.ViewModels;

namespace AppView.PhanTrang
{
    public class PhanTrangKhuyenMai
    {
        public IEnumerable<KhuyenMaiView> listkms { get; set; } = new List<KhuyenMaiView>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
