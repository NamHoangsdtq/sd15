using AppData.Models;

namespace AppView.PhanTrang
{
    public class PhanTrangNhanVien
    {
        public IEnumerable<NhanVien> listNv { get; set; } = new List<NhanVien>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
