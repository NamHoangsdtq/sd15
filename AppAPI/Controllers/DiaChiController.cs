using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiaChiController : ControllerBase
    {
        private readonly IDiaChiService _diaChiService;

        public DiaChiController()
        {
            _diaChiService = new DiaChiService();
        }

        [HttpGet("GetByKhachHang/{idKhachHang}")]
        public IActionResult GetByKhachHang(Guid idKhachHang)
        {
            var diaChis = _diaChiService.GetDiaChiByKhachHang(idKhachHang);
            return Ok(diaChis);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var diaChi = _diaChiService.GetDiaChiById(id);
            if (diaChi == null) return NotFound();
            return Ok(diaChi);
        }

        [HttpPost("Create")]
        public IActionResult Create(DiaChi diaChi)
        {
            var result = _diaChiService.CreateDiaChi(diaChi);
            if (result.Success)
                return Ok(new { success = true, message = result.ErrorMessage });
            return BadRequest(new { success = false, message = result.ErrorMessage });
        }

        [HttpPut("Update")]
        public IActionResult Update(DiaChi diaChi)
        {
            var result = _diaChiService.UpdateDiaChi(diaChi);
            if (result.Success)
                return Ok(new { success = true, message = result.ErrorMessage });
            return BadRequest(new { success = false, message = result.ErrorMessage });
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            var result = _diaChiService.DeleteDiaChi(id);
            if (result.Success)
                return Ok(new { success = true, message = result.ErrorMessage });
            return BadRequest(new { success = false, message = result.ErrorMessage });
        }

        [HttpPut("SetDefault/{idDiaChi}/{idKhachHang}")]
        public IActionResult SetDefault(Guid idDiaChi, Guid idKhachHang)
        {
            var result = _diaChiService.SetDefaultDiaChi(idDiaChi, idKhachHang);
            if (result.Success)
                return Ok(new { success = true, message = result.ErrorMessage });
            return BadRequest(new { success = false, message = result.ErrorMessage });
        }
    }
}