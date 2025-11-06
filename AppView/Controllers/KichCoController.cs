using AppData.Models;
using AppView.PhanTrang;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace AppView.Controllers
{
    public class KichCoController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly AssignmentDBContext dBContext;
        // GET: KickCoController
        public int PageSize = 8;
        public KichCoController()
        {
            _httpClient = new HttpClient();
            dBContext = new AssignmentDBContext();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
        }

        public async Task<IActionResult> Show(int ProductPage = 1)
        {
            try
            {
                string apiUrl = $"https://localhost:7095/api/KichCo/GetAllKichCo";
                var response = await _httpClient.GetAsync(apiUrl);
                string apiData = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<KichCo>>(apiData);
                return View(new PhanTrangKichCo
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
        public async Task<IActionResult> SearchTheoTen(string? Ten, int ProductPage = 1)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Ten))
                {
                    ViewData["SearchError"] = "Vui lòng nhập tên để tìm kiếm";
                    return RedirectToAction("Show");
                }
                string apiUrl = $"https://localhost:7095/api/KichCo/TimKiemKichCo?name={Ten}";
                var response = await _httpClient.GetAsync(apiUrl);
                string apiData = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<KichCo>>(apiData);
                if (users.Count == 0)
                {
                    ViewData["SearchError"] = "Không tìm thấy kết quả phù hợp";
                }
                return View("Show", new PhanTrangKichCo
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
        public async Task<IActionResult> Create(KichCo kc)
        {
            try
            {
                kc.TrangThai = 1;
                string apiUrl = $"https://localhost:7095/api/KichCo/ThemKichCo?ten={kc.Ten}";
                var reponsen = await _httpClient.PostAsync(apiUrl, null);
                if (reponsen.IsSuccessStatusCode)
                {
                    return RedirectToAction("Show");
                }
                else if (reponsen.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.ErrorMessage = "Kích cỡ này đã có trong danh sách hoặc không khả dụng";
                    return View();
                }
                return View(kc);
            }
            catch
            {
                return Redirect("https://localhost:5001/");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/KichCo/GetKichCoById?id={id}";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<KichCo>(apiData);
            return View(user);
        }
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            try
            {
                string apiUrl = $"https://localhost:7095/api/KichCo/GetKichCoById?id={id}";
                var response = _httpClient.GetAsync(apiUrl).Result;
                var apiData = response.Content.ReadAsStringAsync().Result;
                var user = JsonConvert.DeserializeObject<KichCo>(apiData);
                return View(user);
            }
            catch
            {
                return Redirect("https://localhost:5001/");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, KichCo nv)
        {
            try
            {
                nv.TrangThai = 1;
                string apiUrl = $"https://localhost:7095/api/KichCo/{id}?ten={nv.Ten}";
                var content = new StringContent(JsonConvert.SerializeObject(nv), Encoding.UTF8, "application/json");
                var reponsen = await _httpClient.PutAsync(apiUrl, content);
                if (reponsen.IsSuccessStatusCode)
                {
                    return RedirectToAction("Show");
                }
                else if (reponsen.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.ErrorMessage = "Kích cỡ này đã có trong danh sách";
                    return View();
                }
                return View(nv);
            }
            catch
            {
                return Redirect("https://localhost:5001/");
            }
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/KichCo/{id}";
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
                var timkiemctsp = dBContext.ChiTietSanPhams.Where(x => x.IDKichCo == id).ToList();
                var timkiem = dBContext.KichCos.FirstOrDefault(x => x.ID == id);
                if (timkiem != null)
                {
                    timkiem.TrangThai = timkiem.TrangThai == 0 ? 1 : 0;
                    if (timkiem.TrangThai == 0)
                    {
                        var sanPhamDangSuDung = timkiemctsp.Where(x => x.TrangThai == 1).ToList();
                        if (sanPhamDangSuDung.Any())
                        {
                            var idSanPhamList = sanPhamDangSuDung.Select(x => x.IDSanPham).Distinct().ToList();
                            var danhSachTenSanPham = dBContext.SanPhams.Where(x => idSanPhamList.Contains(x.ID)).Select(x => x.Ten).ToList();
                            // Tạo thông báo cảnh báo
                            string thongBao = "Có sản phẩm sau đang sử dụng size này: " + string.Join(", ", danhSachTenSanPham);

                            // Trả về thông báo (ví dụ: View kèm dữ liệu hoặc sử dụng TempData)
                            TempData["ThongBao"] = thongBao;
                            return RedirectToAction("Show");
                        }
                        else
                        {
                            foreach (var item in timkiemctsp)
                            {
                                item.TrangThai = 0;
                            }
                            dBContext.ChiTietSanPhams.UpdateRange(timkiemctsp);
                        }
                    }
                    dBContext.KichCos.Update(timkiem);
                    dBContext.SaveChanges();
                    return RedirectToAction("Show");
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return Redirect("https://localhost:5001/");
            }
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
    }
}
