using AppData.Models;
using AppData.ViewModels;

namespace AppView.PhanTrang
{
    public class PhanTrangVouchers
    {
        public IEnumerable<VoucherView> listvouchers { get; set; } = new List<VoucherView>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
