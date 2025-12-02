using AppData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq; // nhớ thêm cái này

namespace AppView.Controllers
{
    public class GopYController : Controller
    {
        private readonly AssignmentDBContext _context;
        private readonly ILogger<GopYController> _logger;

        public GopYController(ILogger<GopYController> logger)
        {
            _logger = logger;

            // KHỞI TẠO DB CONTEXT (tự new, không qua Program)
            _context = new AssignmentDBContext();
        }

        public IActionResult Index()
        {
            var list = _context.GopYs
                               .OrderByDescending(x => x.NgayTao)
                               .ToList();
            return View(list);
        }

        [HttpPost]
        public IActionResult MarkAsRead(int id)
        {
            var item = _context.GopYs.Find(id);
            if (item == null) return NotFound();

            item.IsRead = true;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _context.GopYs.Find(id);
            if (item == null) return NotFound();

            _context.GopYs.Remove(item);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
