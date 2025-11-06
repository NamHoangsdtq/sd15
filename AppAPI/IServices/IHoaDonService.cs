using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using AppData.ViewModels.SanPham;

namespace AppAPI.IServices
{
    public interface IHoaDonService
    {
        public DonMuaSuccessViewModel CreateHoaDon(List<ChiTietHoaDonViewModel> chiTietHoaDons,HoaDonViewModel hoaDon);
        public List<HoaDon> GetAllHoaDon();
        public HoaDon GetHoaDonById(Guid idhd);
        public List<ChiTietHoaDon> GetAllChiTietHoaDon(Guid idHoaDon);
        public bool UpdateTrangThaiGiaoHang(Guid idHoaDon, int trangThai,Guid? idNhanVien);
        public int CheckVoucher(string ten, int tongtien);
        public List<HoaDon> TimKiemVaLocHoaDon(string ten,int? loc);
        public List<HoaDon> LichSuGiaoDich(Guid idNguoiDung);
        //Nhinh sửa
        public bool HoanHang(Guid idhd, Guid idnv);
        public bool ThanhCong(Guid idhd, Guid idnv);
        public bool HoanHangThanhCong(Guid idhd, Guid idnv);
        public bool HuyHD(Guid idhd, Guid idnv);
        Task<bool> CopyHD(Guid idhd, Guid idnv);
        public bool CreateHoaDonOffline(Guid idnhanvien);
        public bool DeleteHoaDon(Guid id);
        public bool UpdateHoaDon(HoaDonThanhToanRequest hoaDon);
        public bool UpdateGhiChuHD(Guid idhd,Guid idnv, string ghichu);
        public bool CheckHDHasLSGD( Guid idHoaDon);
        public LichSuTichDiem GetLichSuGiaoDichByIdHD(Guid idHoaDon);
        public List<HoaDon> GetAllHDCho();
        public HoaDonViewModelBanHang GetHDBanHang(Guid id);
        public List<HoaDonQL> GetAllHDQly();
        public ChiTietHoaDonQL GetCTHDByID(Guid idhd);
        public bool CheckCusUseVoucher (Guid idkh, Guid idvoucher);
    }
}
