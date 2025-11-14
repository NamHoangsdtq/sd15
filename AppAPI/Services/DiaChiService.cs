using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class DiaChiService : IDiaChiService
    {
        private readonly IAllRepository<DiaChi> _diaChiRepos;

        // Sửa lỗi: Khai báo và khởi tạo context giống hệt HoaDonService
        AssignmentDBContext _context = new AssignmentDBContext();

        public DiaChiService()
        {
            // Khởi tạo repos (context đã được tạo ở trên)
            _diaChiRepos = new AllRepository<DiaChi>(_context, _context.DiaChis);
        }

        // --- TẤT CẢ CÁC HÀM CỦA INTERFACE NẰM DƯỚI ĐÂY ---

        public List<DiaChi> GetDiaChiByKhachHang(Guid idKhachHang)
        {
            return _diaChiRepos.GetAll()
                .Where(c => c.IDKhachHang == idKhachHang)
                .OrderByDescending(c => c.IsDefault)
                .ToList();
        }

        public DiaChi GetDiaChiById(Guid id)
        {
            return _diaChiRepos.GetAll().FirstOrDefault(c => c.ID == id);
        }

        public (bool Success, string ErrorMessage) CreateDiaChi(DiaChi diaChi)
        {
            try
            {
                var soLuongDiaChi = _context.DiaChis.Count(c => c.IDKhachHang == diaChi.IDKhachHang);
                if (soLuongDiaChi == 0)
                {
                    diaChi.IsDefault = true;
                }

                if (diaChi.IsDefault)
                {
                    _context.Database.ExecuteSqlRaw(
                        "UPDATE DiaChi SET IsDefault = 0 WHERE IDKhachHang = {0}",
                        diaChi.IDKhachHang
                    );
                }

                diaChi.ID = Guid.NewGuid();
                _diaChiRepos.Add(diaChi);
                _context.SaveChanges();

                return (true, "Thêm địa chỉ thành công.");
            }
            catch (Exception ex)
            {
                // Trả về lỗi chi tiết để debug
                return (false, $"Thêm thất bại: {ex.Message}");
            }
        }

        public (bool Success, string ErrorMessage) UpdateDiaChi(DiaChi diaChi)
        {
            try
            {
                var diaChiGoc = GetDiaChiById(diaChi.ID);
                if (diaChiGoc == null) return (false, "Không tìm thấy địa chỉ.");

                diaChiGoc.TenNguoiNhan = diaChi.TenNguoiNhan;
                diaChiGoc.SoDienThoai = diaChi.SoDienThoai;
                diaChiGoc.DiaChiCuThe = diaChi.DiaChiCuThe;
                diaChiGoc.TinhThanh = diaChi.TinhThanh;
                diaChiGoc.QuanHuyen = diaChi.QuanHuyen;
                diaChiGoc.PhuongXa = diaChi.PhuongXa;

                _diaChiRepos.Update(diaChiGoc);
                _context.SaveChanges();

                return (true, "Cập nhật thành công.");
            }
            catch (Exception ex)
            {
                return (false, $"Cập nhật thất bại: {ex.Message}");
            }
        }

        public (bool Success, string ErrorMessage) DeleteDiaChi(Guid id)
        {
            try
            {
                var diaChi = GetDiaChiById(id);
                if (diaChi == null) return (false, "Không tìm thấy địa chỉ.");

                if (diaChi.IsDefault)
                {
                    return (false, "Không thể xóa địa chỉ mặc định.");
                }

                _diaChiRepos.Delete(diaChi);
                _context.SaveChanges();

                return (true, "Xóa địa chỉ thành công.");
            }
            catch (Exception ex)
            {
                return (false, $"Xóa thất bại: {ex.Message}");
            }
        }

        public (bool Success, string ErrorMessage) SetDefaultDiaChi(Guid idDiaChi, Guid idKhachHang)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Database.ExecuteSqlRaw(
                        "UPDATE DiaChi SET IsDefault = 0 WHERE IDKhachHang = {0}",
                        idKhachHang
                    );

                    _context.Database.ExecuteSqlRaw(
                        "UPDATE DiaChi SET IsDefault = 1 WHERE ID = {0}",
                        idDiaChi
                    );

                    transaction.Commit();
                    return (true, "Đặt làm mặc định thành công.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return (false, $"Đặt mặc định thất bại: {ex.Message}");
                }
            }
        }
    }
}