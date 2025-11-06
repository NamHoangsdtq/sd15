using AppData.Models;
using AppData.ViewModels.ThongKe;

namespace AppAPI.IServices
{
    public interface IThongKeService
    {
       
        decimal DoanhThuNgay(DateTime date);
        decimal DoanhThuThang(int month, int year);
        decimal DoanhThuNam(int year);
        ThongKeViewModel ThongKe(string startDate, string endDate);
        List<ThongKeSanPham> ThongKeSanPham();
    }
}
