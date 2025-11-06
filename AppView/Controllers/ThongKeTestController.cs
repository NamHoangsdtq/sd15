using Microsoft.AspNetCore.Mvc;

namespace AppView.Controllers
{
    public class ThongKeTestController : Controller
    {
        public IActionResult Index()
        {
            // Xử lý logic để lấy dữ liệu biểu đồ từ cơ sở dữ liệu hoặc từ các nguồn khác
            var data = new { labels = new[] { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5" }, values = new[] { 10, 20, 15, 25, 30 } };

            return View(data);
        }
    }
}
