using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels.BanOffline;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class ChiTietHoaDonService : IChiTietHoaDonService
    {
        private readonly AssignmentDBContext _context;
        public ChiTietHoaDonService()
        {
            _context = new AssignmentDBContext();
        }

        public async Task<bool> SaveCTHoaDon(HoaDonChiTietRequest request)
        {
            try
            {
                // Kiểm tra sp tồn tại trong hóa đơn này chưa
                var CTSPexist = _context.ChiTietHoaDons.Where(c => c.IDHoaDon == request.IdHoaDon).Any(c => c.IDCTSP == request.IdChiTietSanPham);
                if (CTSPexist != true) //k tồn tại -> chưa có hdct-> tạo
                {
                    var danhgia = new DanhGia()
                    {
                        ID = request.Id,
                        TrangThai = 0,
                    };
                    await _context.DanhGias.AddAsync(danhgia);
                    await _context.SaveChangesAsync();

                    var hdct = new ChiTietHoaDon()
                    {
                        ID = danhgia.ID,
                        IDHoaDon = request.IdHoaDon,
                        IDCTSP = request.IdChiTietSanPham,
                        SoLuong = request.SoLuong,
                        //DonGia = request.DonGia,
                        TrangThai = 0,
                    };
                    await _context.ChiTietHoaDons.AddAsync(hdct);
                    await _context.SaveChangesAsync();
                    //Trừ số lượng CTSP
                    var ctsp = _context.ChiTietSanPhams.Find(request.IdChiTietSanPham);
                    ctsp.SoLuong -= request.SoLuong;
                    _context.ChiTietSanPhams.Update(ctsp);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    var exist = _context.ChiTietHoaDons.Where(c => c.IDCTSP == request.IdChiTietSanPham && c.IDHoaDon == request.IdHoaDon).FirstOrDefault();
                    var ctsp = _context.ChiTietSanPhams.Find(request.IdChiTietSanPham);
                    exist.SoLuong += request.SoLuong;
                    //exist.DonGia = request.DonGia;
                    _context.Update(exist);
                    await _context.SaveChangesAsync();

                    //Thay đổi số lượng ctsp
                    ctsp.SoLuong -= request.SoLuong;
                    _context.ChiTietSanPhams.Update(ctsp);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCTHoaDon(Guid id)
        {
            try
            {
                var exist = _context.ChiTietHoaDons.Find(id);
                if (exist == null) throw new Exception($"Không tìm thấy CTHD: {id}");
                //Tăng lại số lượng cho sp
                var ctsp = await _context.ChiTietSanPhams.FindAsync(exist.IDCTSP);
                ctsp.SoLuong += exist.SoLuong;
                _context.ChiTietSanPhams.Update(ctsp);
                await _context.SaveChangesAsync();
                //Xóa đánh giá 
                var danhgia = await _context.DanhGias.Where(c => c.ID == id).FirstOrDefaultAsync();
                _context.DanhGias.Remove(danhgia);
                await _context.SaveChangesAsync();
                _context.ChiTietHoaDons.Remove(exist);
                await _context.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public List<ChiTietHoaDon> GetAllCTHoaDon()
        {
            return _context.ChiTietHoaDons.ToList();
        }

        public async Task<List<HoaDonChiTietViewModel>> GetHDCTByIdHD(Guid idhd)
        {
            List<HoaDonChiTietViewModel> lsthdct = await (from cthd in _context.ChiTietHoaDons
                                                          join ctsp in _context.ChiTietSanPhams on cthd.IDCTSP equals ctsp.ID
                                                          join ms in _context.MauSacs on ctsp.IDMauSac equals ms.ID
                                                          join kc in _context.KichCos on ctsp.IDKichCo equals kc.ID
                                                          join sp in _context.SanPhams on ctsp.IDSanPham equals sp.ID
                                                          join km in _context.KhuyenMais.Where(c => c.NgayKetThuc > DateTime.Now && c.TrangThai != 2) on ctsp.IDKhuyenMai equals km.ID
                                                          into kmGroup
                                                          from km in kmGroup.DefaultIfEmpty()
                                                          where cthd.IDHoaDon == idhd
                                                          select new HoaDonChiTietViewModel()
                                                          {
                                                              Id = cthd.ID,
                                                              IdHoaDon = cthd.IDHoaDon,
                                                              IdSP = sp.ID,
                                                              Ten = sp.Ten,
                                                              PhanLoai = ms.Ten + " - " + kc.Ten,
                                                              SoLuong = cthd.SoLuong,
                                                              GiaGoc = ctsp.GiaBan,
                                                              GiaKM = km == null ? ctsp.GiaBan :
                    (km.TrangThai == 1 ? (int)(ctsp.GiaBan / 100 * (100 - km.GiaTri)) :
                    (km.GiaTri < ctsp.GiaBan ? (ctsp.GiaBan - (int)km.GiaTri) : 0)),
                                                              
                                                          }).ToListAsync();
            return lsthdct;
        }

        public async  Task<bool> UpdateSL(Guid id, int sl)
        {
            try
            {
                var cthd = _context.ChiTietHoaDons.Find(id);
                var ctsp = _context.ChiTietSanPhams.Find(cthd.IDCTSP);

                var chenhlech = cthd.SoLuong - sl;
                if (chenhlech < 0 && chenhlech * (-1) > ctsp.SoLuong) return false;
                ctsp.SoLuong += chenhlech;
                _context.ChiTietSanPhams.Update(ctsp);
                await _context.SaveChangesAsync();
                cthd.SoLuong = sl;

                _context.ChiTietHoaDons.Update(cthd);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
