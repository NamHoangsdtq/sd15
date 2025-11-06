using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiSPController : ControllerBase
    {
        private readonly ILoaiSPService _loaiSPService;
        private readonly AssignmentDBContext context;
        public LoaiSPController()
        {
            _loaiSPService = new LoaiSPService();
            context = new AssignmentDBContext();
        }
        #region LoaiSP
        [HttpGet("getAll")]
        public async Task<IActionResult> GetAll()
        {
            var listLsp = await _loaiSPService.GetAllLoaiSP();
            return Ok(listLsp);
        }
        [Route("TimKiemLoaiSP")]
        [HttpGet]
        public async Task<IActionResult> GetAllLoaiSP(string name)
        {
            var tr = context.LoaiSPs.Where(v => v.Ten.Contains(name)).ToList();
            return Ok(tr);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var lsp = await _loaiSPService.GetLoaiSPById(id);
            if (lsp == null) return BadRequest();
            return Ok(lsp);
        }
        [Route("save")]
        [HttpPost, HttpPut]
        public async Task<IActionResult> SaveLoaiSP(LoaiSPRequest lsp)
        {
            if (lsp == null) return BadRequest();
            var loaiSP = await _loaiSPService.SaveLoaiSP(lsp);
            return Ok(loaiSP);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteLoaiSP(Guid id)
        {
            var loaiSP = await _loaiSPService.DeleteLoaiSP(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetLoaiSpTheoCha(Guid id)
        {
            // Lấy danh sách các LoaiSPCha
            var listLoaiSpTheoCha = await context.LoaiSPs
              .Where(lsp => lsp.IDLoaiSPCha == id)
              .ToListAsync();
            if (listLoaiSpTheoCha == null)
            {
                return BadRequest();
            }
            return Ok(listLoaiSpTheoCha);
        }
        [HttpPost]
        public async Task<IActionResult> AddLoaiSPCha(Guid idLoaiSPCha, string ten, int trangthai)
        {
            var tr = _loaiSPService.AddSpCha(idLoaiSPCha, ten, trangthai);
            if (tr == null) return BadRequest();
            return Ok(tr);
        }
        #endregion
    }
}
