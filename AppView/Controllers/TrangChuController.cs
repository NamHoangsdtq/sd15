using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AppView.Controllers
{
    public class TrangChuController : Controller
    {
        private readonly HttpClient _httpClient;
        public TrangChuController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
        }
        public async Task<IActionResult> Index()
        {
            // lam start
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


                        // lam end

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


                        // lam them
                        int cout = temp.GioHangs.Sum(c => c.SoLuong);

                        TempData["SoLuong"] = cout.ToString();
                        // lam end
                        TempData["TrangThai"] = "true";
                        return View(cout);
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
        [HttpGet("/TrangChu/FilterSPHome/{index}")]
        public async Task<IActionResult> FilterSPHome(string index)
        {

            var lstsp = await _httpClient.GetFromJsonAsync<List<HomeProductViewModel>>("SanPham/getAllSPTrangChu");
            lstsp = lstsp.Where(c => c.GiaGoc != 0).ToList();
            // Lấy sản phẩm mới nhất
            var loai = Convert.ToInt32(index);
            if(loai == 1)
            {
                lstsp = lstsp.OrderByDescending(c => c.NgayTao).Take(8).ToList();
            }else if(loai == 2) // Lấy sản phẩm bán chạy
            {
                lstsp = lstsp.OrderByDescending(c=> c.SLBan).Take(8).ToList();
            }
            else // Sp có điểm đánh giá cao nhất
            {
                lstsp = lstsp.OrderByDescending(c => c.SoSao).Take(8).ToList();
            }
            return Json( new { data = lstsp});
        }
    }
}
