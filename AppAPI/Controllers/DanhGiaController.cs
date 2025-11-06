using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhGiaController : ControllerBase
    {
        private readonly IDanhGiaService _danhGiaService;
        public DanhGiaController()
        {
            _danhGiaService = new DanhGiaService();
        }
        [HttpGet("getbyIdSp/{idsp}")]
        public async Task<IActionResult> GetByIdSp(Guid idsp)
        {
            var result = await _danhGiaService.GetDanhGiaByIdSanPham(idsp);
            return Ok(result);
        }
        [HttpGet("getbyIdBt/{idbt}")]
        public async Task<IActionResult> GetByIdBt(Guid idbt)
        {
            var result = await _danhGiaService.GetDanhGiaByIdBthe(idbt);
            return Ok(result);
        }
        [HttpGet("getDaDanhGia/{idkh}")]
        public async Task<IActionResult> GetHDCTDaDanhGia(Guid idkh)
        {
            var result = await _danhGiaService.GetHDCTDaDanhGia(idkh);
            return Ok(result);
        }
        [HttpGet("getChuaDanhGia/{idkh}")]
        public async Task<IActionResult> GetHDCTChuaDanhGia(Guid idkh)
        {
            var result = await _danhGiaService.GetHDCTChuaDanhGia(idkh);
            return Ok(result);
        }
        [HttpPost("save")]
        public async Task<IActionResult> SaveDanhGia(DanhGia dg)
        {
            var result = await _danhGiaService.SaveDanhGia(dg);
            return Ok(result);
        }
        [HttpPost("anDanhGia")]
        public async Task<IActionResult> AnDanhGia(Guid id)
        {
            var result = await _danhGiaService.AnDanhGia(id);
            return Ok(result);
        }
        [HttpPut]
        public bool UpdateDanhGia(Guid idCTHD, int soSao, string? binhLuan)
        {
            return _danhGiaService.UpdateDanhGia(idCTHD,soSao,binhLuan);
        }
    }
}
