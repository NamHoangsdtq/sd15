using AppData.Models;

namespace AppAPI.IServices
{
    public interface IDiaChiService
    {
        // Lấy tất cả địa chỉ của một khách hàng
        List<DiaChi> GetDiaChiByKhachHang(Guid idKhachHang);

        // Lấy 1 địa chỉ theo ID
        DiaChi GetDiaChiById(Guid id);

        // Thêm một địa chỉ mới
        (bool Success, string ErrorMessage) CreateDiaChi(DiaChi diaChi);

        // Cập nhật một địa chỉ
        (bool Success, string ErrorMessage) UpdateDiaChi(DiaChi diaChi);

        // Xóa một địa chỉ
        (bool Success, string ErrorMessage) DeleteDiaChi(Guid id);

        // Đặt làm địa chỉ mặc định
        (bool Success, string ErrorMessage) SetDefaultDiaChi(Guid idDiaChi, Guid idKhachHang);
    }
}
