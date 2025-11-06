using AppData.ViewModels;
using AppData.ViewModels.ThongKe;
using AppView.PhanTrang;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Web.WebPages;

namespace AppView.Controllers
{
    public class ThongKeController : Controller
    {
        private readonly HttpClient _httpClient;
        public ThongKeController()
        {
            _httpClient = new HttpClient();
        }
      
        #region Thống Kê Sản Phẩm Được Mua nhiều Theo Ngày, Tháng, Năm 
        [HttpGet]

        public async Task<IActionResult> ThongKeSP()
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeMSSanPhamBan";

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeMSSanPhamTheoSoLuong>>(apiData);

            return View(ChatLieus);
        }
        [HttpGet]
        public async Task<IActionResult> ThongKeSPTheoThang(DateTime? ngay, DateTime? thang, DateTime? nam)
        {

            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeMSSanPhamBan";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeMSSanPhamTheoSoLuong>>(apiData);
            var timkiem = ChatLieus.ToList();

            if (ngay.HasValue)
            {
                timkiem = ChatLieus.Where(x => x.Ngay.Date == ngay.Value.Date && x.Ngay.Month == ngay.Value.Month && x.Ngay.Year == ngay.Value.Year).ToList();
            }
            if (thang.HasValue)
            {
                timkiem = ChatLieus.Where(x => x.Ngay.Month == thang.Value.Month && x.Ngay.Year == thang.Value.Year).ToList();
            }
            if (nam.HasValue)
            {
                timkiem = ChatLieus.Where(x => x.Ngay.Year == nam.Value.Year).ToList();
            }

            return View("ThongKeSP", timkiem);
        }
        #endregion
        #region Thông Kê Khách Hàng Mua Nhiêù Theo Ngày, Tháng , Năm
        public int PageSize = 8;
        [HttpGet]

        public async Task<IActionResult> ThongKeKH(int ProductPage = 1)
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeKHTheoSoLuongHoaDon";

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeKHMuaNhieu>>(apiData);

