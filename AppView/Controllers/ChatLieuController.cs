using AppData.Models;
using AppData.ViewModels;
using AppView.PhanTrang;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;

namespace AppView.Controllers
{
    public class ChatLieuController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly AssignmentDBContext dBContext;
        public ChatLieuController()
        {
            _httpClient = new HttpClient();
            dBContext = new AssignmentDBContext();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
        }
        // GET: ChatLieuController
        public int PageSize = 8;

        public async Task<IActionResult> Show(int ProductPage = 1)
        {
            try
            {
                string apiUrl = $"https://localhost:7095/api/ChatLieu/GetAllChatLieu";
                var response = await _httpClient.GetAsync(apiUrl);
                string apiData = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<ChatLieu>>(apiData);
                return View(new PhanTrangChatLieu
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
                string apiUrl = $"https://localhost:7095/api/ChatLieu/TimKiemChatLieu?name={Ten}";
                var response = await _httpClient.GetAsync(apiUrl);
                string apiData = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<ChatLieu>>(apiData);
                if (users.Count == 0)
                {
                    ViewData["SearchError"] = "Không tìm thấy kết quả phù hợp";
                }
                return View("Show", new PhanTrangChatLieu
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
        public async Task<IActionResult> Create(ChatLieu mauSac)
        {

            try
            {
                mauSac.TrangThai = 1;
                string apiUrl = $"https://localhost:7095/api/ChatLieu/ThemChatLieu?ten={mauSac.Ten}";
                var reponsen = await _httpClient.PostAsync(apiUrl, null);
                if (reponsen.IsSuccessStatusCode)
                {
                    return RedirectToAction("Show");
                }
                else if (reponsen.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.ErrorMessage = "Chất liệu này đã có trong danh sách";
                    return View();
                }
                return View(mauSac);
            }
            catch
            {
                return Redirect("https://localhost:5001/");
            }

        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/ChatLieu/GetChatLieuById?id={id}";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();

            var user = JsonConvert.DeserializeObject<ChatLieu>(apiData);
            return View(user);
        }
        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/ChatLieu/GetChatLieuById?id={id}";
            var response = _httpClient.GetAsync(apiUrl).Result;
            var apiData = response.Content.ReadAsStringAsync().Result;
            var user = JsonConvert.DeserializeObject<ChatLieu>(apiData);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, ChatLieu nv)
        {
            try
            {
                nv.TrangThai = 1;
                string apiUrl = $"https://localhost:7095/api/ChatLieu/{id}?ten={nv.Ten}";
                var content = new StringContent(JsonConvert.SerializeObject(nv), Encoding.UTF8, "application/json");
                var reponsen = await _httpClient.PutAsync(apiUrl, content);

                if (reponsen.IsSuccessStatusCode)
                {
                    return RedirectToAction("Show");
                }
                else if (reponsen.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.ErrorMessage = "Chất liệu này đã có trong danh sách";
                    return View();
                }
                return View(nv);
            }
            catch { return Redirect("https://localhost:5001/"); }

        }
        public async Task<IActionResult> Delete(Guid id)
        {
            string apiUrl = $"https://localhost:7095/api/ChatLieu/{id}";
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
                var timkiem = dBContext.ChatLieus.FirstOrDefault(x => x.ID == id);
                if (timkiem != null)
                {
                    timkiem.TrangThai = timkiem.TrangThai == 0 ? 1 : 0;
                    dBContext.ChatLieus.Update(timkiem);
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
