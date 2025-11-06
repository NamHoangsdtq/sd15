using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using System.Globalization;
using System.Net;

namespace AppView.Controllers
{
    public class QuanLyHoaDonController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly ITempDataProvider _tempDataProvider;
        public QuanLyHoaDonController(IServiceProvider serviceProvider, ITempDataProvider tempDataProvider)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
            _serviceProvider = serviceProvider;
            _tempDataProvider = tempDataProvider;
        }
        //View QLHD
        public IActionResult _QuanLyHoaDon()
        {
            return View();
        }
        // Load tất cả hóa đơn
        public async Task<IActionResult> LoadAllHoaDon(FilterHD filter)
        {
            var listhdql = await _httpClient.GetFromJsonAsync<List<HoaDonQL>>("HoaDon/GetAllHDQly");
            listhdql = listhdql.OrderByDescending(c => c.ThoiGian).ToList();
            //Lọc thời gian
            if (filter.ngaybd != null)
            {
                string[] formats = { "MM/dd/yyyy HH:mm:ss" };
                DateTime parsedDate = DateTime.ParseExact(filter.ngaybd, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime parsedDate1 = DateTime.ParseExact(filter.ngaykt, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                string output = parsedDate.ToString("MM/dd/yyyy HH:mm:ss");
                string output1 = parsedDate1.ToString("MM/dd/yyyy HH:mm:ss");
                var bd = DateTime.ParseExact(output, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                var kt = DateTime.ParseExact(output1, formats, new CultureInfo("en-US"), DateTimeStyles.None);
                listhdql = listhdql.Where(c => c.ThoiGian >= bd && c.ThoiGian <= kt).ToList();
            }
            //Tìm kiếm 
            if (filter.keyWord != null)
            {
                if (filter.loaitk == 1)
                {
                    listhdql = listhdql.Where(c => c.MaHD.ToLower().Contains(filter.keyWord.Trim().ToLower()) || (c.SDTnhanhang != null && c.SDTnhanhang.Contains(filter.keyWord.Trim()))).ToList();
                }
                else if (filter.loaitk == 2)
                {
                    listhdql = listhdql.Where(c => c.KhachHang.ToLower().Contains(filter.keyWord.Trim().ToLower()) || (c.SDTKH != null && c.SDTKH.Contains(filter.keyWord.Trim()))).ToList();
                }
            }

            //Lọc kênh
            if (filter.loaiHD != null)
            {
                listhdql = listhdql.Where(c => filter.loaiHD.Contains(c.LoaiHD)).ToList();
            }
            //Lọc trạng thái
            if (filter.lstTT != null)
            {
                listhdql = listhdql.Where(c => filter.lstTT.Contains(c.TrangThai)).ToList();
            }

            // Tổng tiền hàng
            var tth = listhdql.Sum(c => c.TongTienHang);
            // Tổng tiền khách đã trả
            var tktra = listhdql.Sum(c => c.KhachDaTra);

            int totalRow = listhdql.Count;
            var model = listhdql.Skip((filter.page - 1) * filter.pageSize).Take(filter.pageSize).ToArray();
            //Lọc loại hd
            return Json(new
            {
                tienhang = tth,
                khachtra = tktra,
                data = model,
                total = totalRow,
                status = true,
            });
        }

        //Chi tiết hóa đơn 
        [HttpGet("/QuanLyHoaDon/ViewChiTietHD/{idhd}")]
        public async Task<IActionResult> ViewChiTietHD(string idhd)
        {
            var hd = await _httpClient.GetFromJsonAsync<ChiTietHoaDonQL>($"HoaDon/ChiTietHoaDonQL/{idhd}");
            return PartialView("_ThongTinHD", hd);
        }
        //Sao chép hóa đơn
        [HttpGet("/QuanLyHoaDon/CopyHD")]
        public async Task<IActionResult> CopyHD(string idhd)
        {
            try
            {
                var loginInfor = new LoginViewModel();
                string? session = HttpContext.Session.GetString("LoginInfor");
                if (session != null)
                {
                    loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                }
                var idnv = loginInfor.Id;

                string url = $"HoaDon/CopyHD?idhd={idhd}&idnv={idnv}";
                var response = await _httpClient.PutAsync(url, null);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("BanHang", "BanHangTaiQuay");
                }
                return Json(new { success = false, message = "Sao chép hóa đơn thất bại" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("_QuanLyHoaDon", "QuanLyHoaDon");
            }
        }
        // Cập nhật trạng thái
        public async Task<IActionResult> DoiTrangThai(Guid idhd, int trangthai)// Dùng cho trạng thái truyền  vào: 10, 3
        {
            try
            {
                var loginInfor = new LoginViewModel();
                string? session = HttpContext.Session.GetString("LoginInfor");
                if (session != null)
                {
                    loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                    var idnv = loginInfor.Id;
                    if (trangthai == 6)
                    {
                        string url = $"HoaDon/GiaoThanhCong?idhd={idhd}&idnv={idnv}";
                        var response = await _httpClient.PutAsync(url, null);
                        if (response.IsSuccessStatusCode)
                        {
                            return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
                        }
                    }
                    else
                    {
                        string url = $"HoaDon?idhoadon={idhd}&trangthai={trangthai}&idnhanvien={idnv}";
                        var response = await _httpClient.PutAsync(url, null);
                        if (response.IsSuccessStatusCode)
                        {
                            return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
                        }
                    }
                }
                return Json(new { success = false, message = "Cập nhật trạng thái thất bại" });
            }
            catch (Exception)
            {
                return RedirectToAction("_QuanLyHoaDon", "QuanLyHoaDon");
            }
        }
        //Hủy hóa đơn
        [HttpGet("/QuanLyHoaDon/HuyHD")]
        public async Task<IActionResult> HuyHD(Guid idhd, string ghichu)
        {
            try
            {
                var loginInfor = new LoginViewModel();
                string? session = HttpContext.Session.GetString("LoginInfor");
                if (session != null)
                {
                    loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                }
                var idnv = loginInfor.Id;
                if(ghichu != null)
                {
                    string url = $"HoaDon/HuyHD?idhd={idhd}&idnv={idnv}";
                    var response = await _httpClient.PutAsync(url, null);
                    if (response.IsSuccessStatusCode)
                    {
                        var stringURL = $"https://localhost:7095/api/HoaDon/UpdateGhichu?idhd={idhd}&idnv={loginInfor.Id}&ghichu={ghichu}";
                        var responseghichu = await _httpClient.PutAsync(stringURL, null);
                        if (responseghichu.IsSuccessStatusCode)
                        {
                            return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
                        }
                    }
                }
                return Json(new { success = false, message = "Ghi chú không được để null" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("_QuanLyHoaDon", "QuanLyHoaDon");
            }
        }
        //Hoàn hàng
        [HttpGet("/QuanLyHoaDon/HoanHang")] //
        public async Task<IActionResult> HoanHang(Guid idhd, string ghichu)
        {
            try
            {
                var loginInfor = new LoginViewModel();
                string? session = HttpContext.Session.GetString("LoginInfor");
                if (session != null)
                {
                    loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                }
                var idnv = loginInfor.Id;

                string url = $"HoaDon/HoanHD?idhd={idhd}&idnv={idnv}";
                var response = await _httpClient.PutAsync(url, null);
                if (response.IsSuccessStatusCode)
                {
                    var stringURL = $"https://localhost:7095/api/HoaDon/UpdateGhichu?idhd={idhd}&idnv={loginInfor.Id}&ghichu={ghichu}";
                    var responseghichu = await _httpClient.PutAsync(stringURL, null);
                    if (responseghichu.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
                    }
                }
                return Json(new { success = false, message = "Cập nhật trạng thái thất bại" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("_QuanLyHoaDon", "QuanLyHoaDon");
            }
        }
        //Hoàn hàng thành công
        [HttpGet("/QuanLyHoaDon/HoanHangTC")] //
        public async Task<IActionResult> HoanHangTC(Guid idhd)
        {
            try
            {
                var loginInfor = new LoginViewModel();
                string? session = HttpContext.Session.GetString("LoginInfor");
                if (session != null)
                {
                    loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                }
                var idnv = loginInfor.Id;

                string url = $"HoaDon/HoanTCHD?idhd={idhd}&idnv={idnv}";
                var response = await _httpClient.PutAsync(url, null);
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
                }
                return Json(new { success = false, message = "Cập nhật trạng thái thất bại" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("_QuanLyHoaDon", "QuanLyHoaDon");
            }
        }

        //Xuất PDF
        [HttpGet("/Admin/QuanLyHoaDon/ExportPDF/{idhd}")]
        public async Task<IActionResult> ExportPDF(Guid idhd)
        {
            try
            {
                var cthd = await _httpClient.GetFromJsonAsync<ChiTietHoaDonQL>($"HoaDon/ChiTietHoaDonQL/{idhd}");
                var view = new ViewAsPdf("ExportHD", cthd)
                {
                    FileName = $"{cthd.MaHD}.pdf",
                    PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                    PageSize = Rotativa.AspNetCore.Options.Size.A4,
                };
                return view;
            }
            catch (Exception ex)
            {
                return RedirectToAction("_QuanLyHoaDon", "QuanLyHoaDon");
            }
        }
        //In hóa đơn
        [HttpGet("/QuanLyHoaDon/PrintHD/{idhd}")]
        public async Task<IActionResult> PrintHD(Guid idhd)
        {
            var cthd = await _httpClient.GetFromJsonAsync<ChiTietHoaDonQL>($"HoaDon/ChiTietHoaDonQL/{idhd}");
            return View("ExportHD", cthd);
        }
    }
}
