using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.QLND;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanLyNguoiDungController : ControllerBase
    {
        private IQuanLyNguoiDungService service;

        public QuanLyNguoiDungController()
        {
            this.service = new QuanLyNguoiDungService();
        }

        // POST api/<DangNhapController>
        [HttpGet("DangNhap")]
        public async Task<IActionResult> Login(string lg, string password)
        {
            LoginViewModel login = await service.Login(lg, password);

            if (login.IsAccountLocked)
            {
                return Unauthorized(login.Message);
            }
            else if (login.Message != null) // Other error messages
            {
                ModelState.AddModelError(string.Empty, login.Message);
                return BadRequest(ModelState);
            }
            return Ok(login);
        }

        // POST api/<DangKyController>
        //[HttpPost("DangKyNhanVien")]
        //public async Task<IActionResult> DangKyNhanVien(NhanVienViewModel nhanVien)
        //{
        //    var nv = await service.RegisterNhanVien(nhanVien);
        //    if (nv == null)
        //    {
        //        return BadRequest();
        //    }

        //    return Ok("Đăng ký thành công");
        //}
        //POST api/<DangKyController>
        [HttpPost("DangKyKhachHang")]
        public async Task<IActionResult> DangKyKhachHang(KhachHangViewModel khachHang)
        {
            var kh = await service.RegisterKhachHang(khachHang);
            if (kh == null)
            {
                return BadRequest();
            }

            return Ok("Đăng ký thành công");

        }
        //[HttpPut("DoiMatKhauNhanVien")]
        //public async Task<IActionResult> DoiMatKhauNV(string email, string oldPassword,string newPassword)
        //{
        //    var dmk = await service.ChangePasswordNhanVien(email, oldPassword, newPassword);
        //    if (!dmk)
        //    {
        //        return Ok("Đổi mật khẩu thành công");
        //    }
        //    else
        //    {
        //        return BadRequest("Đổi mật khẩu không thành công");
        //    }
        //}
        [HttpPut("DoiMatKhau")]
        public async Task<IActionResult> DoiMatKhau(string email, string oldPassword, string newPassword)
        {
            var dmk = await service.ChangePassword(email, oldPassword, newPassword);
            if (!dmk)
            {
                return BadRequest("Đổi mật khẩu  thành công");

            }
            else
            {
                return Ok("Đổi mật khẩu khong  thành công");
            }
        }
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string request)
        {
            if (!IsValidEmailAddress(request))
            {
                return BadRequest("địa chỉ email không hợp lệ");
            }

            var result = await service.ForgetPassword(request);

            if (result)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest("Lỗi khi gửi mail");
            }
        }
        private bool IsValidEmailAddress(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            var emailAddressAttribute = new EmailAddressAttribute();

            return emailAddressAttribute.IsValid(email);
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            if (!IsValidEmailAddress(request.Email))
            {
                return BadRequest("địa chỉ email không hợp lệ");
            }

            if (!IsValidPassword(request.Password))
            {
                return BadRequest("Mật khẩu không hợp lệ");
            }

            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest("mật khẩu không khớp");
            }

            var result = await service.ResetPassword(request);

            if (result)
            {
                return Ok("Đặt lại mật khẩu thành công");
            }
            else
            {
                return BadRequest("Không thể đặt lại mật khẩu");
            }
        }
        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            // Chỉ kiểm tra mật khẩu phải có ít nhất 8 ký tự
            if (password.Length < 8)
            {
                return false;
            }

            // Nếu đã dài hơn 8 ký tự, luôn chấp nhận
            return true;
        }
        //Tam
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> DoiMatKhau(ChangePasswordRequest request)
        {
            var dmk = await service.ChangePassword(request);
            if (!dmk)
            {
                return BadRequest("Đổi mật khẩu khong  thành công");
            }
            else
            {
                return Ok("Đổi mật khẩu  thành công");
            }
        }
        [HttpPut("UpdateProfile1")]
        public async Task<IActionResult> UpdateProfile(LoginViewModel request)
        {
            LoginViewModel dmk = await service.UpdateProfile(request);
            if (dmk == null)
            {
                return BadRequest("Đổi thông tin người dùng không  thành công");
            }
            else
            {
                return Ok(dmk);
            }
        }
        [HttpGet("UseDiemTich")]
        public async Task<int> UseDiemTich(int diem, string id)
        {
            return await service.UseDiemTich(diem, id);
        }
        //End
        //Nhinh
        [HttpPost("AddNhanhKH")]
        public async Task<bool> AddNhanhKH(KhachHang kh)
        {
            return await service.AddNhanhKH(kh);
        }
    }
}
