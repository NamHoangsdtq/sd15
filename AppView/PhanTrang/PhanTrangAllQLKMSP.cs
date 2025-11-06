using AppData.Models;
using AppData.ViewModels;

namespace AppView.PhanTrang
{
    public class PhanTrangAllQLKMSP
    {
        public IEnumerable<AllViewSp> listallsp { get; set; }=new  List<AllViewSp>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
