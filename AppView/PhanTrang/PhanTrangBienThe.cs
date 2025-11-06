using AppData.Models;
using AppData.ViewModels;

namespace AppView.PhanTrang
{
    public class PhanTrangBienThe
    {
        public IEnumerable<ChiTietSanPhamViewModel> listbienthes { get; set; }=new  List<ChiTietSanPhamViewModel>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
