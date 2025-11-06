using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/SanPham")]
    [ApiController]
    public class SanPhamController : ControllerBase
    {
        private readonly ISanPhamService _sanPhamServices;
        public SanPhamController(ISanPhamService sanPhamService)
        {
            this._sanPhamServices = sanPhamService;
        }
        #region SanPham

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllSanPham()
        {
            var listSP = await _sanPhamServices.GetAllSanPham();
            return Ok(listSP);
        }

        [HttpGet("GetAllSanPhamAdmin")]
        public List<SanPhamViewModelAdmin> GetAllSanPhamAdmin()
        {
            var listSP = _sanPhamServices.GetAllSanPhamAdmin();
            return listSP;
        }

        [HttpGet("GetSanPhamById")]
        public async Task<IActionResult> GetSanPhamById(Guid id)
        {
            var sanPham = await _sanPhamServices.GetSanPhamById(id);
            if (sanPham == null) return NotFound();
            return Ok(sanPham);
        }
        [HttpPut("UpdateSanPham")]
        public async Task<bool> UpdateSanPham(SanPhamUpdateRequest request)
        {
            return await _sanPhamServices.UpdateSanPham(request);
        }
        [HttpGet("getByIdLsp/{idLsp}")]
        public async Task<IActionResult> GetSanPhamByIdDanhMuc(Guid idLsp)
        {
            var sanPham = await _sanPhamServices.GetSanPhamByIdDanhMuc(idLsp);
            if (sanPham == null) return NotFound();
            return Ok(sanPham);
        }
        [HttpGet("checkTrungTen")]
        public async Task<IActionResult> CheckTrung(SanPhamRequest request)
        {
            var listSP = _sanPhamServices.CheckTrungTenSP(request);
            return Ok(listSP);
        }
        [HttpPost("timKiemNC")]
        public async Task<IActionResult> TimKiemSanPham(SanPhamTimKiemNangCao sp)
        {
            var listSP = await _sanPhamServices.TimKiemSanPham(sp);
            return Ok(listSP);
        }
        [HttpPost("AddSanPham")]
        public async Task<IActionResult> CreateSanPham(SanPhamRequest request)
        {
            if (request == null) return BadRequest();
            var response = await _sanPhamServices.AddSanPham(request);
            return Ok(response);
        }
        [HttpDelete("UpdateTrangThaiSanPham")]
        public async Task<IActionResult> UpdateTrangThaiSanPham(Guid id, int trangThai)
        {
            await _sanPhamServices.UpdateTrangThaiSanPham(id, trangThai);
            return Ok();
        }
        [HttpPost("AddAnh")]
        public async Task<IActionResult> AddAnhToSanPham(List<AnhRequest> request)
        {
            var reponse = await _sanPhamServices.AddAnhToSanPham(request);
            return Ok(reponse);
        }
        [HttpGet("GetAllAnhSanPham")]
        public List<AnhViewModel> GetAllAnhSanPham(Guid idSanPham)
        {
            return _sanPhamServices.GetAllAnhSanPham(idSanPham);
        }

        [HttpPost("AddImageNoColor")]
        public async Task<bool> AddImageNoColor(Anh anh)
        {
            return await _sanPhamServices.AddImageNoColor(anh);
        }
        [HttpPut("UpdateImage")]
        public async Task<bool> UpdateImage(Anh anh)
        {
            return await _sanPhamServices.UpdateImage(anh);
        }
        [HttpDelete("DeleteImage")]
        public async Task<bool> DeleteImage(string id)
        {
            return await _sanPhamServices.DeleteImage(new Guid(id));
        }
        #endregion

        #region ChiTietSanPham
        [HttpGet("GetChiTietSanPhamByID")]
        public IActionResult GetChiTietSanPhamByID(Guid id)
        {
            var response = _sanPhamServices.GetChiTietSanPhamByID(id);
            return Ok(response);
        }
        [HttpGet("GetAllChiTietSanPhamHome")]
        public async Task<IActionResult> GetAllChiTietSanPhamHome(string idSanPham)
        {
            var lstChiTietSanPham = await _sanPhamServices.GetAllChiTietSanPhamHome(new Guid(idSanPham));
            return Ok(lstChiTietSanPham);
        }
        [HttpGet("GetAllChiTietSanPhamAdmin")]
        public async Task<IActionResult> GetAllChiTietSanPham(Guid idSanPham)
        {
            var lstChiTietSanPham = await _sanPhamServices.GetAllChiTietSanPhamAdmin(idSanPham);
            return Ok(lstChiTietSanPham);
        }
        [HttpPost("AddChiTietSanPhamFromSanPham")]
        public async Task<IActionResult> AddChiTietSanPhamFromSanPham(ChiTietSanPhamUpdateRequest request)
        {
            if (request == null) return BadRequest();
            var response = await _sanPhamServices.AddChiTietSanPhamFromSanPham(request);
            return Ok(response);
        }
        [HttpPost("AddChiTietSanPham")]
        public async Task<IActionResult> AddChiTietSanPham(ChiTietSanPhamAddRequest request)
        {
            if (request == null) return BadRequest();
            var response = await _sanPhamServices.AddChiTietSanPham(request);
            return Ok(response);
        }
        [HttpPut("UpdateSoluongChiTietSanPham")]
        public bool UpdateSoluongChiTietSanPham(ChiTietSanPhamRequest request)
        {
            if (request.SoLuong == null) return false;
            else
            {
                var response = _sanPhamServices.UpdateSoluongChiTietSanPham(request.IDChiTietSanPham, request.SoLuong.Value).Result;
                return response;
            }
        }
        [HttpPut("UpdateGiaBanChiTietSanPham")]
        public int UpdateGiaBanChiTietSanPham(ChiTietSanPhamRequest request)
        {
            if (request.GiaBan == null) return -1;
            else
            {
                var response = _sanPhamServices.UpdateGiaBanChiTietSanPham(request.IDChiTietSanPham, request.GiaBan.Value).Result;
                return response;
            }
        }
        [HttpGet("UpdateTrangThaiChiTietSanPham")]
        public bool UpdateTrangThaiChiTietSanPham(string id)
        {
            var response = _sanPhamServices.UpdateTrangThaiChiTietSanPham(new Guid(id)).Result;
            return response;
        }
        [HttpGet("GetAllChiTietSanPham")]
        public async Task<IActionResult> GetAllChiTietSanPham()
        {
            var sanPham = await _sanPhamServices.GetAllChiTietSanPham();
            return Ok(sanPham);
        }
        [HttpGet("GetIDsanPhamByIdCTSP")]
        public Guid GetIDsanPhamByIdCTSP(Guid idctsp)
        {
            return _sanPhamServices.GetIDsanPhamByIdCTSP(idctsp);
        }
        [HttpGet("DeleteChiTietSanPham")]
        public bool DeleteChiTietSanPham(Guid id)
        {
            return _sanPhamServices.DeleteChiTietSanPham(id).Result;
        }
        [HttpGet("UndoChiTietSanPham")]
        public bool UndoChiTietSanPham(Guid id)
        {
            return _sanPhamServices.UndoChiTietSanPham(id).Result;
        }
        #endregion

        #region LoaiSP
        [HttpGet("GetAllLoaiSPCha")]
        public async Task<IActionResult> GetAllLoaiSPCha()
        {
            var listLsp = await _sanPhamServices.GetAllLoaiSPCha();
            return Ok(listLsp);
        }
        [HttpGet("GetAllLoaiSPCon")]
        public async Task<IActionResult> GetAllLoaiSPCon(string tenLoaiSPCha)
        {
            var listLsp = await _sanPhamServices.GetAllLoaiSPCon(tenLoaiSPCha);
            return Ok(listLsp);
        }
        #endregion

        #region Other
        [HttpGet("GetAllMauSac")]
        public async Task<IActionResult> GetAllMauSac()
        {
            var lstMauSac = await _sanPhamServices.GetAllMauSac();
            return Ok(lstMauSac);
        }
        [HttpGet("GetAllKichCo")]
        public async Task<IActionResult> GetAllKichCo()
        {
            var lstKichCo = await _sanPhamServices.GetAllKichCo();
            return Ok(lstKichCo);
        }
        [HttpGet("GetAllChatLieu")]
        public async Task<IActionResult> GetAllChatLieu()
        {
            var lstChatLieu = await _sanPhamServices.GetAllChatLieu();
            return Ok(lstChatLieu);
        }
        #endregion

        #region SanPhamBanHang
        [HttpGet("getAllSPBanHang")]
        public async Task<IActionResult> GetAllSanPhamBanHang()
        {
            var listSP = await _sanPhamServices.GetAllSanPhamTaiQuay();
            return Ok(listSP);
        }
        [HttpGet("getAllSPTrangChu")]
        public async Task<IActionResult> GetAllSanPhamTrangChu()
        {
            var listSP = await _sanPhamServices.GetAllSanPhamTrangChu();
            return Ok(listSP);
        }
        [HttpGet("getChiTietSPBHById/{idsp}")]
        public async Task<IActionResult> GetChiTietSPBHById(Guid idsp)
        {
            var listSP = await _sanPhamServices.GetChiTietSPBHById(idsp);
            return Ok(listSP);
        }
        [HttpGet("getChiTietCTSPBHById/{idsp}")]
        public async Task<IActionResult> GetChiTietCTSPBHById(Guid idsp)
        {
            var listCTSP = await _sanPhamServices.GetChiTietCTSPBanHang(idsp);
            return Ok(listCTSP);
        }
        #endregion

    }
}
