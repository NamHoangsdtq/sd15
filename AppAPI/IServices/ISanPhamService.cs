using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using AppData.ViewModels.SanPham;

namespace AppAPI.IServices
{
    public interface ISanPhamService
    {
        #region SanPham
        List<SanPhamViewModelAdmin> GetAllSanPhamAdmin();
        Task<List<SanPhamViewModel>> GetAllSanPham();
        Task<List<SanPhamViewModel>> TimKiemSanPham(SanPhamTimKiemNangCao sp);
        Task<SanPhamUpdateRequest> GetSanPhamById(Guid id);
        Task<List<SanPhamViewModel>> GetSanPhamByIdDanhMuc(Guid idloaisp);
        Task<ChiTietSanPhamUpdateRequest> AddSanPham(SanPhamRequest request);
        Task<bool> UpdateSanPham(SanPhamUpdateRequest request);
        Task<bool> UpdateTrangThaiSanPham(Guid id,int trangThai);
        bool CheckTrungTenSP(SanPhamRequest lsp);
        Task<bool> AddAnhToSanPham(List<AnhRequest> request);
        List<AnhViewModel> GetAllAnhSanPham(Guid idSanPham);
        Task<bool> AddImageNoColor(Anh anh);
        Task<bool> UpdateImage(Anh anh);
        Task<bool> DeleteImage(Guid id);
        public Guid GetIDsanPhamByIdCTSP (Guid idctsp);
        #endregion

        #region LoaiSanPham
        Task<List<LoaiSP>> GetAllLoaiSPCha();
        Task<List<LoaiSP>?> GetAllLoaiSPCon(string tenLoaiSPCha);
        Task<LoaiSP> GetLoaiSPById(Guid id);
        Task<LoaiSP> SaveLoaiSP(LoaiSPRequest lsp);
        Task<bool> DeleteLoaiSP(Guid id);
        bool CheckTrungLoaiSP(LoaiSPRequest lsp);
        #endregion

        #region ChiTietSanPham
        Task<ChiTietSanPhamUpdateRequest> AddChiTietSanPham(ChiTietSanPhamAddRequest request);
        ChiTietSanPhamViewModel GetChiTietSanPhamByID(Guid id);
        Task<List<ChiTietSanPham>> GetAllChiTietSanPham(Guid idSanPham);
        Task<ChiTietSanPhamViewModelHome> GetAllChiTietSanPhamHome(Guid idSanPham);
        Task<List<ChiTietSanPhamViewModel>> GetAllChiTietSanPham();
        Task<List<ChiTietSanPhamViewModelAdmin>> GetAllChiTietSanPhamAdmin(Guid idSanPham);
        Task<bool> DeleteChiTietSanPham(Guid id);
        Task<bool> UpdateSoluongChiTietSanPham(Guid id, int soLuong);
        Task<int> UpdateGiaBanChiTietSanPham(Guid id, int giaBan);
        Task<bool> UpdateTrangThaiChiTietSanPham(Guid id);
        Task<bool> UpdateChiTietSanPham(ChiTietSanPham chiTietSanPham);
        public Task<bool> AddChiTietSanPhamFromSanPham(ChiTietSanPhamUpdateRequest request);
        Task<bool> UndoChiTietSanPham(Guid id);
        #endregion
        Task<List<MauSac>> GetAllMauSac();
        Task<List<KichCo>> GetAllKichCo();
        Task<List<ChatLieu>> GetAllChatLieu();
        //Nhinh thêm
        #region SanPhamBanHang
        Task<List<HomeProductViewModel>> GetAllSanPhamTrangChu();
        Task<List<SanPhamBanHang>> GetAllSanPhamTaiQuay();
        Task<ChiTietSanPhamBanHang> GetChiTietSPBHById(Guid idsp); // Sản phẩm và list màu, list size
        Task<List<ChiTietCTSPBanHang>> GetChiTietCTSPBanHang(Guid idsp); // Chitet sp 
        #endregion
    }
}
