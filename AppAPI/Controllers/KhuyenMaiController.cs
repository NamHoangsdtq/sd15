using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using AppData.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiController : ControllerBase
    {
        private readonly IKhuyenMaiServices _khuyenmai;
        private readonly AssignmentDBContext _dbcontext;

        public KhuyenMaiController()
        {
            _dbcontext = new AssignmentDBContext();
            _khuyenmai = new KhuyenMaiServices();
        }

        // GET: api/<KhuyenMaiController>
        [HttpGet]
        public List<KhuyenMai> Get()
        {
            return _khuyenmai.GetAll();
        }
        [Route("GetAllCTSPBySP")]
        [HttpGet]
        public async Task<List<AllViewCTSP>> GetAllCTSPBySp(Guid idSanPham)
        {
            if (!_dbcontext.ChiTietSanPhams.Any(c => c.IDSanPham == idSanPham)) throw new Exception($" khong tim thay san pham co id:{idSanPham}");
            var AllCTSP = await (from CTSP in _dbcontext.ChiTietSanPhams.AsNoTracking()
                                 join mausac in _dbcontext.MauSacs.AsNoTracking() on CTSP.IDMauSac equals mausac.ID
                                 join size in _dbcontext.KichCos.AsNoTracking() on CTSP.IDKichCo equals size.ID
                                 join sp in _dbcontext.SanPhams.AsNoTracking() on CTSP.IDSanPham equals sp.ID
                                 where CTSP.IDSanPham == idSanPham
                                 select new AllViewCTSP()
                                 {
                                     ID = CTSP.ID,
                                     MaCTSP = CTSP.Ma,
                                     TenSanPham = sp.Ten,
                                     TenAnh = (from anh in _dbcontext.Anhs where sp.ID == anh.IDSanPham && mausac.ID == anh.IDMauSac select anh.DuongDan).FirstOrDefault(),
                                     IdKhuyenMai = (from km in _dbcontext.KhuyenMais where CTSP.IDKhuyenMai == km.ID select CTSP.IDKhuyenMai).FirstOrDefault(),
                                     TenMauSac = mausac.Ten,
                                     MaMauSac = mausac.Ma,
                                     TenKichCo = size.Ten,
                                     GiaGoc = CTSP.GiaBan,
                                     GiaKhuyenMai = CTSP.GiaBan,
                                     SoLuong = CTSP.SoLuong,
                                     NgayTao = CTSP.NgayTao,
                                     TrangThai = CTSP.TrangThai
                                 }).ToListAsync();
            return AllCTSP;
        }
        // TimCTSP Theo Id Khuyen Mai
        [Route("GetCTSPByIdKm")]
        [HttpGet]
        public async Task<List<AllViewCTSP>> GetAllCTSPByIdKhuyenMai(Guid idkm)
        {
            if (!_dbcontext.KhuyenMais.Any(c => c.ID == idkm)) throw new Exception($" khong tim thay san pham co id:{idkm}");
            var AllCTSP = await (from CTSP in _dbcontext.ChiTietSanPhams.AsNoTracking()
                                 join mausac in _dbcontext.MauSacs.AsNoTracking() on CTSP.IDMauSac equals mausac.ID
                                 join size in _dbcontext.KichCos.AsNoTracking() on CTSP.IDKichCo equals size.ID
                                 join sp in _dbcontext.SanPhams.AsNoTracking() on CTSP.IDSanPham equals sp.ID

                                 select new AllViewCTSP()
                                 {
                                     ID = CTSP.ID,
                                     TenSanPham = sp.Ten,
                                     IdKhuyenMai = (from km in _dbcontext.KhuyenMais where CTSP.IDKhuyenMai == km.ID select CTSP.IDKhuyenMai).FirstOrDefault(),
                                     TenMauSac = mausac.Ten,
                                     TenKichCo = size.Ten,
                                     GiaGoc = CTSP.GiaBan,
                                     GiaKhuyenMai = CTSP.GiaBan,
                                     SoLuong = CTSP.SoLuong,
                                     NgayTao = CTSP.NgayTao,
                                     TrangThai = CTSP.TrangThai
                                 }).Where(x => x.IdKhuyenMai == idkm).ToListAsync();
            return AllCTSP;
        }

        [Route("GetAllSP")]
        [HttpGet]

        public List<AllViewSp> GetAllSP()
        {

            var result = _dbcontext.SanPhams

                            .Join(_dbcontext.ChatLieus, sp => sp.IDChatLieu, cl => cl.ID, (sp, cl) => new { sp_cl = sp, chatlieus = cl })
                            .Join(_dbcontext.LoaiSPs, sp => sp.sp_cl.IDLoaiSP, lsp => lsp.ID, (sp, lsp) => new { sp_cl_lsp = sp, loaisps = lsp })
                            .Join(_dbcontext.ChiTietSanPhams, sp => sp.sp_cl_lsp.sp_cl.ID, ctsp => ctsp.IDSanPham, (sp, ctsp) => new { sp_cl_lsp_ctsp = sp, chitietsps = ctsp })
                            .GroupBy(x => x.sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.ID)
                            .Select(group => new AllViewSp
                            {
                                ID = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.ID,
                                SoLuongCTSP = group.Sum(x => x.chitietsps.ID != null ? 1 : 0),
                                Ten = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.Ten,
                                MoTa = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.MoTa,
                                TenAnh = (from anhs in _dbcontext.Anhs where @group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.ID == anhs.IDSanPham select anhs.DuongDan).FirstOrDefault(),
                                IdKhuyenMai = (from km in _dbcontext.KhuyenMais where @group.FirstOrDefault().chitietsps.IDKhuyenMai == km.ID select km.ID).FirstOrDefault(),

                                GiaBan = group.FirstOrDefault().chitietsps.GiaBan,
                                IDLoaiSP = group.FirstOrDefault().sp_cl_lsp_ctsp.loaisps.ID,
                                IDLoaiSPCha = group.FirstOrDefault().sp_cl_lsp_ctsp.loaisps.IDLoaiSPCha,
                                IDChatLieu = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.chatlieus.ID,
                                TrangThai = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.TrangThai
                            }).ToList();
            return result;

        }
        [Route("GetAllSPByKhuyenMai")]
        [HttpGet]
        public List<AllViewSp> GetAllSPByKhuyenMai(Guid idkm)
        {
            if (!_dbcontext.KhuyenMais.Any(c => c.ID == idkm)) throw new Exception($"Không tìm thấy sản phẩm có id khuyến mãi:{idkm}");
            var result = _dbcontext.SanPhams

                           .Join(_dbcontext.ChatLieus, sp => sp.IDChatLieu, cl => cl.ID, (sp, cl) => new { sp_cl = sp, chatlieus = cl })
                           .Join(_dbcontext.LoaiSPs, sp => sp.sp_cl.IDLoaiSP, lsp => lsp.ID, (sp, lsp) => new { sp_cl_lsp = sp, loaisps = lsp })
                           .Join(_dbcontext.ChiTietSanPhams, sp => sp.sp_cl_lsp.sp_cl.ID, ctsp => ctsp.IDSanPham, (sp, ctsp) => new { sp_cl_lsp_ctsp = sp, chitietsps = ctsp }).Where(x => x.chitietsps.TrangThai == 1 || x.chitietsps.TrangThai == 2)
                           .Where(x => x.chitietsps.IDKhuyenMai == idkm)
                           .GroupBy(x => x.sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.ID)
                           .Select(group => new AllViewSp
                           {
                               ID = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.ID,
                               MaSP = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.Ma,
                               SoLuongCTSP = group.Sum(x => x.chitietsps.ID != null ? 1 : 0),
                               Ten = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.Ten,
                               MoTa = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.MoTa,
                               TenAnh = (from anhs in _dbcontext.Anhs where @group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.ID == anhs.IDSanPham select anhs.DuongDan).FirstOrDefault(),
                               IdKhuyenMai = group.Where(x => x.chitietsps.IDKhuyenMai != null)
                                 .Select(x => x.chitietsps.IDKhuyenMai)
                                .FirstOrDefault(),
                               GiaBan = group.FirstOrDefault().chitietsps.GiaBan,
                               IDLoaiSP = group.FirstOrDefault().sp_cl_lsp_ctsp.loaisps.ID,
                               IDLoaiSPCha = group.FirstOrDefault().sp_cl_lsp_ctsp.loaisps.IDLoaiSPCha,
                               IDChatLieu = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.chatlieus.ID,
                               TrangThai = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.TrangThai
                           }).Where(x => x.IdKhuyenMai == idkm).Where(x => x.TrangThai == 1).ToList().ToList();
            return result;

        }
        //public List<AllViewSp> GetAllSPByKhuyenMai(Guid idkm)
        //{
        //    if (!_dbcontext.KhuyenMais.Any(c => c.ID == idkm)) throw new Exception($"Không tìm thấy sản phẩm với ID khuyến mãi:{idkm}");

        //    var result = _dbcontext.SanPhams
        //        .Join(_dbcontext.ChatLieus, sp => sp.IDChatLieu, cl => cl.ID, (sp, cl) => new { sp_cl = sp, chatlieus = cl })
        //        .Join(_dbcontext.LoaiSPs, sp => sp.sp_cl.IDLoaiSP, lsp => lsp.ID, (sp, lsp) => new { sp_cl_lsp = sp, loaisps = lsp })
        //        .Join(_dbcontext.ChiTietSanPhams, sp => sp.sp_cl_lsp.sp_cl.ID, ctsp => ctsp.IDSanPham, (sp, ctsp) => new { sp_cl_lsp_ctsp = sp, chitietsps = ctsp })
        //        .Where(x => x.chitietsps.TrangThai == 1 || x.chitietsps.TrangThai == 2)
        //        .Where(x => x.chitietsps.IDKhuyenMai == idkm) // Chỉ lọc theo khuyến mãi
        //        .Select(x => new AllViewSp
        //        {
        //            ID = x.sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.ID,
        //            MaSP = x.sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.Ma,
        //            Ten = x.sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.Ten,
        //            MoTa = x.sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.MoTa,
        //            TenAnh = (from anhs in _dbcontext.Anhs where x.sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.ID == anhs.IDSanPham select anhs.DuongDan).FirstOrDefault(),
        //            IdKhuyenMai = x.chitietsps.IDKhuyenMai,
        //            GiaBan = x.chitietsps.GiaBan,
        //            IDLoaiSP = x.sp_cl_lsp_ctsp.loaisps.ID,
        //            IDLoaiSPCha = x.sp_cl_lsp_ctsp.loaisps.IDLoaiSPCha,
        //            IDChatLieu = x.sp_cl_lsp_ctsp.sp_cl_lsp.chatlieus.ID,
        //            TrangThai = x.sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.TrangThai
        //        })
        //        .OrderBy(x => x.MaSP)
        //        .ToList();

        //    return result;
        //}

        [Route("GetAllSPByKmLoaiSPChatLieu")]
        [HttpGet]

        public async Task<List<AllViewSp>> GetAllSPByKm(Guid? idkm, Guid? idLoaiSP, Guid? idChatLieu)
        {
            if (!_dbcontext.KhuyenMais.Any(c => c.ID == idkm) && !_dbcontext.LoaiSPs.Any(c => c.ID == idLoaiSP) && !_dbcontext.ChatLieus.Any(y => y.ID == idChatLieu)) throw new Exception($" khong tim thay san pham co id:{idkm},{idLoaiSP},{idChatLieu}");
            var AllCTSP = (from SP in _dbcontext.SanPhams.AsNoTracking()
                           join anh in _dbcontext.Anhs.AsNoTracking() on SP.ID equals anh.IDSanPham
                           join loaisp in _dbcontext.LoaiSPs.AsNoTracking() on SP.IDLoaiSP equals loaisp.ID
                           join chatlieu in _dbcontext.ChatLieus.AsNoTracking() on SP.IDChatLieu equals chatlieu.ID
                           join CTSP in _dbcontext.ChiTietSanPhams.AsNoTracking() on SP.ID equals CTSP.IDSanPham
                           join km in _dbcontext.KhuyenMais.AsNoTracking() on CTSP.IDKhuyenMai equals km.ID
                           select new { SP, anh, loaisp, chatlieu, CTSP, km });
            // Tim Theo IdKhuyenMai
            if (!string.IsNullOrEmpty(idkm.ToString()))
            {
                AllCTSP = AllCTSP.AsNoTracking().Where(x => x.km.ID == idkm);
            }
            if (!string.IsNullOrEmpty(idLoaiSP.ToString()))
            {
                AllCTSP = AllCTSP.AsNoTracking().Where(x => x.loaisp.ID == idLoaiSP);

            }
            if (!string.IsNullOrEmpty(idChatLieu.ToString()))
            {
                AllCTSP = AllCTSP.AsNoTracking().Where(x => x.chatlieu.ID == idChatLieu);
            }
            var result = await AllCTSP.AsNoTracking().Select(c => new AllViewSp()
            {
                ID = c.SP.ID,
                Ten = c.SP.Ten,
                MoTa = c.SP.MoTa,
                TenAnh = c.anh.DuongDan,
                IdKhuyenMai = c.km.ID,

                GiaBan = c.CTSP.GiaBan,
                IDLoaiSP = c.SP.IDLoaiSP,
                IDLoaiSPCha = c.loaisp.IDLoaiSPCha,
                IDChatLieu = c.chatlieu.ID,
                TrangThai = c.CTSP.TrangThai

            }).ToListAsync();
            return result;
        }
        [Route("GetAllSPNoKM")]
        [HttpGet]

        public List<AllViewSp> GetAllSPNoKm(Guid id)
        {
            if (!_dbcontext.KhuyenMais.Any(c => c.ID == id)) throw new Exception($"Không tìm thấy sản phẩm có id khuyến mãi:{id}");
            var result = _dbcontext.SanPhams

                           .Join(_dbcontext.ChatLieus, sp => sp.IDChatLieu, cl => cl.ID, (sp, cl) => new { sp_cl = sp, chatlieus = cl })
                           .Join(_dbcontext.LoaiSPs, sp => sp.sp_cl.IDLoaiSP, lsp => lsp.ID, (sp, lsp) => new { sp_cl_lsp = sp, loaisps = lsp })
                           .Join(_dbcontext.ChiTietSanPhams, sp => sp.sp_cl_lsp.sp_cl.ID, ctsp => ctsp.IDSanPham, (sp, ctsp) => new { sp_cl_lsp_ctsp = sp, chitietsps = ctsp }).Where(x => x.chitietsps.TrangThai == 1 || x.chitietsps.TrangThai == 2)
                           .GroupBy(x => x.sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.ID)
                           .Select(group => new AllViewSp
                           {
                               ID = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.ID,
                               MaSP = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.Ma,
                               SoLuongCTSP = group.Sum(x => x.chitietsps.ID != null ? 1 : 0),
                               Ten = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.Ten,
                               MoTa = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.MoTa,
                               TenAnh = (from anhs in _dbcontext.Anhs where @group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.ID == anhs.IDSanPham select anhs.DuongDan).FirstOrDefault(),
                               IdKhuyenMai = (from km in _dbcontext.KhuyenMais where @group.FirstOrDefault().chitietsps.IDKhuyenMai == km.ID select km.ID).FirstOrDefault(),

                               GiaBan = group.FirstOrDefault().chitietsps.GiaBan,
                               IDLoaiSP = group.FirstOrDefault().sp_cl_lsp_ctsp.loaisps.ID,
                               IDLoaiSPCha = group.FirstOrDefault().sp_cl_lsp_ctsp.loaisps.IDLoaiSPCha,
                               IDChatLieu = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.chatlieus.ID,
                               TrangThai = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.TrangThai
                           }).Where(x => x.IdKhuyenMai != id).Where(x => x.TrangThai == 1).ToList();
            return result;
        }
        [Route("GetAllSPNoKMByLoaiSPChatLieu")]
        [HttpGet]

        public List<AllViewSp> TKGetAllSPNoKmByLoaiSPChatLieu(Guid id, Guid? idLoaiSP, Guid? idChatLieu)
        {
            if (!_dbcontext.LoaiSPs.Any(c => c.ID == idLoaiSP) && !_dbcontext.ChatLieus.Any(y => y.ID == idChatLieu)) throw new Exception($" khong tim thay san pham co id:{idLoaiSP},{idChatLieu}");
            //var AllCTSP = (from SP in _dbcontext.SanPhams.AsNoTracking()
            //               join anh in _dbcontext.Anhs.AsNoTracking() on SP.ID equals anh.IDSanPham
            //               join loaisp in _dbcontext.LoaiSPs.AsNoTracking() on SP.IDLoaiSP equals loaisp.ID
            //               join chatlieu in _dbcontext.ChatLieus.AsNoTracking() on SP.IDChatLieu equals chatlieu.ID
            //               join CTSP in _dbcontext.ChiTietSanPhams.AsNoTracking() on SP.ID equals CTSP.IDSanPham

            //               select new { SP, anh, loaisp, chatlieu, CTSP });
            //// Tim Theo LoaiSP,Chat Lieu

            //if (!string.IsNullOrEmpty(idLoaiSP.ToString()))
            //{
            //    AllCTSP = AllCTSP.AsNoTracking().Where(x => x.loaisp.ID == idLoaiSP);

            //}
            //if (!string.IsNullOrEmpty(idChatLieu.ToString()))
            //{
            //    AllCTSP = AllCTSP.AsNoTracking().Where(x => x.chatlieu.ID == idChatLieu);
            //}
            //var result = await AllCTSP.AsNoTracking().Select(c => new AllViewSp()
            //{
            //    ID = c.SP.ID,
            //    Ten = c.SP.Ten,
            //    MoTa = c.SP.MoTa,
            //    TenAnh = c.anh.DuongDan,
            //    IdKhuyenMai = (from km in _dbcontext.KhuyenMais where c.CTSP.IDKhuyenMai == km.ID select c.CTSP.IDKhuyenMai).FirstOrDefault(),
            //    TongSoSao = c.SP.TongSoSao,
            //    TongDanhGia = c.SP.TongDanhGia,
            //    GiaBan = c.CTSP.GiaBan,
            //    IDLoaiSP = c.SP.IDLoaiSP,
            //    IDLoaiSPCha = c.loaisp.IDLoaiSPCha,
            //    IDChatLieu = c.chatlieu.ID,
            //    TrangThai = c.CTSP.TrangThai

            //}).Where(x=>x.IdKhuyenMai!=id).ToListAsync();
            //return result;
            if (!_dbcontext.KhuyenMais.Any(c => c.ID == id)) throw new Exception($" khong tim thay san pham co id:{id}");
            var result = _dbcontext.SanPhams

                           .Join(_dbcontext.ChatLieus, sp => sp.IDChatLieu, cl => cl.ID, (sp, cl) => new { sp_cl = sp, chatlieus = cl })
                           .Join(_dbcontext.LoaiSPs, sp => sp.sp_cl.IDLoaiSP, lsp => lsp.ID, (sp, lsp) => new { sp_cl_lsp = sp, loaisps = lsp })
                           .Join(_dbcontext.ChiTietSanPhams, sp => sp.sp_cl_lsp.sp_cl.ID, ctsp => ctsp.IDSanPham, (sp, ctsp) => new { sp_cl_lsp_ctsp = sp, chitietsps = ctsp }).Where(x => x.chitietsps.TrangThai == 1)
                           .GroupBy(x => x.sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.ID)
                           .Select(group => new AllViewSp
                           {
                               ID = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.ID,
                               SoLuongCTSP = group.Sum(x => x.chitietsps.ID != null ? 1 : 0),
                               Ten = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.Ten,
                               MoTa = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.MoTa,
                               TenAnh = (from anhs in _dbcontext.Anhs where @group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.ID == anhs.IDSanPham select anhs.DuongDan).FirstOrDefault(),
                               IdKhuyenMai = (from km in _dbcontext.KhuyenMais where @group.FirstOrDefault().chitietsps.IDKhuyenMai == km.ID select km.ID).FirstOrDefault(),

                               GiaBan = group.FirstOrDefault().chitietsps.GiaBan,
                               IDLoaiSP = group.FirstOrDefault().sp_cl_lsp_ctsp.loaisps.ID,
                               IDLoaiSPCha = group.FirstOrDefault().sp_cl_lsp_ctsp.loaisps.IDLoaiSPCha,
                               IDChatLieu = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.chatlieus.ID,
                               TrangThai = group.FirstOrDefault().sp_cl_lsp_ctsp.sp_cl_lsp.sp_cl.TrangThai
                           }).Where(x => x.IdKhuyenMai != id).ToList();
            if (!string.IsNullOrEmpty(idLoaiSP.ToString()))
            {
                result = result.Where(x => x.IDLoaiSP == idLoaiSP).ToList();

            }
            if (!string.IsNullOrEmpty(idChatLieu.ToString()))
            {
                result = result.Where(x => x.IDChatLieu == idChatLieu).ToList();
            }
            return result;
        }

        // GET api/<KhuyenMaiController>/5
        [HttpGet("{id}")]
        public KhuyenMai Get(Guid id)
        {
            return _khuyenmai.GetById(id);
        }
        [Route("TimKiemTenKM")]
        [HttpGet]
        public List<KhuyenMai> GetByTen(string Ten)
        {
            return _khuyenmai.GetKMByName(Ten);
        }

        // POST api/<KhuyenMaiController>
        [HttpPost]
        public bool Post(KhuyenMaiView kmv)
        {
            return _khuyenmai.Add(kmv);
        }
        [Route("AddKmVoBT")]
        [HttpPut]
        // PUT api/<KhuyenMaiController>/5 
        public bool AddKMVoBienThe(List<Guid> bienThes, Guid IdKhuyenMai)
        {
            return _khuyenmai.AdKMVoBT(bienThes, IdKhuyenMai);
        }

        // Lam start
        [Route("XoaKmRaBT")]
        [HttpPut]
        // PUT api/<KhuyenMaiController>/5 
        public bool XoaKMRaBienThe(List<Guid> bienThes)
        {
            return _khuyenmai.XoaAllKMRaBT(bienThes);
        }
        //Lam end
        // PUT api/<KhuyenMaiController>/5

        [HttpPut("{id}")]
        public bool Put(KhuyenMaiView kmv)
        {
            var khuyenmai = _khuyenmai.GetById(kmv.ID);
            if (khuyenmai != null)
            {
                return _khuyenmai.Update(kmv);
            }
            else
            {
                return false;
            }
        }

        // DELETE api/<KhuyenMaiController>/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            var khuyenmai = _khuyenmai.GetById(id);
            if (khuyenmai != null)
            {
                return _khuyenmai.Delete(khuyenmai.ID);
            }
            else
            {
                return false;
            }
        }
    }
}
