using AppData.Models;
using AppData.ViewModels.SanPham;

namespace AppAPI.IServices
{
    public interface IQlThuocTinhService
    {
        #region MauSac
        Task<MauSac> AddMauSac(string ten, string ma, int trangthai);
        Task<MauSac> GetMauSacById(Guid id);
        Task<bool> DeleteMauSac(Guid id);
        Task<MauSac> UpdateMauSac(Guid id, string ten, string ma, int trangthai);
        Task<List<MauSac>> GetAllMauSac();

        #endregion
        #region Kich Co
        Task<KichCo> AddKichCo(string ten, int trangthai);
        Task<KichCo> GetKickCoById(Guid id);
        Task<bool> DeleteKichCo(Guid id);
        Task<KichCo> UpdateKichCo(Guid id, string ten, int trangthai);
        Task<List<KichCo>> GetAllKichCo();

        #endregion
        #region ChatLieu
        Task<ChatLieu> AddChatLieu(string ten, int trangthai);
        Task<ChatLieu> GetChatLieuById(Guid id);
        Task<bool> DeleteChatLieu(Guid id);
        Task<ChatLieu> UpdateChatLieu(Guid id, string ten, int trangthai);
        Task<List<ChatLieu>> GetAllChatLieu();

        #endregion
    }
}
