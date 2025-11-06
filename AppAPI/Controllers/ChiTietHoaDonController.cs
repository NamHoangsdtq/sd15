using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using AppData.ViewModels.BanOffline;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiTietHoaDonController : ControllerBase
    {
        private readonly IChiTietHoaDonService _idchiTietHoaDon;

        public ChiTietHoaDonController()
        {
            _idchiTietHoaDon = new ChiTietHoaDonService();
        }
        [HttpGet]
        public List<ChiTietHoaDon> getAll()
        {
            return _idchiTietHoaDon.GetAllCTHoaDon();
        }
        [HttpGet("getByIdHD/{idhd}")]
        public async Task<IActionResult> GetById(Guid idhd)
        {
            var lsp = await _idchiTietHoaDon.GetHDCTByIdHD(idhd);
            if (lsp == null) return NotFound();
            return Ok(lsp);
        }
        [HttpPost("saveHDCT")]
        public async Task<IActionResult> Save(HoaDonChiTietRequest request)
        {
            if (request == null) return BadRequest();
            var hdct = await _idchiTietHoaDon.SaveCTHoaDon(request);
            if(hdct == true) return Ok();
            else return BadRequest();
        }
        [HttpPost("UpdateSL")]
        public async Task<IActionResult> UpdateSL(Guid id, int sl)
        {
            var hdct = await _idchiTietHoaDon.UpdateSL(id,sl);
            if( hdct == true) return Ok();
            return BadRequest();
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteLoaiSP(Guid id)
        {
            var loaiSP = await _idchiTietHoaDon.DeleteCTHoaDon(id);
            return Ok();
        }
    }
}
