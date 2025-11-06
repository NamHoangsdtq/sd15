using AppData.Models;
using AppData.ViewModels;

namespace AppView.PhanTrang
{
    public class PhanTrangDon
    {
        public IEnumerable<ListDon> listdon { get; set; } = new List<ListDon>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
