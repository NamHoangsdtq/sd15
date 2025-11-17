using AppAPI.IServices;
using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Mvc;

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonService _iHoaDonService;

        // === SỬA LẠI CONSTRUCTOR ===
        // Yêu cầu DI cung cấp IHoaDonService, thay vì tự 'new'
        public HoaDonController(IHoaDonService hoaDonService)
        {
            _iHoaDonService = hoaDonService;
        }
        // ============================

        // GET: api/<HoaDOnController>
        [HttpGet("GetAll")]
        public List<HoaDon> Get()
        {
            return _iHoaDonService.GetAllHoaDon();
        }

        [HttpGet("GetById/{idhd}")]
        public HoaDon GetById(Guid idhd)
        {
            return _iHoaDonService.GetHoaDonById(idhd);
        }

        // === THÊM ACTION MỚI ĐỂ APPVIEW GỌI ===
        [HttpGet("GetByToken/{token}")]
        public IActionResult GetHoaDonByToken(Guid token)
        {
            var hoaDon = _iHoaDonService.GetHoaDonByToken(token);
            if (hoaDon == null)
            {
                return NotFound("Không tìm thấy đơn hàng.");
            }
            return Ok(hoaDon);
        }
        // =======================================

        [HttpGet("TimKiem")]
        public List<HoaDon> TimKiemVaLoc(string ten, int? loc)
        {
            return _iHoaDonService.TimKiemVaLocHoaDon(ten, loc);
        }

        [HttpGet("CheckVoucher")]
        public int CheckVoucher(string ten, int tongtien)
        {
            return _iHoaDonService.CheckVoucher(ten, tongtien);
        }

        [HttpGet("LichSuGiaoDich")]
        public List<HoaDon> LichSuGiaoDich(Guid idNguoidung)
        {
            return _iHoaDonService.LichSuGiaoDich(idNguoidung);
        }

        [HttpGet("LichSuGiaoDich/{idhd}")]
        public LichSuTichDiem LichSuGiaoDichByIdHD(Guid idhd)
        {
            return _iHoaDonService.GetLichSuGiaoDichByIdHD(idhd);
        }

        [HttpGet("CheckLSGDHD/{idhd}")]
        public bool CheckLichSuGiaoDichHD(Guid idhd)
        {
            return _iHoaDonService.CheckHDHasLSGD(idhd);
        }

        [HttpGet("CheckCustomerUseVoucher")]
        public bool CheckKHUseVoucher(Guid idkh, Guid idvoucher)
        {
            return _iHoaDonService.CheckCusUseVoucher(idkh, idvoucher);
        }

        [HttpPost]
        public DonMuaSuccessViewModel CreateHoaDon(HoaDonViewModel hoaDon)
        {
            return _iHoaDonService.CreateHoaDon(hoaDon.ChiTietHoaDons, hoaDon);
        }

        [HttpPost("Offline/{idnhanvien}")]
        public bool CreateHoaDonOffline(Guid idnhanvien)
        {
            return _iHoaDonService.CreateHoaDonOffline(idnhanvien);
        }

        [HttpGet("GetAllHDCho")]
        public IActionResult GetAllHDCho()
        {
            var lsthdcho = _iHoaDonService.GetAllHDCho();
            return Ok(lsthdcho);
        }

        [HttpGet("GetHDBanHang/{idhd}")]
        public IActionResult GetHDBanHang(Guid idhd)
        {
            var lsthdcho = _iHoaDonService.GetHDBanHang(idhd);
            return Ok(lsthdcho);
        }

        [HttpGet("GetAllHDQly")]
        public IActionResult GetAllHDQly()
        {
            var hdql = _iHoaDonService.GetAllHDQly();
            return Ok(hdql);
        }

        [HttpGet("ChiTietHoaDonQL/{idhd}")]
        public IActionResult ChiTietHoaDonQL(Guid idhd)
        {
            var result = _iHoaDonService.GetCTHDByID(idhd);
            return Ok(result);
        }

        [HttpPut]
        //public IActionResult UpdateTrangThai(Guid idhoadon, int trangthai, Guid? idnhanvien)
        //{
        //    // Gọi service
        //    var (Success, ErrorMessage) = _iHoaDonService.UpdateTrangThaiGiaoHang(idhoadon, trangthai, idnhanvien);

        //    if (Success)
        //    {
        //        return Ok(new { success = true, message = ErrorMessage });
        //    }
        //    else
        //    {
        //        // Trả về 409 Conflict (lỗi hết hàng) hoặc 400
        //        return Conflict(new { success = false, message = ErrorMessage });
        //    }
        //}

        public IActionResult UpdateTrangThai(Guid idhoadon, int trangthai, Guid? idnhanvien, [FromQuery] string? ghichu) // Thêm [FromQuery] string? ghichu
        {
            // Gọi service (truyền ghi chú vào)
            var (Success, ErrorMessage) = _iHoaDonService.UpdateTrangThaiGiaoHang(idhoadon, trangthai, idnhanvien, ghichu);

            if (Success)
            {
                return Ok(new { success = true, message = ErrorMessage });
            }
            else
            {
                // Trả về Conflict (409) hoặc BadRequest (400)
                return Conflict(new { success = false, message = ErrorMessage });
            }
        }


        [HttpPut("UpdateHoaDon")]
        public IActionResult UpDateHoaDon(HoaDonThanhToanRequest hoaDon)
        {
            var (Success, ErrorMessage) = _iHoaDonService.UpdateHoaDon(hoaDon);

            if (Success)
            {
                return Ok(new { success = true, message = ErrorMessage });
            }
            else
            {
                return Conflict(new { success = false, message = ErrorMessage });
            }
        }

        [HttpPut("GiaoThanhCong")]
        public IActionResult GiaoThanhCong(Guid idhd, Guid idnv)
        {
            var result = _iHoaDonService.ThanhCong(idhd, idnv);
            return Ok(result);
        }

        [HttpPut("HoanHD")]
        public IActionResult HoanHD(Guid idhd, Guid idnv)
        {
            var result = _iHoaDonService.HoanHang(idhd, idnv);
            return Ok(result);
        }

        [HttpPut("HuyHD")] // Route này khớp với cái AppView đang gọi
        public IActionResult HuyHD(Guid idhd, Guid idnv)
        {
            try
            {
                // Gọi đến hàm HuyHD (Hủy đơn) trong Service của bạn
                var result = _iHoaDonService.HuyHD(idhd, idnv);
                if (result)
                {
                    return Ok(new { success = true, message = "Hủy đơn thành công." });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Không thể hủy đơn." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPut("HoanTCHD")]
        public IActionResult HoanTCHD(Guid idhd, Guid idnv)
        {
            var result = _iHoaDonService.HoanHangThanhCong(idhd, idnv);
            return Ok(result);
        }

        [HttpPut("CopyHD")]
        public IActionResult TraHD(Guid idhd, Guid idnv)
        {
            var result = _iHoaDonService.CopyHD(idhd, idnv);
            return Ok(result);
        }

        [HttpPut("UpdateGhichu")]
        public bool UpdateGhiChuHD(Guid idhd, Guid idnv, string ghichu)
        {
            return _iHoaDonService.UpdateGhiChuHD(idhd, idnv, ghichu);
        }

        [HttpDelete("deleteHoaDon/{id}")]
        public bool Delete(Guid id)
        {
            return _iHoaDonService.DeleteHoaDon(id);
        }
    }
}