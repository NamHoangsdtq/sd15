using AppAPI.IServices;
using AppAPI.Services;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly IKhachHangService _khachHangService;
        private readonly AssignmentDBContext _dbcontext;
        public KhachHangController()
        {
            _khachHangService = new KhachHangService();
            _dbcontext = new AssignmentDBContext();
            
        }
        // GET: api/<SanPhamController>
        [HttpGet]
        public List<KhachHang> GetAllKhachHang()
        {
            return _khachHangService.GetAll();
        }
        [Route("TimKiemKH")]
        [HttpGet]
        public List<KhachHang> GetAllKhachHang(string? Ten, string? SDT)
        {
            return _dbcontext.KhachHangs.Where(x=>x.SDT.Contains(SDT)|| x.Ten.Contains(Ten)).ToList();
        }
        [Route("GetById")]
        [HttpGet]
        public KhachHang GetById(Guid id)
        {
            return _khachHangService.GetById(id);
        }
        [HttpGet("GetKhachHangByEmail")] 
        public KhachHangViewModel GetKhachHangByEmail(string email)
        {
            var temp = _dbcontext.KhachHangs.FirstOrDefault(x => x.Email == email);
            if (temp != null)
            {
                var khachHang = new KhachHangViewModel()
                {
                    Id = temp.IDKhachHang,
                    Email = temp.Email
                };
                return khachHang;
            }
            else return new KhachHangViewModel();
        }
        [HttpGet("ChangeForgotPassword")]
        public async Task<bool> ChangeForgotPassword(string id, string password)
        {
            try
            {
                var tempID = new Guid(id);  
                var temp = _dbcontext.KhachHangs.First(x => x.IDKhachHang == tempID);
                temp.Password = MaHoaMatKhauS(password);
                _dbcontext.Update(temp);
                await _dbcontext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private string MaHoaMatKhauS(string matKhau)
        {
            // Ở đây, bạn có thể sử dụng bất kỳ phương thức mã hóa mật khẩu nào phù hợp
            // Ví dụ: sử dụng thư viện BCrypt.Net để mã hóa mật khẩu
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(matKhau);
            return hashedPassword;

            // Đây chỉ là ví dụ đơn giản, không nên sử dụng trong môi trường thực tế
            //return matKhau;
        }
        //Nhinh
        [HttpGet("getBySDT")]
        public KhachHang GetBySDT(string sdt)
        {
            return _khachHangService.GetBySDT(sdt);
        }
        [HttpGet("getAllHDKH")]
        public async Task<List<HoaDon>> GetAllHDKH(Guid idkh)
        {
            return await _khachHangService.GetAllHDKH(idkh);
        }

        // GET api/<SanPhamController>/5
        [Route("PostKHView")]
        [HttpPost]
        public bool PostKHView(KhachHangView khv)
        {
            khv.IDKhachHang = Guid.NewGuid();
            KhachHang kh = new KhachHang();
            kh.IDKhachHang = khv.IDKhachHang;
            kh.Ten = khv.Ten?.Trim();
            kh.Password = MaHoaMatKhau(khv.Password).Trim();
            kh.GioiTinh=khv.GioiTinh;
            kh.NgaySinh=khv.NgaySinh;
            kh.Email = khv.Email?.Trim();
            kh.DiaChi=khv.DiaChi?.Trim();
            kh.SDT = khv.SDT?.Trim();
            kh.TrangThai=1;
            kh.DiemTich = 0;
            _dbcontext.KhachHangs.Add(kh);
            GioHang gh= new GioHang();
            gh.IDKhachHang=kh.IDKhachHang;
            gh.NgayTao=DateTime.Now;
            _dbcontext.GioHangs.Add(gh);
            _dbcontext.SaveChanges();
            return true;
        }
        private string MaHoaMatKhau(string matKhau)
        {
            // Ở đây, bạn có thể sử dụng bất kỳ phương thức mã hóa mật khẩu nào phù hợp
            // Ví dụ: sử dụng thư viện BCrypt.Net để mã hóa mật khẩu
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(matKhau);
            return hashedPassword;

            // Đây chỉ là ví dụ đơn giản, không nên sử dụng trong môi trường thực tế
            //return matKhau;
        }


        // POST api/<SanPhamController>
        [HttpPost]
        public async Task<IActionResult> Post(KhachHangViewModel khachHang)
        {
            var kh = await _khachHangService.Add(khachHang);
            if (kh == null)
            {
                return BadRequest();
            }

            return Ok("Đăng ký thành công");
        }
        // PUT api/<SanPhamController>/5
        [Route("PutKhView")]
        [HttpPut]
        public bool PutKhView(KhachHangView khv)
        {
            var kh = _khachHangService.GetById(khv.IDKhachHang);
            if (kh != null)
            {
                
                kh.Ten = khv.Ten?.Trim();
                //kh.Password = MaHoaMatKhau(khv.Password);
                kh.GioiTinh = khv.GioiTinh;
                kh.NgaySinh = khv.NgaySinh;
                //kh.Email = khv.Email;
                kh.DiaChi = khv.DiaChi?.Trim();
                //kh.SDT = khv.SDT;
                //kh.TrangThai = khv.TrangThai;
                
                _dbcontext.KhachHangs.Update(kh);
                _dbcontext.SaveChanges();
                return true;
            }
            return false;

        }

        // DELETE api/<SanPhamController>/5
        [HttpDelete("{id}")]
        public bool Delete(Guid id)
        {
            var result = _khachHangService.Delete(id);
            return result;
        }
    }
}
