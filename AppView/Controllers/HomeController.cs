using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.Mail;
using AppData.ViewModels.QLND;
using AppData.ViewModels.SanPham;
using AppData.ViewModels.VNPay;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace AppView.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        // 1. KHAI BÁO DB CONTEXT
        private readonly AssignmentDBContext dBContext;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");

            // 2. KHỞI TẠO DB CONTEXT
            dBContext = new AssignmentDBContext();
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var session = HttpContext.Session.GetString("LoginInfor");
                if (String.IsNullOrEmpty(session))
                {
                    List<GioHangRequest> lstGioHang = new List<GioHangRequest>();
                    if (Request.Cookies["Cart"] != null)
                    {
                        lstGioHang = JsonConvert.DeserializeObject<List<GioHangRequest>>(Request.Cookies["Cart"]);
                    }
                    int cout = lstGioHang.Sum(c => c.SoLuong);
                    TempData["SoLuong"] = cout.ToString();

                    if (Request.Cookies["Cart"] != null)
                    {
                        var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCart?request=" + Request.Cookies["Cart"]);
                        if (response.IsSuccessStatusCode)
                        {
                            var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result);
                            TempData["TrangThai"] = "false";
                            return View(temp.GioHangs);
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        TempData["TongTien"] = "0";
                        return View(new List<GioHangRequest>());
                    }
                }
                else
                {
                    var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                    if (loginInfor.vaiTro == 1)
                    {
                        var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCartLogin?idNguoiDung=" + loginInfor.Id);
                        if (response.IsSuccessStatusCode)
                        {
                            var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result);
                            int cout = temp.GioHangs.Sum(c => c.SoLuong);
                            TempData["SoLuong"] = cout.ToString();
                            TempData["TrangThai"] = "true";
                            return View(temp.GioHangs);
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        TempData["SoLuong"] = "0";
                        return View(new List<GioHangRequest>());
                    }
                }
            }
            catch
            {
                TempData["TongTien"] = "0";
                return View(new List<GioHangRequest>());
            }
        }

        public IActionResult Privacy() => View();
        public IActionResult AboutUs() => View();
        public IActionResult ProductDetail() => View();

        #region SanPham
        [HttpPost]
        public JsonResult ShowProduct(FilterData filter)
        {
            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/getAll").Result;
                List<SanPhamViewModel> lstSanpham = new List<SanPhamViewModel>();
                if (response.IsSuccessStatusCode)
                {
                    lstSanpham = JsonConvert.DeserializeObject<List<SanPhamViewModel>>(response.Content.ReadAsStringAsync().Result);
                    List<SanPhamViewModel> lstSanphamfn = new List<SanPhamViewModel>();

                    // (Giữ nguyên logic lọc sản phẩm của bạn)
                    if (filter.priceRange != null && filter.priceRange.Count > 0 && !filter.priceRange.Contains("all"))
                    {
                        foreach (var item in filter.priceRange)
                        {
                            if (item == "1") lstSanphamfn.AddRange(lstSanpham.Where(p => p.GiaBan < 100000 && p.TrangThaiCTSP == 1));
                            else if (item == "2") lstSanphamfn.AddRange(lstSanpham.Where(p => p.GiaBan >= 100000 && p.GiaBan < 200000 && p.TrangThaiCTSP == 1));
                            else if (item == "3") lstSanphamfn.AddRange(lstSanpham.Where(p => p.GiaBan >= 200000 && p.GiaBan < 300000 && p.TrangThaiCTSP == 1));
                            else if (item == "4") lstSanphamfn.AddRange(lstSanpham.Where(p => p.GiaBan >= 300000 && p.GiaBan < 400000 && p.TrangThaiCTSP == 1));
                            else if (item == "5") lstSanphamfn.AddRange(lstSanpham.Where(p => p.GiaBan >= 400000 && p.GiaBan < 500000 && p.TrangThaiCTSP == 1));
                            else if (item == "6") lstSanphamfn.AddRange(lstSanpham.Where(p => p.GiaBan > 500000 && p.TrangThaiCTSP == 1));
                        }
                    }
                    else lstSanphamfn = lstSanpham;

                    List<SanPhamViewModel> lsttam = new List<SanPhamViewModel>();
                    List<SanPhamViewModel> lsttam1 = new List<SanPhamViewModel>();
                    if (filter.loaiSP != null && filter.loaiSP.Count > 0)
                    {
                        foreach (var x in filter.loaiSP)
                        {
                            lsttam = lstSanphamfn.Where(p => p.LoaiSP == x).ToList();
                            foreach (var item in lsttam)
                            {
                                if (lsttam1.FirstOrDefault(p => p.ID == item.ID) == null) lsttam1.Add(item);
                            }
                        }
                        lstSanphamfn = lsttam1;
                    }

                    if (filter.search != null)
                    {
                        lstSanphamfn = lstSanphamfn.Where(p => p.Ten.ToLower().Contains(filter.search.ToLower())).ToList();
                    }

                    List<SanPhamViewModel> lstcotam = new List<SanPhamViewModel>();
                    List<SanPhamViewModel> lstcotam1 = new List<SanPhamViewModel>();
                    if (filter.kichCo != null && filter.kichCo.Count > 0)
                    {
                        foreach (var x in filter.kichCo)
                        {
                            lstcotam = lstSanphamfn.Where(p => p.IDKichCo == x).ToList();
                            foreach (var item in lstcotam)
                            {
                                if (lstcotam1.FirstOrDefault(p => p.ID == item.ID) == null) lstcotam1.Add(item);
                            }
                        }
                        lstSanphamfn = lstcotam1;
                    }

                    if (filter.sortSP != null)
                    {
                        if (filter.sortSP == "2" || filter.sortSP == "4") lstSanphamfn = lstSanphamfn.OrderBy(p => p.GiaBan).ToList();
                        else if (filter.sortSP == "3" || filter.sortSP == "5") lstSanphamfn = lstSanphamfn.OrderByDescending(p => p.GiaBan).ToList();
                        else if (filter.sortSP == "6") lstSanphamfn = lstSanphamfn.OrderBy(p => p.NgayTao.Value).ToList();
                        else if (filter.sortSP == "7") lstSanphamfn = lstSanphamfn.OrderByDescending(p => p.NgayTao.Value).ToList();
                        else if (filter.sortSP == "9") lstSanphamfn = lstSanphamfn.OrderByDescending(p => p.SoLuong).ToList();
                    }

                    List<SanPhamViewModel> lstSanPhamfnR = new List<SanPhamViewModel>();
                    foreach (var item in lstSanphamfn)
                    {
                        if (item.TrangThaiCTSP == 1)
                        {
                            if (lstSanPhamfnR.FirstOrDefault(p => p.ID == item.ID) == null) lstSanPhamfnR.Add(item);
                        }
                        else
                        {
                            SanPhamViewModel sanPhamViewModel = lstSanpham.FirstOrDefault(p => p.ID == item.ID && p.TrangThaiCTSP == 1);
                            if (sanPhamViewModel != null)
                            {
                                if (lstSanPhamfnR.FirstOrDefault(p => p.ID == sanPhamViewModel.ID) == null) lstSanPhamfnR.Add(sanPhamViewModel);
                            }
                        }
                    }

                    var model = lstSanPhamfnR.Skip((filter.page - 1) * filter.pageSize).Take(filter.pageSize).ToList();
                    return Json(new { data = model, total = lstSanPhamfnR.Count, status = true });
                }
                else return Json(new { status = false });
            }
            catch { return Json(new { status = false }); }
        }
        #endregion

        [HttpGet]
        public IActionResult Shop()
        {
            HttpResponseMessage responseLoaiSP = _httpClient.GetAsync(_httpClient.BaseAddress + "LoaiSP/getAll").Result;
            if (responseLoaiSP.IsSuccessStatusCode) ViewData["listLoaiSP"] = JsonConvert.DeserializeObject<List<LoaiSP>>(responseLoaiSP.Content.ReadAsStringAsync().Result);

            HttpResponseMessage responseMauSac = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllMauSac").Result;
            if (responseMauSac.IsSuccessStatusCode) ViewData["listMauSac"] = JsonConvert.DeserializeObject<List<MauSac>>(responseMauSac.Content.ReadAsStringAsync().Result);

            HttpResponseMessage responseKichCo = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllKichCo").Result;
            if (responseKichCo.IsSuccessStatusCode) ViewData["listKichCo"] = JsonConvert.DeserializeObject<List<KichCo>>(responseKichCo.Content.ReadAsStringAsync().Result);

            HttpResponseMessage responseChatLieu = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllChatLieu").Result;
            if (responseChatLieu.IsSuccessStatusCode) ViewData["listChatLieu"] = JsonConvert.DeserializeObject<List<ChatLieu>>(responseChatLieu.Content.ReadAsStringAsync().Result);

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetail(string idSanPham)
        {
            try
            {
                var session = HttpContext.Session.GetString("LoginInfor");
                if (String.IsNullOrEmpty(session))
                {
                    List<GioHangRequest> lstGioHang = new List<GioHangRequest>();
                    if (Request.Cookies["Cart"] != null)
                    {
                        lstGioHang = JsonConvert.DeserializeObject<List<GioHangRequest>>(Request.Cookies["Cart"]);
                    }
                    int cout = lstGioHang.Sum(c => c.SoLuong);
                    TempData["SoLuong"] = cout.ToString();

                    if (Request.Cookies["Cart"] != null)
                    {
                        var response1 = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCart?request=" + Request.Cookies["Cart"]);
                        if (response1.IsSuccessStatusCode)
                        {
                            TempData["TrangThai"] = "false";
                            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllChiTietSanPhamHome?idSanPham=" + idSanPham);
                            var chiTietSanPham = JsonConvert.DeserializeObject<ChiTietSanPhamViewModelHome>(response.Content.ReadAsStringAsync().Result);
                            return View(chiTietSanPham);
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        TempData["TongTien"] = "0";
                        HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllChiTietSanPhamHome?idSanPham=" + idSanPham);
                        var chiTietSanPham = JsonConvert.DeserializeObject<ChiTietSanPhamViewModelHome>(response.Content.ReadAsStringAsync().Result);
                        return View(chiTietSanPham);
                    }
                }
                else
                {
                    var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                    if (loginInfor.vaiTro == 1)
                    {
                        var response2 = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCartLogin?idNguoiDung=" + loginInfor.Id);
                        if (response2.IsSuccessStatusCode)
                        {
                            var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response2.Content.ReadAsStringAsync().Result);
                            int cout = temp.GioHangs.Sum(c => c.SoLuong);
                            TempData["SoLuong"] = cout.ToString();
                            TempData["TrangThai"] = "true";
                            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllChiTietSanPhamHome?idSanPham=" + idSanPham);
                            var chiTietSanPham = JsonConvert.DeserializeObject<ChiTietSanPhamViewModelHome>(response.Content.ReadAsStringAsync().Result);
                            return View(chiTietSanPham);
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        TempData["SoLuong"] = "0";
                        HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllChiTietSanPhamHome?idSanPham=" + idSanPham);
                        var chiTietSanPham = JsonConvert.DeserializeObject<ChiTietSanPhamViewModelHome>(response.Content.ReadAsStringAsync().Result);
                        return View(chiTietSanPham);
                    }
                }
            }
            catch (Exception)
            {
                return Redirect("https://localhost:5001/");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProductDetailFromCart(Guid idctsp)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetIDsanPhamByIdCTSP?idctsp=" + idctsp);
                var idsanpham = JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result);
                return RedirectToAction("ProductDetail", "Home", new { idSanPham = idsanpham });
            }
            catch { return BadRequest(); }
        }

        [HttpGet]
        public JsonResult ShowDanhGiabyIdSP(Guid id, int page, int pageSize)
        {
            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"DanhGia/getbyIdSp/{id}").Result;
                List<DanhGiaViewModel> lstDanhGiaSanpham = new List<DanhGiaViewModel>();
                if (response.IsSuccessStatusCode)
                {
                    lstDanhGiaSanpham = JsonConvert.DeserializeObject<List<DanhGiaViewModel>>(response.Content.ReadAsStringAsync().Result);
                    var model = lstDanhGiaSanpham.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    return Json(new { data = model, total = lstDanhGiaSanpham.Count, status = true });
                }
                else return Json(new { status = false });
            }
            catch { return Json(new { status = false }); }
        }

        [HttpPost]
        public async Task<ActionResult> TongSoLuong(int? sl)
        {
            try
            {
                var session = HttpContext.Session.GetString("LoginInfor");
                if (String.IsNullOrEmpty(session))
                {
                    List<GioHangRequest> lstGioHang = new List<GioHangRequest>();
                    if (Request.Cookies["Cart"] != null)
                    {
                        lstGioHang = JsonConvert.DeserializeObject<List<GioHangRequest>>(Request.Cookies["Cart"]);
                    }
                    int cout = lstGioHang.Sum(c => c.SoLuong);
                    TempData["SoLuong"] = cout.ToString();

                    if (Request.Cookies["Cart"] != null)
                    {
                        var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCart?request=" + Request.Cookies["Cart"]);
                        if (response.IsSuccessStatusCode)
                        {
                            var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result);
                            cout = temp.GioHangs.Sum(c => c.SoLuong);
                            TempData["SoLuong"] = cout.ToString();
                            return Json(new { success = true, message = "Add to cart successfully", sl = cout });
                        }
                        else return Json(new { error = true, message = "  Not Add to cart " });
                    }
                    return Json(new { success = true, message = "Add to cart successfully", sl = cout });
                }
                else
                {
                    var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                    if (loginInfor.vaiTro == 1)
                    {
                        var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCartLogin?idNguoiDung=" + loginInfor.Id);
                        if (response.IsSuccessStatusCode)
                        {
                            var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result);
                            if (temp.GioHangs.Sum(x => x.SoLuong) == 0)
                            {
                                TempData["SoLuong"] = sl.Value.ToString();
                                return Json(new { success = true, message = "Add to cart successfully", sl = TempData["SoLuong"] });
                            }
                            int cout = temp.GioHangs.Sum(c => c.SoLuong);
                            TempData["SoLuong"] = cout.ToString();
                            TempData["TrangThai"] = "true";
                            return Json(new { success = true, message = "Add to cart successfully", sl = cout });
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        TempData["SoLuong"] = "0";
                        return Json(new { error = false, message = "  Not Add to cart " });
                    }
                }
            }
            catch { return Json(new { error = false, message = "  Not Add to cart " }); }
        }

        #region Cart
        public async Task<IActionResult> ShoppingCart()
        {
            try
            {
                var session = HttpContext.Session.GetString("LoginInfor");
                if (String.IsNullOrEmpty(session))
                {
                    List<GioHangRequest> lstGioHang = new List<GioHangRequest>();
                    if (Request.Cookies["Cart"] != null)
                    {
                        lstGioHang = JsonConvert.DeserializeObject<List<GioHangRequest>>(Request.Cookies["Cart"]);
                    }
                    int cout = lstGioHang.Sum(c => c.SoLuong);
                    TempData["SoLuong"] = cout.ToString();

                    if (Request.Cookies["Cart"] != null)
                    {
                        var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCart?request=" + Request.Cookies["Cart"]);
                        if (response.IsSuccessStatusCode)
                        {
                            var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result);
                            HttpContext.Session.SetString("TongTien", temp.TongTien.ToString());
                            HttpContext.Session.SetString("ListBienThe", JsonConvert.SerializeObject(temp.GioHangs));
                            HttpContext.Session.SetString("Quantity", temp.GioHangs.Sum(x => x.SoLuong).ToString());
                            TempData["TrangThai"] = "false";
                            return View(temp.GioHangs);
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        TempData["TongTien"] = "0";
                        return View(new List<GioHangRequest>());
                    }
                }
                else
                {
                    var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                    if (loginInfor.vaiTro == 1)
                    {
                        var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCartLogin?idNguoiDung=" + loginInfor.Id);
                        if (response.IsSuccessStatusCode)
                        {
                            var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result);
                            HttpContext.Session.SetString("TongTien", temp.TongTien.ToString());
                            HttpContext.Session.SetString("ListBienThe", JsonConvert.SerializeObject(temp.GioHangs));
                            HttpContext.Session.SetString("Quantity", temp.GioHangs.Sum(x => x.SoLuong).ToString());
                            int cout = temp.GioHangs.Sum(c => c.SoLuong);
                            TempData["SoLuong"] = cout.ToString();
                            TempData["TrangThai"] = "true";
                            return View(temp.GioHangs);
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        TempData["SoLuong"] = "0";
                        return View(new List<GioHangRequest>());
                    }
                }
            }
            catch { return View(new List<GioHangRequest>()); }
        }

        [HttpGet]
        public async Task<IActionResult> CheckCart()
        {
            try
            {
                var session = HttpContext.Session.GetString("LoginInfor");
                if (String.IsNullOrEmpty(session))
                {
                    if (Request.Cookies["Cart"] != null)
                    {
                        var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCart?request=" + Request.Cookies["Cart"]);
                        if (response.IsSuccessStatusCode)
                        {
                            var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result);
                            return Json(temp.GioHangs);
                        }
                        else return BadRequest();
                    }
                    else return View(new List<GioHangRequest>());
                }
                else
                {
                    var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                    if (loginInfor.vaiTro == 1)
                    {
                        var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCartLogin?idNguoiDung=" + loginInfor.Id);
                        if (response.IsSuccessStatusCode)
                        {
                            var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result);
                            return Json(temp.GioHangs);
                        }
                        else return BadRequest();
                    }
                    else return View(new List<GioHangRequest>());
                }
            }
            catch { return View(new List<GioHangRequest>()); }
        }

        [HttpGet]
        public ActionResult DeleteFromCart(Guid id)
        {
            try
            {
                List<GioHangRequest> bienThes = new List<GioHangRequest>();
                string? result = Request.Cookies["Cart"];
                if (result != null) bienThes = JsonConvert.DeserializeObject<List<GioHangRequest>>(result);
                bienThes.Remove(bienThes.Find(p => p.IDChiTietSanPham == id));

                var session = HttpContext.Session.GetString("LoginInfor");
                if (session == null)
                {
                    CookieOptions cookie = new CookieOptions();
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Append("Cart", JsonConvert.SerializeObject(bienThes), cookie);
                    return RedirectToAction("ShoppingCart");
                }
                else
                {
                    var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                    if (loginInfor.vaiTro == 1)
                    {
                        var response = _httpClient.DeleteAsync(_httpClient.BaseAddress + $"GioHang/Deletebyid?idctsp={id}&idNguoiDung={loginInfor.Id}").Result;
                        return RedirectToAction("ShoppingCart");
                    }
                    else return RedirectToAction("ShoppingCart");
                }
            }
            catch { return RedirectToAction("ShoppingCart"); }
        }

        [HttpPost]
        public ActionResult AddToCart(string id, int? sl, int? slcl)
        {
            try
            {
                List<GioHangRequest> lstGioHang;
                string? result = Request.Cookies["Cart"];
                var session = HttpContext.Session.GetString("LoginInfor");
                if (session == null)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        lstGioHang = new List<GioHangRequest>() { new GioHangRequest() { IDChiTietSanPham = new Guid(id), SoLuong = (sl != null) ? sl.Value : 1 } };
                    }
                    else
                    {
                        lstGioHang = JsonConvert.DeserializeObject<List<GioHangRequest>>(result);
                        var tempBienThe = lstGioHang.FirstOrDefault(x => x.IDChiTietSanPham == new Guid(id));
                        if (tempBienThe != null)
                        {
                            if (sl != null)
                            {
                                if (tempBienThe.SoLuong + sl.Value > slcl.Value)
                                    return Json(new { success = false, message = $"Bạn đã có {tempBienThe.SoLuong} sản phẩm trong giỏ hàng. Không thể thêm số lượng đã chọn vào giỏ hàng vì sẽ vượt quá giới hạn mua hàng của bạn." });
                            }
                            if (sl == null) tempBienThe.SoLuong++;
                            else tempBienThe.SoLuong += sl.Value;
                        }
                        else
                        {
                            lstGioHang.Add(new GioHangRequest() { IDChiTietSanPham = new Guid(id), SoLuong = (sl != null) ? sl.Value : 1 });
                        }
                    }
                    CookieOptions cookie = new CookieOptions();
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Append("Cart", JsonConvert.SerializeObject(lstGioHang), cookie);
                    return Json(new { success = true, message = "Thêm vào giỏ hàng thành công" });
                }
                else
                {
                    var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                    if (loginInfor.vaiTro == 1)
                    {
                        var chiTietGioHang = new ChiTietGioHang() { ID = Guid.NewGuid(), SoLuong = (sl != null) ? sl.Value : 1, IDCTSP = new Guid(id), IDNguoiDung = loginInfor.Id };
                        var response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "GioHang/AddCart", chiTietGioHang).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = response.Content.ReadFromJsonAsync<dynamic>().Result;
                            var message = responseContent.GetProperty("message").GetString();
                            return Json(new { success = true, message = message });
                        }
                        else
                        {
                            var responseContent = response.Content.ReadFromJsonAsync<dynamic>().Result;
                            var message = responseContent.GetProperty("message").GetString();
                            return Json(new { success = false, message = message });
                        }
                    }
                    else return Json(new { success = false, message = "Chỉ khách hàng mới thêm được vào giỏ hàng" });
                }
            }
            catch { return Json(new { success = false, message = "Thêm vào giỏ hàng thất bại" }); }
        }

        [HttpPost]
        public IActionResult UpdateCart(List<string> dssl)
        {
            try
            {
                int cout = 0;
                var session = HttpContext.Session.GetString("LoginInfor");
                if (session == null)
                {
                    List<GioHangRequest> chiTietSanPhams;
                    string? result = Request.Cookies["Cart"];
                    chiTietSanPhams = JsonConvert.DeserializeObject<List<GioHangRequest>>(result);
                    foreach (var item in dssl)
                    {
                        Guid id = Guid.Parse(item.Substring(0, 36));
                        int sl = Convert.ToInt32(item.Substring(36, item.Length - 36));
                        foreach (var x in chiTietSanPhams)
                        {
                            if (x.IDChiTietSanPham == id) x.SoLuong = sl;
                        }
                    }
                    CookieOptions cookie = new CookieOptions();
                    cookie.Expires = DateTime.Now.AddDays(30);
                    Response.Cookies.Append("Cart", JsonConvert.SerializeObject(chiTietSanPhams), cookie);
                    var response = _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCart?request=" + JsonConvert.SerializeObject(chiTietSanPhams)).Result;
                    var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result);
                    TempData["TongTien"] = temp.TongTien.ToString();
                    cout = temp.GioHangs.Sum(x => x.SoLuong);
                    TempData["SoLuong"] = cout.ToString();
                    TempData["ListBienThe"] = JsonConvert.SerializeObject(temp.GioHangs);
                    return Json(new { success = true, message = "Cập nhật giỏ hàng thành công", data = temp.GioHangs, cout });
                }
                else
                {
                    var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                    if (loginInfor.vaiTro == 1)
                    {
                        foreach (var item in dssl)
                        {
                            Guid id = Guid.Parse(item.Substring(0, 36));
                            int sl = Convert.ToInt32(item.Substring(36, item.Length - 36));
                            var response = _httpClient.PutAsync(_httpClient.BaseAddress + $"GioHang/UpdateCart?idctsp={id}&soluong={sl}&idNguoiDung={loginInfor.Id}", null).Result;
                            var responses = _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCartLogin?idNguoiDung=" + loginInfor.Id).Result;
                            var temp = JsonConvert.DeserializeObject<GioHangViewModel>(responses.Content.ReadAsStringAsync().Result);
                            TempData["TongTien"] = temp.TongTien.ToString();
                            cout = temp.GioHangs.Sum(x => x.SoLuong);
                            TempData["SoLuong"] = cout.ToString();
                        }
                        var responses1 = _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCartLogin?idNguoiDung=" + loginInfor.Id).Result;
                        var temp1 = JsonConvert.DeserializeObject<GioHangViewModel>(responses1.Content.ReadAsStringAsync().Result);
                        TempData["ListBienThe"] = JsonConvert.SerializeObject(temp1.GioHangs);
                        return Json(new { success = true, message = "Cập nhật giỏ hàng thành công", data = temp1.GioHangs, cout });
                    }
                    else return Json(new { success = false, message = "Chỉ khách hàng mới thêm được vào giỏ hàng" });
                }
            }
            catch { return Json(new { success = true, message = "Cập nhật giỏ hàng thất bại" }); }
        }

        [HttpPost]
        public ActionResult BuyNow(string id, int soLuong, int? slcl)
        {
            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"SanPham/GetChiTietSanPhamByID?id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    var session = HttpContext.Session.GetString("LoginInfor");
                    if (session == null)
                    {
                        List<GioHangRequest> lstGioHang;
                        string? result = Request.Cookies["Cart"];
                        if (string.IsNullOrEmpty(result))
                        {
                            lstGioHang = new List<GioHangRequest>() { new GioHangRequest() { IDChiTietSanPham = new Guid(id), SoLuong = (soLuong != null) ? soLuong : 1 } };
                        }
                        else
                        {
                            lstGioHang = JsonConvert.DeserializeObject<List<GioHangRequest>>(result);
                            var tempBienThe = lstGioHang.FirstOrDefault(x => x.IDChiTietSanPham == new Guid(id));
                            if (tempBienThe != null)
                            {
                                if (soLuong != null && tempBienThe.SoLuong + soLuong > slcl.Value)
                                    return Json(new { success = false, message = $"Bạn đã có {tempBienThe.SoLuong} sản phẩm trong giỏ hàng. Không thể thêm số lượng đã chọn vào giỏ hàng vì sẽ vượt quá giới hạn mua hàng của bạn." });

                                if (soLuong == null) tempBienThe.SoLuong++;
                                else tempBienThe.SoLuong += soLuong;
                            }
                            else
                            {
                                lstGioHang.Add(new GioHangRequest() { IDChiTietSanPham = new Guid(id), SoLuong = (soLuong != null) ? soLuong : 1 });
                            }
                        }
                        CookieOptions cookie = new CookieOptions();
                        cookie.Expires = DateTime.Now.AddDays(30);
                        Response.Cookies.Append("Cart", JsonConvert.SerializeObject(lstGioHang), cookie);
                        return Json(new { success = true, message = "Add to cart successfully" });
                    }
                    else
                    {
                        var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                        if (loginInfor.vaiTro == 1)
                        {
                            var chiTietGioHang = new ChiTietGioHang() { ID = Guid.NewGuid(), SoLuong = (soLuong != null) ? soLuong : 1, IDCTSP = new Guid(id), IDNguoiDung = loginInfor.Id };
                            var response1 = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "GioHang/AddCart", chiTietGioHang).Result;
                            if (response1.IsSuccessStatusCode)
                            {
                                var responseContent = response1.Content.ReadFromJsonAsync<dynamic>().Result;
                                var message = responseContent.GetProperty("message").GetString();
                                return Json(new { success = true, message = message });
                            }
                            else
                            {
                                var responseContent = response1.Content.ReadFromJsonAsync<dynamic>().Result;
                                var message = responseContent.GetProperty("message").GetString();
                                return Json(new { success = false, message = message });
                            }
                        }
                        else return Json(new { success = false, message = "Chỉ khách hàng mới thêm được vào giỏ hàng" });
                    }
                }
                else return Json(new { success = false, message = "Mua hàng thất bại" });
            }
            catch { return Json(new { success = false, message = "Mua hàng thất bại" }); }
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult Login(string actionName)
        {
            try
            {
                TempData["ActionName"] = actionName;
                return View();
            }
            catch
            {
                TempData["ActionName"] = "Index";
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Login(string login, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                {
                    ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin đăng nhập.";
                    return View();
                }
                if (login.Contains('@')) login = login.Replace("@", "%40");

                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"QuanLyNguoiDung/DangNhap?lg={login}&password={password}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    HttpContext.Session.SetString("LoginInfor", result);
                    var user = JsonConvert.DeserializeObject<LoginViewModel>(result);
                    if (user.vaiTro == 1) return RedirectToAction("Index", "TrangChu");
                    else return RedirectToAction("BanHang", "BanHangTaiQuay");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ViewBag.ErrorMessage = "Bạn không có quyền truy cập vào tài khoản này.";
                    return View();
                }
                else
                {
                    ViewBag.ErrorMessage = "Email hoặc password không chính xác.";
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.ErrorMessage = "Đã có lỗi hệ thống xảy ra. Vui lòng thử lại.";
                return View();
            }
        }
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(KhachHangViewModel khachHang)
        {
            if (!ModelState.IsValid) return View(khachHang);

            try
            {
                khachHang.Id = Guid.NewGuid();
                khachHang.DiemTich = 0;
                khachHang.TrangThai = 1;

                HttpResponseMessage response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + $"QuanLyNguoiDung/DangKyKhachHang", khachHang).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Tài khoản đã được đăng ký thành công!";
                    return View("RegisterSuccess");
                }

                ViewBag.ErrorMessage = "Tài khoản đã được đăng ký với email hoặc số điện thoại này!";
                return View(khachHang);
            }
            catch
            {
                ViewBag.ErrorMessage = "Đã có lỗi hệ thống xảy ra. Vui lòng thử lại.";
                return View(khachHang);
            }
        }

        [HttpGet]
        public IActionResult Profile()
        {
            try
            {
                var session = HttpContext.Session.GetString("LoginInfor");
                if (session != null)
                {
                    LoginViewModel loginViewModel = JsonConvert.DeserializeObject<LoginViewModel>(session);
                    var response = _httpClient.GetAsync(_httpClient.BaseAddress + $"KhachHang/GetById?id={loginViewModel.Id}").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        loginViewModel.DiemTich = JsonConvert.DeserializeObject<KhachHang>(response.Content.ReadAsStringAsync().Result).DiemTich;
                        return View(loginViewModel);
                    }
                    else return View(loginViewModel);
                }
                return Redirect("https://localhost:5001/");
            }
            catch { return Redirect("https://localhost:5001/"); }
        }

        [HttpPut]
        public ActionResult UpdateProfile(string ten, string email, string sdt, int? gioitinh, DateTime? ngaysinh, string? diachi)
        {
            try
            {
                if (ten == null || email == null) return Json(new { success = false, message = "Không được để trống thông tin" });
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (!match.Success) return Json(new { success = false, message = "Email sai" });
                if (Regex.Match(sdt, @"^(\+[0-9])$").Success) return Json(new { success = false, message = "Số điện thoại sai sai" });

                var session = HttpContext.Session.GetString("LoginInfor");
                LoginViewModel khachhang = new LoginViewModel();
                khachhang.Id = JsonConvert.DeserializeObject<LoginViewModel>(session).Id;
                khachhang.Ten = ten;
                khachhang.Email = email;
                khachhang.SDT = sdt;
                khachhang.GioiTinh = gioitinh;
                khachhang.NgaySinh = ngaysinh;
                khachhang.DiaChi = diachi;
                khachhang.DiemTich = JsonConvert.DeserializeObject<LoginViewModel>(session).DiemTich;
                khachhang.vaiTro = JsonConvert.DeserializeObject<LoginViewModel>(session).vaiTro;
                khachhang.IsAccountLocked = JsonConvert.DeserializeObject<LoginViewModel>(session).IsAccountLocked;
                khachhang.Message = "lmao";
                var response = _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "QuanLyNguoiDung/UpdateProfile1", khachhang).Result;
                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.Remove("LoginInfor");
                    HttpContext.Session.SetString("LoginInfor", response.Content.ReadAsStringAsync().Result);
                    return Json(new { success = true, message = "Cập nhật thông tin cá nhân thành công" });
                }
                else return Json(new { success = false, message = "Cập nhật thông tin cá nhân thất bại" });
            }
            catch { return Json(new { success = false, message = "Cập nhật thông tin cá nhân thất bại" }); }
        }

        public IActionResult PurchaseOrder()
        {
            try
            {
                var session = HttpContext.Session.GetString("LoginInfor");
                if (session != null)
                {
                    LoginViewModel loginViewModel = JsonConvert.DeserializeObject<LoginViewModel>(session);
                    _logger.LogInformation($"Đang lấy đơn hàng cho User ID: {loginViewModel.Id}");
                    List<DonMuaViewModel> donMuaViewModels = new List<DonMuaViewModel>();
                    HttpResponseMessage responseDonMua = _httpClient.GetAsync(_httpClient.BaseAddress + $"LichSuTichDiem/GetAllDonMua?IDkhachHang={loginViewModel.Id}").Result;

                    if (responseDonMua.IsSuccessStatusCode)
                    {
                        donMuaViewModels = JsonConvert.DeserializeObject<List<DonMuaViewModel>>(responseDonMua.Content.ReadAsStringAsync().Result);
                        _logger.LogInformation($"API GetAllDonMua thành công. Tìm thấy {donMuaViewModels.Count} đơn hàng.");
                    }
                    else
                    {
                        string apiError = responseDonMua.Content.ReadAsStringAsync().Result;
                        _logger.LogError($"LỖI API GetAllDonMua: Status={responseDonMua.StatusCode}, Lỗi chi tiết={apiError}");
                    }
                    return View("PurchaseOrder", donMuaViewModels);
                }
                else return Redirect("https://localhost:5001/");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi nghiêm trọng trong Action PurchaseOrder.");
                return Redirect("https://localhost:5001/");
            }
        }

        public IActionResult PurchaseOrderDetail(Guid idHoaDon)
        {
            try
            {
                List<DonMuaChiTietViewModel> DonMuaCT = new List<DonMuaChiTietViewModel>();
                HttpResponseMessage responseDonMuaCT = _httpClient.GetAsync(_httpClient.BaseAddress + $"LichSuTichDiem/GetAllDonMuaChiTiet?idHoaDon={idHoaDon}").Result;
                if (responseDonMuaCT.IsSuccessStatusCode)
                {
                    DonMuaCT = JsonConvert.DeserializeObject<List<DonMuaChiTietViewModel>>(responseDonMuaCT.Content.ReadAsStringAsync().Result);
                }
                return View("PurchaseOrderDetail", DonMuaCT);
            }
            catch { return Redirect("https://localhost:5001/"); }
        }

        public IActionResult LichSuTieuDiemTichDiem() => View();
        public IActionResult LichSuTieuDiemTichDiembyuser(HoaDon danhGiaCTHDView, int page, int pageSize)
        {
            try
            {
                var session = HttpContext.Session.GetString("LoginInfor");
                LoginViewModel loginViewModel = JsonConvert.DeserializeObject<LoginViewModel>(session);
                List<LichSuTichDiemTieuDiemViewModel> listLSTD = new List<LichSuTichDiemTieuDiemViewModel>();
                List<LichSuTichDiemTieuDiemViewModel> listLSTDFN = new List<LichSuTichDiemTieuDiemViewModel>();
                HttpResponseMessage responseDonMua = _httpClient.GetAsync(_httpClient.BaseAddress + $"LichSuTichDiem/GetALLLichSuTichDiembyIdUser?IDkhachHang={loginViewModel.Id}").Result;
                if (responseDonMua.IsSuccessStatusCode)
                {
                    listLSTD = JsonConvert.DeserializeObject<List<LichSuTichDiemTieuDiemViewModel>>(responseDonMua.Content.ReadAsStringAsync().Result);
                    listLSTD = listLSTD.OrderBy(p => p.TrangThaiLSTD).ToList();
                    listLSTD = listLSTD.Where(p => p.Diem > 0).ToList();

                    if (danhGiaCTHDView.TrangThaiGiaoHang != 2 && danhGiaCTHDView.TrangThaiGiaoHang != null)
                    {
                        if (danhGiaCTHDView.TrangThaiGiaoHang == 0)
                        {
                            listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 0));
                            listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 3 && p.TrangThaiGiaoHang == 5));
                            listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 3 && p.TrangThaiGiaoHang == 4));
                        }
                        else
                        {
                            listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 1 && p.TrangThaiGiaoHang == 6));
                            listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 1 && p.TrangThaiGiaoHang == 9));
                            listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 1 && p.TrangThaiGiaoHang == 4));
                            listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 1 && p.TrangThaiGiaoHang == 5));
                            listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 2 && p.TrangThaiGiaoHang == 7));
                            listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 4 && p.TrangThaiGiaoHang == 5));
                        }
                        listLSTDFN = listLSTDFN.OrderBy(p => p.TrangThaiLSTD).ThenBy(p => p.NgayTao).ToList();
                        var model = listLSTDFN.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        return Json(new { data = model, total = listLSTDFN.Count, status = true });
                    }
                    else
                    {
                        listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 0));
                        listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 3 && p.TrangThaiGiaoHang == 4));
                        listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 3 && p.TrangThaiGiaoHang == 5));
                        listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 1 && p.TrangThaiGiaoHang == 6));
                        listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 1 && p.TrangThaiGiaoHang == 9));
                        listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 1 && p.TrangThaiGiaoHang == 4));
                        listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 1 && p.TrangThaiGiaoHang == 5));
                        listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 2 && p.TrangThaiGiaoHang == 7));
                        listLSTDFN.AddRange(listLSTD.Where(p => p.TrangThaiLSTD == 4 && p.TrangThaiGiaoHang == 5));
                        listLSTDFN = listLSTDFN.OrderBy(p => p.TrangThaiLSTD).ThenBy(p => p.NgayTao).ToList();
                        var model = listLSTDFN.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        return Json(new { data = model, total = listLSTDFN.Count, status = true });
                    }
                }
                else return Json(new { status = false });
            }
            catch { return Redirect("https://localhost:5001/"); }
        }

        public IActionResult ReviewProducts(Guid idCTHD)
        {
            try
            {
                ChiTietHoaDonDanhGiaViewModel hdctDanhGia = new ChiTietHoaDonDanhGiaViewModel();
                HttpResponseMessage responseDonMuaCT = _httpClient.GetAsync(_httpClient.BaseAddress + $"LichSuTichDiem/GetCTHDDANHGIA?idhdct={idCTHD}").Result;
                if (responseDonMuaCT.IsSuccessStatusCode)
                {
                    hdctDanhGia = JsonConvert.DeserializeObject<ChiTietHoaDonDanhGiaViewModel>(responseDonMuaCT.Content.ReadAsStringAsync().Result);
                }
                return View("ReviewProducts", hdctDanhGia);
            }
            catch { return Redirect("https://localhost:5001/"); }
        }

        [HttpPost]
        public IActionResult ChangePassword(string newPassword, string oldPassword)
        {
            try
            {
                var session = HttpContext.Session.GetString("LoginInfor");
                ChangePasswordRequest request = new ChangePasswordRequest();
                request.ID = JsonConvert.DeserializeObject<LoginViewModel>(session).Id;
                request.NewPassword = newPassword;
                request.OldPassword = oldPassword;
                var response = _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "QuanLyNguoiDung/ChangePassword", request).Result;
                HttpContext.Session.Remove("LoginInfor");
                if (response.IsSuccessStatusCode) return RedirectToAction("Login", new { actionName = "Index" });
                else return BadRequest();
            }
            catch { return BadRequest(); }
        }

        public IActionResult DanhGiaSanPham([FromBody] DanhGiaCTHDViewModel danhGiaCTHDView)
        {
            try
            {
                if (danhGiaCTHDView.danhgia == "") danhGiaCTHDView.danhgia = "Người dùng này không để lại bình luận";
                HttpResponseMessage responseDonMuaCT = _httpClient.PutAsync(_httpClient.BaseAddress + $"DanhGia?idCTHD={danhGiaCTHDView.idCTHD}&soSao={danhGiaCTHDView.soSao}&binhLuan={danhGiaCTHDView.danhgia}", null).Result;
                return RedirectToAction("PurchaseOrderDetail", "Home", new { idHoaDon = danhGiaCTHDView.idHD });
            }
            catch { return Redirect("https://localhost:5001/"); }
        }

        [HttpPost]
        public async Task<IActionResult> HuyDonHang(Guid idHoaDon, string? ghiChu)
        {
            try
            {
                Guid? idNhanVien = null;
                var ghichuEncoded = WebUtility.UrlEncode(ghiChu ?? "");
                string url = $"HoaDon?idhoadon={idHoaDon}&trangthai=8&idnhanvien={idNhanVien}&ghichu={ghichuEncoded}";
                var response = await _httpClient.PutAsync(url, null);
                if (response.IsSuccessStatusCode) return Json(new { success = true });
                else
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return Json(new { success = false, message = "Không thể hủy đơn hàng. Lỗi: " + content });
                }
            }
            catch (Exception ex) { return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message }); }
        }

        public IActionResult HoanTacHuyDonHang(Guid idHoaDon)
        {
            try
            {
                HttpResponseMessage responseDonMua = _httpClient.PutAsync(_httpClient.BaseAddress + $"HoaDon?idhoadon={idHoaDon}&trangthai=2", null).Result;
                return RedirectToAction("PurchaseOrder");
            }
            catch { return RedirectToAction("PurchaseOrder"); }
        }

        public IActionResult DoiTraHang(Guid idHoaDon)
        {
            try
            {
                HttpResponseMessage responseDonMua = _httpClient.PutAsync(_httpClient.BaseAddress + $"HoaDon?idhoadon={idHoaDon}&trangthai=9", null).Result;
                return RedirectToAction("PurchaseOrder");
            }
            catch { return RedirectToAction("PurchaseOrder"); }
        }

        public IActionResult HoanTacDoiTraHang(Guid idHoaDon)
        {
            try
            {
                HttpResponseMessage responseDonMua = _httpClient.PutAsync(_httpClient.BaseAddress + $"HoaDon?idhoadon={idHoaDon}&trangthai=6", null).Result;
                return RedirectToAction("PurchaseOrder");
            }
            catch { return RedirectToAction("PurchaseOrder"); }
        }

        public IActionResult XacNhanGHTC(Guid idHoaDon)
        {
            try
            {
                HttpResponseMessage responseDonMua = _httpClient.PutAsync(_httpClient.BaseAddress + $"HoaDon?idhoadon={idHoaDon}&trangthai=6", null).Result;
                if (responseDonMua.IsSuccessStatusCode) RedirectToAction("PurchaseOrderDetail", idHoaDon);
                return RedirectToAction("PurchaseOrder");
            }
            catch { return RedirectToAction("PurchaseOrder"); }
        }

        public IActionResult GetHoaDonByTrangThai(HoaDon danhGiaCTHDView, int page, int pageSize, string Search)
        {
            try
            {
                var session = HttpContext.Session.GetString("LoginInfor");
                LoginViewModel loginViewModel = JsonConvert.DeserializeObject<LoginViewModel>(session);
                List<DonMuaViewModel> donMuaViewModels = new List<DonMuaViewModel>();
                HttpResponseMessage responseDonMua = _httpClient.GetAsync(_httpClient.BaseAddress + $"LichSuTichDiem/GetAllDonMua?IDkhachHang={loginViewModel.Id}").Result;
                if (responseDonMua.IsSuccessStatusCode)
                {
                    donMuaViewModels = JsonConvert.DeserializeObject<List<DonMuaViewModel>>(responseDonMua.Content.ReadAsStringAsync().Result);
                    donMuaViewModels = donMuaViewModels.OrderByDescending(p => p.NgayTao).ToList();

                    foreach (var item in donMuaViewModels)
                    {
                        item.Ngaytao1 = item.NgayTao.ToString("dd/MM/yyyy");
                        item.Ngaythanhtoan1 = item.NgayThanhToan != null ? item.NgayThanhToan.Value.ToString("dd/MM/yyyy") : null;
                        item.Ngaynhanhang1 = item.NgayNhanHang != null ? item.NgayNhanHang.Value.ToString("dd/MM/yyyy") : null;
                    }
                    if (Search != null) donMuaViewModels = donMuaViewModels.Where(p => p.MaHD.ToLower().Contains(Search.ToLower())).ToList();

                    if (danhGiaCTHDView.TrangThaiGiaoHang != 0 && danhGiaCTHDView.TrangThaiGiaoHang != null)
                    {
                        if (danhGiaCTHDView.TrangThaiGiaoHang == 5)
                        {
                            donMuaViewModels = donMuaViewModels.Where(p => p.TrangThaiGiaoHang == 4 || p.TrangThaiGiaoHang == 5 || p.TrangThaiGiaoHang == 9).ToList();
                        }
                        else if (danhGiaCTHDView.TrangThaiGiaoHang == 2)
                        {
                            donMuaViewModels = donMuaViewModels.Where(p => p.TrangThaiGiaoHang == 2 || p.TrangThaiGiaoHang == 8).ToList();
                        }
                        else
                        {
                            donMuaViewModels = donMuaViewModels.Where(p => p.TrangThaiGiaoHang == danhGiaCTHDView.TrangThaiGiaoHang).ToList();
                        }
                        var model = donMuaViewModels.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        return Json(new { data = model, total = donMuaViewModels.Count, status = true });
                    }
                    else
                    {
                        var model = donMuaViewModels.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        return Json(new { data = model, total = donMuaViewModels.Count, status = true });
                    }
                }
                else return Json(new { status = false });
            }
            catch { return Json(new { status = false }); }
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        #endregion

        #region ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            HttpContext.Session.Remove("LoginInfor");
            return View();
        }
        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email)) return View();
                var khachHang = JsonConvert.DeserializeObject<KhachHangViewModel>(_httpClient.GetAsync(_httpClient.BaseAddress + "KhachHang/GetKhachHangByEmail?email=" + email).Result.Content.ReadAsStringAsync().Result);
                if (khachHang.Email != null)
                {
                    string ma = Guid.NewGuid().ToString().Substring(0, 5);
                    MailData mailData = new MailData() { EmailToId = email, EmailToName = email, EmailSubject = "Khôi Phục Mật Khẩu", EmailBody = "Mã xác nhận của bạn là: " + ma };
                    HttpResponseMessage response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "Mail/SendMail", mailData).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        HttpContext.Session.SetString("ForgotPassword", khachHang.Email + "|" + ma);
                        return RedirectToAction("SubmitForgotPassword");
                    }
                    else return BadRequest();
                }
                else
                {
                    ViewData["EmailError"] = "Email không tồn tại trong hệ thống";
                    return View();
                }
            }
            catch { return BadRequest(); }
        }
        [HttpGet]
        public IActionResult SubmitForgotPassword() => View();

        [HttpPost]
        public IActionResult SubmitForgotPassword(string code)
        {
            try
            {
                string[] submit = HttpContext.Session.GetString("ForgotPassword").Split("|");
                if (submit.Length == 2 && code == submit[1]) return RedirectToAction("ChangeForgotPassword");
                else
                {
                    ViewData["ErrorMessage"] = "Mã xác nhận không chính xác. Vui lòng thử lại.";
                    return View();
                }
            }
            catch
            {
                ViewData["ErrorMessage"] = "Phiên làm việc của bạn đã hết hạn. Vui lòng yêu cầu mã mới.";
                return View();
            }
        }
        [HttpGet]
        public IActionResult ChangeForgotPassword() => View();

        public async Task<IActionResult> ChangeForgotPassword(string password, string confirmPassword)
        {
            var forgotPasswordSession = HttpContext.Session.GetString("ForgotPassword");
            if (string.IsNullOrEmpty(forgotPasswordSession))
            {
                TempData["ErrorMessage"] = "Phiên đổi mật khẩu đã hết hạn hoặc không hợp lệ. Vui lòng thử lại.";
                return RedirectToAction("Login", "Home");
            }

            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    ViewData["PasswordError"] = "Mật khẩu không được bỏ trống";
                    return View();
                }
                else if (password.Length < 8)
                {
                    ViewData["PasswordError"] = "Mật khẩu phải lớn hơn 8 ký tự";
                    return View();
                }

                if (string.IsNullOrEmpty(confirmPassword))
                {
                    ViewData["ConfirmPasswordError"] = "Xác nhận mật khẩu không được bỏ trống";
                    return View();
                }
                else if (password != confirmPassword)
                {
                    ViewData["ConfirmPasswordError"] = "Mật khẩu và xác nhận mật khẩu không khớp";
                    return View();
                }

                string[] submit = forgotPasswordSession.Split("|");
                string email = submit[0];
                string maXacNhan = submit[1];

                ResetPasswordRequest request = new ResetPasswordRequest
                {
                    Email = email,
                    Password = password,
                    ConfirmPassword = confirmPassword,
                    ResetToken = maXacNhan
                };

                string apiUrl = "https://localhost:7095/api/QuanLyNguoiDung/ResetPassword";
                var response = await _httpClient.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Session.Remove("ForgotPassword");
                    TempData["PasswordUpdateSuccess"] = true;
                    return View();
                }
                else
                {
                    string apiError = await response.Content.ReadAsStringAsync();
                    ViewData["ErrorMessage"] = $"Không thể đặt lại mật khẩu. Lỗi từ API: {apiError}";
                    return View();
                }
            }
            catch
            {
                ViewData["ErrorMessage"] = "Lỗi hệ thống, vui lòng thử lại.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    ResetPasswordRequest request = new ResetPasswordRequest { Email = email };
                    return View(request);
                }
                else
                {
                    TempData["Message"] = "Invalid email.";
                    return RedirectToAction("Login", new { actionNam = "Index" });
                }
            }
            catch
            {
                TempData["Message"] = "Invalid email.";
                return RedirectToAction("Login", new { actionNam = "Index" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (request.Password != request.ConfirmPassword)
                    {
                        ModelState.AddModelError("", "Passwords do not match");
                        return View("ResetPassword");
                    }
                    string apiUrl = "https://localhost:7095/api/QuanLyNguoiDung/ResetPassword";
                    var response = await _httpClient.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Password reset successfully";
                        return RedirectToAction("Login", new { actionNam = "Index" });
                    }
                    else ModelState.AddModelError("", "Không thể đặt lại mật khẩu");
                }
                return View("ResetPassword");
            }
            catch { return View("ResetPassword"); }
        }
        #endregion

        #region CheckOut
        [HttpGet]
        public JsonResult UseDiemTich(int diem, string id, int tongTien)
        {
            try
            {
                var response = _httpClient.GetAsync(_httpClient.BaseAddress + $"QuanLyNguoiDung/UseDiemTich?diem={diem}&id={id}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var soTienGiam = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                    if (soTienGiam <= (tongTien / 5)) return Json(new { SoTienGiam = soTienGiam, TrangThai = true });
                    else return Json(new { Loi = "Số tiền giảm khi sử dụng điểm phải nhỏ hơn 20% giá trị đơn hàng", TrangThai = false });
                }
                else return Json(new { Loi = "Không kết nối được với server", TrangThai = false });
            }
            catch { return Json(new { Loi = "Không kết nối được với server", TrangThai = false }); }
        }
        [HttpGet]
        public IActionResult CheckOut() => View();

        public string Order(HoaDonViewModel hoaDon)
        {
            try
            {
                List<ChiTietHoaDonViewModel> lstChiTietHoaDon = new List<ChiTietHoaDonViewModel>();
                string temp = HttpContext.Session.GetString("ListBienThe");
                string trangThai = TempData.Peek("TrangThai") as string;

                foreach (var item in JsonConvert.DeserializeObject<List<GioHangRequest>>(temp))
                {
                    ChiTietHoaDonViewModel chiTietHoaDon = new ChiTietHoaDonViewModel();
                    chiTietHoaDon.IDChiTietSanPham = item.IDChiTietSanPham;
                    chiTietHoaDon.SoLuong = item.SoLuong;
                    chiTietHoaDon.DonGia = item.DonGia.Value;
                    lstChiTietHoaDon.Add(chiTietHoaDon);
                }
                hoaDon.ChiTietHoaDons = lstChiTietHoaDon;
                hoaDon.TrangThai = Convert.ToBoolean(trangThai);
                var session = HttpContext.Session.GetString("LoginInfor");
                if (session != null)
                {
                    hoaDon.IDNguoiDung = JsonConvert.DeserializeObject<LoginViewModel>(session).Id;
                }

                if (hoaDon.PhuongThucThanhToan == "COD")
                {
                    HttpResponseMessage response = _httpClient.PostAsJsonAsync("HoaDon", hoaDon).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponseJson = response.Content.ReadAsStringAsync().Result;
                        var donMua = JsonConvert.DeserializeObject<DonMuaSuccessViewModel>(apiResponseJson);
                        donMua.GioHangs = JsonConvert.DeserializeObject<List<GioHangRequest>>(temp);
                        TempData["CheckOutSuccess"] = JsonConvert.SerializeObject(donMua);

                        HttpContext.Session.Remove("TongTien");
                        HttpContext.Session.Remove("Quantity");
                        HttpContext.Session.Remove("ListBienThe");

                        // === TRỪ SỐ LƯỢNG VOUCHER KHI THANH TOÁN COD ===
                        TruSoLuongVoucher(hoaDon.TenVoucher);
                        // ==============================================

                        if (!hoaDon.TrangThai) Response.Cookies.Delete("Cart");
                        TempData["SoLuong"] = "0";
                        return "https://localhost:5001/Home/CheckOutSuccess";
                    }
                    else return "";
                }
                else if (hoaDon.PhuongThucThanhToan == "VNPay")
                {
                    TempData["HoaDon"] = JsonConvert.SerializeObject(hoaDon);
                    string vnp_Returnurl = "https://localhost:5001/Home/PaymentCallBack";
                    string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
                    string vnp_TmnCode = "FLYBWUDB";
                    string vnp_HashSecret = "6ZCJ0ZOJ6MOSREICS95ETK29ZNGDYQ2Y";
                    string ipAddr = HttpContext.Connection.RemoteIpAddress?.ToString();

                    OrderInfo order = new OrderInfo();
                    order.OrderId = DateTime.Now.Ticks;
                    order.Amount = hoaDon.TongTien;
                    order.Status = "0";
                    order.CreatedDate = DateTime.Now;

                    VnPayLibrary vnpay = new VnPayLibrary();
                    vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                    vnpay.AddRequestData("vnp_Command", "pay");
                    vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                    vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString());
                    vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
                    vnpay.AddRequestData("vnp_CurrCode", "VND");
                    vnpay.AddRequestData("vnp_IpAddr", ipAddr);
                    vnpay.AddRequestData("vnp_Locale", "vn");
                    vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
                    vnpay.AddRequestData("vnp_OrderType", "other");
                    vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                    vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString());

                    string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                    return paymentUrl;
                }
                else return "";
            }
            catch { return "https://localhost:5001/Home/CheckOutSuccess"; }
        }

        public IActionResult PaymentCallBack()
        {
            try
            {
                if (Request.Query.Count > 0)
                {
                    string vnp_HashSecret = "6ZCJ0ZOJ6MOSREICS95ETK29ZNGDYQ2Y";
                    var vnpayData = Request.Query;
                    VnPayLibrary vnpay = new VnPayLibrary();

                    foreach (var s in vnpayData)
                    {
                        if (!string.IsNullOrEmpty(s.Key) && s.Key.StartsWith("vnp_"))
                        {
                            vnpay.AddResponseData(s.Key, vnpayData[s.Key]);
                        }
                    }
                    // ... các bước lấy data vnpay ... (giữ nguyên)
                    string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                    string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                    String vnp_SecureHash = Request.Query["vnp_SecureHash"];
                    bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);

                    if (checkSignature)
                    {
                        if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                        {
                            var hoaDon = JsonConvert.DeserializeObject<HoaDonViewModel>(TempData["HoaDon"].ToString());
                            hoaDon.NgayThanhToan = DateTime.Now;
                            HttpResponseMessage response = _httpClient.PostAsJsonAsync("HoaDon", hoaDon).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                var apiResponseJson = response.Content.ReadAsStringAsync().Result;
                                var donMua = JsonConvert.DeserializeObject<DonMuaSuccessViewModel>(apiResponseJson);

                                string temp = HttpContext.Session.GetString("ListBienThe");
                                donMua.GioHangs = JsonConvert.DeserializeObject<List<GioHangRequest>>(temp);
                                TempData["CheckOutSuccess"] = JsonConvert.SerializeObject(donMua);

                                HttpContext.Session.Remove("TongTien");
                                HttpContext.Session.Remove("Quantity");
                                HttpContext.Session.Remove("ListBienThe");

                                // === TRỪ SỐ LƯỢNG VOUCHER KHI THANH TOÁN VNPAY ===
                                TruSoLuongVoucher(hoaDon.TenVoucher);
                                // =================================================

                                if (!hoaDon.TrangThai) Response.Cookies.Delete("Cart");
                                TempData["SoLuong"] = "0";
                                return RedirectToAction("CheckOutSuccess");
                            }
                            else return BadRequest();
                        }
                        else return BadRequest();
                    }
                    else return BadRequest();
                }
                else return BadRequest();
            }
            catch { return BadRequest(); }
        }

        [HttpGet]
        public IActionResult CheckOutSuccess()
        {
            try
            {
                var donMuaJson = TempData.Peek("CheckOutSuccess") as string;
                if (string.IsNullOrEmpty(donMuaJson)) return RedirectToAction("Index", "Home");
                var donMua = JsonConvert.DeserializeObject<DonMuaSuccessViewModel>(donMuaJson);

                var temp = HttpContext.Session.GetString("LoginInfor");
                if (temp != null && donMua != null)
                {
                    var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(temp);
                    loginInfor.DiemTich -= donMua.DiemSuDung;
                    HttpContext.Session.SetString("LoginInfor", JsonConvert.SerializeObject(loginInfor));
                }
                return View(donMua);
            }
            catch { return RedirectToAction("Index", "Home"); }
        }

        [HttpGet]
        public async Task<JsonResult> UseVoucher(string ma, int tongTien)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "Voucher/GetVoucherByMa?ma=" + ma);
                if (response.IsSuccessStatusCode)
                {
                    var voucher = JsonConvert.DeserializeObject<Voucher>(await response.Content.ReadAsStringAsync());
                    if (voucher != null)
                    {
                        if (voucher.NgayKetThuc > DateTime.Now && voucher.NgayApDung < DateTime.Now)
                        {
                            if (voucher.SoLuong > 0)
                            {
                                if (voucher.SoTienCan < tongTien) return Json(new { HinhThuc = voucher.HinhThucGiamGia, GiaTri = voucher.GiaTri, TrangThai = true });
                                else return Json(new { Loi = "Voucher chưa đạt đủ điều kiện: Tổng tiền sản phẩm lớn hơn " + voucher.SoTienCan.ToString("n0") + " VNĐ", TrangThai = false });
                            }
                            else return Json(new { Loi = "Voucher đã sử dụng hết", TrangThai = false });
                        }
                        else return Json(new { Loi = "Mã voucher hết hạn", TrangThai = false });
                    }
                    else return Json(new { Loi = "Không tìm thấy voucher", TrangThai = false });
                }
                else return Json(new { HinhThuc = false, GiaTri = 0 });
            }
            catch { return Json(new { HinhThuc = false, GiaTri = 0 }); }
        }
        [HttpGet]
        public JsonResult GetAllVoucherByTien(int tongTien)
        {
            try
            {
                var response = _httpClient.GetAsync("Voucher/GetAllVoucherByTien?tongTien=" + tongTien).Result;
                if (response.IsSuccessStatusCode)
                {
                    var lst = JsonConvert.DeserializeObject<List<Voucher>>(response.Content.ReadAsStringAsync().Result);
                    return Json(new { TrangThai = true, KetQua = lst });
                }
                else return Json(new { TrangThai = false });
            }
            catch { return Json(new { TrangThai = false }); }
        }
        #endregion

        #region Other
        public IActionResult BlogDetails() => View();
        public IActionResult Contacts() => View();
        public IActionResult Blog() => View();
        #endregion

        public IActionResult SoDiaChi()
        {
            try
            {
                var session = HttpContext.Session.GetString("LoginInfor");
                if (session != null)
                {
                    LoginViewModel loginViewModel = JsonConvert.DeserializeObject<LoginViewModel>(session);
                    return View(loginViewModel);
                }
                else return RedirectToAction("Login", "Home", new { actionName = "Index" });
            }
            catch { return RedirectToAction("Login", "Home", new { actionName = "Index" }); }
        }

        // === HÀM TRỪ SỐ LƯỢNG VOUCHER ===
        private void TruSoLuongVoucher(string? tenVoucher)
        {
            if (!string.IsNullOrEmpty(tenVoucher))
            {
                try
                {
                    var voucher = dBContext.Vouchers.FirstOrDefault(x => x.Ten == tenVoucher);
                    if (voucher != null && voucher.SoLuong > 0)
                    {
                        voucher.SoLuong = voucher.SoLuong - 1;
                        if (voucher.SoLuong == 0) voucher.TrangThai = 0;
                        dBContext.Vouchers.Update(voucher);
                        dBContext.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Lỗi khi trừ voucher: " + ex.Message);
                }
            }
        }
    }
}