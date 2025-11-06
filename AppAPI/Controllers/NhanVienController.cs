using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using AppData.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhanVienController : ControllerBase
    {
        private readonly INhanVienService _nhanVienService;
        private readonly AssignmentDBContext _dbContext;
        public NhanVienController()
        {
            _nhanVienService = new NhanVienService();
            _dbContext = new AssignmentDBContext();
        }
        // GET: api/<NhanVienController>
        [HttpGet("GetAll")]
        public List<NhanVien> GetAllNhanVien()
        {
            return _nhanVienService.GetAll();
        }
        [Route("TimKiemNhanVien")]
        [HttpGet]
        public List<NhanVien> GetAllNhanVien(string? name)
        {
            return _dbContext.NhanViens.Where(v => v.Ten.Contains(name) || v.SDT.Contains(name)).ToList();


        }

        // GET api/<NhanVienController>/5
        [Route("GetById")]
        [HttpGet]
        public NhanVien? GetById(Guid id)
        {
            return _nhanVienService.GetById(id);
        }


        // POST api/<NhanVienController>
        [HttpPost("DangKyNhanVien")]
        public async Task<IActionResult> Add(string ten, string email, string password, string sdt, string diachi, int trangthai, Guid idvaitro)
        {
            var tr = await _nhanVienService.Add(ten, email, password, sdt, diachi, trangthai, idvaitro);
            if (tr == null)
            {
                return BadRequest();
            }
            return Ok(tr);
        }

        // PUT api/<NhanVienController>/5
        [HttpPut("{id}")]
        public bool Put(Guid id, string ten, string email, string password, string sdt, string diachi, int trangthai, Guid idvaitro)
        {
            var nv = _nhanVienService.GetById(id);
            if (nv != null)
            {
                nv.Ten = ten;
                nv.Email = email;
                nv.PassWord = password;
                nv.SDT = sdt;
                nv.DiaChi = diachi;
                nv.TrangThai = trangthai;
                nv.IDVaiTro = idvaitro;
                _dbContext.NhanViens.Update(nv);
                _dbContext.SaveChanges();
                return true;
            }
            return false;

        }

        // DELETE api/<NhanVienController>/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            if (_nhanVienService.Delete(id))
            {
                return true;
            }
            return false;
        }

    }
}
