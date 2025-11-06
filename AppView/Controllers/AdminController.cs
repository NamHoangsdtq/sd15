using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using AppView.IServices;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace AppView.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IFileService _iFileService;
        public AdminController(IWebHostEnvironment hostEnvironment, IFileService iFileService)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
            _hostEnvironment = hostEnvironment;
            _iFileService = iFileService;
        }
        public IActionResult HomePageAdmin(Guid id)
        {
            return RedirectToAction("BanHang", "BanHangTaiQuay");
        }
        public IActionResult ProductManager()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ShowProduct(FilterData filter)
        {
            try
            {
                var response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllSanPhamAdmin").Result;
                List<SanPhamViewModelAdmin> lstSanpham = new List<SanPhamViewModelAdmin>();
                if (response.IsSuccessStatusCode)
                {
                    lstSanpham = JsonConvert.DeserializeObject<List<SanPhamViewModelAdmin>>(response.Content.ReadAsStringAsync().Result);
                    //Sắp xếp
                    if (filter.sortSP == "1")
                    {
                        lstSanpham = lstSanpham.OrderBy(x => Convert.ToInt32(x.Ma.Substring(2))).ToList();
                    }
                    else if (filter.sortSP == "6")
                    {
                        lstSanpham = lstSanpham.OrderBy(x => x.Ten).ToList();
                    }
                    else if (filter.sortSP == "2")
                    {
                        lstSanpham = lstSanpham.OrderBy(x => x.GiaBan).ToList();
                    }
                    else if (filter.sortSP == "3")
                    {
                        lstSanpham = lstSanpham.OrderByDescending(x => x.GiaBan).ToList();
                    }
                    else if (filter.sortSP == "4")
                    {
                        lstSanpham = lstSanpham.OrderBy(x => x.SoLuong).ToList();
                    }
                    else if (filter.sortSP == "5")
                    {
                        lstSanpham = lstSanpham.OrderByDescending(x => x.SoLuong).ToList();
                    }
                    //Tìm kiếm theo tên sản phẩm
                    if (filter.search != null)
                    {
                        lstSanpham = lstSanpham.Where(x => x.Ten.ToLower().Contains(filter.search.ToLower())).ToList();
                    }
                    //Tìm kiếm theo giá
                    if (filter.minPrice != null)
                    {
                        lstSanpham = lstSanpham.Where(x => x.GiaBan >= filter.minPrice).ToList();
                    }
                    if (filter.maxPrice != null)
                    {
                        lstSanpham = lstSanpham.Where(x => x.GiaBan <= filter.maxPrice).ToList();
                    }
                    //Tìm kiếm theo loại sản phẩm
                    if (filter.loaiSPCha != "all")
                    {
                        lstSanpham = lstSanpham.Where(x => x.LoaiSPCha == filter.loaiSPCha).ToList();
                        if (filter.loaiSPCon != "all")
                        {
                            lstSanpham = lstSanpham.Where(x => x.LoaiSPCon == filter.loaiSPCon).ToList();
                        }
                    }
                    var model = lstSanpham.Skip((filter.page - 1) * filter.pageSize).Take(filter.pageSize).ToList();
                    return Json(new
                    {
                        data = model,
                        total = lstSanpham.Count,
                        status = true
                    });
                }
                else return Json(new { status = false });
            }
            catch
            {
                return Json(new { status = false });
            }

        }
        [HttpPost]
        public JsonResult UpdateTrangThaiSanPham(string idSanPham, int trangThai)
        {
            try
            {
                var response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "SanPham/UpdateTrangThaiSanPham?id=" + idSanPham + "&trangThai=" + trangThai).Result;
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { KetQua = trangThai, Status = true });
                }
                else return Json(new { Status = false });
            }
            catch
            {
                return Json(new { Status = false });
            }
        }
        public JsonResult GetLoaiSPCha()
        {
            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllLoaiSPCha").Result;
                var loaiSP = JsonConvert.DeserializeObject<List<LoaiSP>>(response.Content.ReadAsStringAsync().Result);
                return Json(loaiSP);
            }
            catch
            {
                return Json(new List<LoaiSP>());
            }
        }
        public JsonResult GetAllMauSac()
        {
            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllMauSac").Result;
                var mauSac = JsonConvert.DeserializeObject<List<MauSac>>(response.Content.ReadAsStringAsync().Result);
                return Json(mauSac);
            }
            catch
            {
                return Json(new List<MauSac>());
            }
        }
        public JsonResult GetAllKichCo()
        {
            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllKichCo").Result;
                var kichCo = JsonConvert.DeserializeObject<List<KichCo>>(response.Content.ReadAsStringAsync().Result);
                return Json(kichCo);
            }
            catch
            {
                return Json(new List<KichCo>());
            }

        }
        public JsonResult GetAllChatLieu()
        {
            try
            {
                HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllChatLieu").Result;
                var chatLieu = JsonConvert.DeserializeObject<List<ChatLieu>>(response.Content.ReadAsStringAsync().Result);
                return Json(chatLieu);
            }
            catch
            {
                return Json(new List<ChatLieu>());
            }
        }
        public JsonResult GetLoaiSPCon(string tenLoaiSPCha)
        {
            try
            {
                string? response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllLoaiSPCon?tenLoaiSPCha=" + tenLoaiSPCha).Result.Content.ReadAsStringAsync().Result;
                if (response != null)
                {
                    var loaiSP = JsonConvert.DeserializeObject<List<LoaiSP>>(response);
                    return Json(new { KetQua = loaiSP, TrangThai = true });
                }
                else return Json(new { TrangThai = false });
            }
            catch
            {
                return Json(new { TrangThai = false });
            }
        }
        [HttpGet]
        public IActionResult AddSanPham()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddSanPham(SanPhamRequest sanPhamRequest)
        {
            try
            {
                //Xóa màu deleted           
                sanPhamRequest.MauSacs.RemoveAll(XoaMau);
                //Xoá size deleted
                sanPhamRequest.KichCos.RemoveAll(XoaSize);
                //Gọi API
                HttpResponseMessage response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "SanPham/AddSanPham", sanPhamRequest).Result;
                if (response.IsSuccessStatusCode)
                {
                    var temp = response.Content.ReadAsStringAsync().Result;
                    var chiTietSanPham = JsonConvert.DeserializeObject<ChiTietSanPhamUpdateRequest>(temp);
                    if(!chiTietSanPham.ChiTietSanPhams.Any() || chiTietSanPham.ChiTietSanPhams == null)
                    {
                        return RedirectToAction("ProductManager");
                    }
                    TempData["UpdateChiTietSanPham"] = temp;
                    TempData["MauSac"] = JsonConvert.SerializeObject(sanPhamRequest.MauSacs);
                    return RedirectToAction("UpdateChiTietSanPham");
                }
                else return BadRequest();
            }
            catch { return RedirectToAction("ProductManager"); }
        }
        private static bool XoaMau(MauSac mau)
        {
            return mau.Ten == "Deleted" || String.IsNullOrEmpty(mau.Ten);
        }
        private static bool XoaSize(string size)
        {
            return size == "Deleted" || String.IsNullOrEmpty(size);
        }
        [HttpGet]
        public IActionResult ProductDetail(string idSanPham)
        {
            TempData["IDSanPham"] = idSanPham;
            return View();

        }
        [HttpGet]
        public JsonResult ShowProductDetail(string id, int page, int pageSize, string? ma, int? minPrice, int? maxPrice, int? minQuantity, int? maxQuantity, int? sort)
        {
            try
            {
                var response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllChiTietSanPhamAdmin?idSanPham=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    var lstSanpham = JsonConvert.DeserializeObject<List<ChiTietSanPhamViewModelAdmin>>(response.Content.ReadAsStringAsync().Result);
                    //Sắp xếp
                    if (sort == 1)
                    {
                        lstSanpham = lstSanpham.OrderBy(x => x.Ma).ToList();
                    }
                    else if (sort == 2)
                    {
                        lstSanpham = lstSanpham.OrderBy(x => x.GiaBan).ToList();
                    }
                    else if (sort == 3)
                    {
                        lstSanpham = lstSanpham.OrderByDescending(x => x.GiaBan).ToList();
                    }
                    else if (sort == 4)
                    {
                        lstSanpham = lstSanpham.OrderBy(x => x.SoLuong).ToList();
                    }
                    else if (sort == 5)
                    {
                        lstSanpham = lstSanpham.OrderByDescending(x => x.SoLuong).ToList();
                    }
                    //Tìm kiếm theo tên sản phẩm
                    if (ma != null)
                    {
                        lstSanpham = lstSanpham.Where(x => x.Ma.Contains(ma.ToUpper())).ToList();
                    }
                    //Tìm kiếm theo giá
                    if (minPrice != null)
                    {
                        lstSanpham = lstSanpham.Where(x => x.GiaBan >= minPrice).ToList();
                    }
                    if (maxPrice != null)
                    {
                        lstSanpham = lstSanpham.Where(x => x.GiaBan <= maxPrice).ToList();
                    }
                    //Tìm kiếm theo số lượng
                    if (minQuantity != null)
                    {
                        lstSanpham = lstSanpham.Where(x => x.SoLuong >= minQuantity).ToList();
                    }
                    if (maxQuantity != null)
                    {
                        lstSanpham = lstSanpham.Where(x => x.SoLuong <= maxQuantity).ToList();
                    }
                    var model = lstSanpham.Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    return Json(new
                    {
                        data = model,
                        total = lstSanpham.Count,
                        status = true
                    }); ;
                }
                else return Json(new { status = false });
            }
            catch
            {
                return Json(new { status = false });
            }
        }
        [HttpGet]
        public IActionResult QuanLyAnh(Guid idSanPham)
        {
            try
            {
                var response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/GetAllAnhSanPham?idSanPham=" + idSanPham).Result;
                if (response.IsSuccessStatusCode)
                {
                    var lstAnh = JsonConvert.DeserializeObject<List<AnhViewModel>>(response.Content.ReadAsStringAsync().Result);
                    ViewData["IDSanPham"] = idSanPham.ToString();
                    return View("QuanLyAnh", lstAnh.OrderBy(x => x.TenMau));
                }
                else return View("QuanLyAnh", new List<AnhViewModel>());
            }
            catch
            {
                return View("QuanLyAnh", new List<AnhViewModel>());
            }
        }
        [HttpPost]
        public IActionResult AddAnhNoColor(IFormFile file, string idSanPham)
        {
            try
            {
                string wwwrootPath = _hostEnvironment.WebRootPath;
                var anh = new Anh() { ID = Guid.NewGuid(), DuongDan = _iFileService.AddFile(file, wwwrootPath).Result, IDSanPham = new Guid(idSanPham), TrangThai = 1 };
                var response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "SanPham/AddImageNoColor", anh).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("QuanLyAnh", new { idSanPham });
                }
                else return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult UpdateImage(IFormFile file, string id, string idSanPham, string duongDan)
        {
            try
            {
                string wwwrootPath = _hostEnvironment.WebRootPath;
                var anh = new Anh() { ID = new Guid(id), DuongDan = _iFileService.AddFile(file, wwwrootPath).Result, TrangThai = 1 };
                var response = _httpClient.PutAsJsonAsync("SanPham/UpdateImage", anh).Result;
                if (response.IsSuccessStatusCode)
                {
                    if (_iFileService.DeleteFile(duongDan, wwwrootPath))
                    {
                        return RedirectToAction("QuanLyAnh", new { idSanPham });
                    }
                    else return BadRequest();
                }
                else return BadRequest();
            }
            catch { return BadRequest(); }
        }
        [HttpGet]
        public IActionResult DeleteImage(string duongDan, string id, string idSanPham)
        {
            try
            {
                string wwwrootPath = _hostEnvironment.WebRootPath;
                var response = _httpClient.DeleteAsync("SanPham/DeleteImage?id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    if (_iFileService.DeleteFile(duongDan, wwwrootPath))
                    {
                        return RedirectToAction("QuanLyAnh", new { idSanPham });
                    }
                    else return BadRequest();
                }
                else return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public IActionResult AddChiTietSanPham(string idSanPham)
        {
            TempData["IDSanPham"] = idSanPham;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddChiTietSanPham(ChiTietSanPhamAddRequest request)
        {
            try
            {
                request.MauSacs.RemoveAll(XoaMau);
                request.KichCos.RemoveAll(XoaSize);
                string idSanPham = TempData.Peek("IDSanPham").ToString();
                request.IDSanPham = new Guid(idSanPham);
                var response = await _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "SanPham/AddChiTietSanPham", request);
                if (response.IsSuccessStatusCode)
                {
                    var temp = response.Content.ReadAsStringAsync().Result;
                    var chiTietSanPham = JsonConvert.DeserializeObject<ChiTietSanPhamUpdateRequest>(temp);
                    if (!chiTietSanPham.ChiTietSanPhams.Any() || chiTietSanPham.ChiTietSanPhams == null)
                    {
                        return RedirectToAction("ProductDetail", new { idSanPham = idSanPham });
                    }
                    TempData["UpdateChiTietSanPham"] = temp;
                    return RedirectToAction("UpdateChiTietSanPham");
                }
                else return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public IActionResult AddAnhToMauSac()
        {
            try
            {
                var lstAnhRequest = JsonConvert.DeserializeObject<List<AnhRequest>>(TempData["MauSacs"].ToString());
                return View(lstAnhRequest);
            }
            catch
            {
                return View(new List<AnhRequest>());
            }
        }
        [HttpPost]
        public JsonResult UpdateSoluongChiTietSanPham(string id, int soLuong)
        {
            try
            {
                ChiTietSanPhamRequest request = new ChiTietSanPhamRequest() { IDChiTietSanPham = new Guid(id), SoLuong = soLuong };
                var response = _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "SanPham/UpdateSoluongChiTietSanPham", request).Result;
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { Message = soLuong.ToString(), TrangThai = true });
                }
                else
                {
                    return Json(new { Message = "Error", TrangThai = false });
                }
            }
            catch
            {
                return Json(new { Message = "Error", TrangThai = false });
            }
        }
        [HttpPost]
        public JsonResult UpdateGiaBanChiTietSanPham(string id, int giaBan)
        {
            try
            {
                ChiTietSanPhamRequest request = new ChiTietSanPhamRequest() { IDChiTietSanPham = new Guid(id), GiaBan = giaBan };
                var response = _httpClient.PutAsJsonAsync(_httpClient.BaseAddress + "SanPham/UpdateGiaBanChiTietSanPham", request).Result;
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { Message = response.Content.ReadAsStringAsync().Result, TrangThai = true });
                }
                else
                {
                    return Json(new { Message = "Error", TrangThai = false });
                }
            }
            catch
            {
                return Json(new { Message = "Error", TrangThai = false });
            }
        }
        [HttpPost]
        public JsonResult UpdateTrangThaiChiTietSanPham(string id)
        {
            try
            {
                var response = _httpClient.GetAsync(_httpClient.BaseAddress + "SanPham/UpdateTrangThaiChiTietSanPham?id=" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { TrangThai = true });
                }
                else
                {
                    return Json(new { TrangThai = false });
                }
            }
            catch
            {
                return Json(new { TrangThai = false });
            }
        }
        [HttpGet]
        public IActionResult UpdateChiTietSanPham()
        {
            try
            {
                var request = JsonConvert.DeserializeObject<ChiTietSanPhamUpdateRequest>(TempData.Peek("UpdateChiTietSanPham").ToString());
                TempData["SanPham"] = request.IDSanPham.ToString();
                TempData["MaSP"] = request.Ma;
                if (request.MauSacs != null)
                {
                    TempData["MauSac"] = JsonConvert.SerializeObject(request.MauSacs);
                }
                TempData["Location"] = request.Location.ToString();
                return View(request);
            }
            catch
            {
                return View(new ChiTietSanPhamUpdateRequest());
            }
        }
        [HttpPost]
        public IActionResult UpdateChiTietSanPham(ChiTietSanPhamUpdateRequest request)
        {
            try
            {
                request.IDSanPham = new Guid(TempData.Peek("SanPham").ToString());
                request.Ma = TempData["MaSP"] as string;
                request.Location = Convert.ToInt32(TempData["Location"] as string);
                var response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "SanPham/AddChiTietSanPhamFromSanPham", request).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("AddAnhToSanPham");
                }
                else return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public IActionResult AddAnhToSanPham()
        {
            try
            {
                var str = TempData["MauSac"] as string;
                if(str == null)
                {
                    return View(new List<MauSac>());
                }
                else
                {
                    var lst = JsonConvert.DeserializeObject<List<MauSac>>(str);
                    return View(lst);
                }               
            }
            catch
            {
                return View(new List<MauSac>());
            }
        }
        [HttpPost]
        public IActionResult AddAnhToSanPham(List<string> maMaus, List<IFormFile> images)
        {
            try
            {
                string wwwrootPath = _hostEnvironment.WebRootPath;
                string idSanPham = TempData.Peek("SanPham").ToString();
                List<AnhRequest> lstAnhRequest = new List<AnhRequest>();
                for (var i = 0; i < maMaus.Count; i++)
                {
                    lstAnhRequest.Add(new AnhRequest() { IDSanPham = new Guid(idSanPham), MaMau = maMaus[i], DuongDan = images.Count <= i ? "" : _iFileService.AddFile(images[i], wwwrootPath).Result });
                }
                HttpResponseMessage response = _httpClient.PostAsJsonAsync(_httpClient.BaseAddress + "SanPham/AddAnh", lstAnhRequest).Result;
                if (response.IsSuccessStatusCode) return RedirectToAction("ProductDetail", new { idSanPham = idSanPham });
                else return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }
        public FileResult GenerateQRCode(string id, string ma)
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(id, QRCodeGenerator.ECCLevel.Q);
            QRCode qRCode = new QRCode(qRCodeData);
            using (MemoryStream stream = new MemoryStream())
            {
                using (Bitmap bitmap = qRCode.GetGraphic(20))
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    return File(stream.ToArray(), "image/png", ma + ".png");
                }
            }
        }
        [HttpGet]
        public async Task<JsonResult> DeleteProductDetail(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync("SanPham/DeleteChiTietSanPham?id=" + id);
                if (response.IsSuccessStatusCode)
                {
                    var ketQua = Convert.ToBoolean(await response.Content.ReadAsStringAsync());
                    if (ketQua)
                    {
                        return Json(new { TrangThai = true });
                    }
                    else return Json(new { TrangThai = false, Loi = "Không thể xóa sản phẩm mặc định" });
                }
                else
                {
                    return Json(new { TrangThai = false, Loi = "Error" });
                }
            }
            catch
            {
                return Json(new { TrangThai = false, Loi = "Error" });
            }
        }
        [HttpGet]
        public async Task<JsonResult> UndoProductDetail(string id)
        {
            try
            {
                var response = await _httpClient.GetAsync("SanPham/UndoChiTietSanPham?id=" + id);
                if (response.IsSuccessStatusCode)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
            }
            catch
            {
                return Json(false);
            }
        }
        [HttpGet]
        public IActionResult UpdateSanPham(Guid id)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<SanPhamUpdateRequest>(_httpClient.GetAsync("SanPham/GetSanPhamById?id=" + id).Result.Content.ReadAsStringAsync().Result);
                return View(response);
            }
            catch
            {
                return View(new SanPhamUpdateRequest());
            }
        }
        [HttpPost]
        public IActionResult UpdateSanPham(SanPhamUpdateRequest request)
        {
            try
            {
                var response = _httpClient.PutAsJsonAsync("SanPham/UpdateSanPham", request).Result;
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ProductManager");
                }
                else return BadRequest();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
