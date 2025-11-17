using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.ThongKe;
using AppView.PhanTrang;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class VouchersController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly AssignmentDBContext dBContext;
        public VouchersController()
        {
            _httpClient = new HttpClient();
            dBContext = new AssignmentDBContext();
        }
        public int PageSize = 8;

        // get all vocher
        [HttpGet]
        public async Task<IActionResult> GetAllVoucher(int ProductPage = 1)
        {
            string apiURL = $"https://localhost:7095/api/Voucher";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<VoucherView>>(apiData);
            return View(new PhanTrangVouchers
            {
                listvouchers = roles
                        .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = roles.Count()
                }

            });
        }

        // tim kiem ten
        [HttpGet]
        public async Task<IActionResult> TimKiemTenVC(string Ten, int ProductPage = 1)
        {
            string apiURL = $"https://localhost:7095/api/Voucher";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<VoucherView>>(apiData);
            return View("GetAllVoucher", new PhanTrangVouchers
            {
                listvouchers = roles.Where(x => x.Ten.Contains(Ten.Trim()))
                        .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = roles.Count()
                }
            });
        }

        // create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(VoucherView voucher)
        {
            try
            {
                string apiURL = $"https://localhost:7095/api/Voucher";
                var response1 = await _httpClient.GetAsync(apiURL);
                var apiData = await response1.Content.ReadAsStringAsync();
                var roles = JsonConvert.DeserializeObject<List<VoucherView>>(apiData);

                if (voucher.SoTienCan != null || voucher.Ten != null || voucher.GiaTri != null || voucher.HinhThucGiamGia != null || voucher.TrangThai != null || voucher.NgayApDung != null || voucher.NgayKetThuc != null)
                {
                    if (voucher.SoTienCan < 0) ViewData["SoTienCan"] = "Số tiền cần không được âm ";
                    if (voucher.GiaTri <= 0) ViewData["GiaTri"] = "Mời bạn nhập giá trị lớn hơn 0";
                    if (voucher.SoLuong <= 0) ViewData["SoLuong"] = "Mời bạn nhập số lượng lớn hơn 0";
                    if (voucher.NgayKetThuc < voucher.NgayApDung) ViewData["Ngay"] = "Ngày kết thúc phải lớn hơn ngày áp dụng";

                    var timkiem = roles.FirstOrDefault(x => x.Ten == voucher.Ten.Trim());
                    if (timkiem != null) ViewData["Ma"] = "Mã này đã tồn tại";

                    if (voucher.HinhThucGiamGia == 1)
                    {
                        if (voucher.SoTienCan == 0)
                        {
                            if (voucher.GiaTri > 100 || voucher.GiaTri <= 0)
                            {
                                ViewData["GiaTri"] = "Giá trị từ 1 đến 100";
                                return View();
                            }
                            if (voucher.GiaTri <= 100 && voucher.GiaTri > 0)
                            {
                                if (voucher.SoTienCan >= 0 && voucher.GiaTri > 0 && voucher.SoLuong > 0 && voucher.NgayKetThuc >= voucher.NgayApDung && timkiem == null)
                                {
                                    var response = await _httpClient.PostAsJsonAsync($"https://localhost:7095/api/Voucher", voucher);
                                    if (response.IsSuccessStatusCode) return RedirectToAction("GetAllVoucher");
                                    return View();
                                }
                            }
                        }
                        if (voucher.SoTienCan > 0)
                        {
                            if (voucher.GiaTri <= voucher.SoTienCan)
                            {
                                if (voucher.GiaTri <= 100 && voucher.GiaTri > 0)
                                {
                                    if (voucher.SoTienCan >= 0 && voucher.GiaTri > 0 && voucher.SoLuong > 0 && voucher.NgayKetThuc >= voucher.NgayApDung && timkiem == null)
                                    {
                                        var response = await _httpClient.PostAsJsonAsync($"https://localhost:7095/api/Voucher", voucher);
                                        if (response.IsSuccessStatusCode) return RedirectToAction("GetAllVoucher");
                                        return View();
                                    }
                                }
                                if (voucher.GiaTri > 100 || voucher.GiaTri <= 0)
                                {
                                    ViewData["GiaTri"] = "Giá trị từ 1 đến 100";
                                    return View();
                                }
                            }
                            if (voucher.GiaTri > voucher.SoTienCan)
                            {
                                ViewData["GiaTri"] = "Giá trị phải nhỏ hơn hoặc bằng số tiền cần";
                                return View();
                            }
                        }
                    }
                    if (voucher.HinhThucGiamGia == 0)
                    {
                        if (voucher.SoTienCan == 0)
                        {
                            if (voucher.GiaTri <= 0)
                            {
                                ViewData["GiaTri"] = "Giá trị phải lớn hơn 0";
                                return View();
                            }
                            if (voucher.GiaTri > 0)
                            {
                                if (voucher.SoTienCan >= 0 && voucher.GiaTri > 0 && voucher.SoLuong > 0 && voucher.NgayKetThuc >= voucher.NgayApDung && timkiem == null)
                                {
                                    var response = await _httpClient.PostAsJsonAsync($"https://localhost:7095/api/Voucher", voucher);
                                    if (response.IsSuccessStatusCode) return RedirectToAction("GetAllVoucher");
                                    return View();
                                }
                            }
                        }
                        if (voucher.SoTienCan > 0)
                        {
                            if (voucher.GiaTri <= voucher.SoTienCan)
                            {
                                if (voucher.GiaTri <= 0)
                                {
                                    ViewData["GiaTri"] = "Giá trị phải lớn hơn 0";
                                    return View();
                                }
                                if (voucher.GiaTri > 0)
                                {
                                    if (voucher.SoTienCan >= 0 && voucher.GiaTri > 0 && voucher.SoLuong > 0 && voucher.NgayKetThuc >= voucher.NgayApDung && timkiem == null)
                                    {
                                        var response = await _httpClient.PostAsJsonAsync($"https://localhost:7095/api/Voucher", voucher);
                                        if (response.IsSuccessStatusCode) return RedirectToAction("GetAllVoucher");
                                        return View();
                                    }
                                }
                            }
                            if (voucher.GiaTri > voucher.SoTienCan)
                            {
                                ViewData["GiaTri"] = "Giá trị phải nhỏ hơn hoặc bằng số tiền cần";
                                return View();
                            }
                        }
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // update
        [HttpGet]
        public IActionResult Updates(Guid id)
        {
            try
            {
                var url = $"https://localhost:7095/api/Voucher/{id}";
                var response = _httpClient.GetAsync(url).Result;
                var result = response.Content.ReadAsStringAsync().Result;
                var KhuyenMais = JsonConvert.DeserializeObject<VoucherView>(result);
                return View(KhuyenMais);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Updates(VoucherView voucher)
        {
            try
            {
                // === LOGIC MỚI: Nếu số lượng <= 0 thì set trạng thái về 0 (Không hoạt động) ===
                if (voucher.SoLuong <= 0)
                {
                    voucher.TrangThai = 0;
                }
                // ==============================================================================

                if (voucher.SoTienCan == 0)
                {
                    if (voucher.SoLuong >= 0 && voucher.NgayKetThuc >= voucher.NgayApDung) // Cho phép số lượng = 0 để lưu trạng thái tắt
                    {
                        var response = await _httpClient.PutAsJsonAsync($"https://localhost:7095/api/Voucher/{voucher.Id}", voucher);
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("GetAllVoucher");
                        }
                        return View();
                    }
                    else
                    {
                        if (voucher.NgayKetThuc < voucher.NgayApDung) ViewData["Ngay"] = "Ngày kết thúc phải lớn hơn ngày áp dụng";
                        return View();
                    }
                }
                if (voucher.SoTienCan > 0)
                {
                    if (voucher.SoTienCan >= voucher.GiaTri)
                    {
                        if (voucher.SoLuong >= 0 && voucher.NgayKetThuc >= voucher.NgayApDung) // Cho phép số lượng = 0
                        {
                            var response = await _httpClient.PutAsJsonAsync($"https://localhost:7095/api/Voucher/{voucher.Id}", voucher);
                            if (response.IsSuccessStatusCode)
                            {
                                return RedirectToAction("GetAllVoucher");
                            }
                            return View();
                        }
                    }
                    if (voucher.SoTienCan < voucher.GiaTri)
                    {
                        ViewData["SoTienCan"] = "Số tiền cần phải lớn hơn giá trị";
                        return View();
                    }
                }
                else
                {
                    if (voucher.SoTienCan < 0)
                    {
                        ViewData["SoTienCan"] = "Mời bạn nhập số tiền cần không âm";
                    }
                    return View();
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        // --- CÁC CHỨC NĂNG THAY ĐỔI TRẠNG THÁI NHANH ---

        public async Task<IActionResult> SuDung(Guid id)
        {
            try
            {
                var timkiem = dBContext.Vouchers.FirstOrDefault(x => x.ID == id);
                if (timkiem != null)
                {
                    // Kiểm tra nếu còn số lượng thì mới cho kích hoạt
                    if (timkiem.SoLuong > 0)
                    {
                        timkiem.TrangThai = 1;
                        dBContext.Vouchers.Update(timkiem);
                        dBContext.SaveChanges();
                        return RedirectToAction("GetAllVoucher");
                    }
                    // Nếu hết số lượng thì không cho kích hoạt (hoặc reload trang mà ko làm gì)
                    return RedirectToAction("GetAllVoucher");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> KoSuDung(Guid id)
        {
            try
            {
                var timkiem = dBContext.Vouchers.FirstOrDefault(x => x.ID == id);
                if (timkiem != null)
                {
                    timkiem.TrangThai = 0;
                    dBContext.Vouchers.Update(timkiem);
                    dBContext.SaveChanges();
                    return RedirectToAction("GetAllVoucher");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        // === CHỨC NĂNG MỚI: KHÁCH HÀNG SỬ DỤNG VOUCHER (TRỪ SỐ LƯỢNG) ===
        public IActionResult DaSuDungVoucher(Guid id)
        {
            try
            {
                var voucher = dBContext.Vouchers.FirstOrDefault(x => x.ID == id);
                if (voucher != null)
                {
                    // Chỉ trừ nếu voucher đang hoạt động
                    if (voucher.TrangThai == 1 && voucher.SoLuong > 0)
                    {
                        voucher.SoLuong = voucher.SoLuong - 1;

                        // Logic chính: Nếu số lượng về 0 thì TẮT trạng thái
                        if (voucher.SoLuong == 0)
                        {
                            voucher.TrangThai = 0;
                        }

                        dBContext.Vouchers.Update(voucher);
                        dBContext.SaveChanges();
                    }
                }
                return RedirectToAction("GetAllVoucher");
            }
            catch
            {
                return RedirectToAction("GetAllVoucher");
            }
        }
    }
}