using AppData.Models;

namespace AppView.PhanTrang
{
    public class PhanTrangLoaiSP
    {
        public IEnumerable<LoaiSP> listlsp { get; set; } = new List<LoaiSP>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
