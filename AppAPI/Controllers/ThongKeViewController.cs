using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.ThongKe;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKeViewController : ControllerBase
    {
        private readonly AssignmentDBContext _dbContext;
        public ThongKeViewController()
        {
            _dbContext = new AssignmentDBContext();
        }
        //GET: api/<ThongKeViewController>
        [HttpGet("ThongKeMSSanPhamBan")]
        public List<ThongKeMSSanPhamTheoSoLuong> ThongKeSanPhamMuaSapXep()
        {
            var result = _dbContext.ChiTietHoaDons
                .Join(_dbContext.ChiTietSanPhams, cthd => cthd.IDCTSP, cts => cts.ID, (cthd, cts) => new { ChiTietHoaDon = cthd, ChiTietSanPham = cts })
                .Join(_dbContext.SanPhams, cthd_cts => cthd_cts.ChiTietSanPham.IDSanPham, sp => sp.ID, (cthd_cts, sp) => new { ChiTietHoaDon_ChiTietSanPham = cthd_cts, SanPham = sp })
                .Join(_dbContext.MauSacs, cthd_cts_sp => cthd_cts_sp.ChiTietHoaDon_ChiTietSanPham.ChiTietSanPham.IDMauSac, ms => ms.ID, (cthd_cts_sp, ms) => new { ChiTietHoaDon_ChiTietSanPham_SanPham = cthd_cts_sp, MauSac = ms })
                .Join(_dbContext.HoaDons, cthd_cts_sp_ms => cthd_cts_sp_ms.ChiTietHoaDon_ChiTietSanPham_SanPham.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.IDHoaDon, hd => hd.ID, (cthd_cts_sp_ms, hd) => new { ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac = cthd_cts_sp_ms, HoaDon = hd })
               
                .GroupBy(cthd_cts_sp_ms_hd => cthd_cts_sp_ms_hd.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham_SanPham.SanPham.ID)
                .Select(group => new ThongKeMSSanPhamTheoSoLuong
                {
                    idSanPham = group.FirstOrDefault().ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham_SanPham.SanPham.ID,
                    TenSP = group.FirstOrDefault().ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham_SanPham.SanPham.Ten.Trim().ToString(),
                    
                    SoLuong = group.Sum(cthd_cts_sp_ms_hd => cthd_cts_sp_ms_hd.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham_SanPham.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.SoLuong),
                    Gia= group.FirstOrDefault().ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham_SanPham.ChiTietHoaDon_ChiTietSanPham.ChiTietSanPham.GiaBan,
                    DoanhThu= group.Sum(cthd_cts_sp_ms_hd => cthd_cts_sp_ms_hd.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham_SanPham.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.SoLuong) * group.FirstOrDefault().ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham_SanPham.ChiTietHoaDon_ChiTietSanPham.ChiTietSanPham.GiaBan,
                    Ngay = group.FirstOrDefault().HoaDon.NgayThanhToan.Value
                })

                .OrderByDescending(item => item.SoLuong).Where(x=>x.Ngay.Month==DateTime.Now.Month).Take(7)
                .ToList();

            return result;
        }
        
        [HttpGet("ThongKeKHTheoSoLuongHoaDon")]
        public List<ThongKeKHMuaNhieu> ThongKeKHTheoSoLuongHoaDon()
        {
            var result = _dbContext.LichSuTichDiems
                .Join(_dbContext.HoaDons, lstd => lstd.IDHoaDon, hd => hd.ID, (lstd, hd) => new { LichSuTichDiem = lstd, HoaDon = hd })
                .Join(_dbContext.ChiTietHoaDons, hd_cthd => hd_cthd.HoaDon.ID, cthd => cthd.IDHoaDon, (hd_cthd, cthd) => new { HoaDon_ChiTietHoaDon = hd_cthd, ChiTietHoaDon = cthd })

                .Join(_dbContext.KhachHangs, lstd_kh => lstd_kh.HoaDon_ChiTietHoaDon.LichSuTichDiem.IDKhachHang, kh => kh.IDKhachHang, (lstd_kh, kh) => new { LichSuTichDiem_KhachHang = lstd_kh, KhachHang = kh }).
                Where(x => x.LichSuTichDiem_KhachHang.HoaDon_ChiTietHoaDon.HoaDon.NgayThanhToan.HasValue)
                .GroupBy(x=> new { x.KhachHang.IDKhachHang,x.LichSuTichDiem_KhachHang.HoaDon_ChiTietHoaDon.HoaDon.NgayThanhToan.Value.Month })
                .Select(group => new ThongKeKHMuaNhieu
                {

                   idkh=group.FirstOrDefault().KhachHang.IDKhachHang,
                   Ten=group.FirstOrDefault().KhachHang.Ten,
                   SDT=group.FirstOrDefault().KhachHang.SDT,
                   Email=group.FirstOrDefault().KhachHang.Email,
                   SoDonMua=group.Sum(x=>x.LichSuTichDiem_KhachHang.HoaDon_ChiTietHoaDon.HoaDon.ID!=null?1:0),
                   TongSoTien=group.Sum(x=>x.LichSuTichDiem_KhachHang.HoaDon_ChiTietHoaDon.HoaDon.TienShip+x.LichSuTichDiem_KhachHang.ChiTietHoaDon.DonGia*x.LichSuTichDiem_KhachHang.ChiTietHoaDon.SoLuong),
                   Ngay=group.FirstOrDefault().LichSuTichDiem_KhachHang.HoaDon_ChiTietHoaDon.HoaDon.NgayThanhToan.Value
                })

                .OrderByDescending(item => item.TongSoTien).Take(10)
                .ToList();

            return result;
       
        }
        [HttpGet("ThongKeDoanhThuTheoNgay")]
        public async Task<IActionResult> ThongKeDoanhThuTheoNgay()
        {
            var result = await _dbContext.ChiTietHoaDons
                .Include(ch => ch.HoaDon)
                .Where(ch => ch.HoaDon.NgayThanhToan.HasValue&&ch.HoaDon.LoaiHD!=1)
                .GroupBy(ch => ch.HoaDon.NgayThanhToan.Value.Date)
                .Select(group => new ThongKeDoanhThu
                {
                    Ngay = group.Key,
                    SoHoaDon = group.Count(),
                    //DoanhThu = group.Sum(ch => ch.DonGia * ch.SoLuong + ch.HoaDon.TienShip - ch.HoaDon.ThueVAT.Value)
                    DoanhThu = group.Sum(ch => ch.DonGia * ch.SoLuong + ch.HoaDon.TienShip)
                })
                .OrderByDescending(t => t.Ngay.Date).
                Where(x=>x.Ngay.Date<=DateTime.Today.Date).Take(7)
                .ToListAsync();

            return Ok(result);
        }
        [HttpGet("ThongKeDoanhThuTheoNgayOffline")]
        public async Task<IActionResult> ThongKeDoanhThuTheoNgayOffline()
        {
            var result = await _dbContext.ChiTietHoaDons
                .Include(ch => ch.HoaDon)
                .Where(ch => ch.HoaDon.NgayThanhToan.HasValue && ch.HoaDon.LoaiHD == 1)
                .GroupBy(ch => ch.HoaDon.NgayThanhToan.Value.Date)
                .Select(group => new ThongKeDoanhThu
                {
                    Ngay = group.Key,
                    SoHoaDon = group.Count(),
                    //DoanhThu = group.Sum(ch => ch.DonGia * ch.SoLuong + ch.HoaDon.TienShip - ch.HoaDon.ThueVAT.Value)
                    DoanhThu = group.Sum(ch => ch.DonGia * ch.SoLuong + ch.HoaDon.TienShip)
                })
                .OrderByDescending(t => t.Ngay.Date).
                Where(x => x.Ngay.Date <= DateTime.Today.Date).Take(7)
                .ToListAsync();

            return Ok(result);
        }
        [HttpGet("LocThongKeDoanhThuTheoNgay")]
        public async Task<IActionResult> LocThongKeDoanhThuTheoNgay(DateTime NgayStart,DateTime NgayEnd)
        {
            if(NgayStart > NgayEnd)
            {
                return null;
            }
            var result = await _dbContext.ChiTietHoaDons
                .Include(ch => ch.HoaDon)
                .Where(ch => ch.HoaDon.NgayThanhToan.HasValue)
                .GroupBy(ch => ch.HoaDon.NgayThanhToan.Value.Date)
                .Select(group => new ThongKeDoanhThu
                {
                    Ngay = group.Key,
                    SoHoaDon = group.Count(),
                    DoanhThu = group.Sum(ch => ch.DonGia * ch.SoLuong + ch.HoaDon.TienShip)
                })
                .OrderByDescending(t => t.Ngay.Date)
                .Where(x=>x.Ngay.Date>=NgayStart.Date&& x.Ngay.Date<=NgayEnd.Date)
                .ToListAsync();
            return Ok(result);
        }
        [HttpGet("ThongKeDoanhThuTheoThang")]
        public List<ThongKeDoanhThu> ThongKeDoanhThuTheoThang()
        {
            var result = _dbContext.ChiTietHoaDons
                .Join(_dbContext.ChiTietSanPhams, cthd => cthd.IDCTSP, cts => cts.ID, (cthd, cts) => new { ChiTietHoaDon = cthd, ChiTietSanPham = cts })
                .Join(_dbContext.SanPhams, cthd_cts => cthd_cts.ChiTietSanPham.IDSanPham, sp => sp.ID, (cthd_cts, sp) => new { ChiTietHoaDon_ChiTietSanPham = cthd_cts, SanPham = sp })

                .Join(_dbContext.HoaDons, cthd_cts_sp_ms => cthd_cts_sp_ms.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.IDHoaDon, hd => hd.ID, (cthd_cts_sp_ms, hd) => new { ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac = cthd_cts_sp_ms, HoaDon = hd }).
                Where(x=>x.HoaDon.NgayThanhToan.HasValue&&x.HoaDon.LoaiHD!=1)
                .GroupBy(cthd_cts_sp_ms_hd => cthd_cts_sp_ms_hd.HoaDon.NgayThanhToan.Value.Month)
                .Select(group => new ThongKeDoanhThu
                {
                   
                    SoHoaDon = group.Sum(x => x.HoaDon.ID != null ? 1 : 0),
                    //DoanhThu = group.Sum(x => x.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.DonGia * x.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.SoLuong + x.HoaDon.TienShip-x.HoaDon.ThueVAT.Value),
                    DoanhThu = group.Sum(x => x.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.DonGia * x.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.SoLuong + x.HoaDon.TienShip),
                    Ngay = group.FirstOrDefault().HoaDon.NgayThanhToan.Value
                })

                .OrderByDescending(item => item.Ngay.Month).Where(x=>x.Ngay.Month<=DateTime.Today.Month).Take(7)
                .ToList();
            return result;
        }
        [HttpGet("ThongKeDoanhThuTheoThangOffline")]
        public List<ThongKeDoanhThu> ThongKeDoanhThuTheoThangOffline()
        {
            var result = _dbContext.ChiTietHoaDons
                .Join(_dbContext.ChiTietSanPhams, cthd => cthd.IDCTSP, cts => cts.ID, (cthd, cts) => new { ChiTietHoaDon = cthd, ChiTietSanPham = cts })
                .Join(_dbContext.SanPhams, cthd_cts => cthd_cts.ChiTietSanPham.IDSanPham, sp => sp.ID, (cthd_cts, sp) => new { ChiTietHoaDon_ChiTietSanPham = cthd_cts, SanPham = sp })

                .Join(_dbContext.HoaDons, cthd_cts_sp_ms => cthd_cts_sp_ms.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.IDHoaDon, hd => hd.ID, (cthd_cts_sp_ms, hd) => new { ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac = cthd_cts_sp_ms, HoaDon = hd }).
                Where(x => x.HoaDon.NgayThanhToan.HasValue && x.HoaDon.LoaiHD == 1)
                .GroupBy(cthd_cts_sp_ms_hd => cthd_cts_sp_ms_hd.HoaDon.NgayThanhToan.Value.Month)
                .Select(group => new ThongKeDoanhThu
                {

                    SoHoaDon = group.Sum(x => x.HoaDon.ID != null ? 1 : 0),
                    //DoanhThu = group.Sum(x => x.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.DonGia * x.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.SoLuong + x.HoaDon.TienShip - x.HoaDon.ThueVAT.Value),
                    DoanhThu = group.Sum(x => x.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.DonGia * x.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.SoLuong + x.HoaDon.TienShip),
                    Ngay = group.FirstOrDefault().HoaDon.NgayThanhToan.Value
                })

                .OrderByDescending(item => item.Ngay.Month).Where(x => x.Ngay.Month <= DateTime.Today.Month).Take(7)
                .ToList();
            return result;
        }
        [HttpGet("LocThongKeDoanhThuTheoThang")]
        public List<ThongKeDoanhThu> LocThongKeDoanhThuTheoThang(DateTime NgayStart, DateTime NgayEnd)
        {
            if (NgayStart > NgayEnd)
            {
                return null;
            }
            var result = _dbContext.ChiTietHoaDons
                .Join(_dbContext.ChiTietSanPhams, cthd => cthd.IDCTSP, cts => cts.ID, (cthd, cts) => new { ChiTietHoaDon = cthd, ChiTietSanPham = cts })
                .Join(_dbContext.SanPhams, cthd_cts => cthd_cts.ChiTietSanPham.IDSanPham, sp => sp.ID, (cthd_cts, sp) => new { ChiTietHoaDon_ChiTietSanPham = cthd_cts, SanPham = sp })

                .Join(_dbContext.HoaDons, cthd_cts_sp_ms => cthd_cts_sp_ms.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.IDHoaDon, hd => hd.ID, (cthd_cts_sp_ms, hd) => new { ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac = cthd_cts_sp_ms, HoaDon = hd })
                .Where(x => x.HoaDon.NgayThanhToan.HasValue)
                .GroupBy(cthd_cts_sp_ms_hd => cthd_cts_sp_ms_hd.HoaDon.NgayThanhToan.Value.Month)
                .Select(group => new ThongKeDoanhThu
                {
                    
                    SoHoaDon = group.Sum(x => x.HoaDon.ID != null ? 1 : 0),
                    DoanhThu = group.Sum(x => x.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.DonGia * x.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.SoLuong + x.HoaDon.TienShip),
                    Ngay = group.FirstOrDefault().HoaDon.NgayThanhToan.Value
                })

                .OrderByDescending(item => item.Ngay.Month).Where(x => x.Ngay.Month >= NgayStart.Month && x.Ngay.Month <= NgayEnd.Month).Take(10)
                .ToList();
            return result;
        }
        [HttpGet("ThongKeDoanhThuTheoNam")]
        public List<ThongKeDoanhThu> ThongKeDoanhThuTheoNam()
        {
            var result = _dbContext.ChiTietHoaDons
                .Join(_dbContext.ChiTietSanPhams, cthd => cthd.IDCTSP, cts => cts.ID, (cthd, cts) => new { ChiTietHoaDon = cthd, ChiTietSanPham = cts })
                .Join(_dbContext.SanPhams, cthd_cts => cthd_cts.ChiTietSanPham.IDSanPham, sp => sp.ID, (cthd_cts, sp) => new { ChiTietHoaDon_ChiTietSanPham = cthd_cts, SanPham = sp })

                .Join(_dbContext.HoaDons, cthd_cts_sp_ms => cthd_cts_sp_ms.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.IDHoaDon, hd => hd.ID, (cthd_cts_sp_ms, hd) => new { ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac = cthd_cts_sp_ms, HoaDon = hd })
                .Where(x => x.HoaDon.NgayThanhToan.HasValue)
                .GroupBy(cthd_cts_sp_ms_hd => cthd_cts_sp_ms_hd.HoaDon.NgayThanhToan.Value.Year)
                .Select(group => new ThongKeDoanhThu
                {
                    
                    SoHoaDon = group.Sum(x => x.HoaDon.ID != null ? 1 : 0),
                    DoanhThu = group.Sum(x => x.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.DonGia * x.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.SoLuong + x.HoaDon.TienShip),
                    Ngay = group.FirstOrDefault().HoaDon.NgayThanhToan.Value
                })

                .OrderByDescending(item => item.Ngay.Year).Take(10)
                .ToList();

            return result;
        }
        [HttpGet("LocThongKeDoanhThuTheoNam")]
        public List<ThongKeDoanhThu> LocThongKeDoanhThuTheoNam(DateTime NgayStart, DateTime NgayEnd)
        {
            if (NgayStart > NgayEnd)
            {
                return null;
            }
            var result = _dbContext.ChiTietHoaDons
                .Join(_dbContext.ChiTietSanPhams, cthd => cthd.IDCTSP, cts => cts.ID, (cthd, cts) => new { ChiTietHoaDon = cthd, ChiTietSanPham = cts })
                .Join(_dbContext.SanPhams, cthd_cts => cthd_cts.ChiTietSanPham.IDSanPham, sp => sp.ID, (cthd_cts, sp) => new { ChiTietHoaDon_ChiTietSanPham = cthd_cts, SanPham = sp })

                .Join(_dbContext.HoaDons, cthd_cts_sp_ms => cthd_cts_sp_ms.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.IDHoaDon, hd => hd.ID, (cthd_cts_sp_ms, hd) => new { ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac = cthd_cts_sp_ms, HoaDon = hd })
                .Where(x => x.HoaDon.NgayThanhToan.HasValue)
                .GroupBy(cthd_cts_sp_ms_hd => cthd_cts_sp_ms_hd.HoaDon.NgayThanhToan.Value.Year)
                .Select(group => new ThongKeDoanhThu
                {
                   
                    SoHoaDon = group.Sum(x => x.HoaDon.ID != null ? 1 : 0),
                    DoanhThu = group.Sum(x => x.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.DonGia * x.ChiTietHoaDon_ChiTietSanPham_SanPham_MauSac.ChiTietHoaDon_ChiTietSanPham.ChiTietHoaDon.SoLuong + x.HoaDon.TienShip),
                    Ngay = group.FirstOrDefault().HoaDon.NgayThanhToan.Value
                })

                .OrderByDescending(item => item.Ngay.Year).Where(x => x.Ngay.Year >= NgayStart.Year && x.Ngay.Year <= NgayEnd.Year).Take(10)
                .ToList();
            return result;
        }
        // thong ke So CTSP da ban
        [HttpGet("ThongKeSLCTSPBan")]
        public ThongKeSLSPDaBan DemSanPhamBan()
        {
            var dem = _dbContext.ChiTietHoaDons
                .Join(_dbContext.HoaDons, cthd => cthd.IDHoaDon, hd => hd.ID, (cthd, hd) => new { ChiTietHoaDon = cthd, HoaDon = hd }).
                Where(x => x.HoaDon.NgayThanhToan.HasValue &&x.HoaDon.LoaiHD!=1)
                .GroupBy(x => x.HoaDon.NgayThanhToan.Value.Month).
                Select(group => new ThongKeSLSPDaBan
                {
                    SoLuong = group.Sum(x => x.ChiTietHoaDon.SoLuong),
                    Ngay = group.FirstOrDefault().HoaDon.NgayThanhToan.Value
                }).Where(x => x.Ngay.Month == DateTime.Now.Month).FirstOrDefault();
            return dem;             
        }
        [HttpGet("ThongKeSLCTSPBanOffline")]
        public ThongKeSLSPDaBan DemSanPhamBanOffline()
        {
            var dem = _dbContext.ChiTietHoaDons
                .Join(_dbContext.HoaDons, cthd => cthd.IDHoaDon, hd => hd.ID, (cthd, hd) => new { ChiTietHoaDon = cthd, HoaDon = hd }).
                Where(x => x.HoaDon.NgayThanhToan.HasValue &&x.HoaDon.LoaiHD==1)
                .GroupBy(x => x.HoaDon.NgayThanhToan.Value.Month).
                Select(group => new ThongKeSLSPDaBan
                {
                    SoLuong = group.Sum(x => x.ChiTietHoaDon.SoLuong),
                    Ngay = group.FirstOrDefault().HoaDon.NgayThanhToan.Value
                }).Where(x => x.Ngay.Month == DateTime.Now.Month).FirstOrDefault();
            return dem;
        }
        [HttpGet("ThongKeSLCTSP")]
        public int DemSanPham()
        {
            var dem = _dbContext.ChiTietSanPhams.
                
                Select(group => new ThongKeSLCTSP
                {
                    SoLuong = group.SoLuong
                   
                }).Sum(x=>x.SoLuong);
            return dem;
        }
        [HttpGet("ThongKeTongDTTrongThang")]
        public ThongKeDTTrongThang TongDoanhThu()
        {
            var tim = _dbContext.ChiTietHoaDons
                .Join(_dbContext.HoaDons, cthd => cthd.IDHoaDon, hd => hd.ID, (cthd, hd) => new { ChiTietHoaDon = cthd, HoaDon = hd }).
                Where(x => x.HoaDon.NgayThanhToan.HasValue&&x.HoaDon.LoaiHD!=1)
                .GroupBy(x => x.HoaDon.NgayThanhToan.Value.Month).
                Select(group => new ThongKeDTTrongThang
                {
                    //TongTien = group.Sum(x => (x.ChiTietHoaDon.SoLuong * x.ChiTietHoaDon.DonGia + x.HoaDon.TienShip-x.HoaDon.ThueVAT.Value)),
                    TongTien = group.Sum(x => (x.ChiTietHoaDon.SoLuong * x.ChiTietHoaDon.DonGia + x.HoaDon.TienShip)),
                    Ngay = group.FirstOrDefault().HoaDon.NgayThanhToan.Value
                }).Where(x => x.Ngay.Month == DateTime.Now.Month).FirstOrDefault();
            return tim;
        }
        [HttpGet("ThongKeTongDTTrongThangOffline")]
        public ThongKeDTTrongThang TongDoanhThuOffline()
        {
            var tim = _dbContext.ChiTietHoaDons
                .Join(_dbContext.HoaDons, cthd => cthd.IDHoaDon, hd => hd.ID, (cthd, hd) => new { ChiTietHoaDon = cthd, HoaDon = hd }).
                Where(x => x.HoaDon.NgayThanhToan.HasValue && x.HoaDon.LoaiHD == 1)
                .GroupBy(x => x.HoaDon.NgayThanhToan.Value.Month).
                Select(group => new ThongKeDTTrongThang
                {
                    //TongTien = group.Sum(x => (x.ChiTietHoaDon.SoLuong * x.ChiTietHoaDon.DonGia + x.HoaDon.TienShip - x.HoaDon.ThueVAT.Value)),
                    TongTien = group.Sum(x => (x.ChiTietHoaDon.SoLuong * x.ChiTietHoaDon.DonGia + x.HoaDon.TienShip)),
                    Ngay = group.FirstOrDefault().HoaDon.NgayThanhToan.Value
                }).Where(x => x.Ngay.Month == DateTime.Now.Month).FirstOrDefault();
            return tim;
        }
        [HttpGet("ThongKeSoDonTrongThang")]
        public ThongKeSDonTrongThang TongSoDon()
        {
            var tim = _dbContext.ChiTietHoaDons
                .Join(_dbContext.HoaDons, cthd => cthd.IDHoaDon, hd => hd.ID, (cthd, hd) => new { ChiTietHoaDon = cthd, HoaDon = hd }).
                Where(x => x.HoaDon.NgayThanhToan.HasValue && x.HoaDon.LoaiHD != 1)
                .GroupBy(x => x.HoaDon.NgayThanhToan.Value.Month).
                Select(group => new ThongKeSDonTrongThang
                {
                    SoDon = group.Sum(x=>x.HoaDon.ID!=null?1:0),
                    Ngay = group.FirstOrDefault().HoaDon.NgayThanhToan.Value
                }).Where(x => x.Ngay.Month == DateTime.Now.Month).FirstOrDefault();
            return tim;
        }
        [HttpGet("ThongKeSoDonTrongThangOffline")]
        public ThongKeSDonTrongThang TongSoDonOffline()
        {
            var tim = _dbContext.ChiTietHoaDons
                .Join(_dbContext.HoaDons, cthd => cthd.IDHoaDon, hd => hd.ID, (cthd, hd) => new { ChiTietHoaDon = cthd, HoaDon = hd }).
                Where(x => x.HoaDon.NgayThanhToan.HasValue && x.HoaDon.LoaiHD == 1)
                .GroupBy(x => x.HoaDon.NgayThanhToan.Value.Month).
                Select(group => new ThongKeSDonTrongThang
                {
                    SoDon = group.Sum(x => x.HoaDon.ID != null ? 1 : 0),
                    Ngay = group.FirstOrDefault().HoaDon.NgayThanhToan.Value
                }).Where(x => x.Ngay.Month == DateTime.Now.Month).FirstOrDefault();
            return tim;
        }
        
    }



}

