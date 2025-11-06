using AppData.Models;
using AppData.ViewModels;
using AppView.PhanTrang;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace AppView.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly HttpClient httpClients;

        public KhachHangController()
        {
            httpClients = new HttpClient();
        }

        public int PageSize = 10;
        // Get ALl KH
        public async Task<IActionResult> GetAllKhachHang(int ProductPage = 1)
        {
            string apiUrl = "https://localhost:7095/api/KhachHang";
            var response = await httpClients.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var kh = JsonConvert.DeserializeObject<List<KhachHangView>>(apiData);
            return View(new PhanTrangKhachHang
            {
                listkh = kh
                       .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = kh.Count()
                }

            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLSTDByIDKH(Guid id)
        {
           
            string apiURL = $"https://localhost:7095/api/LichSuTichDiem/TongDonThanhCong?id={id}";
            var response = await httpClients.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var DonThanhCong = JsonConvert.DeserializeObject<TongDon?>(apiData);
            ViewBag.DonThanhCong = DonThanhCong;
            string apiURL1 = $"https://localhost:7095/api/LichSuTichDiem/TongDonHuy?id={id}";
            var response1 = await httpClients.GetAsync(apiURL1);
            var apiData1 = await response1.Content.ReadAsStringAsync();
            var DonHuy = JsonConvert.DeserializeObject<TongDon?>(apiData1);
            ViewBag.DonHuy = DonHuy;
            string apiURL2 = $"https://localhost:7095/api/LichSuTichDiem/TongDonHoanHang?id={id}";
            var response2 = await httpClients.GetAsync(apiURL2);
            var apiData2 = await response2.Content.ReadAsStringAsync();
            var DonHoanHang = JsonConvert.DeserializeObject<TongDon?>(apiData2);
            ViewBag.DonHoanHang = DonHoanHang;

            HttpContext.Session.SetString("DonKH", id.ToString());
            return View(
                );

        }
        [HttpGet]
         public  async Task<IActionResult> DonThanhCong( int ProductPage = 1)
          {
            var id = Guid.Parse(HttpContext.Session.GetString("DonKH"));
            string apiURL2 = $"https://localhost:7095/api/LichSuTichDiem/ListDonThanhCong?id={id}";
            var response2 = await httpClients.GetAsync(apiURL2);
            var apiData2 = await response2.Content.ReadAsStringAsync();
            var Don = JsonConvert.DeserializeObject<List<ListDon>>(apiData2); 
            return View(new PhanTrangDon
            {
               listdon = Don
                      .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = Don.Count()
                }

            });
        }
        [HttpGet]
        public async Task<IActionResult> DonHuy( int ProductPage = 1)
        {
            var id = Guid.Parse(HttpContext.Session.GetString("DonKH"));
            string apiURL2 = $"https://localhost:7095/api/LichSuTichDiem/ListDonHuy?id={id}";
            var response2 = await httpClients.GetAsync(apiURL2);
            var apiData2 = await response2.Content.ReadAsStringAsync();
            var Don = JsonConvert.DeserializeObject<List<ListDon>>(apiData2);
            return View(new PhanTrangDon
            {
                listdon = Don
                      .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = Don.Count()
                }

            });
        }
        [HttpGet]
        public async Task<IActionResult> DonHoanHang(int ProductPage = 1)
        {
            var id = Guid.Parse(HttpContext.Session.GetString("DonKH"));
            string apiURL2 = $"https://localhost:7095/api/LichSuTichDiem/ListDonHoanHang?id={id}";
            var response2 = await httpClients.GetAsync(apiURL2);
            var apiData2 = await response2.Content.ReadAsStringAsync();
            var Don = JsonConvert.DeserializeObject<List<ListDon>>(apiData2);
            return View(new PhanTrangDon
            {
                listdon = Don
                      .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = Don.Count()
                }

            });
        }

        // tim kiem KH theo Ten Hoac SDT
        [HttpGet]
        public async Task<IActionResult> GetAllKHTheoTimKiem(string? Ten, string? SDT,int ProductPage = 1)
        {
            string apiUrl = $"https://localhost:7095/api/KhachHang/TimKiemKH?Ten={Ten?.Trim()}&SDT={SDT?.Trim()}";
            var response = await httpClients.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var kh = JsonConvert.DeserializeObject<List<KhachHangView>>(apiData);
            return View("GetAllKhachHang", new PhanTrangKhachHang
            {
                listkh = kh
                       .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = kh.Count()
                }

            });
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Create(KhachHangView kh, string nhaplai)
        {
            try
            {
                string apiUrl1 = "https://localhost:7095/api/KhachHang";
                var response1 = await httpClients.GetAsync(apiUrl1);
                string apiData1 = await response1.Content.ReadAsStringAsync();
                var kh1 = JsonConvert.DeserializeObject<List<KhachHangView>>(apiData1);
                if (kh.Password != null || kh.Email != null)
                {
                    if (kh.Password.Length < 8)
                    {
                        ViewBag.MatKhau = "Mật Khẩu phải lớn hơn 7 kí tự";
                    }
                    var timkiem = kh1.Where(x => x.SDT == kh.SDT.Trim()).FirstOrDefault();

                    if (!kh.Email.Contains("@"))
                    {
                        ViewBag.email = kh.Email.Replace("@", "%40");
                    }
                    var email = kh1.Where(x => x.Email == kh.Email.Trim()).FirstOrDefault();
                    if (email != null)
                    {
                        ViewBag.email = "email đã tồn tại";

                    }
                    if (nhaplai != kh.Password)
                    {
                        ViewBag.NhapLai = "Nhập lại mật khẩu không đúng ";
                    }
                    if (kh.SDT != null)
                    {
                        if (kh.SDT.Length < 10)
                        {
                            ViewBag.SDT = "Số Điện thoại không hợp lệ";
                        }
                        else
                        {
                            if (timkiem != null)
                            {
                                ViewBag.SDT = "Số Điện thoại này đã được đăng kí";
                                return View();
                            }
                        }
                    }
                    if ((kh.Email.Contains("@") && email == null && nhaplai == kh.Password) || (kh.Email.Contains("@") && email == null && nhaplai == kh.Password && kh.SDT.Length >= 10 && timkiem == null))
                    {
                        var url = $"https://localhost:7095/api/KhachHang/PostKHView";
                        var response = await httpClients.PostAsJsonAsync(url, kh);
                        if (response.IsSuccessStatusCode) return RedirectToAction("GetAllKhachHang");
                        return View();
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
           
        }
        [HttpGet]

        public async Task<IActionResult> Details(Guid id)
        {

            string apiUrl = "https://localhost:7095/api/KhachHang/GetById?id=" + id;
            var response = await httpClients.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var NhaCungCaps = JsonConvert.DeserializeObject<KhachHangView>(apiData);
            return View(NhaCungCaps);

        }
        [HttpGet]
        public async Task<IActionResult> Updates(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/KhachHang/GetById?id={id}";
            var response = await httpClients.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var kh = JsonConvert.DeserializeObject<KhachHangView>(apiData);

            return View(kh);
        }
        [HttpPost]

        public async Task<IActionResult> Updates(KhachHangView kh)
        {
            var url =
         $"https://localhost:7095/api/KhachHang/PutKhView";
            var response = await httpClients.PutAsJsonAsync(url, kh);
            if (response.IsSuccessStatusCode) return RedirectToAction("GetAllKhachHang");
            return View();
        }


        public async Task<IActionResult> Delete(Guid id)
        {
            var url = $"https://localhost:7095/api/KhachHang/{id}";
            var response = await httpClients.DeleteAsync(url);
            if (response.IsSuccessStatusCode) return RedirectToAction("GetAllKhachHang");
            return BadRequest();
        }
    }
}
