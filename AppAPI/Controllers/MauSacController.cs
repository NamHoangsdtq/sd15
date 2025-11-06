using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MauSacController : ControllerBase
    {
        private readonly IQlThuocTinhService service;
        private readonly AssignmentDBContext _dbContext;
        public MauSacController()
        {
            service = new QlThuocTinhService();
            _dbContext = new AssignmentDBContext();
        }
        #region MauSac
        [HttpGet("GetAllMauSac")]
        public async Task<IActionResult> GetAllMauSac()
        {
            var tr = await service.GetAllMauSac();
            return Ok(tr);
        }
        [Route("TimKiemMauSac")]
        [HttpGet]
        public List<MauSac> GetAllMauSac(string? name)
        {
            return _dbContext.MauSacs.Where(v => v.Ten.Contains(name) || v.Ma.Contains(name)).ToList();
        }
        [Route("GetMauSacById")]
        [HttpGet]
        public async Task<IActionResult> GetMauSacById(Guid id)
        {
            var cl = await service.GetMauSacById(id);
            if (cl == null) return BadRequest();
            return Ok(cl);
        }
        [HttpPost("ThemMauSac")]
        public async Task<IActionResult> Add(string ten, string ma, int trangthai)
        {

            var tr = await service.AddMauSac(ten, ma, trangthai);
            if (tr == null)
            {
                return BadRequest();
            }
            return Ok(tr);
        }

        // PUT api/<NhanVienController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, string ten, string ma, int trangthai)
        {
            var bv = await service.UpdateMauSac(id, ten, ma, trangthai);
            if (bv == null)
            {
                return BadRequest();
            }

            return Ok(bv);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMauSac(Guid id)
        {
            var loaiSP = await service.DeleteMauSac(id);
            return Ok(loaiSP);
        }
        #endregion



    }
}
