using AppData.ViewModels;

namespace AppView.PhanTrang
{
    public class PhanTrangCTSPBySP
    {
        public IEnumerable<AllViewCTSP> listallctspbysp { get; set; } = new List<AllViewCTSP>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
