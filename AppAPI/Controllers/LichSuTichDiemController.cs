using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using AppData.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LichSuTichDiemController : ControllerBase
    {
        private readonly ILishSuTichDiemServices _lichsu;
        private readonly AssignmentDBContext _dbcontext;
        public LichSuTichDiemController()
        {
            _dbcontext=new AssignmentDBContext();
           _lichsu = new LishSuTichDiemServices();
        }
        // GET: api/<LichSuTichDiemController>
        [HttpGet]
        public List<LichSuTichDiem> Get()
        {
            return _lichsu.GetAll();
        }
        [HttpGet("GetAllDonMua")]
        public async Task<List<DonMuaViewModel>> GetAllDonMua(Guid IDkhachHang)
        {
            var listDonMua = await _lichsu.getAllDonMua(IDkhachHang);
            return listDonMua;
        }
        [HttpGet("GetALLLichSuTichDiembyIdUser")]
        public async Task<List<LichSuTichDiemTieuDiemViewModel>> GetALLLichSuTichDiembyIdUser(Guid IDkhachHang)
        {
            var listDonMua = await _lichsu.GetALLLichSuTichDiembyIdUser(IDkhachHang);
            return listDonMua;
        }
        [HttpGet("GetAllDonMuaChiTiet")]
        public async Task<List<DonMuaChiTietViewModel>> GetAllDonMuaCT(Guid idHoaDon)
        {
            var listDonMuaCT = await _lichsu.getAllDonMuaChiTiet(idHoaDon);
            return listDonMuaCT;
        }
        [HttpGet("GetCTHDDANHGIA")]
        public Task<ChiTietHoaDonDanhGiaViewModel> getCTHDDanhGia(Guid idhdct)
        {
            return _lichsu.getCTHDDanhGia(idhdct);
        }
        // laam Strat
        [Route("GetLSTDByIdKH")]
        [HttpGet]
        public async Task<List<LichSuTichDiemView>> GetAllLSTDByKH(Guid idkh)
        {
            
            var AllCTSP = await (from LSTD in _dbcontext.LichSuTichDiems.AsNoTracking()
                                 join kh in _dbcontext.KhachHangs.AsNoTracking() on LSTD.IDKhachHang equals kh.IDKhachHang
                                 join hd in _dbcontext.HoaDons.AsNoTracking() on LSTD.IDHoaDon equals hd.ID
                                 join qdd in _dbcontext.QuyDoiDiems.AsNoTracking() on LSTD.IDQuyDoiDiem equals qdd.ID
                                  
                                 select new LichSuTichDiemView()
                                 {
                                     Id=LSTD.ID,
                                     IDKhachHang = kh.IDKhachHang,
                                     IDHoaDon = hd.ID,
                                     MaHD=hd.MaHD,
                                     IDQuyDoiDiem = qdd.ID,
                                     NgayTichOrTieuDiem = hd.NgayThanhToan,
                                     TenKhachHang = kh.Ten,
                                     SDT = kh.SDT,
                                     //SoDiemTichOrTieu = qdd.SoDiem,
                                     DiemTichKH = kh.DiemTich,
                                    
                                     TrangThai = LSTD.TrangThai
                                 }).Where(x=>x.IDKhachHang==idkh).ToListAsync();
            return AllCTSP;
        }
        [Route("TongDonThanhCong")]
        [HttpGet]
        public TongDon? TongDonThanhCong(Guid id)
        {
            var result = _dbcontext.LichSuTichDiems
                .Join(_dbcontext.HoaDons, lstd => lstd.IDHoaDon, hd => hd.ID, (lstd, hd) => new { LichSuTichDiem = lstd, HoaDon = hd }).
                Where(x => x.LichSuTichDiem.IDKhachHang == id&&x.HoaDon.TrangThaiGiaoHang==6).
                GroupBy(x => x.HoaDon.TrangThaiGiaoHang == 6).Select(group => new TongDon
                {
                    Id = id,
                    SoDon = group.Sum(x => x.HoaDon.ID != null ? 1 : 0),
                    SoTien=group.Sum(x=>x.HoaDon.TongTien)
                }).FirstOrDefault();
            if (result == null)
            {
                return new TongDon
                {
                    Id=id,
                    SoDon = 0,
                    SoTien = 0
                };
            }
            else
            {
                return result;
            }

        }
        [Route("TongDonHuy")]
        [HttpGet]
        public TongDon? TongDonHuy(Guid id)
        {
            var result = _dbcontext.LichSuTichDiems
                .Join(_dbcontext.HoaDons, lstd => lstd.IDHoaDon, hd => hd.ID, (lstd, hd) => new { LichSuTichDiem = lstd, HoaDon = hd }).
                Where(x => x.LichSuTichDiem.IDKhachHang == id && x.HoaDon.TrangThaiGiaoHang == 7 ).
                GroupBy(x => x.HoaDon.TrangThaiGiaoHang == 7).Select(group => new TongDon
                {
                    Id = id,
                    SoDon = group.Sum(x => x.HoaDon.ID != null ? 1 : 0),
                    SoTien = group.Sum(x => x.HoaDon.TongTien)
                }).FirstOrDefault();
            if (result == null)
            {
                return new TongDon
                {
                    Id = id,
                    SoDon = 0,
                    SoTien = 0
                };
            }
            else
            {
                return result;
            }

        }
        [Route("TongDonHoanHang")]
        [HttpGet]
        public TongDon? TongDonHoanHang(Guid id)
        {
           var result = _dbcontext.LichSuTichDiems
                .Join(_dbcontext.HoaDons, lstd => lstd.IDHoaDon, hd => hd.ID, (lstd, hd) => new { LichSuTichDiem = lstd, HoaDon = hd }).
                Where(x => x.LichSuTichDiem.IDKhachHang == id && x.HoaDon.TrangThaiGiaoHang == 5 ).
                GroupBy(x => x.HoaDon.TrangThaiGiaoHang == 5).Select(group => new TongDon
                {
                    Id = id,
                    SoDon = group.Sum(x => x.HoaDon.ID != null ? 1 : 0),
                    SoTien = group.Sum(x => x.HoaDon.TongTien)
                }).FirstOrDefault();
            if (result == null)
            {
                return new TongDon
                {
                    Id = id,
                    SoDon = 0,
                    SoTien = 0
                };
            }
            else
            {
                return result;
            }



        }
        [Route("ListDonThanhCong")]
        [HttpGet]
        public List<ListDon> ListDonThanhCong(Guid id)
        {
            var result = _dbcontext.LichSuTichDiems
                .Join(_dbcontext.HoaDons, lstd => lstd.IDHoaDon, hd => hd.ID, (lstd, hd) => new { LichSuTichDiem = lstd, HoaDon = hd }).
                Where(x => x.LichSuTichDiem.IDKhachHang == id && x.HoaDon.TrangThaiGiaoHang == 6 && x.HoaDon.NgayThanhToan.HasValue).
                Select(group => new ListDon {
                    MaDon=group.HoaDon.MaHD,
                    NgayThanhToan=group.HoaDon.NgayThanhToan.Value,
                    SoTien=group.HoaDon.TongTien.Value
                }).ToList();
            return result;

        }
        [Route("ListDonHuy")]
        [HttpGet]
        public List<ListDon> ListDonHuy(Guid id)
        {
            var result = _dbcontext.LichSuTichDiems
                .Join(_dbcontext.HoaDons, lstd => lstd.IDHoaDon, hd => hd.ID, (lstd, hd) => new { LichSuTichDiem = lstd, HoaDon = hd }).
                Where(x => x.LichSuTichDiem.IDKhachHang == id && x.HoaDon.TrangThaiGiaoHang == 7 && x.HoaDon.NgayThanhToan.HasValue).
                Select(group => new ListDon
                {
                    MaDon = group.HoaDon.MaHD,
                    NgayThanhToan = group.HoaDon.NgayThanhToan.Value,
                    SoTien = group.HoaDon.TongTien.Value
                }).ToList();
            return result;

        }
        [Route("ListDonHoanHang")]
        [HttpGet]
        public List<ListDon> ListDonHoanHang(Guid id)
        {
            var result = _dbcontext.LichSuTichDiems
                .Join(_dbcontext.HoaDons, lstd => lstd.IDHoaDon, hd => hd.ID, (lstd, hd) => new { LichSuTichDiem = lstd, HoaDon = hd }).
                Where(x => x.LichSuTichDiem.IDKhachHang == id && x.HoaDon.TrangThaiGiaoHang == 5 && x.HoaDon.NgayThanhToan.HasValue).
                Select(group => new ListDon
                {
                    MaDon = group.HoaDon.MaHD,
                    NgayThanhToan = group.HoaDon.NgayThanhToan.Value,
                    SoTien = group.HoaDon.TongTien.Value
                }).ToList();
            return result;

        }
        // laam end
        // GET api/<LichSuTichDiemController>/5
        [HttpGet("{id}")]
        public LichSuTichDiem Get(Guid id)
        {
            return _lichsu.GetById(id);
        }

        // POST api/<LichSuTichDiemController>
        [HttpPost]
        public bool Post(int diem, int trangthai, Guid IdKhachHang, Guid IdQuyDoiDiem, Guid IdHoaDon)
        {
            return _lichsu.Add(diem, trangthai, IdKhachHang, IdQuyDoiDiem, IdHoaDon);
        }

        // PUT api/<LichSuTichDiemController>/5
        [HttpPut("{id}")]
        public bool Put(Guid id, int diem, int trangthai, Guid IdKhachHang, Guid IdQuyDoiDiem, Guid IdHoaDon)
        {
            var lichsu= _lichsu.GetById(id);
            if (lichsu != null)
            {
                return _lichsu.Update(lichsu.ID,diem, trangthai, IdKhachHang, IdQuyDoiDiem, IdHoaDon);
            }
            else
            {
                return false;
            }
        }

        // DELETE api/<LichSuTichDiemController>/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            var lichsu = _lichsu.GetById(id);
            if (lichsu != null)
            {
                return _lichsu.Delete(lichsu.ID);
            }
            else
            {
                return false;
            }
        }
    }
}
