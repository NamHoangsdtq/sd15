using AppData.Models;
using AppData.ViewModels;

namespace AppAPI.IServices
{
    public interface IDanhGiaService
    {
        Task<DanhGia> SaveDanhGia(DanhGia danhGia);
        Task<List<ChiTietHoaDon>> GetHDCTDaDanhGia(Guid idkh);
        Task<List<ChiTietHoaDon>> GetHDCTChuaDanhGia(Guid idkh);
        Task<List<DanhGiaViewModel>> GetDanhGiaByIdSanPham(Guid idsp);
        Task<List<DanhGiaViewModel>> GetDanhGiaByIdBthe(Guid idbt);
        Task<bool> AnDanhGia(Guid id);
        public bool UpdateDanhGia(Guid idCTHD,int soSao,string? binhLuan);
    }
}
