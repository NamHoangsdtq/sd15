using AppData.Models;

namespace AppView.PhanTrang
{
    public class PhanTrangKichCo
    {
        public IEnumerable<KichCo> listNv { get; set; } = new List<KichCo>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
