using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AppData.Models; // Namespace model của bạn

namespace AppView.Controllers
{
    public class OrderTrackingController : Controller
    {
        private readonly HttpClient _httpClient;

        public OrderTrackingController() // Không inject DbContext
        {
            _httpClient = new HttpClient();
            // Đảm bảo địa chỉ API này là chính xác
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
        }

        // Route: /theo-doi/{token}
        [HttpGet("theo-doi/{token}")]
        public async Task<IActionResult> Track(Guid token)
        {
            if (token == Guid.Empty)
            {
                // Tạo 1 View tên là OrderNotFound.cshtml để báo lỗi
                return View("OrderNotFound");
            }

            // === GỌI API BẠN ĐÃ TẠO BÊN AppAPI ===
            var response = await _httpClient.GetAsync($"HoaDon/GetByToken/{token}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();

                // Deserialize object HoaDon (đã bao gồm ChiTietHoaDon)
                var order = JsonConvert.DeserializeObject<HoaDon>(json);

                // Trả về View "OrderStatusView" với model là Hóa Đơn
                return View("OrderStatusView", order);
            }
            else
            {
                // API trả về lỗi (NotFound, BadRequest...)
                return View("OrderNotFound");
            }
        }
    }
}