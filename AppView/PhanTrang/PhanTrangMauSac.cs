using AppData.Models;

namespace AppView.PhanTrang
{
    public class PhanTrangMauSac
    {
        public IEnumerable<MauSac> listNv { get; set; } = new List<MauSac>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
