using AppData.Models;

namespace AppView.PhanTrang
{
    public class PhanTrangQuyDoiDiem
    {
        public IEnumerable<QuyDoiDiem> listqdd { get; set; } = new List<QuyDoiDiem>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
