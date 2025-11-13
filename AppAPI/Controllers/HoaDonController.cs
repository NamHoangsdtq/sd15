using AppAPI.IServices;
using AppAPI.Services;
using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonService _iHoaDonService;
        public HoaDonController()
        {
            _iHoaDonService = new HoaDonService();
        }

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
            var lsthdcho =  _iHoaDonService.GetAllHDCho();
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
        public IActionResult UpdateTrangThai(Guid idhoadon, int trangthai, Guid? idnhanvien)
        {
            // Gọi service
            var (Success, ErrorMessage) = _iHoaDonService.UpdateTrangThaiGiaoHang(idhoadon, trangthai, idnhanvien);

            if (Success)
            {
                // Trả về 200 OK
                // Bạn có thể trả về bool 'true' như cũ nếu frontend đang mong đợi
                // return Ok(true); 
                // Hoặc trả về object JSON (khuyến nghị):
                return Ok(new { success = true, message = ErrorMessage });
            }
            else
            {
                // Trả về lỗi 409 Conflict (lỗi hết hàng) hoặc 400
                // Bạn có thể trả về bool 'false' như cũ nếu frontend đang mong đợi
                // return Ok(false); 
                // Hoặc trả về object JSON (khuyến nghị):
                return Conflict(new { success = false, message = ErrorMessage });
            }
        }

        [HttpPut("UpdateHoaDon")]
        public IActionResult UpDateHoaDon(HoaDonThanhToanRequest hoaDon)
        {
            var (Success, ErrorMessage) = _iHoaDonService.UpdateHoaDon(hoaDon);

            if (Success)
            {
                // Trả về 200 OK
                return Ok(new { success = true, message = ErrorMessage });
            }
            else
            {
                // Trả về 409 Conflict (lỗi hết hàng)
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
        //[HttpGet("PhuongThucThanhToan")]
        //public List<PhuongThucThanhToan> GetAllPTTT()
        //{
        //    return _iHoaDonService.GetAllPTTT();
        //}
        //[HttpPost("PhuongThucThanhToan")]
        //public bool CreatePTT(PhuongThucThanhToan pttt)
        //{
        //    return _iHoaDonService.CreatePTTT(pttt);
        //}
        //[HttpPut("PhuongThucThanhToan")]
        //public bool UpdatePTT(PhuongThucThanhToan pttt)
        //{
        //    return _iHoaDonService.UpdatePTTT(pttt);
        //}
        //[HttpDelete("PhuongThucThanhToan/{id}")]
        //public bool DeletePTT(Guid id)
        //{
        //    return _iHoaDonService.DeletePTTT(id);
        //}
    }
}
