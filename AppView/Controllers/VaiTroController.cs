using AppData.Models;
using AppView.PhanTrang;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Text;

namespace AppView.Controllers
{
    public class VaiTroController : Controller
    {
        private HttpClient _httpClient;
        public VaiTroController()
        {
                _httpClient = new HttpClient();
        }
        public int PageSize = 8;
         
        // lam them
        public async Task<IActionResult> GetAllVaiTro(int ProductPage = 1)
        {
            string apiURL = $"https://localhost:7095/api/VaiTro";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<VaiTro>>(apiData);
            return View(new PhanTrangVaiTro
            {
                listvts = roles
                        .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = roles.Count()
                }

            }
                );

        }
        // Tim kiem Ten Vai tro
        [HttpGet]
        public async Task<IActionResult> TimKiemVTTheoTen(string Ten,int ProductPage = 1)
        {
            string apiURL = $"https://localhost:7095/api/VaiTro";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<VaiTro>>(apiData);
            return View("GetAllVaiTro", new PhanTrangVaiTro
            {
                listvts = roles.Where(x=>x.Ten.Contains(Ten))
                        .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = roles.Count()
                }

            }
                );

        }


        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            string apiURL = $"https://localhost:7095/api/VaiTro/{Id}";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<VaiTro>(apiData);
            return View(roles);
        }

 
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VaiTro vaiTro)
        {
            string apiURL = $"https://localhost:7095/api/VaiTro?ten={vaiTro.Ten}&Status={vaiTro.TrangThai = 1}";
            var content = new StringContent(JsonConvert.SerializeObject(vaiTro), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(apiURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllVaiTro");
            }
            return View();
        }

        public async Task<IActionResult> Edit(Guid Id)
        {
            string apiURL = $"https://localhost:7095/api/VaiTro/{Id}";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<VaiTro>(apiData);
            return View(roles);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid Id, VaiTro vaiTro)
        {
            string apiURL = $"https://localhost:7095/api/VaiTro/{Id}?ten={vaiTro.Ten}&trnagthai={vaiTro.TrangThai}";
            var content = new StringContent(JsonConvert.SerializeObject(vaiTro), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(apiURL, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllVaiTro");
            }
            return View();
        }
        
    }
}
