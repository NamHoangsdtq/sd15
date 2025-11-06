using AppData.Models;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using AppView.PhanTrang;
using AppData.ViewModels;
using DocumentFormat.OpenXml.InkML;
using System.Net;

namespace AppView.Controllers
{
    public class LoaiSPController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly AssignmentDBContext dBContext;
        public LoaiSPController()
        {
            _httpClient = new HttpClient();
            dBContext = new AssignmentDBContext();
        }
        public int PageSize = 8;
        // laam them 
        public async Task<IActionResult> Show(int ProductPage = 1)
        {
            try
            {
                string apiUrl = "https://localhost:7095/api/LoaiSP/getAll";
                var response = await _httpClient.GetAsync(apiUrl);
                string apiData = await response.Content.ReadAsStringAsync();
                var LoaiSPs = JsonConvert.DeserializeObject<List<LoaiSP>>(apiData);
                var filteredLoaiSPs = LoaiSPs.Where(lsp => lsp.IDLoaiSPCha == null).ToList();
                return View(new PhanTrangLoaiSP
                {
                    listlsp = filteredLoaiSPs.Skip((ProductPage - 1) * PageSize).Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        ItemsPerPage = PageSize,
                        CurrentPage = ProductPage,
                        TotalItems = filteredLoaiSPs.Count()
                    }
                });
            }
            catch (Exception)
            {

                throw;
            }
        }
        // Tim kiem Loai SP theo ten
        [HttpGet]
        public async Task<IActionResult> TimKiemLoaiSPTheoTen(string? ten, int ProductPage = 1)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ten))
                {
                    ViewData["SearchError"] = "Vui lòng nhập tên để tìm kiếm";
                    return RedirectToAction("Show");
                }
                string apiUrl = $"https://localhost:7095/api/LoaiSP/TimKiemLoaiSP?name={ten}";
                var response = await _httpClient.GetAsync(apiUrl);
                string apiData = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<LoaiSP>>(apiData);
                if (users.Count == 0)
                {
                    ViewData["SearchError"] = "Không tìm thấy kết quả phù hợp";
                }
                return View("Show", new PhanTrangLoaiSP
                {
                    listlsp = users
                             .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        ItemsPerPage = PageSize,
                        CurrentPage = ProductPage,
                        TotalItems = users.Count()
                    }
                });
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IActionResult> Create()
        {
            try
            {
                var responseLoaiSP = _httpClient.GetAsync(_httpClient.BaseAddress + $"https://localhost:7095/api/LoaiSP/getAll").Result;
                if (responseLoaiSP.IsSuccessStatusCode)
                {
                    ViewData["listLoaiSP"] = JsonConvert.DeserializeObject<List<LoaiSP>>(responseLoaiSP.Content.ReadAsStringAsync().Result);
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create(LoaiSPRequest lsp)
        {
            try
            {
                lsp.ID = Guid.NewGuid();
                lsp.TrangThai = 1;
                string apiURL = $"https://localhost:7095/api/LoaiSP/save";
                var content = new StringContent(JsonConvert.SerializeObject(lsp), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(apiURL, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Show");
                }
                ViewBag.ErrorMessage = "Loại sản phẩm này đã có trong danh sách";
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IActionResult> Details(Guid id)
        {

            string apiUrl = $"https://localhost:7095/api/LoaiSP/getById/{id}";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var LoaiSPs = JsonConvert.DeserializeObject<LoaiSP>(apiData);
            return View(LoaiSPs);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                string apiUrl = $"https://localhost:7095/api/LoaiSP/getById/{id}";
                var response = await _httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest();
                }

                var lspJson = await response.Content.ReadAsStringAsync();
                var lsp = JsonConvert.DeserializeObject<LoaiSP>(lspJson); // sử dụng LoaiSP thay vì LoaiSPRequest

                return View(lsp);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LoaiSP lsp)
        {
            try
            {
                string apiUrl = $"https://localhost:7095/api/LoaiSP/save";
                var content = new StringContent(JsonConvert.SerializeObject(lsp), Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Show");
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.ErrorMessage = "Loại sản phẩm này đã có trong danh sách";
                    return View();
                }
                else
                {
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLoaiSpById(Guid id, int ProductPage = 1)
        {
            // list loai san pham con
            try
            {
                string apiUrl = $"https://localhost:7095/api/LoaiSP?id={id}";
                var response = await _httpClient.GetAsync(apiUrl);
                string apiData = await response.Content.ReadAsStringAsync();
                var LoaiSPs = JsonConvert.DeserializeObject<List<LoaiSP>>(apiData);
                return View(new PhanTrangLoaiSP
                {
                    listlsp = LoaiSPs.Where(x => x.TrangThai == 1)
                            .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                    PagingInfo = new PagingInfo
                    {
                        ItemsPerPage = PageSize,
                        CurrentPage = ProductPage,
                        TotalItems = LoaiSPs.Count()
                    }
                });
            }
            catch (Exception) { throw; }
        }

        [HttpGet]

        public async Task<IActionResult> EditLoaiSPCon(Guid id)
        {
            try
            {
                string apiUrl = $"https://localhost:7095/api/LoaiSP/getById/{id}";
                var response = await _httpClient.GetAsync(apiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest();
                }
                var lspJson = await response.Content.ReadAsStringAsync();
                var lsp = JsonConvert.DeserializeObject<LoaiSP>(lspJson);
                return View(lsp);
            }
            catch (Exception) { throw; }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLoaiSPCon(LoaiSP lsp)
        {
            try
            {
                lsp.TrangThai = 1;
                string apiUrl = $"https://localhost:7095/api/LoaiSP/save";
                var content = new StringContent(JsonConvert.SerializeObject(lsp), Encoding.UTF8, "application/json");
                var reponsen = await _httpClient.PutAsync(apiUrl, content);
                if (reponsen.IsSuccessStatusCode)
                {
                    return RedirectToAction("GetLoaiSpById", "LoaiSP", new { id = lsp.IDLoaiSPCha });
                }
                else if (reponsen.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.ErrorMessage = "Loại sản phẩm này đã có trong danh sách";
                    return View();
                }
                return RedirectToAction("GetLoaiSpById", "LoaiSP", new { id = lsp.IDLoaiSPCha });
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<IActionResult> CreateLoaiSPCon()
        {
            var responseLoaiSP = _httpClient.GetAsync(_httpClient.BaseAddress + $"https://localhost:7095/api/LoaiSP/getAll").Result;
            if (responseLoaiSP.IsSuccessStatusCode)
            {
                ViewData["listLoaiSP"] = JsonConvert.DeserializeObject<List<LoaiSP>>(responseLoaiSP.Content.ReadAsStringAsync().Result);
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateLoaiSPCon(LoaiSP lsp)
        {
            lsp.TrangThai = 1;
            string apiURL = $"https://localhost:7095/api/LoaiSP/save";
            var content = new StringContent(JsonConvert.SerializeObject(lsp), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetLoaiSpById");
            }
            return View();
        }
        public async Task<IActionResult> Sua(Guid id)
        {
            try
            {
                var timkiem = dBContext.LoaiSPs.FirstOrDefault(x => x.ID == id);
                if (timkiem != null)
                {
                    timkiem.TrangThai = timkiem.TrangThai == 0 ? 1 : 0;
                    dBContext.LoaiSPs.Update(timkiem);
                    dBContext.SaveChanges();
                    return RedirectToAction("Show");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception) { throw; }
        }
    }
}
