using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class DanhGiaService : IDanhGiaService
    {
        private readonly AssignmentDBContext _context;
        private readonly IAllRepository<DanhGia> reposDanhGia;
        public DanhGiaService()
        {
            _context = new AssignmentDBContext();
            reposDanhGia = new AllRepository<DanhGia>(_context, _context.DanhGias);
        }

        public async Task<bool> AnDanhGia(Guid id)
        {
            try
            {
                var dg = await _context.DanhGias.FindAsync(id);
                dg.TrangThai = 3;
                _context.DanhGias.Update(dg);
                await _context.SaveChangesAsync();
                return true;
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<DanhGiaViewModel>> GetDanhGiaByIdBthe(Guid idbt)
        {
            //var result = await (from dg in _context.DanhGias
            //                    join kh in _context.KhachHangs
            //                    on dg.IDKhachHang equals kh.IDKhachHang
            //                    join bt in _context.BienThes on dg.IDBienThe equals bt.ID
            //                    where bt.ID == idbt && dg.TrangThai != 3
            //                    select new DanhGiaViewModel()
            //                    {
            //                        ID = dg.ID,
            //                        Sao = dg.Sao,
            //                        BinhLuan = dg.BinhLuan,
            //                        TenKH = kh.Ten,
            //                        IDBienThe = dg.IDBienThe,
            //                    }).ToListAsync();
            //return result;
            throw new NotImplementedException();
        }
        public async Task<List<DanhGiaViewModel>> GetDanhGiaByIdSanPham(Guid idsp)
        {
            var query = await (from sp in _context.SanPhams.Where(p => p.ID == idsp)
                               join ctsp in _context.ChiTietSanPhams on sp.ID equals ctsp.IDSanPham
                               join cthd in _context.ChiTietHoaDons on ctsp.ID equals cthd.IDCTSP
                               join dg in _context.DanhGias.Where(p => p.TrangThai == 1) on cthd.ID equals dg.ID
                               join hd in _context.HoaDons on cthd.IDHoaDon equals hd.ID
                               //join lstd in _context.LichSuTichDiems on hd.ID equals lstd.IDHoaDon
                               // kh in _context.KhachHangs on lstd.IDKhachHang equals kh.IDKhachHang
                               join cl in _context.ChatLieus on sp.IDChatLieu equals cl.ID
                               join ms in _context.MauSacs on ctsp.IDMauSac equals ms.ID
                               join kc in _context.KichCos on ctsp.IDKichCo equals kc.ID
                               select new DanhGiaViewModel()
                               {
                                   ID = dg.ID,
                                   Sao = dg.Sao,
                                   BinhLuan = dg.BinhLuan,
                                   TrangThai = dg.TrangThai,
                                   TenKH = _context.KhachHangs.FirstOrDefault(p=>p.IDKhachHang == _context.LichSuTichDiems.FirstOrDefault(p=>p.IDHoaDon == hd.ID).IDKhachHang).Ten,
                                   ChatLieu = cl.Ten,
                                   MauSac = ms.Ten,
                                   KichCo = kc.Ten,
                                   NgayDanhGia = dg.NgayDanhGia
                               }).ToListAsync();
            return query;
            throw new NotImplementedException();
        }

        public async Task<List<ChiTietHoaDon>> GetHDCTChuaDanhGia(Guid idkh)
        {
            var query = await (from kh in _context.KhachHangs
                               join lstd in _context.LichSuTichDiems on kh.IDKhachHang equals lstd.IDKhachHang
                               join hd in _context.HoaDons on lstd.IDHoaDon equals hd.ID
                               join hdct in _context.ChiTietHoaDons on hd.ID equals hdct.IDHoaDon
                               where kh.IDKhachHang == idkh && hdct.TrangThai == 3
                               select new ChiTietHoaDon()
                               {
                                   ID = hdct.ID,
                                   IDHoaDon = hdct.ID,
                                   IDCTSP = hdct.IDCTSP,
                                   SoLuong = hdct.SoLuong
                               }).ToListAsync();
            return query;
        }

        public async Task<List<ChiTietHoaDon>> GetHDCTDaDanhGia(Guid idkh)
        {
            var query = await (from kh in _context.KhachHangs
                               join lstd in _context.LichSuTichDiems on kh.IDKhachHang equals lstd.IDKhachHang
                               join hd in _context.HoaDons on lstd.IDHoaDon equals hd.ID
                               join hdct in _context.ChiTietHoaDons on hd.ID equals hdct.IDHoaDon
                               where kh.IDKhachHang == idkh && hdct.TrangThai == 4
                               select new ChiTietHoaDon()
                               {
                                   ID = hdct.ID,
                                   IDHoaDon = hdct.ID,
                                   IDCTSP = hdct.IDCTSP,
                                   SoLuong = hdct.SoLuong
                               }).ToListAsync();
            return query;
        }

        public async Task<DanhGia> SaveDanhGia(DanhGia danhGia)
        {
            //try
            //{
            //    var dg = await _context.DanhGias.FindAsync(danhGia.ID);
            //    if (dg != null && dg.TrangThai == 0) // Đánh giá tồn tại và chưa từng chỉnh sửa
            //    {
            //        dg.Sao = danhGia.Sao;
            //        dg.BinhLuan = danhGia.BinhLuan;
            //        dg.TrangThai = 1;
            //        _context.DanhGias.Update(dg);
            //        await _context.SaveChangesAsync();
            //        return dg;
            //    }
            //    else
            //    {
            //        DanhGia rv = new DanhGia()
            //        {
            //            ID = new Guid(),
            //            Sao = danhGia.Sao,
            //            BinhLuan = danhGia.BinhLuan,
            //            TrangThai = 0,
            //            IDKhachHang = danhGia.IDKhachHang,
            //            IDBienThe = danhGia.IDBienThe,
            //        };
            //        await _context.DanhGias.AddAsync(rv);
            //        await _context.SaveChangesAsync();
            //        return rv;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
            throw new NotImplementedException();
        }

        public bool UpdateDanhGia(Guid idCTHD, int soSao, string? binhLuan)
        {
            try
            {
                DanhGia danhGia = reposDanhGia.GetAll().FirstOrDefault(p=>p.ID == idCTHD);
                danhGia.BinhLuan = binhLuan;
                danhGia.Sao = soSao;
                danhGia.NgayDanhGia = DateTime.Now;
                danhGia.TrangThai = 1;
                reposDanhGia.Update(danhGia);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
