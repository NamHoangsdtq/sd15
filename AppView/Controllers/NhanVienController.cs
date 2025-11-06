using AppData.Models;
using AppData.ViewModels;
using AppView.PhanTrang;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace AppView.Controllers
{
    public class NhanVienController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly AssignmentDBContext dBContext;
        public NhanVienController()
        {
            _httpClient = new HttpClient();
            dBContext = new AssignmentDBContext();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
        }
        public int PageSize = 8;

        public async Task<IActionResult> Show(int ProductPage = 1)
        {
            try
            {
                string apiUrl = $"https://localhost:7095/api/NhanVien/GetAll";
                var response = await _httpClient.GetAsync(apiUrl);
                string apiData = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<NhanVien>>(apiData);
                return View(new PhanTrangNhanVien
                {
                    listNv = users
                            .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        ItemsPerPage = PageSize,
                        CurrentPage = ProductPage,
                        TotalItems = users.Count()
                    }
                });
            }
            catch
            {
                return Redirect("https://localhost:5001/");
            }
        }

        [HttpGet]
        public async Task<IActionResult> SearchTheoTen(string? Ten, int ProductPage = 1)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Ten))
                {
                    ViewData["SearchError"] = "Vui lòng nhập tên để tìm kiếm";
                    return RedirectToAction("Show");
                }
                string apiUrl = $"https://localhost:7095/api/NhanVien/TimKiemNhanVien?name={Ten}";
                var response = await _httpClient.GetAsync(apiUrl);
                string apiData = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<NhanVien>>(apiData);
                if (users.Count == 0)
                {
                    ViewData["SearchError"] = "Không tìm thấy kết quả phù hợp";
                }
                return View("Show", new PhanTrangNhanVien
                {
                    listNv = users
                             .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        ItemsPerPage = PageSize,
                        CurrentPage = ProductPage,
                        TotalItems = users.Count()
                    }
                });
            }
            catch { return Redirect("https://localhost:5001/"); }
        }

        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NhanVien nhanVien)
        {
            try
            {
                nhanVien.TrangThai = 1;
                var vt = dBContext.VaiTros.FirstOrDefault(x => x.Ten == "Nhân viên");
                string apiUrl = $"https://localhost:7095/api/NhanVien/DangKyNhanVien?ten={nhanVien.Ten}&email={nhanVien.Email}&password={nhanVien.PassWord}&sdt={nhanVien.SDT}&diachi={nhanVien.DiaChi}";
                var reponsen = await _httpClient.PostAsync(apiUrl, null);
                if (reponsen.IsSuccessStatusCode)
                {
                    return RedirectToAction("Show");
                }
                else if (reponsen.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.ErrorMessage = "Email hoặc sdt này đã được đăng ký";
                    return View();
                }

                return View(nhanVien);
            }
            catch { return Redirect("https://localhost:5001/"); }

        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                string apiUrl = $"https://localhost:7095/api/NhanVien/GetById?id={id}";
                var response = await _httpClient.GetAsync(apiUrl);
                string apiData = await response.Content.ReadAsStringAsync();

                var user = JsonConvert.DeserializeObject<NhanVien>(apiData);
                var vt = await dBContext.VaiTros.FindAsync(user.IDVaiTro);
                ViewBag.TenVaiTro = vt.Ten;
                return View(user);
            }
            catch { return Redirect("https://localhost:5001/"); }
        }

        public IActionResult Edit(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/NhanVien/GetById?id={id}";
            var response = _httpClient.GetAsync(apiUrl).Result;
            var apiData = response.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<NhanVien>(apiData);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, NhanVien nv)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string apiUrl = $"https://localhost:7095/api/NhanVien/{id}?ten={nv.Ten}&email={nv.Email}&password={nv.PassWord}&sdt={nv.SDT}&diachi={nv.DiaChi}&trangthai={nv.TrangThai}&idvaitro={nv.IDVaiTro}";

                    var reponsen = await _httpClient.PutAsync(apiUrl, null);
                    if (reponsen.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Show");
                    }
                }
                return View(nv);

            }
            catch { return Redirect("https://localhost:5001/"); }
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/NhanVien/{id}";
            var reposen = await _httpClient.DeleteAsync(apiUrl);
            if (reposen.IsSuccessStatusCode)
            {
                return RedirectToAction("Show");
            }
            return RedirectToAction("Show");
        }
        public async Task<IActionResult> Sua(Guid id)
        {
            try
            {
                var timkiem = dBContext.NhanViens.FirstOrDefault(x => x.ID == id);
                if (timkiem != null)
                {
                    timkiem.TrangThai = timkiem.TrangThai == 1 ? 0 : 1;
                    dBContext.NhanViens.Update(timkiem);
                    dBContext.SaveChanges();
                    return RedirectToAction("Show");
                }
                else
                {
                    return View();
                }
            }
            catch { return Redirect("https://localhost:5001/"); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Next(int ProductPage = 1)
        {
            ProductPage++;
            return await Show(ProductPage);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Previous(int ProductPage = 1)
        {
            ProductPage--;
            return await Show(ProductPage);
        }
        [HttpGet]
        public IActionResult ProfileNhanVien_Admin()
        {
            try
            {
                var session = HttpContext.Session.GetString("LoginInfor");
                LoginViewModel loginViewModel = JsonConvert.DeserializeObject<LoginViewModel>(session);
                return View(loginViewModel);
            }
            catch (Exception)
            {
                return Redirect("https://localhost:5001/");
            }
            
        }
        [HttpPut]
        public ActionResult UpdateProfile(string ten, string email, string sdt, string? diachi)
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
                khachhang.DiaChi = diachi;
                khachhang.DiemTich = JsonConvert.DeserializeObject<LoginViewModel>(session).DiemTich;
                khachhang.vaiTro = JsonConvert.DeserializeObject<LoginViewModel>(session).vaiTro;
                khachhang.IsAccountLocked = JsonConvert.DeserializeObject<LoginViewModel>(session).IsAccountLocked;
                khachhang.Message = "lmao";
                var response = _httpClient.PutAsJsonAsync("https://localhost:7095/api/" + "QuanLyNguoiDung/UpdateProfile1", khachhang).Result;
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
        public IActionResult ChangePassword()
        {
            return PartialView("ChangePassword");
        }
    }
}
