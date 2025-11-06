using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.Mail;
using AppData.ViewModels.QLND;
using AppData.ViewModels.SanPham;
using AppData.ViewModels.VNPay;
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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
        }
        public async Task<IActionResult> Index()
        {
            // lam start
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
                    // laam them
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

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ProductDetail()
        {
            return View();
        }
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
                    //
                    List<SanPhamViewModel> lstSanphamfn = new List<SanPhamViewModel>();
                    //price-filter
                    if (filter.priceRange != null && filter.priceRange.Count > 0 && !filter.priceRange.Contains("all"))
                    {
                        foreach (var item in filter.priceRange)
                        {
                            if (item == "1")
                            {
                                foreach (var x in lstSanpham.Where(p => p.GiaBan < 100000 && p.TrangThaiCTSP == 1).ToList())
                                {
                                    lstSanphamfn.Add(x);
                                }
                            }
                            else if (item == "2")
                            {
                                foreach (var x in lstSanpham.Where(p => p.GiaBan >= 100000 && p.GiaBan < 200000 && p.TrangThaiCTSP == 1).ToList())
                                {
                                    lstSanphamfn.Add(x);
                                }
                            }
                            else if (item == "3")
                            {
                                foreach (var x in lstSanpham.Where(p => p.GiaBan >= 200000 && p.GiaBan < 300000 && p.TrangThaiCTSP == 1).ToList())
                                {
                                    lstSanphamfn.Add(x);
                                }
                            }
                            else if (item == "4")
                            {
                                foreach (var x in lstSanpham.Where(p => p.GiaBan >= 300000 && p.GiaBan < 400000 && p.TrangThaiCTSP == 1).ToList())
                                {
                                    lstSanphamfn.Add(x);
                                }
                            }
                            else if (item == "5")
                            {
                                foreach (var x in lstSanpham.Where(p => p.GiaBan >= 400000 && p.GiaBan < 500000 && p.TrangThaiCTSP == 1).ToList())
                                {
                                    lstSanphamfn.Add(x);
                                }
                            }
                            else if (item == "6")
                            {
                                foreach (var x in lstSanpham.Where(p => p.GiaBan > 500000 && p.TrangThaiCTSP == 1).ToList())
                                {
                                    lstSanphamfn.Add(x);
                                }
                            }

                        }
                    }
                    else
                    {
                        lstSanphamfn = lstSanpham;
                    }
                    //loaiSP-filter
                    List<SanPhamViewModel> lsttam = new List<SanPhamViewModel>();
                    List<SanPhamViewModel> lsttam1 = new List<SanPhamViewModel>();
                    if (filter.loaiSP != null && filter.loaiSP.Count > 0)
                    {
                        foreach (var x in filter.loaiSP)
                        {
                            lsttam = lstSanphamfn.Where(p => p.LoaiSP == x).ToList();
                            foreach (var item in lsttam)
                            {
                                if (lsttam1.FirstOrDefault(p => p.ID == item.ID) == null)
                                {
                                    lsttam1.Add(item);
                                }
                            }
                        }
                        lstSanphamfn = lsttam1;
                    }
                    //Search
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
                                if (lstcotam1.FirstOrDefault(p => p.ID == item.ID) == null)
                                {
                                    lstcotam1.Add(item);
                                }
                            }
                        }
                        lstSanphamfn = lstcotam1;
                    }
                    //sort
                    if (filter.sortSP != null)
                    {
                        if (filter.sortSP == "2")
                        {
                            lstSanphamfn = lstSanphamfn.OrderBy(p => p.GiaBan).ToList();
                        }
                        else if (filter.sortSP == "3")
                        {
                            lstSanphamfn = lstSanphamfn.OrderByDescending(p => p.GiaBan).ToList();
                        }
                        else if (filter.sortSP == "4")
                        {
                            lstSanphamfn = lstSanphamfn.OrderBy(p => p.GiaBan).ToList();
                        }
                        else if (filter.sortSP == "5")
                        {
                            lstSanphamfn = lstSanphamfn.OrderByDescending(p => p.GiaBan).ToList();
                        }
                        else if (filter.sortSP == "6")
                        {
                            lstSanphamfn = lstSanphamfn.OrderBy(p => p.NgayTao.Value).ToList();
                        }
                        else if (filter.sortSP == "7")
                        {
                            lstSanphamfn = lstSanphamfn.OrderByDescending(p => p.NgayTao.Value).ToList();
                        }
                        else if (filter.sortSP == "9")
                        {
                            lstSanphamfn = lstSanphamfn.OrderByDescending(p => p.SoLuong).ToList();
                        }
                    }
                    List<SanPhamViewModel> lstSanPhamfnR = new List<SanPhamViewModel>();
                    foreach (var item in lstSanphamfn)
                    {
                        if (item.TrangThaiCTSP == 1)
                        {
                            if (lstSanPhamfnR.FirstOrDefault(p => p.ID == item.ID) == null)
                            {
                                lstSanPhamfnR.Add(item);
                            }
                        }
                        else
                        {
                            SanPhamViewModel sanPhamViewModel = lstSanpham.FirstOrDefault(p => p.ID == item.ID && p.TrangThaiCTSP == 1);
                            if (sanPhamViewModel != null)
                            {
                                if (lstSanPhamfnR.FirstOrDefault(p => p.ID == sanPhamViewModel.ID) == null)
                                {
                                    lstSanPhamfnR.Add(sanPhamViewModel);
                                }
                            }
                        }
                    }
                    //

                    var model = lstSanPhamfnR.Skip((filter.page - 1) * filter.pageSize).Take(filter.pageSize).ToList();
                    return Json(new
                    {
                        data = model,
                        total = lstSanPhamfnR.Count,
                        status = true
                    });
                }
                else return Json(new { status = false });
            }
            catch
            {
                return Json(new { status = false });
            }
        }
        [HttpGet]
        public IActionResult Shop()
        {
            HttpResponseMessage responseLoaiSP = _httpClient.GetAsync(_httpClient.BaseAddress + "LoaiSP/getAll").Result;
            if (responseLoaiSP.IsSuccessStatusCode)
            {
                ViewData["listLoaiSP"] = JsonConvert.DeserializeObject<List<LoaiSP>>(responseLoaiSP.Content.ReadAsStringAsync().Result);
            }
            HttpResponseMessage responseMauSac = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllMauSac").Result;
            if (responseMauSac.IsSuccessStatusCode)
            {
                ViewData["listMauSac"] = JsonConvert.DeserializeObject<List<MauSac>>(responseMauSac.Content.ReadAsStringAsync().Result);
            }
            HttpResponseMessage responseKichCo = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllKichCo").Result;
            if (responseKichCo.IsSuccessStatusCode)
            {
                ViewData["listKichCo"] = JsonConvert.DeserializeObject<List<KichCo>>(responseKichCo.Content.ReadAsStringAsync().Result);
            }
            HttpResponseMessage responseChatLieu = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllChatLieu").Result;
            if (responseChatLieu.IsSuccessStatusCode)
            {
                ViewData["listChatLieu"] = JsonConvert.DeserializeObject<List<ChatLieu>>(responseChatLieu.Content.ReadAsStringAsync().Result);
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ProductDetail(string idSanPham)
        { // lam start
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
                    // laam them
                    int cout = lstGioHang.Sum(c => c.SoLuong);
                    TempData["SoLuong"] = cout.ToString();

                    if (Request.Cookies["Cart"] != null)
                    {
                        var response1 = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCart?request=" + Request.Cookies["Cart"]);
                        if (response1.IsSuccessStatusCode)
                        {
                            var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response1.Content.ReadAsStringAsync().Result);


                            // lam end

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


                            // lam them
                            int cout = temp.GioHangs.Sum(c => c.SoLuong);

                            TempData["SoLuong"] = cout.ToString();
                            // lam end
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

                // lam end
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
            catch
            {
                return BadRequest();
            }
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
                    return Json(new
                    {
                        data = model,
                        total = lstDanhGiaSanpham.Count,
                        status = true
                    });
                }
                else return Json(new { status = false });
            }
            catch
            {
                return Json(new { status = false });
            }
        }
        #endregion

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
                    // laam them
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
                            // lam end

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

                            // lam them
                            int cout = temp.GioHangs.Sum(c => c.SoLuong);


                            TempData["SoLuong"] = cout.ToString();
                            // lam end
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
            catch
            {
                return Json(new { error = false, message = "  Not Add to cart " });
            }
        }
        #region Cart
        [HttpGet]
        public async Task<IActionResult> ShoppingCart()
        {
            try
            {
                var session = HttpContext.Session.GetString("LoginInfor");
                //Không đăng nhập
                if (String.IsNullOrEmpty(session))
                {
                    List<GioHangRequest> lstGioHang = new List<GioHangRequest>();
                    if (Request.Cookies["Cart"] != null)
                    {
                        lstGioHang = JsonConvert.DeserializeObject<List<GioHangRequest>>(Request.Cookies["Cart"]);
                    }
                    // laam them
                    int cout = lstGioHang.Sum(c => c.SoLuong);
                    TempData["SoLuong"] = cout.ToString();


                    if (Request.Cookies["Cart"] != null)
                    {
                        var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCart?request=" + Request.Cookies["Cart"]);
                        if (response.IsSuccessStatusCode)
                        {
                            var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result);
                            TempData["TongTien"] = temp.TongTien.ToString();

                            // lam end
                            TempData["ListBienThe"] = JsonConvert.SerializeObject(temp.GioHangs);
                            TempData["TrangThai"] = "false";
                            TempData["Quantity"] = temp.GioHangs.Sum(x => x.SoLuong).ToString();
                            return View(temp.GioHangs);
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        TempData["TongTien"] = "0";
                        return View(new List<GioHangRequest>());
                    }
                }//Có đăng nhập
                else
                {
                    var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                    if (loginInfor.vaiTro == 1)
                    {
                        var response = await _httpClient.GetAsync(_httpClient.BaseAddress + "GioHang/GetCartLogin?idNguoiDung=" + loginInfor.Id);
                        if (response.IsSuccessStatusCode)
                        {
                            var temp = JsonConvert.DeserializeObject<GioHangViewModel>(response.Content.ReadAsStringAsync().Result);
                            TempData["TongTien"] = temp.TongTien.ToString();
                            TempData["ListBienThe"] = JsonConvert.SerializeObject(temp.GioHangs);
                            // lam them
                            int cout = temp.GioHangs.Sum(c => c.SoLuong);

                            TempData["SoLuong"] = cout.ToString();
                            // lam end
                            TempData["TrangThai"] = "true";
                            TempData["Quantity"] = temp.GioHangs.Sum(x => x.SoLuong).ToString();
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
                return View(new List<GioHangRequest>());
            }
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
                    else
                    {
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
                            return Json(temp.GioHangs);
                        }
                        else return BadRequest();
                    }
                    else
                    {
                        return View(new List<GioHangRequest>());
                    }
                }
            }
            catch
            {
                return View(new List<GioHangRequest>());
            }
        }
        [HttpGet]
        public ActionResult DeleteFromCart(Guid id)
        {
            try
            {
                List<GioHangRequest> bienThes = new List<GioHangRequest>();
                string? result = Request.Cookies["Cart"];
                if (result != null)
                {
                    bienThes = JsonConvert.DeserializeObject<List<GioHangRequest>>(result);
                }
                bienThes.Remove(bienThes.Find(p => p.IDChiTietSanPham == id));
                //
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
                        if (response.IsSuccessStatusCode) return RedirectToAction("ShoppingCart");
                        else return RedirectToAction("ShoppingCart");
                    }
                    else return RedirectToAction("ShoppingCart");
                }
            }
            catch
            {
                return RedirectToAction("ShoppingCart");
            }
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
                        //var idSP = 
                        if (tempBienThe != null)
                        {
                            // Kiểm tra nếu số lượng muốn thêm vượt quá tồn kho
                            if (sl != null)
                            {
                                if (tempBienThe.SoLuong + sl.Value > slcl.Value)
                                {
                                    return Json(new { success = false, message = $"Bạn đã có {tempBienThe.SoLuong} sản phẩm trong giỏ hàng. Không thể thêm số lượng đã chọn vào giỏ hàng vì sẽ vượt quá giới hạn mua hàng của bạn." });
                                }
                                //if (tempBienThe.SoLuong + sl > 15)
                                //{
                                //    return Json(new { success = false, message = $"Bạn chỉ có thể thêm tối đa 15 sản phẩm này vào giỏ hàng" });
                                //}

                            }
                            if (sl == null)
                            {
                                tempBienThe.SoLuong++;
                                //if ( tempBienThe.SoLuong > 15)
                                //{
                                //    tempBienThe.SoLuong--;
                                //    return Json(new { success = false, message = $"Bạn chỉ có thể thêm tối đa 15 sản phẩm này vào giỏ hàng" }); 
                                //}
                            }
                            else
                            {
                                tempBienThe.SoLuong += sl.Value;
                            }

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
                { //backup nếu có lỗi
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
            catch
            {
                return Json(new { success = false, message = "Thêm vào giỏ hàng thất bại" });
            }
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
                            if (x.IDChiTietSanPham == id)
                            {
                                x.SoLuong = sl;
                            }
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
            catch (Exception)
            {

                return Json(new { success = true, message = "Cập nhật giỏ hàng thất bại" });
            }

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
                        //moiw
                        List<GioHangRequest> lstGioHang;
                        //ChiTietSanPhamViewModel chiTietSanPham = JsonConvert.DeserializeObject<ChiTietSanPhamViewModel>(response.Content.ReadAsStringAsync().Result);
                        string? result = Request.Cookies["Cart"];
                        if (string.IsNullOrEmpty(result))
                        {
                            //chiTietSanPham.SoLuong = (sl != null)?sl.Value:1;
                            lstGioHang = new List<GioHangRequest>() { new GioHangRequest() { IDChiTietSanPham = new Guid(id), SoLuong = (soLuong != null) ? soLuong : 1 } };
                        }
                        else
                        {
                            lstGioHang = JsonConvert.DeserializeObject<List<GioHangRequest>>(result);
                            var tempBienThe = lstGioHang.FirstOrDefault(x => x.IDChiTietSanPham == new Guid(id));
                            if (tempBienThe != null)
                            {
                                // Kiểm tra nếu số lượng muốn thêm vượt quá tồn kho
                                if (soLuong != null && tempBienThe.SoLuong + soLuong > slcl.Value)
                                {
                                    return Json(new { success = false, message = $"Bạn đã có {tempBienThe.SoLuong} sản phẩm trong giỏ hàng. Không thể thêm số lượng đã chọn vào giỏ hàng vì sẽ vượt quá giới hạn mua hàng của bạn." });
                                }
                                //if (soLuong != null && tempBienThe.SoLuong + soLuong > 15)
                                //{
                                //    return Json(new { success = false, message = $"Bạn chỉ có thể thêm tối đa 15 sản phẩm này vào giỏ hàng" });
                                //}
                                if (soLuong == null)
                                {
                                    tempBienThe.SoLuong++;
                                }
                                else
                                {
                                    tempBienThe.SoLuong += soLuong;
                                }

                            }
                            else
                            {
                                //chiTietSanPham.SoLuong = (sl != null) ? sl.Value : 1;
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
            catch
            {
                return Json(new { success = false, message = "Mua hàng thất bại" });
            }
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
        //[HttpPost]
        //public async Task<ActionResult> Login(string login, string password)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
        //        {
        //            ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin đăng nhập.";
        //            return View();
        //        }
        //        if (login.Contains('@'))
        //        {
        //            login = login.Replace("@", "%40");
        //        }
        //        HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"QuanLyNguoiDung/DangNhap?lg={login}&password={password}").Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            string result = await response.Content.ReadAsStringAsync();
        //            HttpContext.Session.SetString("LoginInfor", result);
        //            var user = JsonConvert.DeserializeObject<LoginViewModel>(result);
        //            if (user.vaiTro == 1)
        //            {
        //                string actionName = TempData["ActionName"].ToString();
        //                return RedirectToAction("Index", "TrangChu");
        //            }
        //            else return RedirectToAction("BanHang", "BanHangTaiQuay");
        //        }
        //        else if (response.StatusCode == HttpStatusCode.Unauthorized)
        //        {
        //            ViewBag.ErrorMessage = "Bạn không có quyền truy cập vào tài khoản này.";
        //            return View();
        //        }
        //        else
        //        {
        //            ViewBag.ErrorMessage = "Email hoặc password không chính xác.";
        //            return View();
        //        }
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
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
                if (login.Contains('@'))
                {
                    login = login.Replace("@", "%40");
                }

                // QUAY LẠI DÙNG GetAsync VÌ API CỦA BẠN YÊU CẦU
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + $"QuanLyNguoiDung/DangNhap?lg={login}&password={password}").Result;

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    HttpContext.Session.SetString("LoginInfor", result);
                    var user = JsonConvert.DeserializeObject<LoginViewModel>(result);
                    if (user.vaiTro == 1)
                    {
                        // SỬA LỖI GỐC: Đã xóa dòng "TempData["ActionName"].ToString()"
                        return RedirectToAction("Index", "TrangChu");
                    }
                    else return RedirectToAction("BanHang", "BanHangTaiQuay");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ViewBag.ErrorMessage = "Bạn không có quyền truy cập vào tài khoản này.";
                    return View();
                }
                else
                {
                    // API của bạn trả về BadRequest (400) nếu sai
                    ViewBag.ErrorMessage = "Email hoặc password không chính xác.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Lỗi NullReferenceException (nếu có) sẽ được bắt ở đây
                ViewBag.ErrorMessage = "Đã có lỗi hệ thống xảy ra. Vui lòng thử lại.";
                return View();
            }
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        //public IActionResult Register(KhachHangViewModel khachHang)
        //{
        //    try
        //    {
        //        khachHang.Id = Guid.NewGuid();
        //        khachHang.DiemTich = 0;
        //        khachHang.TrangThai = 1;
        //        HttpResponseMessage response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + $"QuanLyNguoiDung/DangKyKhachHang", khachHang).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            TempData["SuccessMessage"] = "Tài khoản đã được đăng ký thành công!";
        //            //return RedirectToAction("Login", new { actionName = "Index" });
        //            return View("RegisterSuccess");
        //        }
        //        ViewBag.ErrorMessage = "Tài khoản đã được đăng ký với email hoặc số điện thoại này!";
        //        return View();
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        [HttpPost]
        public IActionResult Register(KhachHangViewModel khachHang)
        {
            // Thêm bước kiểm tra ModelState trước khi gọi API
            if (!ModelState.IsValid)
            {
                return View(khachHang); // Trả về lỗi validation
            }

            try
            {
                khachHang.Id = Guid.NewGuid();
                khachHang.DiemTich = 0;
                khachHang.TrangThai = 1;

                HttpResponseMessage response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + $"QuanLyNguoiDung/DangKyKhachHang", khachHang).Result;

                if (response.IsSuccessStatusCode)
                {
                    // Dùng TempData là đúng rồi vì bạn render view mới
                    TempData["SuccessMessage"] = "Tài khoản đã được đăng ký thành công!";
                    return View("RegisterSuccess"); // Chuyển đến view mới
                }

                ViewBag.ErrorMessage = "Tài khoản đã được đăng ký với email hoặc số điện thoại này!";
                return View(khachHang); // Sửa 1: Trả về model để giữ dữ liệu form
            }
            catch
            {
                ViewBag.ErrorMessage = "Đã có lỗi hệ thống xảy ra. Vui lòng thử lại.";
                return View(khachHang); // Sửa 2: Trả về model
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
                    //LoginViewModel loginViewModel = JsonConvert.DeserializeObject<LoginViewModel>(loginInfor);
                    var response = _httpClient.GetAsync(_httpClient.BaseAddress + $"KhachHang/GetById?id={loginViewModel.Id}").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        loginViewModel.DiemTich = JsonConvert.DeserializeObject<KhachHang>(response.Content.ReadAsStringAsync().Result).DiemTich;
                        return View(loginViewModel);
                    }
                    else
                    {
                        return View(loginViewModel);
                    }
                }
                return Redirect("https://localhost:5001/");
            }
            catch
            {
                return Redirect("https://localhost:5001/");
            }

        }
        [HttpPut]
        public ActionResult UpdateProfile(string ten, string email, string sdt, int? gioitinh, DateTime? ngaysinh, string? diachi)
        {
            try
            {
                if (ten == null || email == null)
                {
                    return Json(new { success = false, message = "Không được để trống thông tin" });
                }
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                if (!match.Success)
                {
                    return Json(new { success = false, message = "Email sai" });
                }
                if (Regex.Match(sdt, @"^(\+[0-9])$").Success)
                {
                    return Json(new { success = false, message = "Số điện thoại sai sai" });
                }
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
                else
                {
                    return Json(new { success = false, message = "Cập nhật thông tin cá nhân thất bại" });
                }
            }
            catch (Exception)
            {
                return Json(new { success = false, message = "Cập nhật thông tin cá nhân thất bại" });
            }

        }
        public IActionResult PurchaseOrder()
        {
            //try
            //{
            //    var session = HttpContext.Session.GetString("LoginInfor");
            //    if (session != null)
            //    {
            //        LoginViewModel loginViewModel = JsonConvert.DeserializeObject<LoginViewModel>(session);
            //        List<DonMuaViewModel> donMuaViewModels = new List<DonMuaViewModel>();
            //        HttpResponseMessage responseDonMua = _httpClient.GetAsync(_httpClient.BaseAddress + $"LichSuTichDiem/GetAllDonMua?IDkhachHang={loginViewModel.Id}").Result;
            //        if (responseDonMua.IsSuccessStatusCode)
            //        {
            //            donMuaViewModels = JsonConvert.DeserializeObject<List<DonMuaViewModel>>(responseDonMua.Content.ReadAsStringAsync().Result);
            //        }
            //        return View("PurchaseOrder", donMuaViewModels);
            //    }
            //    else
            //    {
            //        return Redirect("https://localhost:5001/");
            //    }
            //}
            //catch (Exception)
            //{
            //    return Redirect("https://localhost:5001/");
            //}
            try
            {
                var session = HttpContext.Session.GetString("LoginInfor");
                if (session != null)
                {
                    LoginViewModel loginViewModel = JsonConvert.DeserializeObject<LoginViewModel>(session);

                    // Log ID của user để kiểm tra
                    _logger.LogInformation($"Đang lấy đơn hàng cho User ID: {loginViewModel.Id}");

                    List<DonMuaViewModel> donMuaViewModels = new List<DonMuaViewModel>();

                    // Gọi API
                    HttpResponseMessage responseDonMua = _httpClient.GetAsync(_httpClient.BaseAddress + $"LichSuTichDiem/GetAllDonMua?IDkhachHang={loginViewModel.Id}").Result;

                    // --- BẮT ĐẦU SỬA LỖI ---
                    if (responseDonMua.IsSuccessStatusCode)
                    {
                        // Nếu API thành công
                        donMuaViewModels = JsonConvert.DeserializeObject<List<DonMuaViewModel>>(responseDonMua.Content.ReadAsStringAsync().Result);
                        _logger.LogInformation($"API GetAllDonMua thành công. Tìm thấy {donMuaViewModels.Count} đơn hàng.");
                    }
                    else
                    {
                        // Nếu API thất bại, GHI LẠI LỖI
                        string apiError = responseDonMua.Content.ReadAsStringAsync().Result;
                        _logger.LogError($"LỖI API GetAllDonMua: Status={responseDonMua.StatusCode}, Lỗi chi tiết={apiError}");
                    }
                    // --- KẾT THÚC SỬA LỖI ---

                    return View("PurchaseOrder", donMuaViewModels);
                }
                else
                {
                    return Redirect("https://localhost:5001/");
                }
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi nếu có ngoại lệ (ví dụ: lỗi try...catch)
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
            catch (Exception)
            {
                return Redirect("https://localhost:5001/");
            }

        }
        public IActionResult LichSuTieuDiemTichDiem()
        {
            return View();
        }
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
                    //foreach (var item in listLSTD)
                    //{
                    //    item.Ngaytao1 = item.NgayTao.ToString("dd/MM/yyyy");
                    //    item.Ngaythanhtoan1 = item.NgayThanhToan != null ? item.NgayThanhToan.Value.ToString("dd/MM/yyyy") : null;
                    //    item.Ngaynhanhang1 = item.NgayNhanHang != null ? item.NgayNhanHang.Value.ToString("dd/MM/yyyy") : null;
                    //}
                    if (danhGiaCTHDView.TrangThaiGiaoHang != 2 && danhGiaCTHDView.TrangThaiGiaoHang != null)
                    {
                        if (danhGiaCTHDView.TrangThaiGiaoHang == 0)
                        {
                            List<LichSuTichDiemTieuDiemViewModel> listLSTD1 = new List<LichSuTichDiemTieuDiemViewModel>();
                            listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 0).ToList();
                            foreach (var item in listLSTD1)
                            {
                                listLSTDFN.Add(item);
                            }
                            listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 3 && p.TrangThaiGiaoHang == 5).ToList();
                            foreach (var item in listLSTD1)
                            {
                                listLSTDFN.Add(item);
                            }
                            listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 3 && p.TrangThaiGiaoHang == 4).ToList();
                            foreach (var item in listLSTD1)
                            {
                                listLSTDFN.Add(item);
                            }
                        }
                        else
                        {
                            List<LichSuTichDiemTieuDiemViewModel> listLSTD1 = new List<LichSuTichDiemTieuDiemViewModel>();
                            listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 1 && p.TrangThaiGiaoHang == 6).ToList();
                            foreach (var item in listLSTD1)
                            {
                                listLSTDFN.Add(item);
                            }
                            listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 1 && p.TrangThaiGiaoHang == 9).ToList();
                            foreach (var item in listLSTD1)
                            {
                                listLSTDFN.Add(item);
                            }
                            listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 1 && p.TrangThaiGiaoHang == 4).ToList();
                            foreach (var item in listLSTD1)
                            {
                                listLSTDFN.Add(item);
                            }
                            listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 1 && p.TrangThaiGiaoHang == 5).ToList();
                            foreach (var item in listLSTD1)
                            {
                                listLSTDFN.Add(item);
                            }
                            listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 2 && p.TrangThaiGiaoHang == 7).ToList();
                            foreach (var item in listLSTD1)
                            {
                                listLSTDFN.Add(item);
                            }
                            listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 4 && p.TrangThaiGiaoHang == 5).ToList();
                            foreach (var item in listLSTD1)
                            {
                                listLSTDFN.Add(item);
                            }
                        }
                        listLSTDFN = listLSTDFN.OrderBy(p => p.TrangThaiLSTD).ToList();
                        listLSTDFN = listLSTDFN.OrderBy(p => p.NgayTao).ToList();
                        var model = listLSTDFN.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        return Json(new
                        {
                            data = model,
                            total = listLSTDFN.Count,
                            status = true
                        });
                    }
                    else
                    {
                        List<LichSuTichDiemTieuDiemViewModel> listLSTD1 = new List<LichSuTichDiemTieuDiemViewModel>();
                        listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 0).ToList();
                        foreach (var item in listLSTD1)
                        {
                            listLSTDFN.Add(item);
                        }
                        listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 3 && p.TrangThaiGiaoHang == 4).ToList();
                        foreach (var item in listLSTD1)
                        {
                            listLSTDFN.Add(item);
                        }
                        listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 3 && p.TrangThaiGiaoHang == 5).ToList();
                        foreach (var item in listLSTD1)
                        {
                            listLSTDFN.Add(item);
                        }
                        listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 1 && p.TrangThaiGiaoHang == 6).ToList();
                        foreach (var item in listLSTD1)
                        {
                            listLSTDFN.Add(item);
                        }
                        listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 1 && p.TrangThaiGiaoHang == 9).ToList();
                        foreach (var item in listLSTD1)
                        {
                            listLSTDFN.Add(item);
                        }
                        listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 1 && p.TrangThaiGiaoHang == 4).ToList();
                        foreach (var item in listLSTD1)
                        {
                            listLSTDFN.Add(item);
                        }
                        listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 1 && p.TrangThaiGiaoHang == 5).ToList();
                        foreach (var item in listLSTD1)
                        {
                            listLSTDFN.Add(item);
                        }
                        listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 2 && p.TrangThaiGiaoHang == 7).ToList();
                        foreach (var item in listLSTD1)
                        {
                            listLSTDFN.Add(item);
                        }
                        listLSTD1 = listLSTD.Where(p => p.TrangThaiLSTD == 4 && p.TrangThaiGiaoHang == 5).ToList();
                        foreach (var item in listLSTD1)
                        {
                            listLSTDFN.Add(item);
                        }
                        listLSTDFN = listLSTDFN.OrderBy(p => p.TrangThaiLSTD).ToList();
                        listLSTDFN = listLSTDFN.OrderBy(p => p.NgayTao).ToList();
                        var model = listLSTDFN.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        return Json(new
                        {
                            data = model,
                            total = listLSTDFN.Count,
                            status = true
                        });
                    }
                }
                else return Json(new { status = false });

            }
            catch (Exception)
            {
                return Redirect("https://localhost:5001/");
            }

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
            catch (Exception)
            {
                return Redirect("https://localhost:5001/");
            }

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
            catch
            {
                return BadRequest();
            }
        }
        public IActionResult DanhGiaSanPham([FromBody] DanhGiaCTHDViewModel danhGiaCTHDView)
        {
            try
            {
                if (danhGiaCTHDView.danhgia == "")
                {
                    danhGiaCTHDView.danhgia = "Người dùng này không để lại bình luận";
                }
                HttpResponseMessage responseDonMuaCT = _httpClient.PutAsync(_httpClient.BaseAddress + $"DanhGia?idCTHD={danhGiaCTHDView.idCTHD}&soSao={danhGiaCTHDView.soSao}&binhLuan={danhGiaCTHDView.danhgia}", null).Result;
                if (responseDonMuaCT.IsSuccessStatusCode)
                {
                    RedirectToAction("PurchaseOrderDetail", "Home", new { idHoaDon = danhGiaCTHDView.idHD });
                }
                return RedirectToAction("PurchaseOrderDetail", "Home", new { idHoaDon = danhGiaCTHDView.idHD });
            }
            catch (Exception)
            {
                return Redirect("https://localhost:5001/");
            }

        }
        public IActionResult HuyDonHang(Guid idHoaDon)
        {
            try
            {
                HttpResponseMessage responseDonMua = _httpClient.PutAsync(_httpClient.BaseAddress + $"HoaDon?idhoadon={idHoaDon}&trangthai=8", null).Result;
                if (responseDonMua.IsSuccessStatusCode)
                {
                    RedirectToAction("PurchaseOrder");
                }
                return RedirectToAction("PurchaseOrder");
            }
            catch (Exception)
            {
                return RedirectToAction("PurchaseOrder");
            }

        }
        public IActionResult HoanTacHuyDonHang(Guid idHoaDon)
        {
            try
            {
                HttpResponseMessage responseDonMua = _httpClient.PutAsync(_httpClient.BaseAddress + $"HoaDon?idhoadon={idHoaDon}&trangthai=2", null).Result;
                if (responseDonMua.IsSuccessStatusCode)
                {
                    RedirectToAction("PurchaseOrder");
                }
                return RedirectToAction("PurchaseOrder");
            }
            catch (Exception)
            {
                return RedirectToAction("PurchaseOrder");
            }

        }
        public IActionResult DoiTraHang(Guid idHoaDon)
        {
            try
            {
                HttpResponseMessage responseDonMua = _httpClient.PutAsync(_httpClient.BaseAddress + $"HoaDon?idhoadon={idHoaDon}&trangthai=9", null).Result;
                if (responseDonMua.IsSuccessStatusCode)
                {
                    RedirectToAction("PurchaseOrder");
                }
                return RedirectToAction("PurchaseOrder");
            }
            catch (Exception)
            {
                return RedirectToAction("PurchaseOrder");
            }

        }
        public IActionResult HoanTacDoiTraHang(Guid idHoaDon)
        {
            try
            {
                HttpResponseMessage responseDonMua = _httpClient.PutAsync(_httpClient.BaseAddress + $"HoaDon?idhoadon={idHoaDon}&trangthai=6", null).Result;
                if (responseDonMua.IsSuccessStatusCode)
                {
                    RedirectToAction("PurchaseOrder");
                }
                return RedirectToAction("PurchaseOrder");

            }
            catch (Exception)
            {
                return RedirectToAction("PurchaseOrder");
            }

        }
        public IActionResult XacNhanGHTC(Guid idHoaDon)
        {
            try
            {
                HttpResponseMessage responseDonMua = _httpClient.PutAsync(_httpClient.BaseAddress + $"HoaDon?idhoadon={idHoaDon}&trangthai=6", null).Result;
                if (responseDonMua.IsSuccessStatusCode)
                {
                    RedirectToAction("PurchaseOrderDetail", idHoaDon);
                }
                return RedirectToAction("PurchaseOrder");
            }
            catch
            {
                return RedirectToAction("PurchaseOrder");
            }
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
                    if (Search != null)
                    {
                        donMuaViewModels = donMuaViewModels.Where(p => p.MaHD.ToLower().Contains(Search.ToLower())).ToList();
                    }
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
                        return Json(new
                        {
                            data = model,
                            total = donMuaViewModels.Count,
                            status = true
                        });
                    }
                    else
                    {
                        var model = donMuaViewModels.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                        return Json(new
                        {
                            data = model,
                            total = donMuaViewModels.Count,
                            status = true
                        });
                    }
                }
                else return Json(new { status = false });
            }
            catch
            {
                return Json(new { status = false });
            }
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        [HttpGet]

        #endregion

        #region ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            HttpContext.Session.Remove("LoginInfor");
            return View();
        }
        [HttpPost]
        //public IActionResult ForgotPassword(string email)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(email))
        //        {
        //            return View();
        //        }
        //        var khachHang = JsonConvert.DeserializeObject<KhachHangViewModel>(_httpClient.GetAsync(_httpClient.BaseAddress + "KhachHang/GetKhachHangByEmail?email=" + email).Result.Content.ReadAsStringAsync().Result);
        //        if (khachHang.Email != null)
        //        {
        //            string ma = Guid.NewGuid().ToString().Substring(0, 5);
        //            MailData mailData = new MailData() { EmailToId = email, EmailToName = email, EmailSubject = "Khôi Phục Mật Khẩu", EmailBody = "Mã xác nhận của bạn là: " + ma };
        //            HttpResponseMessage response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "Mail/SendMail", mailData).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                HttpContext.Session.SetString("ForgotPassword", khachHang.Id + ":" + ma);
        //                return RedirectToAction("SubmitForgotPassword");
        //            }
        //            else return BadRequest();
        //        }
        //        else
        //        {
        //            ViewData["EmailError"] = "Email không tồn tại trong hệ thống";
        //            return View();
        //        }
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}
        // [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return View();
                }
                var khachHang = JsonConvert.DeserializeObject<KhachHangViewModel>(_httpClient.GetAsync(_httpClient.BaseAddress + "KhachHang/GetKhachHangByEmail?email=" + email).Result.Content.ReadAsStringAsync().Result);
                if (khachHang.Email != null)
                {
                    string ma = Guid.NewGuid().ToString().Substring(0, 5);
                    MailData mailData = new MailData() { EmailToId = email, EmailToName = email, EmailSubject = "Khôi Phục Mật Khẩu", EmailBody = "Mã xác nhận của bạn là: " + ma };
                    HttpResponseMessage response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "Mail/SendMail", mailData).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        // SỬA Ở ĐÂY: Lưu cả email và mã vào Session
                        // Dùng dấu | để phân tách vì email có thể chứa :
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
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public IActionResult SubmitForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitForgotPassword(string code)
        {
            try
            {
                // LỖI Ở ĐÂY: Bạn đang tách bằng dấu ":"
                // string[] submit = HttpContext.Session.GetString("ForgotPassword").Split(":");

                // SỬA LẠI: Phải tách bằng dấu "|" giống như lúc lưu
                string[] submit = HttpContext.Session.GetString("ForgotPassword").Split("|");

                if (submit.Length == 2 && code == submit[1]) // submit[0] là email, submit[1] là mã
                {
                    return RedirectToAction("ChangeForgotPassword");
                }
                else
                {
                    // Thay vì trả về BadRequest, hãy trả về View với thông báo lỗi
                    ViewData["ErrorMessage"] = "Mã xác nhận không chính xác. Vui lòng thử lại.";
                    return View();
                }
            }
            catch
            {
                // Lỗi này xảy ra khi Session hết hạn hoặc không tìm thấy
                ViewData["ErrorMessage"] = "Phiên làm việc của bạn đã hết hạn. Vui lòng yêu cầu mã mới.";
                return View();
            }
        }
        [HttpGet]
        public IActionResult ChangeForgotPassword()
        {
            return View();
        }
        [HttpPost]
        //public IActionResult ChangeForgotPassword(string password, string confirmPassword)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(password))
        //        {
        //            ViewData["PasswordError"] = "Mật khẩu không được bỏ trống";
        //            return View();
        //        }
        //        else if (password.Length < 8)
        //        {
        //            ViewData["PasswordError"] = "Mật khẩu phải lớn hơn 8 ký tự";
        //            return View();
        //        }

        //        if (string.IsNullOrEmpty(confirmPassword))
        //        {
        //            ViewData["ConfirmPasswordError"] = "Xác nhận mật khẩu không được bỏ trống";
        //            return View();
        //        }
        //        else if (password != confirmPassword)
        //        {
        //            ViewData["ConfirmPasswordError"] = "Mật khẩu và xác nhận mật khẩu không khớp";
        //            return View();
        //        }

        //        if (password == confirmPassword)
        //        {
        //            string[] submit = HttpContext.Session.GetString("ForgotPassword").Split(":");
        //            KhachHangViewModel khachHang = new KhachHangViewModel() { Id = new Guid(submit[0]), Password = password };
        //            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "KhachHang/ChangeForgotPassword?id=" + khachHang.Id + "&password=" + khachHang.Password).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                return RedirectToAction("Login", new { actionName = "Index" });
        //            }
        //            else
        //            {
        //                ViewData["ErrorMessage"] = "Không thể đặt lại mật khẩu. Vui lòng kiểm tra lại thông tin.";
        //                return View();
        //            }
        //        }
        //        else
        //        {
        //            ViewData["ErrorMessage"] = "Không thể đặt lại mật khẩu. Vui lòng kiểm tra lại thông tin.";
        //            return View();
        //        }
        //    }
        //    catch
        //    {
        //        ViewData["ConfirmPasswordError"] = "Error";
        //        return View();
        //    }
        //}

        public async Task<IActionResult> ChangeForgotPassword(string password, string confirmPassword) // Đổi hàm này thành async Task
        {
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

                if (password == confirmPassword)
                {
                    // SỬA Ở ĐÂY: Đọc Email từ Session và gọi đúng API
                    string[] submit = HttpContext.Session.GetString("ForgotPassword").Split("|");
                    string email = submit[0];
                    string maXacNhan = submit[1];

                    // 1. Tạo request đúng
                    ResetPasswordRequest request = new ResetPasswordRequest
                    {
                        Email = email,
                        Password = password,
                        ConfirmPassword = confirmPassword,
                        ResetToken = maXacNhan
                    };

                    // 2. Gọi API đúng (POST /api/QuanLyNguoiDung/ResetPassword)
                    string apiUrl = "https://localhost:7095/api/QuanLyNguoiDung/ResetPassword";
                    var response = await _httpClient.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {   
                        HttpContext.Session.Remove("ForgotPassword"); // Xóa session
                        return RedirectToAction("Login", new { actionName = "Index" });
                    }
                    else
                    {
                        ViewData["ErrorMessage"] = "Không thể đặt lại mật khẩu. Vui lòng thử lại.";
                        return View();
                    }
                }
                else
                {
                    ViewData["ErrorMessage"] = "Không thể đặt lại mật khẩu. Vui lòng kiểm tra lại thông tin.";
                    return View();
                }
            }
            catch
            {
                ViewData["ConfirmPasswordError"] = "Lỗi hệ thống, vui lòng thử lại.";
                return View();
            }
        }
        [HttpGet]
        public IActionResult ResetPassword(string email)
        {
            try
            {
                // Kiểm tra email và chuyển đến trang ResetPassword nếu email hợp lệ
                if (!string.IsNullOrEmpty(email))
                {
                    ResetPasswordRequest request = new ResetPasswordRequest
                    {
                        Email = email
                    };
                    return View(request);
                }
                else
                {
                    // Xử lý khi email không hợp lệ
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
                    // Validate password and confirm password match
                    if (request.Password != request.ConfirmPassword)
                    {
                        ModelState.AddModelError("", "Passwords do not match");
                        return View("ResetPassword");
                    }

                    // Implement the code to save the new password in your QuanLyNguoiDungService
                    string apiUrl = "https://localhost:7095/api/QuanLyNguoiDung/ResetPassword";
                    var response = await _httpClient.PostAsync(apiUrl, new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Password reset successfully";
                        return RedirectToAction("Login", new { actionNam = "Index" });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Không thể đặt lại mật khẩu");
                    }
                }

                return View("ResetPassword");
            }
            catch
            {
                return View("ResetPassword");
            }
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
                    if (soTienGiam <= (tongTien / 5))
                    {
                        return Json(new { SoTienGiam = soTienGiam, TrangThai = true });
                    }
                    else
                    {
                        return Json(new { Loi = "Số tiền giảm khi sử dụng điểm phải nhỏ hơn 20% giá trị đơn hàng", TrangThai = false });
                    }
                }
                else return Json(new { Loi = "Không kết nối được với server", TrangThai = false });
            }
            catch
            {
                return Json(new { Loi = "Không kết nối được với server", TrangThai = false });
            }
        }
        [HttpGet]
        public IActionResult CheckOut()
        {
            return View();
        }
        [HttpPost]
        //public string Order(HoaDonViewModel hoaDon)
        //{
        //    try
        //    {
        //        List<ChiTietHoaDonViewModel> lstChiTietHoaDon = new List<ChiTietHoaDonViewModel>();
        //        string temp = TempData.Peek("ListBienThe") as string;
        //        string trangThai = TempData.Peek("TrangThai") as string;
        //        foreach (var item in JsonConvert.DeserializeObject<List<GioHangRequest>>(temp))
        //        {
        //            ChiTietHoaDonViewModel chiTietHoaDon = new ChiTietHoaDonViewModel();
        //            chiTietHoaDon.IDChiTietSanPham = item.IDChiTietSanPham;
        //            chiTietHoaDon.SoLuong = item.SoLuong;
        //            chiTietHoaDon.DonGia = item.DonGia.Value;
        //            lstChiTietHoaDon.Add(chiTietHoaDon);
        //        }
        //        hoaDon.ChiTietHoaDons = lstChiTietHoaDon;
        //        hoaDon.TrangThai = Convert.ToBoolean(trangThai);
        //        var session = HttpContext.Session.GetString("LoginInfor");
        //        if (session != null)
        //        {
        //            hoaDon.IDNguoiDung = JsonConvert.DeserializeObject<LoginViewModel>(session).Id;
        //        }
        //        TempData.Remove("TongTien");
        //        TempData.Remove("Quantity");
        //        if (hoaDon.PhuongThucThanhToan == "COD")
        //        {
        //            HttpResponseMessage response = _httpClient.PostAsJsonAsync("HoaDon", hoaDon).Result;
        //            if (response.IsSuccessStatusCode)
        //            {
        //                TempData["CheckOutSuccess"] = response.Content.ReadAsStringAsync().Result;
        //                if (!hoaDon.TrangThai) Response.Cookies.Delete("Cart");
        //                // lam them
        //                TempData["SoLuong"] = "0";
        //                // lam end
        //                return "https://localhost:5001/Home/CheckOutSuccess";
        //            }
        //            else return "";
        //        }
        //        else if (hoaDon.PhuongThucThanhToan == "VNPay")
        //        {
        //            TempData["HoaDon"] = JsonConvert.SerializeObject(hoaDon);
        //            string vnp_Returnurl = "https://localhost:5001/Home/PaymentCallBack"; //URL nhan ket qua tra ve 
        //            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
        //            string vnp_TmnCode = "FLYBWUDB"; //Ma định danh merchant kết nối (Terminal Id)
        //            string vnp_HashSecret = "6ZCJ0ZOJ6MOSREICS95ETK29ZNGDYQ2Y"; //Secret Key
        //            string ipAddr = HttpContext.Connection.RemoteIpAddress?.ToString();
        //            //Get payment input
        //            OrderInfo order = new OrderInfo();
        //            order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
        //            order.Amount = hoaDon.TongTien;
        //            order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
        //            order.CreatedDate = DateTime.Now;
        //            //Save order to db

        //            //Build URL for VNPAY
        //            VnPayLibrary vnpay = new VnPayLibrary();

        //            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
        //            vnpay.AddRequestData("vnp_Command", "pay");
        //            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
        //            vnpay.AddRequestData("vnp_Amount", (order.Amount * 100).ToString());
        //            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
        //            vnpay.AddRequestData("vnp_CurrCode", "VND");
        //            vnpay.AddRequestData("vnp_IpAddr", ipAddr);
        //            vnpay.AddRequestData("vnp_Locale", "vn");
        //            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
        //            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

        //            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
        //            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

        //            //Add Params of 2.1.0 Version
        //            //Billing

        //            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
        //            //log.InfoFormat("VNPAY URL: {0}", paymentUrl);

        //            return paymentUrl;
        //        }
        //        else return "";
        //    }
        //    catch
        //    {
        //        return "https://localhost:5001/Home/CheckOutSuccess";
        //    }
        //}
        public string Order(HoaDonViewModel hoaDon)
        {
            try
            {
                List<ChiTietHoaDonViewModel> lstChiTietHoaDon = new List<ChiTietHoaDonViewModel>();
                string temp = TempData.Peek("ListBienThe") as string; // temp = JSON của giỏ hàng
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
                TempData.Remove("TongTien");
                TempData.Remove("Quantity");
                if (hoaDon.PhuongThucThanhToan == "COD")
                {
                    HttpResponseMessage response = _httpClient.PostAsJsonAsync("HoaDon", hoaDon).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        // --- PHẦN SỬA LỖI BẮT ĐẦU ---

                        // 1. Lấy JSON thông tin đơn hàng từ API (Tên, Địa chỉ, Tổng tiền...)
                        var apiResponseJson = response.Content.ReadAsStringAsync().Result;

                        // 2. Chuyển nó thành model DonMuaSuccessViewModel
                        var donMua = JsonConvert.DeserializeObject<DonMuaSuccessViewModel>(apiResponseJson);

                        // 3. GÁN GIỎ HÀNG (từ biến 'temp') vào model
                        donMua.GioHangs = JsonConvert.DeserializeObject<List<GioHangRequest>>(temp);

                        // 4. Lưu model ĐẦY ĐỦ (đã có GioHangs) vào TempData
                        TempData["CheckOutSuccess"] = JsonConvert.SerializeObject(donMua);

                        // --- PHẦN SỬA LỖI KẾT THÚC ---

                        if (!hoaDon.TrangThai)
                        {
                            // 1. User là GUEST (TrangThai = false) -> Xóa Cookie
                            Response.Cookies.Delete("Cart");
                            _logger.LogInformation("Đã xóa giỏ hàng Cookie cho khách.");
                        }
                        else
                        {
                            // 2. User đã LOGIN (TrangThai = true) -> Xóa giỏ hàng trong DB

                            // Đặt breakpoint (dấu chấm đỏ) ở dòng này để kiểm tra xem hoaDon.IDNguoiDung có G-U-I-D đúng không.
                            var clearCartResponse = _httpClient.DeleteAsync(_httpClient.BaseAddress + $"GioHang/ClearCartLogin?idNguoiDung={hoaDon.IDNguoiDung}").Result;

                            // *** KIỂM TRA KẾT QUẢ API ***
                            if (clearCartResponse.IsSuccessStatusCode)
                            {
                                _logger.LogInformation($"Đã xóa giỏ hàng DB cho user: {hoaDon.IDNguoiDung}");
                            }
                            else
                            {
                                // GHI LẠI LỖI NẾU API THẤT BẠI
                                string apiError = clearCartResponse.Content.ReadAsStringAsync().Result;
                                _logger.LogError($"LỖI: Không thể xóa giỏ hàng DB cho user: {hoaDon.IDNguoiDung}. API trả về: {apiError}");
                            }
                        }
                            // lam them
                            TempData["SoLuong"] = "0";
                        // lam end
                        return "https://localhost:5001/Home/CheckOutSuccess";
                    }
                    else return "";
                }
                else if (hoaDon.PhuongThucThanhToan == "VNPay")
                {
                    TempData["HoaDon"] = JsonConvert.SerializeObject(hoaDon);
                    string vnp_Returnurl = "https://localhost:5001/Home/PaymentCallBack"; //URL nhan ket qua tra ve 
                    string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
                    string vnp_TmnCode = "FLYBWUDB"; //Ma định danh merchant kết nối (Terminal Id)
                    string vnp_HashSecret = "6ZCJ0ZOJ6MOSREICS95ETK29ZNGDYQ2Y"; //Secret Key
                    string ipAddr = HttpContext.Connection.RemoteIpAddress?.ToString();
                    //Get payment input
                    OrderInfo order = new OrderInfo();
                    order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
                    order.Amount = hoaDon.TongTien;
                    order.Status = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending" khởi tạo giao dịch chưa có IPN
                    order.CreatedDate = DateTime.Now;
                    //Save order to db

                    //Build URL for VNPAY
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
                    vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other

                    vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                    vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

                    //Add Params of 2.1.0 Version
                    //Billing

                    string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
                    //log.InfoFormat("VNPAY URL: {0}", paymentUrl);

                    return paymentUrl;
                }
                else return "";
            }
            catch
            {
                return "https://localhost:5001/Home/CheckOutSuccess";
            }
        }
        //[HttpGet]
        //public IActionResult PaymentCallBack()
        //{
        //    try
        //    {
        //        if (Request.Query.Count > 0)
        //        {
        //            string vnp_HashSecret = "6ZCJ0ZOJ6MOSREICS95ETK29ZNGDYQ2Y"; //Chuoi bi mat
        //            var vnpayData = Request.Query;
        //            VnPayLibrary vnpay = new VnPayLibrary();

        //            foreach (var s in vnpayData)
        //            {
        //                //get all querystring data
        //                if (!string.IsNullOrEmpty(s.Key) && s.Key.StartsWith("vnp_"))
        //                {
        //                    vnpay.AddResponseData(s.Key, vnpayData[s.Key]);
        //                }
        //            }
        //            //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay    
        //            //vnp_TransactionNo: Ma GD tai he thong VNPAY
        //            //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
        //            //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

        //            long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
        //            long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
        //            string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
        //            string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
        //            String vnp_SecureHash = Request.Query["vnp_SecureHash"];
        //            String TerminalID = Request.Query["vnp_TmnCode"];
        //            long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
        //            String bankCode = Request.Query["vnp_BankCode"];

        //            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
        //            if (checkSignature)
        //            {
        //                if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
        //                {
        //                    //Thanh toan thanh cong
        //                    TempData.Remove("TongTien");
        //                    TempData.Remove("Quantity");
        //                    var hoaDon = JsonConvert.DeserializeObject<HoaDonViewModel>(TempData["HoaDon"].ToString());
        //                    hoaDon.NgayThanhToan = DateTime.Now;
        //                    HttpResponseMessage response = _httpClient.PostAsJsonAsync("HoaDon", hoaDon).Result;
        //                    if (response.IsSuccessStatusCode)
        //                    {
        //                        TempData["CheckOutSuccess"] = response.Content.ReadAsStringAsync().Result;
        //                        if (!hoaDon.TrangThai) Response.Cookies.Delete("Cart");
        //                        // lam them
        //                        TempData["SoLuong"] = "0";
        //                        // lam end
        //                        return RedirectToAction("CheckOutSuccess");
        //                    }
        //                    else return BadRequest();
        //                }
        //                else
        //                {
        //                    //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
        //                    return BadRequest();
        //                }
        //            }
        //            else
        //            {
        //                return BadRequest();
        //            }
        //        }
        //        else return BadRequest();
        //    }
        //    catch
        //    {
        //        return BadRequest();
        //    }
        //}
        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            try
            {
                if (Request.Query.Count > 0)
                {
                    string vnp_HashSecret = "6ZCJ0ZOJ6MOSREICS95ETK29ZNGDYQ2Y"; //Chuoi bi mat
                    var vnpayData = Request.Query;
                    VnPayLibrary vnpay = new VnPayLibrary();

                    foreach (var s in vnpayData)
                    {
                        //get all querystring data
                        if (!string.IsNullOrEmpty(s.Key) && s.Key.StartsWith("vnp_"))
                        {
                            vnpay.AddResponseData(s.Key, vnpayData[s.Key]);
                        }
                    }
                    //vnp_TxnRef: Ma don hang merchant gui VNPAY tai command=pay   
                    //vnp_TransactionNo: Ma GD tai he thong VNPAY
                    //vnp_ResponseCode:Response code from VNPAY: 00: Thanh cong, Khac 00: Xem tai lieu
                    //vnp_SecureHash: HmacSHA512 cua du lieu tra ve

                    long orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                    long vnpayTranId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                    string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                    string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                    String vnp_SecureHash = Request.Query["vnp_SecureHash"];
                    String TerminalID = Request.Query["vnp_TmnCode"];
                    long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / 100;
                    String bankCode = Request.Query["vnp_BankCode"];

                    bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                    if (checkSignature)
                    {
                        if (vnp_ResponseCode == "00" && vnp_TransactionStatus == "00")
                        {
                            //Thanh toan thanh cong
                            TempData.Remove("TongTien");
                            TempData.Remove("Quantity");
                            var hoaDon = JsonConvert.DeserializeObject<HoaDonViewModel>(TempData["HoaDon"].ToString());
                            hoaDon.NgayThanhToan = DateTime.Now;
                            HttpResponseMessage response = _httpClient.PostAsJsonAsync("HoaDon", hoaDon).Result;
                            if (response.IsSuccessStatusCode)
                            {
                                // --- PHẦN SỬA LỖI BẮT ĐẦU ---

                                // 1. Lấy JSON thông tin đơn hàng từ API
                                var apiResponseJson = response.Content.ReadAsStringAsync().Result;

                                // 2. Chuyển nó thành model
                                var donMua = JsonConvert.DeserializeObject<DonMuaSuccessViewModel>(apiResponseJson);

                                // 3. Lấy JSON giỏ hàng từ TempData và gán vào model
                                string temp = TempData.Peek("ListBienThe") as string;
                                donMua.GioHangs = JsonConvert.DeserializeObject<List<GioHangRequest>>(temp);

                                // 4. Lưu model ĐẦY ĐỦ (đã có GioHangs) vào TempData
                                TempData["CheckOutSuccess"] = JsonConvert.SerializeObject(donMua);

                                // --- PHẦN SỬA LỖI KẾT THÚC ---

                                if (!hoaDon.TrangThai)
                                {
                                    // 1. User là GUEST (TrangThai = false) -> Xóa Cookie
                                    Response.Cookies.Delete("Cart");
                                    _logger.LogInformation("Đã xóa giỏ hàng Cookie cho khách (VNPay).");
                                }
                                else
                                {
                                    // 2. User đã LOGIN (TrangThai = true) -> Xóa giỏ hàng trong DB

                                    // Đặt breakpoint (dấu chấm đỏ) ở dòng này
                                    var clearCartResponse = _httpClient.DeleteAsync(_httpClient.BaseAddress + $"GioHang/ClearCartLogin?idNguoiDung={hoaDon.IDNguoiDung}").Result;

                                    // *** KIỂM TRA KẾT QUẢ API ***
                                    if (clearCartResponse.IsSuccessStatusCode)
                                    {
                                        _logger.LogInformation($"Đã xóa giỏ hàng DB cho user: {hoaDon.IDNguoiDung} (VNPay).");
                                    }
                                    else
                                    {
                                        // GHI LẠI LỖI NẾU API THẤT BẠI
                                        string apiError = clearCartResponse.Content.ReadAsStringAsync().Result;
                                        _logger.LogError($"LỖI: Không thể xóa giỏ hàng DB cho user: {hoaDon.IDNguoiDung} (VNPay). API trả về: {apiError}");
                                    }
                                }
                                    // lam them
                                    TempData["SoLuong"] = "0";
                                // lam end
                                return RedirectToAction("CheckOutSuccess");
                            }
                            else return BadRequest();
                        }
                        else
                        {
                            //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                            return BadRequest();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }
        //[HttpGet]
        //public IActionResult CheckOutSuccess()
        //{
        //    try
        //    {
        //        var donMua = JsonConvert.DeserializeObject<DonMuaSuccessViewModel>(TempData.Peek("CheckOutSuccess") as string);
        //        var temp = HttpContext.Session.GetString("LoginInfor");
        //        if (temp != null && donMua != null)
        //        {
        //            var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(temp);
        //            loginInfor.DiemTich -= donMua.DiemSuDung;
        //            HttpContext.Session.SetString("LoginInfor", JsonConvert.SerializeObject(loginInfor));
        //        }
        //        return View(donMua);
        //    }
        //    catch
        //    {
        //        return View(new DonMuaSuccessViewModel());
        //    }
        //}
        [HttpGet]
        public IActionResult CheckOutSuccess()
        {
            try
            {
                // 1. Đọc TempData
                var donMuaJson = TempData.Peek("CheckOutSuccess") as string;

                // 2. KIỂM TRA: Nếu rỗng (do F5, hoặc vào trực tiếp)
                if (string.IsNullOrEmpty(donMuaJson))
                {
                    // Chuyển về trang chủ
                    return RedirectToAction("Index", "Home");
                }

                // 3. Deserialize model (lúc này đã có GioHangs)
                var donMua = JsonConvert.DeserializeObject<DonMuaSuccessViewModel>(donMuaJson);

                var temp = HttpContext.Session.GetString("LoginInfor");
                if (temp != null && donMua != null)
                {
                    var loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(temp);
                    loginInfor.DiemTich -= donMua.DiemSuDung;
                    HttpContext.Session.SetString("LoginInfor", JsonConvert.SerializeObject(loginInfor));
                }

                // 4. Trả về View với model đầy đủ
                return View(donMua);
            }
            catch
            {
                // 5. Nếu có bất kỳ lỗi nào, cũng về trang chủ
                return RedirectToAction("Index", "Home");
            }
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
                                if (voucher.SoTienCan < tongTien)
                                {
                                    return Json(new { HinhThuc = voucher.HinhThucGiamGia, GiaTri = voucher.GiaTri, TrangThai = true });
                                }
                                else
                                {
                                    return Json(new { Loi = "Voucher chưa đạt đủ điều kiện: Tổng tiền sản phẩm lớn hơn " + voucher.SoTienCan.ToString("n0") + " VNĐ", TrangThai = false });
                                }
                            }
                            else
                            {
                                return Json(new { Loi = "Voucher đã sử dụng hết", TrangThai = false });
                            }
                        }
                        else
                        {
                            return Json(new { Loi = "Mã voucher hết hạn", TrangThai = false });
                        }
                    }
                    else
                    {
                        return Json(new { Loi = "Không tìm thấy voucher", TrangThai = false });
                    }
                }
                else return Json(new { HinhThuc = false, GiaTri = 0 });
            }
            catch
            {
                return Json(new { HinhThuc = false, GiaTri = 0 });
            }
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
            catch
            {
                return Json(new { TrangThai = false });
            }
        }
        #endregion

        #region Other
        public IActionResult BlogDetails()
        {
            return View();
        }
        public IActionResult Contacts()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        #endregion
    }
}