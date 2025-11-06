using AppData.ViewModels;

namespace AppView.PhanTrang
{
    public class PhanTrangLSTD
    {
        public IEnumerable<LichSuTichDiemView> listLSTDs { get; set; } = new List<LichSuTichDiemView>();
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();
    }
}
