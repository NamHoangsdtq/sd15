using AppData.ViewModels;

namespace AppView.PhanTrang
{
    public class PhanTrangKhachHang
    {
        public IEnumerable<KhachHangView> listkh { get; set; }= new List<KhachHangView>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    } 
}
