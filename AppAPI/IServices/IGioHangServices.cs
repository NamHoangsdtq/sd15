using AppData.Models;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.IServices
{
    public interface IGioHangServices
    {
        public bool Add(Guid IdKhachHang, DateTime ngaytao);
        public bool Update(Guid Id, DateTime ngaytao);
        public bool Delete(Guid Id);
        public GioHang GetById(Guid Id);
        public List<GioHang> GetAll();
        GioHangViewModel GetCart(List<GioHangRequest> request);
        GioHangViewModel GetCartLogin(string idNguoiDung);
        Task<bool> DeleteCartbyIDCTSP (Guid idctsp, Guid idNguoiDung);
        Task<bool> UpdateCart(Guid idctsp, int soluong, Guid idNguoiDung);
        Task<bool> DeleteCart(Guid idNguoiDung);
        Task<IActionResult> AddCart(ChiTietGioHang chiTietGioHang); //backup nếu có lỗi
        public bool ClearCartLogin(Guid idNguoiDung);
    }
}