            return View(new PhanTrangThongKeKH
            {
                listkhs = ChatLieus
                        .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = ChatLieus.Count()
                }

            }
                );
        }
        [HttpGet]
        public async Task<IActionResult> ThongKeSPKHTheoThang(DateTime ThangStart, DateTime ThangEnd, int ProductPage = 1)
        {

            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeKHTheoSoLuongHoaDon";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeKHMuaNhieu>>(apiData);
            var timkiem = ChatLieus.ToList();

            timkiem = timkiem.Where(x => x.Ngay.Month >= ThangStart.Month && x.Ngay.Month <= ThangEnd.Month).ToList();

            return View("ThongKeKH",new PhanTrangThongKeKH
            {
                listkhs = timkiem
                         .Skip((ProductPage - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = ProductPage,
                    TotalItems = timkiem.Count()
                }

            }
                 );
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLSTDByIDKH(Guid id,int ProductPage = 1)
        {
            //
            string apiURL = $"https://localhost:7095/api/LichSuTichDiem/GetLSTDByIdKH?idkh={id}";
            var response = await _httpClient.GetAsync(apiURL);
            var apiData = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<LichSuTichDiemView>>(apiData);
            return View(new PhanTrangLSTD
            {
                listLSTDs = roles
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
        #endregion
        #region Thông Kê Doanh Thu  Theo Ngày, Tháng , Năm
        [HttpGet]
        public async Task<IActionResult> ThongKeDoanhThuTheoNgay()
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeDoanhThuTheoNgay";

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeDoanhThu>>(apiData);

            return View(ChatLieus);
        }
        [HttpGet]
        public async Task<IActionResult> LocThongKeDoanhThuTheoNgay(DateTime NgayStart,DateTime NgayEnd)
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeDoanhThuTheoNgay";

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeDoanhThu>>(apiData);
            var timkiem = ChatLieus.ToList();
            timkiem=timkiem.Where(x=>x.Ngay.Date>=NgayStart.Date&&x.Ngay.Date<=NgayEnd.Date).ToList();
            return View("ThongKeDoanhThuTheoNgay", timkiem);
        }
        [HttpGet]
        public async Task<IActionResult> ThongKeDoanhThuTheoThang()
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeDoanhThuTheoThang";

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeDoanhThu>>(apiData);

            return View(ChatLieus);
        }
        [HttpGet]
        public async Task<IActionResult> LocThongKeDoanhThuTheoThang(DateTime NgayStart, DateTime NgayEnd)
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeDoanhThuTheoThang";

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeDoanhThu>>(apiData);
            var timkiem = ChatLieus.ToList();
            timkiem = timkiem.Where(x => x.Ngay.Month >= NgayStart.Month && x.Ngay.Month<= NgayEnd.Month).ToList();

            return View("ThongKeDoanhThuTheoThang", timkiem);
        }
        [HttpGet]
        public async Task<IActionResult> ThongKeDoanhThuTheoNam()
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeDoanhThuTheoNam";

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeDoanhThu>>(apiData);

            return View(ChatLieus);
        }
        [HttpGet]
        public async Task<IActionResult> LocThongKeDoanhThuTheoNam(DateTime NgayStart, DateTime NgayEnd)
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeDoanhThuTheoNam";

            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeDoanhThu>>(apiData);
            var timkiem = ChatLieus.ToList();
            timkiem = timkiem.Where(x => x.Ngay.Year >= NgayStart.Year && x.Ngay.Year <= NgayEnd.Year).ToList();
            return View("ThongKeDoanhThuTheoNam", timkiem);
        }
        #endregion
        #region Thống Kê Số liệu 
        [HttpGet]
        public async Task<IActionResult> ThongKe()
        {
            string apiUrl = $"https://localhost:7095/api/ThongKeView/ThongKeSLCTSPBan";
            var response = await _httpClient.GetAsync(apiUrl);
            string apiData = await response.Content.ReadAsStringAsync();
            var ThongKeSLSPBan = JsonConvert.DeserializeObject<ThongKeSLSPDaBan>(apiData);
            if (ThongKeSLSPBan != null)
            {
                ViewBag.ThongKeSLSPDaBan = ThongKeSLSPBan;
            }

            string apiUrl8 = $"https://localhost:7095/api/ThongKeView/ThongKeSLCTSPBanOffline";
            var response8 = await _httpClient.GetAsync(apiUrl8);
            string apiData8 = await response8.Content.ReadAsStringAsync();
            var ThongKeSLSPBanOffline = JsonConvert.DeserializeObject<ThongKeSLSPDaBan>(apiData8);
            if (ThongKeSLSPBanOffline != null)
            {
                ViewBag.ThongKeSLSPDaBanOffline = ThongKeSLSPBanOffline;
            }


            string apiUrl1 = $"https://localhost:7095/api/ThongKeView/ThongKeSLCTSP";

            var response1 = await _httpClient.GetAsync(apiUrl1);
            string apiData1 = await response1.Content.ReadAsStringAsync();
            var ThongKeSLCTSP = JsonConvert.DeserializeObject<int>(apiData1);
            if (ThongKeSLCTSP != null)
            {
                ViewBag.ThongKeSLCTSP = ThongKeSLCTSP;
            }


            string apiUrl2 = $"https://localhost:7095/api/ThongKeView/ThongKeTongDTTrongThang";
            var response2 = await _httpClient.GetAsync(apiUrl2);
            string apiData2 = await response2.Content.ReadAsStringAsync();
            var ThongKeDTTrongThang = JsonConvert.DeserializeObject<ThongKeDTTrongThang>(apiData2);
            if (ThongKeDTTrongThang != null)
            {
                ViewBag.ThongKeDTTrongThang = ThongKeDTTrongThang;
            }

            string apiUrl9 = $"https://localhost:7095/api/ThongKeView/ThongKeTongDTTrongThangOffline";
            var response9 = await _httpClient.GetAsync(apiUrl9);
            string apiData9 = await response9.Content.ReadAsStringAsync();
            var ThongKeDTTrongThangOffline = JsonConvert.DeserializeObject<ThongKeDTTrongThang>(apiData9);
            if (ThongKeDTTrongThangOffline != null)
            {
                ViewBag.ThongKeDTTrongThangOffline = ThongKeDTTrongThangOffline;
            }


            string apiUrl3 = $"https://localhost:7095/api/ThongKeView/ThongKeSoDonTrongThang";
            var response3 = await _httpClient.GetAsync(apiUrl3);
            string apiData3 = await response3.Content.ReadAsStringAsync();
            var ThongKeSDonTrongThang = JsonConvert.DeserializeObject<ThongKeSDonTrongThang>(apiData3);
            if (ThongKeSDonTrongThang != null)
            {
                ViewBag.ThongKeSDonTrongThang = ThongKeSDonTrongThang;
            }

            string apiUrl10 = $"https://localhost:7095/api/ThongKeView/ThongKeSoDonTrongThangOffline";
            var response10 = await _httpClient.GetAsync(apiUrl10);
            string apiData10 = await response10.Content.ReadAsStringAsync();
            var ThongKeSDonTrongThangOffline = JsonConvert.DeserializeObject<ThongKeSDonTrongThang>(apiData10);
            if (ThongKeSDonTrongThangOffline != null)
            {
                ViewBag.ThongKeSDonTrongThangOffline = ThongKeSDonTrongThangOffline;
            }


            string apiUrl4 = $"https://localhost:7095/api/ThongKeView/ThongKeKHTheoSoLuongHoaDon";
            var response4 = await _httpClient.GetAsync(apiUrl4);
            string apiData4 = await response4.Content.ReadAsStringAsync();
            var ChatLieus = JsonConvert.DeserializeObject<List<ThongKeKHMuaNhieu>>(apiData4);
            var timkiem = ChatLieus.ToList();

            timkiem = timkiem.Where(x => x.Ngay.Month == DateTime.Now.Month).Take(7).ToList();
            ViewBag.ThongKeKHMuaNhieu = timkiem;

            string apiUrl5 = $"https://localhost:7095/api/ThongKeView/ThongKeDoanhThuTheoNgay";

            var response5 = await _httpClient.GetAsync(apiUrl5);
            string apiData5 = await response5.Content.ReadAsStringAsync();
            var ThongKeDoanhThuTheoNgay = JsonConvert.DeserializeObject<List<ThongKeDoanhThu>>(apiData5);
            ViewBag.ThongKeDoanhThuTheoNgay = ThongKeDoanhThuTheoNgay;

            string apiUrl11 = $"https://localhost:7095/api/ThongKeView/ThongKeDoanhThuTheoNgayOffline";

            var response11 = await _httpClient.GetAsync(apiUrl11);
            string apiData11 = await response11.Content.ReadAsStringAsync();
            var ThongKeDoanhThuTheoNgayOffline = JsonConvert.DeserializeObject<List<ThongKeDoanhThu>>(apiData11);
            ViewBag.ThongKeDoanhThuTheoNgayOffline = ThongKeDoanhThuTheoNgayOffline;

            string apiUrl6 = $"https://localhost:7095/api/ThongKeView/ThongKeDoanhThuTheoThang";

            var response6 = await _httpClient.GetAsync(apiUrl6);
            string apiData6 = await response6.Content.ReadAsStringAsync();
            var ThongKeDoanhThuTheoThang = JsonConvert.DeserializeObject<List<ThongKeDoanhThu>>(apiData6);
            ViewBag.ThongKeDoanhThuTheoThang = ThongKeDoanhThuTheoThang;

            string apiUrl12 = $"https://localhost:7095/api/ThongKeView/ThongKeDoanhThuTheoThangOffline";

            var response12 = await _httpClient.GetAsync(apiUrl12);
            string apiData12 = await response12.Content.ReadAsStringAsync();
            var ThongKeDoanhThuTheoThangOffline = JsonConvert.DeserializeObject<List<ThongKeDoanhThu>>(apiData12);
            ViewBag.ThongKeDoanhThuTheoThangOffline = ThongKeDoanhThuTheoThangOffline;

            string apiUrl7 = $"https://localhost:7095/api/ThongKeView/ThongKeMSSanPhamBan";

            var response7 = await _httpClient.GetAsync(apiUrl7);
            string apiData7 = await response7.Content.ReadAsStringAsync();
            var ThongKeMSSanPhamTheoSoLuong = JsonConvert.DeserializeObject<List<ThongKeMSSanPhamTheoSoLuong>>(apiData7);
            ViewBag.ThongKeMSSanPhamTheoSoLuong = ThongKeMSSanPhamTheoSoLuong;

            return View();
        }

        #endregion

        //Tam
        [HttpGet]
        public async Task<IActionResult> ThongKeAdmin(string startDate, string endDate)
        {
            try
            {
                if (startDate == null || endDate == null)
                {
                    startDate = DateTime.Now.AddDays(-7).ToString();
                    endDate = DateTime.Now.ToString();
                }
                var response = await _httpClient.GetAsync("https://localhost:7095/api/ThongKe/ThongKe?startDate=" + startDate + "&endDate=" + endDate);
                var lst = JsonConvert.DeserializeObject<ThongKeViewModel>(response.Content.ReadAsStringAsync().Result);
                return View(lst);
            }
            catch
            {
                return View(new ThongKeViewModel());
            }
        }
        public async Task<FileResult> ExportExcel()
        {
            var response = await _httpClient.GetAsync("https://localhost:7095/api/ThongKe/ThongKeSanPham");
            var lst = JsonConvert.DeserializeObject<List<ThongKeSanPham>>(await response.Content.ReadAsStringAsync());
            var fileName = "thongKeSanPham.xlsx";
            DataTable dataTable = new DataTable("SanPham");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Tên sản phẩm"),
                new DataColumn("Số lượng bán ra"),
                new DataColumn("Tổng doanh thu")
            });
            foreach(var item in lst)
            {
                dataTable.Rows.Add(item.TenSP, item.SoLuong,item.DoanhThu);
            }
            using(XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using(MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",fileName);
                }
            }
        }
        //End
    }
}
