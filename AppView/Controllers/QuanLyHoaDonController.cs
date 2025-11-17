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
        //public async Task<IActionResult> DoiTrangThai(Guid idhd, int trangthai)
        //{
        //    try
        //    {
        //        var loginInfor = new LoginViewModel();
        //        string? session = HttpContext.Session.GetString("LoginInfor");
        //        if (session != null)
        //        {
        //            loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
        //            var idnv = loginInfor.Id;

        //            HttpResponseMessage response;

        //            if (trangthai == 6) // Xử lý logic GiaoThanhCong (giữ nguyên)
        //            {
        //                string url = $"HoaDon/GiaoThanhCong?idhd={idhd}&idnv={idnv}";
        //                response = await _httpClient.PutAsync(url, null);
        //            }
        //            else // Xử lý các trạng thái khác (BAO GỒM trangthai 10 - XÁC NHẬN)
        //            {
        //                string url = $"HoaDon?idhoadon={idhd}&trangthai={trangthai}&idnhanvien={idnv}";
        //                response = await _httpClient.PutAsync(url, null);
        //            }

        //            // === BẮT ĐẦU SỬA LỖI XỬ LÝ RESPONSE ===

        //            if (response.IsSuccessStatusCode)
        //            {
        //                // Thành công (200 OK)
        //                return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
        //            }
        //            else
        //            {
        //                // Thất bại (409 Conflict, 500 Server Error, v.v.)
        //                // Đọc nội dung lỗi từ API
        //                var responseString = await response.Content.ReadAsStringAsync();

        //                // Cố gắng parse JSON lỗi từ API ({"success":false, "message":"..."})
        //                try
        //                {
        //                    var errorResponse = JsonConvert.DeserializeObject<ApiResponseViewModel>(responseString);
        //                    // Trả về thông báo lỗi cụ thể (ví dụ: "...hết hàng")
        //                    return Json(new { success = false, message = errorResponse.Message });
        //                }
        //                catch
        //                {
        //                    // Nếu không parse được (lỗi 500, v.v.), trả về thông báo lỗi chung
        //                    return Json(new { success = false, message = $"Cập nhật thất bại. (API Status: {response.StatusCode})" });
        //                }
        //            }
        //            // === KẾT THÚC SỬA LỖI ===
        //        }
        //        return Json(new { success = false, message = "Cập nhật trạng thái thất bại (Lỗi Session)" });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Báo lỗi exception nếu có
        //        return Json(new { success = false, message = $"Cập nhật thất bại (Exception): {ex.Message}" });
        //    }
        //}



        //Hủy hóa đơn
        //[HttpPost("/QuanLyHoaDon/HuyHD")]
        //public async Task<IActionResult> HuyHD(Guid idhd, string ghichu)
        //{
        //    try
        //    {
        //        var loginInfor = new LoginViewModel();
        //        string? session = HttpContext.Session.GetString("LoginInfor");
        //        if (session != null)
        //        {
        //            loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
        //        }
        //        var idnv = loginInfor.Id;
        //        if(ghichu != null)
        //        {
        //            string url = $"HoaDon/HuyHD?idhd={idhd}&idnv={idnv}";
        //            var response = await _httpClient.PutAsync(url, null);
        //            if (response.IsSuccessStatusCode)
        //            {
        //                var stringURL = $"https://localhost:7095/api/HoaDon/UpdateGhichu?idhd={idhd}&idnv={loginInfor.Id}&ghichu={ghichu}";
        //                var responseghichu = await _httpClient.PutAsync(stringURL, null);
        //                if (responseghichu.IsSuccessStatusCode)
        //                {
        //                    return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
        //                }
        //            }
        //        }
        //        return Json(new { success = false, message = "Ghi chú không được để null" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("_QuanLyHoaDon", "QuanLyHoaDon");
        //    }
        //}

        // (Trong file QuanLyHoaDonController.cs)
        // (Nhớ thêm "using System.Net;" ở đầu file nếu chưa có)

        [HttpPost] // Đảm bảo đây là [HttpPost]
        public async Task<IActionResult> DoiTrangThai(Guid idhd, int trangthai, string? ghichu) // Thêm string? ghichu
        {
            try
            {
                var loginInfor = new LoginViewModel();
                string? session = HttpContext.Session.GetString("LoginInfor");
                if (session != null)
                {
                    loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                    var idnv = loginInfor.Id;

                    // 1. Kiểm tra logic ghi chú
                    // Nếu là trạng thái "xấu" mà không có ghi chú -> báo lỗi
                    if ((trangthai == 7 || trangthai == 9 || trangthai == 4 || trangthai == 11) && string.IsNullOrWhiteSpace(ghichu))
                    {
                        return Json(new { success = false, message = "Vui lòng nhập ghi chú/lý do." });
                    }

                    // 2. Mã hóa ghi chú (nếu có) để gửi qua URL
                    var ghichuEncoded = string.IsNullOrWhiteSpace(ghichu) ? "" : WebUtility.UrlEncode(ghichu);

                    // 3. Gọi API (LUÔN GỌI ENDPOINT NÀY)
                    // Endpoint này là [HttpPut] trong HoaDonController (bên AppAPI)
                    string url = $"HoaDon?idhoadon={idhd}&trangthai={trangthai}&idnhanvien={idnv}&ghichu={ghichuEncoded}";
                    var response = await _httpClient.PutAsync(url, null);

                    // 4. Xử lý kết quả
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
                    }
                    else
                    {
                        // Đọc lỗi trả về từ API (ví dụ: "Hết hàng")
                        var responseString = await response.Content.ReadAsStringAsync();
                        try
                        {
                            var errorResponse = JsonConvert.DeserializeObject<ApiResponseViewModel>(responseString);
                            return Json(new { success = false, message = errorResponse.Message });
                        }
                        catch
                        {
                            return Json(new { success = false, message = $"Cập nhật thất bại. (API Status: {response.StatusCode})" });
                        }
                    }
                }
                return Json(new { success = false, message = "Cập nhật trạng thái thất bại (Lỗi Session)" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Cập nhật thất bại (Exception): {ex.Message}" });
            }
        }

     

        //[HttpPost("/QuanLyHoaDon/HuyHD")] // Nhận POST từ JavaScript (Đúng)
        //public async Task<IActionResult> HuyHD(Guid idhd, string ghichu)
        //{
        //    try
        //    {
        //        var loginInfor = new LoginViewModel();
        //        string? session = HttpContext.Session.GetString("LoginInfor");
        //        if (session != null)
        //        {
        //            loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
        //        }
        //        var idnv = loginInfor.Id;

        //        // === BƯỚC 1: SỬA LẠI LOGIC KIỂM TRA ===
        //        // Kiểm tra xem 'ghichu' CÓ BỊ RỖNG không. Dùng IsNullOrWhiteSpace.
        //        if (string.IsNullOrWhiteSpace(ghichu))
        //        {
        //            return Json(new { success = false, message = "Ghi chú không được để null" });
        //        }

        //        // === BƯỚC 2: MÃ HÓA (ENCODE) GHI CHÚ ===
        //        // Mã hóa chuỗi ghi chú để gửi qua URL an toàn
        //        var ghichuEncoded = WebUtility.UrlEncode(ghichu);

        //        // Nếu code chạy đến đây, 'ghichu' đã hợp lệ
        //        // 3. Gọi API để HỦY đơn (trạng thái 7)
        //        string url = $"HoaDon/HuyHD?idhd={idhd}&idnv={idnv}";
        //        var response = await _httpClient.PutAsync(url, null);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            // 4. Gọi API để CẬP NHẬT GHI CHÚ (đã mã hóa)
        //            // Dùng đường dẫn tương đối (không hardcode localhost)
        //            var stringURL = $"HoaDon/UpdateGhichu?idhd={idhd}&idnv={loginInfor.Id}&ghichu={ghichuEncoded}";
        //            var responseghichu = await _httpClient.PutAsync(stringURL, null);

        //            if (responseghichu.IsSuccessStatusCode)
        //            {
        //                return Json(new { success = true, message = "Đã hủy hóa đơn thành công" });
        //            }
        //            else
        //            {
        //                // Vẫn báo thành công, nhưng cảnh báo lỗi ghi chú
        //                return Json(new { success = true, message = "Hủy đơn thành công, nhưng không thể lưu ghi chú." });
        //            }
        //        }

        //        // Nếu hủy đơn thất bại
        //        return Json(new { success = false, message = "Hủy đơn thất bại (lỗi API)." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { success = false, message = "Lỗi hệ thống: " + ex.Message });
        //    }
        //}


        //Hoàn hàng
        //[HttpGet("/QuanLyHoaDon/HoanHang")] //
        //public async Task<IActionResult> HoanHang(Guid idhd, string ghichu)
        //{
        //    try
        //    {
        //        var loginInfor = new LoginViewModel();
        //        string? session = HttpContext.Session.GetString("LoginInfor");
        //        if (session != null)
        //        {
        //            loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
        //        }
        //        var idnv = loginInfor.Id;

        //        string url = $"HoaDon/HoanHD?idhd={idhd}&idnv={idnv}";
        //        var response = await _httpClient.PutAsync(url, null);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var stringURL = $"https://localhost:7095/api/HoaDon/UpdateGhichu?idhd={idhd}&idnv={loginInfor.Id}&ghichu={ghichu}";
        //            var responseghichu = await _httpClient.PutAsync(stringURL, null);
        //            if (responseghichu.IsSuccessStatusCode)
        //            {
        //                return Json(new { success = true, message = "Cập nhật trạng thái thành công" });
        //            }
        //        }
        //        return Json(new { success = false, message = "Cập nhật trạng thái thất bại" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("_QuanLyHoaDon", "QuanLyHoaDon");
        //    }
        //}
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
    public class ApiResponseViewModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
