using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatLieuController : ControllerBase
    {
        private readonly IQlThuocTinhService service;
        private readonly AssignmentDBContext _dbContext;
        public ChatLieuController()
        {
            service = new QlThuocTinhService();
            _dbContext = new AssignmentDBContext();
        }
        #region KichCo
        [HttpGet("GetAllChatLieu")]
        public async Task<IActionResult> GetAllChatLieu()
        {
            var cl = await service.GetAllChatLieu();
            return Ok(cl);
        }
        [Route("TimKiemChatLieu")]
        [HttpGet]
        public List<ChatLieu> GetAllChatLieu(string? name)
        {
            return _dbContext.ChatLieus.Where(v => v.Ten.Contains(name)).ToList();
        }
        [Route("GetChatLieuById")]
        [HttpGet]
        public async Task<IActionResult> GetChatLieuById(Guid id)
        {
            var cl = await service.GetChatLieuById(id);
            if (cl == null) return BadRequest();
            return Ok(cl);
        }
        [HttpPost("ThemChatLieu")]
        public async Task<IActionResult> Add(string ten, int trangthai)
        {

            var nv = await service.AddChatLieu(ten, trangthai);
            if (nv == null)
            {
                return BadRequest();
            }
            return Ok(nv);
        }

        // PUT api/<NhanVienController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, string ten, int trangthai)
        {
            var bv = await service.UpdateChatLieu(id, ten, trangthai);
            if (bv == null)
            {
                return BadRequest(); // Trả về BadRequest nếu tên trùng
            }

            return Ok(bv);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChatLieu(Guid id)
        {
            var loaiSP = await service.DeleteChatLieu(id);
            return Ok(loaiSP);
        }
        #endregion
    }
}
