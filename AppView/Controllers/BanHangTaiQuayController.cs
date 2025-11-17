using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace AppView.Controllers
{
    public class BanHangTaiQuayController : Controller
    {
        private readonly HttpClient _httpClient;

        public BanHangTaiQuayController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");

        }
        //Giao diện bán hàng
        //[HttpGet]
        //public async Task<IActionResult> BanHang()
        //{
        //    var listhdcho = await _httpClient.GetFromJsonAsync<List<HoaDon>>("HoaDon/GetAllHDCho");
        //    var deletehdcho = listhdcho.Where(c => !c.NgayTao.Date.Equals(DateTime.Today.Date)).ToList();
        //    foreach (var item in deletehdcho)
        //    {
        //        var response = await _httpClient.DeleteAsync($"HoaDon/deleteHoaDon/{item.ID}");
        //    }
        //    listhdcho = await _httpClient.GetFromJsonAsync<List<HoaDon>>("HoaDon/GetAllHDCho");
        //    ViewData["lsthdcho"] = listhdcho;
        //    return View();
        //}//Giao diện bán hàng
        [HttpGet]
        public async Task<IActionResult> BanHang()
        {
            // --- Phần code dọn dẹp hóa đơn cũ: Giữ nguyên ---
            var listhdcho = await _httpClient.GetFromJsonAsync<List<HoaDon>>("HoaDon/GetAllHDCho");
            var deletehdcho = listhdcho.Where(c => !c.NgayTao.Date.Equals(DateTime.Today.Date)).ToList();
            foreach (var item in deletehdcho)
            {
                var response = await _httpClient.DeleteAsync($"HoaDon/deleteHoaDon/{item.ID}");
            }
            // --- Hết phần dọn dẹp ---

            // Lấy lại danh sách hóa đơn chờ (đã sạch)
            listhdcho = await _httpClient.GetFromJsonAsync<List<HoaDon>>("HoaDon/GetAllHDCho");

            // *** BẮT ĐẦU SỬA ĐỔI ***
            // 1. Lấy thông tin user đang đăng nhập
            var loginInfor = new LoginViewModel();
            string? session = HttpContext.Session.GetString("LoginInfor");
            if (session != null)
            {
                loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
            }
            else
            {
                // Nếu không có session, trả về danh sách rỗng (hoặc chuyển hướng login)
                ViewData["lsthdcho"] = new List<HoaDon>();
                return View();
            }

            // 2. Lọc danh sách hóa đơn CHỈ CỦA user hiện tại
            // *** LƯU Ý: Giả định model 'HoaDon' có thuộc tính 'IDNhanVien' ***
            // Nếu tên khác, bạn phải đổi 'hd.IDNhanVien' cho chính xác
            var hdChoCuaUser = listhdcho.Where(hd => hd.IDNhanVien == loginInfor.Id).ToList();

            // 3. Gửi danh sách ĐÃ LỌC sang View
            ViewData["lsthdcho"] = hdChoCuaUser;
            // *** KẾT THÚC SỬA ĐỔI ***

            return View();
        }





        // Sản phẩm
        [HttpGet]
        public async Task<IActionResult> LoadSp(int page, int pagesize)
        {
            var listsanPham = await _httpClient.GetFromJsonAsync<List<SanPhamBanHang>>("SanPham/getAllSPBanHang");
            listsanPham = listsanPham.Where(c => c.GiaGoc > 0).ToList();
            var model = listsanPham.Skip((page - 1) * pagesize).Take(pagesize).ToList();
            int totalRow = listsanPham.Count;
            return Json(new
            {
                data = model,
                total = totalRow,
                status = true,
            });
        }
        //Hiển thị sản phẩm
        [HttpGet("/BanHangTaiQuay/ShowSPDetail/{idsp}")]
        public async Task<IActionResult> ShowSPDetail(string idsp)
        {
            var sP = await _httpClient.GetFromJsonAsync<ChiTietSanPhamBanHang>($"SanPham/getChiTietSPBHById/{idsp}");
            return PartialView("_SanPhamDetail", sP);
        }
        //Hiển thị lọc
        public async Task<IActionResult> ShowFilterSP()
        {
            var lsp = await _httpClient.GetFromJsonAsync<List<LoaiSP>>($"LoaiSP/getAll");
            ViewData["lstLSP"] = lsp;
            return PartialView("_LocSP");
        }
        // Lọc sản phẩm
        public async Task<IActionResult> LocSP(FilterSP filter)
        {
            try
            {
                var listSP = await _httpClient.GetFromJsonAsync<List<SanPhamBanHang>>("SanPham/getAllSPBanHang");
                //Lọc danh mục 
                if (filter.lstDM != null)
                {
                    listSP = listSP.Where(c => filter.lstDM.Contains(c.IdLsp)).ToList();
                }
                //Lọc giá
                if (filter.khoangGia != 0)
                {
                    switch (filter.khoangGia)
                    {
                        case 1:
                            listSP = listSP.Where(c => c.GiaBan < 100000).ToList();
                            break;
                        case 2:
                            listSP = listSP.Where(c => c.GiaBan >= 100000 && c.GiaBan < 200000).ToList();
                            break;
                        case 3:
                            listSP = listSP.Where(c => c.GiaBan >= 200000 && c.GiaBan < 300000).ToList();
                            break;
                        case 4:
                            listSP = listSP.Where(c => c.GiaBan >= 300000 && c.GiaBan < 400000).ToList();
                            break;
                        case 5:
                            listSP = listSP.Where(c => c.GiaBan >= 400000 && c.GiaBan < 500000).ToList();
                            break;
                        case 6:
                            listSP = listSP.Where(c => c.GiaBan >= 500000).ToList();
                            break;
                        default:
                            break;
                    }
                }
                //Phân trang
                if (listSP.Count == 0)
                {
                    listSP = await _httpClient.GetFromJsonAsync<List<SanPhamBanHang>>("SanPham/getAllSPBanHang");
                }
                var model = listSP.Skip((filter.page - 1) * filter.pageSize).Take(filter.pageSize).ToList();
                int totalRow = listSP.Count;
                return Json(new
                {
                    data = model,
                    total = totalRow,
                    status = true,
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("BanHang", "BanHangTaiQuay");
            }
        }
        //Tìm kiếm sản phẩm
        [HttpGet("/BanHangTaiQuay/Search/{keyword}")]
        public async Task<IActionResult> Search(string keyword)
        {
            try
            {
                var listsanPham = await _httpClient.GetFromJsonAsync<List<SanPhamBanHang>>("SanPham/getAllSPBanHang");
                listsanPham = listsanPham.Where(c => c.GiaGoc != 0).ToList();
                var distinctResult = listsanPham
                    .Where(c => c.Ten.ToLower().Contains(keyword.Trim().ToLower()))
                    .Distinct()
                    .ToList();
                var result = new List<SanPhamBanHang>();
                if (distinctResult.Count < 3)
                {
                    var additionalItems = distinctResult.Take(result.Count).ToList();
                    result.AddRange(additionalItems);
                }
                result = distinctResult.Take(3).ToList();
                return Json(new { data = result });
            }
            catch (Exception ex)
            {
                return RedirectToAction("BanHang", "BanHangTaiQuay");
            }
        }
        // Lấy Load CTSP trong SP
        [HttpGet("/BanHangTaiQuay/ShowListCTSP/{idsp}")]
        public async Task<IActionResult> ShowListCTSP(string idsp)
        {
            var lstctsP = await _httpClient.GetFromJsonAsync<List<ChiTietCTSPBanHang>>($"SanPham/getChiTietCTSPBHById/{idsp}");
            return Json(new { data = lstctsP });
        }
        public async Task<IActionResult> FilterCTSP(FilterCTSP filter)
        {
            try
            {
                var lstctsP = await _httpClient.GetFromJsonAsync<List<ChiTietCTSPBanHang>>($"SanPham/getChiTietCTSPBHById/{filter.IdSanPham}");
                //Lọc màu
                if (filter.lstIdMS != null)
                {
                    lstctsP = lstctsP.Where(c => filter.lstIdMS.Contains(c.idMauSac)).ToList();
                }
                //Lọc kích thước
                if (filter.lstIdKC != null)
                {
                    lstctsP = lstctsP.Where(c => filter.lstIdKC.Contains(c.idKichCo)).ToList();
                }
                return Json(new { data = lstctsP });
            }
            catch (Exception ex)
            {
                return RedirectToAction("BanHang", "BanHangTaiQuay");
            }
        }
        //Update ghi chú
        public async Task<IActionResult> UpdateGhichu(Guid idhd, string ghichu)
        {
            try
            {
                var loginInfor = new LoginViewModel();
                string? session = HttpContext.Session.GetString("LoginInfor");
                if (session != null)
                {
                    loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                }

                if (ghichu != null)
                {
                    var stringURL = $"https://localhost:7095/api/HoaDon/UpdateGhichu?idhd={idhd}&idnv={loginInfor.Id}&ghichu={ghichu}";
                    var response = await _httpClient.PutAsync(stringURL, null);
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(new { success = true, message = "Lưu ghi chú thành công" });
                    }
                    else
                        return Json(new { success = false, message = "Lưu ghi chú thất bại" });
                }
                else
                {
                    return Json(new { success = false, message = "Ghi chú không được để trống" });
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("BanHang", "BanHangTaiQuay");
            }
        }
        //Lấy Hóa đơn chi tiết
        [HttpGet("/BanHangTaiQuay/getCTHD/{id}")]
        public async Task<IActionResult> getCTHD(string id)
        {
            // 1. Nếu id bị null, rỗng hoặc không phải Guid
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out Guid guidId))
            {
                // THAY ĐỔI Ở ĐÂY:
                // Thay vì return BadRequest("..."), ta trả về View rỗng để giao diện vẫn hiển thị đẹp
                // Tạo một model rỗng để không bị lỗi NullReference bên View
                var emptyModel = new HoaDonViewModelBanHang();

                // Lấy danh sách khách hàng (để combobox không bị trống)
                try
                {
                    var kh = await _httpClient.GetFromJsonAsync<List<KhachHang>>($"KhachHang");
                    ViewBag.lstKH = kh;
                }
                catch
                {
                    ViewBag.lstKH = new List<KhachHang>();
                }

                return PartialView("GioHang", emptyModel);
            }

            // ... (Phần code gọi API bên dưới giữ nguyên như cũ) ...
            try
            {
                var hdon = await _httpClient.GetFromJsonAsync<HoaDonViewModelBanHang>($"HoaDon/GetHDBanHang/{id}");
                var kh = await _httpClient.GetFromJsonAsync<List<KhachHang>>($"KhachHang");
                ViewBag.lstKH = kh;
                return PartialView("GioHang", hdon);
            }
            catch (Exception ex)
            {
                return PartialView("GioHang", new HoaDonViewModelBanHang());
            }
        }
        // Thêm hóa đơn chi tiết
        public async Task<ActionResult> addHdct(HoaDonChiTietRequest request)
        {
            try
            {
                var ctsp = await _httpClient.GetFromJsonAsync<ChiTietSanPhamViewModel>($"SanPham/GetChiTietSanPhamByID?id={request.IdChiTietSanPham}");
                if (ctsp == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy sản phẩm này" });
                }
                if (ctsp != null && ctsp.TrangThai == 0)
                {
                    return Json(new { success = false, message = "Sản phẩm không hoạt động" });
                }
                else if (ctsp != null && request.SoLuong > ctsp.SoLuong)
                {
                    return Json(new { success = false, message = "Sản phẩm đã hết hàng" });
                }
                else
                {
                    HoaDonChiTietRequest hdct = new HoaDonChiTietRequest()
                    {
                        Id = new Guid(),
                        IdChiTietSanPham = request.IdChiTietSanPham,
                        IdHoaDon = request.IdHoaDon,
                        SoLuong = request.SoLuong,
                    };
                    var response = await _httpClient.PostAsJsonAsync("ChiTietHoaDon/saveHDCT/", hdct);
                    if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Thêm sản phẩm thành công" });
                }
                return Json(new { success = false, message = "Thêm sản phẩm thất bại" });
            }
            catch
            {
                return Json(new { success = false, message = "Thêm sản phẩm thất bại" });
            }
        }
        //Xóa chi tiết hóa đơn
        [HttpDelete("/BanHangTaiQuay/deleteHdct/{id}")]
        public async Task<ActionResult> deleteHdct(String id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"ChiTietHoaDon/delete/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return Json(new { success = true, message = "Xóa thành công" });
                }
                else
                    return Json(new { success = false, message = "Xóa thất bại" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("BanHang", "BanHangTaiQuay");
            }
        }
        //Cập nhật số lượng 
        public async Task<IActionResult> UpdateSL(string idhdct, int sl)
        {
            try
            {
                var response = await _httpClient.PostAsync($"ChiTietHoaDon/UpdateSL?id={idhdct}&sl={sl}", null);
                if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Cập nhật số lượng thành công" });
                else return Json(new { success = false, message = "Số lượng sản phẩm không đủ" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("BanHang", "BanHangTaiQuay");
            }
        }
        //Check voucher

        [HttpGet]
        public async Task<IActionResult> CheckVoucher(Guid idvoucher, int ttien)
        {
            try
            {
                var vc = await _httpClient.GetFromJsonAsync<Voucher>($"Voucher/{idvoucher}");
                if (vc.HinhThucGiamGia == 0)
                {
                    return Json(new { success = true, idvoucher = vc.ID, giatri = vc.GiaTri, message = "Bạn được giảm " + vc.GiaTri.ToString("n0") + " VND" });

                }
                else if (vc.HinhThucGiamGia == 1)
                {
                    return Json(new { success = true, idvoucher = vc.ID, giatri = (ttien * vc.GiaTri / 100), message = "Bạn được giảm " + vc.GiaTri + "%" });
                }
                return Json(new { message = "Đã xảy ra lỗi" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("BanHang", "BanHangTaiQuay");
            }
            //if (vc != null && idkh != Guid.Empty)
            //{
            //    var response = await _httpClient.GetFromJsonAsync<bool>($"HoaDon/CheckCustomerUseVoucher?idkh={idkh}&idvoucher={vc.ID}");
            //    if (response == true)
            //    {
            //        return Json(new { success = false, message = "Khách hàng đã sử dụng voucher này" });
            //    }
            //}

            //if (vc == null)
            //{
            //    return Json(new { success = false, message = "Voucher hết hạn hoặc không hợp lệ" });
            //}
            //else if (vc.SoTienCan > ttien)
            //{
            //    return Json(new { success = false, message = "Thanh toán tối thiểu " + vc.SoTienCan.ToString("n0") + " để áp dụng" });
            //}
            //else if (vc.HinhThucGiamGia == 0)
            //{
            //    return Json(new { success = true, idvoucher = vc.ID, giatri = vc.GiaTri, message = "Bạn được giảm " + vc.GiaTri.ToString("n0") + " VND" });

            //}
            //else if (vc.HinhThucGiamGia == 1)
            //{
            //    return Json(new { success = true, idvoucher = vc.ID, giatri = (ttien * vc.GiaTri / 100), message = "Bạn được giảm " + vc.GiaTri + "%" });
            //}
            //return Json(new { message = "Đã xảy ra lỗi" });
        }

        //Load Modal Thanh Tóan
        [HttpGet("/BanHangTaiQuay/ViewThanhToan/{id}")]
        public async Task<IActionResult> ViewThanhToan(string id)
        {
            try
            {
                var hd = await _httpClient.GetFromJsonAsync<HoaDon>($"HoaDon/GetById/{id}");
                var lstcthd = await _httpClient.GetFromJsonAsync<List<HoaDonChiTietViewModel>>($"ChiTietHoaDon/getByIdHD/{id}");
                lstcthd = lstcthd.Where(c => c.SoLuong > 0).ToList();
                //Voucher
                string apiURL = $"https://localhost:7095/api/Voucher";
                var listvc = await _httpClient.GetFromJsonAsync<List<Voucher>>(apiURL);
                //Quy đổi điểm
                var qdd = await _httpClient.GetFromJsonAsync<List<QuyDoiDiem>>("QuyDoiDiem");
                var qddActive = qdd.FirstOrDefault(c => c.TrangThai != 0);
                //Kiểm tra là hóa đơn của khách có tài khoản không?
                var khachHang = "Khách lẻ";
                Guid idkh = Guid.Empty;
                int? dtkh = 0;
                var response = await _httpClient.GetFromJsonAsync<bool>($"HoaDon/CheckLSGDHD/{id}");
                if (response == true)
                {
                    var lstd = await _httpClient.GetFromJsonAsync<LichSuTichDiem>($"HoaDon/LichSuGiaoDich/{id}");
                    //Sửa lịch sử tích điểm nếu có thay đổi quy đổi điểm bên quản trị
                    string url = $"LichSuTichDiem/{lstd.ID}?diem={lstd.Diem}&trangthai={lstd.TrangThai}&IdKhachHang={lstd.IDKhachHang}&IdQuyDoiDiem={qddActive.ID}&IdHoaDon={id}";
                    var lstdresponse = await _httpClient.PutAsync(url, null);
                    //Lấy tên kh
                    var kh = await _httpClient.GetFromJsonAsync<KhachHang>($"KhachHang/GetById?id={lstd.IDKhachHang}");
                    khachHang = kh.Ten;
                    idkh = kh.IDKhachHang;
                    dtkh = kh.DiemTich == null ? 0 : kh.DiemTich;
                }
                var loginInfor = new LoginViewModel();
                string? session = HttpContext.Session.GetString("LoginInfor");
                if (session != null)
                {
                    loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                }
                var soluong = lstcthd.Sum(c => c.SoLuong);
                var ttien = lstcthd.Sum(c => c.SoLuong * c.GiaKM);
                // Lấy danh sách voucher áp dụng được cho hóa đơn (SL >0, chưa hết hạn, trạng thái còn hoạt động)
                //Thỏa mãn Điều kiện giá trị tối thiểu và  khách hàng chưa sử dụng voucher này và với hóa đơn giảm trực tiếp thì check việc tổng tiền  có lớn hơn giá trị không
                if (listvc != null)
                {
                    listvc = listvc.Where(c => c.NgayKetThuc > DateTime.Now && c.SoLuong > 0 && c.TrangThai != 0).ToList();
                    listvc = listvc.Where(c => c.SoTienCan <= ttien).ToList();
                    if (listvc != null && idkh != Guid.Empty)
                    {
                        var lsthdmua = await _httpClient.GetFromJsonAsync<List<HoaDon>>($"KhachHang/getAllHDKH?idkh={idkh}");
                        listvc = listvc.Where(c => !lsthdmua.Any(x => x.IDVoucher == c.ID)).ToList();
                    }
                    // Lấy hóa đơn thỏa ko giảm hết số tiền
                    listvc = listvc.Where(c => !listvc.Any(x => x.GiaTri > ttien && x.TrangThai == 0)).ToList();
                }
                var hdtt = new HoaDonThanhToanViewModel()
                {
                    Id = hd.ID,
                    MaHD = hd.MaHD,
                    NgayThanhToan = DateTime.Now,
                    KhachHang = khachHang,
                    IdKhachHang = idkh,
                    TongSL = soluong,
                    TongTien = ttien,
                    DiemKH = dtkh,
                    DiemTichHD = qddActive != null && qddActive.TiLeTichDiem != 0 ? Convert.ToInt32(ttien / qddActive?.TiLeTichDiem) : 0,
                    NhanVien = loginInfor.Ten,
                };
                ViewBag.TLTieu = (qddActive != null && qddActive.TiLeTichDiem != 0) ? (qddActive.TiLeTieuDiem) : 0;
                ViewBag.LoaiQDD = qddActive != null ? (qddActive.TrangThai) : 0;
                ViewData["LstVoucher"] = listvc;
                return PartialView("_ThanhToan", hdtt);
            }
            catch (Exception ex)
            {
                return RedirectToAction("BanHang", "BanHangTaiQuay");
            }
        }

        //ThanhToan
        public async Task<IActionResult> ThanhToan(HoaDonThanhToanRequest request)
        {
            try
            {
                var hdrequest = new HoaDonThanhToanRequest()
                {
                    Id = request.Id,
                    IdNhanVien = request.IdNhanVien,
                    NgayThanhToan = DateTime.Now,
                    IdVoucher = request.IdVoucher == Guid.Empty ? Guid.Empty : request.IdVoucher,
                    PTTT = request.PTTT,
                    TongTien = request.TongTien,
                    DiemTichHD = request.DiemTichHD,
                    DiemSD = request.DiemSD,
                    TrangThai = 6,
                };
                var response = await _httpClient.PutAsJsonAsync("HoaDon/UpdateHoaDon/", hdrequest);
                if (response.IsSuccessStatusCode) return Json(new { success = true, message = "Thanh toán thành công" });
                return Json(new { success = false, message = "Thanh toán thất bại" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("_QuanLyHoaDon", "QuanLyHoaDon");
            }
        }

        //Thêm nhanh khách hàng
        [HttpPost]
        public async Task<IActionResult> AddKhachHang(KhachHang request)
        {
            try
            {
                KhachHang khview = new KhachHang();
                khview.IDKhachHang = Guid.NewGuid();
                khview.SDT = request.SDT;
                khview.Email = request.Email;
                khview.Ten = request.Ten;
                khview.DiaChi = request.DiaChi;
                khview.NgaySinh = request.NgaySinh;
                khview.GioiTinh = request.GioiTinh;
                khview.Password = khview.IDKhachHang.ToString().Substring(0, 8);
                khview.TrangThai = 1;
                khview.DiemTich = 0;
                var lstkh = await _httpClient.GetFromJsonAsync<List<KhachHang>>("KhachHang");
                if (request.SDT != null && lstkh.Any(c => c.SDT != null && c.SDT.Trim().Equals(request.SDT.Trim())))
                {
                    return Json(new { success = false, message = "Số điện thoại đã được sử dụng" });
                }
                if (request.Email != null && lstkh.Any(c => c.Email != null && c.Email.Trim().Equals(request.Email.Trim())))
                {
                    return Json(new { success = false, message = "Email đã được sử dụng" });
                }
                else
                {
                    var url = $"https://localhost:7095/api/QuanLyNguoiDung/AddNhanhKH";
                    var response = await _httpClient.PostAsJsonAsync(url, khview);
                    if (response.IsSuccessStatusCode) // Thêm khách hàng thành công -> tạo lịch sử tích điểm
                    {
                        var qdd = await _httpClient.GetFromJsonAsync<List<QuyDoiDiem>>("QuyDoiDiem");
                        var idqdd = qdd.FirstOrDefault(c => c.TrangThai != 0).ID;
                        var kh = new KhachHang();
                        if (request.SDT != null)
                        {
                            kh = await _httpClient.GetFromJsonAsync<KhachHang>($"KhachHang/getBySDT?sdt={request.SDT}");
                        }
                        else if (request.Email != null)
                        {
                            kh = await _httpClient.GetFromJsonAsync<KhachHang>($"KhachHang/getBySDT?sdt={request.Email}");
                        }

                        var IDHD = request.IDKhachHang; // Luu tam idhd qua idkh
                                                        // ktra hd đã có lstd 
                        var checkexist = await _httpClient.GetFromJsonAsync<bool>($"HoaDon/CheckLSGDHD/{IDHD}");
                        if (checkexist == true) // Tồn tại-> xóa
                        {
                            var lstdexist = await _httpClient.GetFromJsonAsync<LichSuTichDiem>($"HoaDon/LichSuGiaoDich/{IDHD}");
                            var deletelstd = await _httpClient.DeleteAsync($"LichSuTichDiem/{lstdexist.ID}");
                        }
                        string apiUrl = $"https://localhost:7095/api/LichSuTichDiem?diem=0&trangthai=1&IdKhachHang={kh.IDKhachHang}&IdQuyDoiDiem={idqdd}&IdHoaDon={IDHD}";
                        var lstdresponse = await _httpClient.PostAsync(apiUrl, null);
                        return Json(new { success = true, message = "Thêm khách hàng thành công" });

                    }
                }
                return Json(new { success = false, message = "Thêm khách hàng thất bại" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Thêm khách hàng thất bại" });
            }
        }

        //Sửa khách hàng
        public async Task<IActionResult> UpdateKHinHD(string idkh, string idhd)
        {
            try
            {
                var qdd = await _httpClient.GetFromJsonAsync<List<QuyDoiDiem>>("QuyDoiDiem");
                var idqdd = qdd.FirstOrDefault(c => c.TrangThai != 0).ID;
                var checkexist = await _httpClient.GetFromJsonAsync<bool>($"HoaDon/CheckLSGDHD/{idhd}");
                if (checkexist == true) // Tồn tại-> sửa
                {
                    var lstd = await _httpClient.GetFromJsonAsync<LichSuTichDiem>($"HoaDon/LichSuGiaoDich/{idhd}");
                    string apiUrl = $"https://localhost:7095/api/LichSuTichDiem/{lstd.ID}?diem={lstd.Diem}&trangthai={lstd.TrangThai}&IdKhachHang={idkh}&IdQuyDoiDiem={lstd.IDQuyDoiDiem}&IdHoaDon={idhd}";
                    var response = await _httpClient.PutAsync(apiUrl, null);
                }
                else // Chưa có lstd-> tạo mới
                {
                    string apiUrl = $"https://localhost:7095/api/LichSuTichDiem?diem=0&trangthai=1&IdKhachHang={idkh}&IdQuyDoiDiem={idqdd}&IdHoaDon={idhd}";
                    var lstdresponse = await _httpClient.PostAsync(apiUrl, null);
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
        //Xóa khách hàng
        [HttpGet("/BanHangTaiQuay/DeleteKHinHD/{idhd}")]
        public async Task<IActionResult> DelefteKHinHD(string idhd)
        {
            try
            {
                var checkexist = await _httpClient.GetFromJsonAsync<bool>($"HoaDon/CheckLSGDHD/{idhd}");
                if (checkexist == true) // Tồn tại-> xóa
                {
                    var lstd = await _httpClient.GetFromJsonAsync<LichSuTichDiem>($"HoaDon/LichSuGiaoDich/{idhd}");
                    var response = await _httpClient.DeleteAsync($"LichSuTichDiem/{lstd.ID}");
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
        //Tìm kiếm khách hàng
        [HttpGet("/BanHangTaiQuay/SearchKH/{keyword}")]
        public async Task<IActionResult> SearchKH(string keyword)
        {
            try
            {
                var lstkh = await _httpClient.GetFromJsonAsync<List<KhachHang>>("KhachHang");
                var distinctResult = lstkh
                                    .Where(c => c.Ten.ToLower().Contains(keyword.Trim().ToLower()) || (c.SDT != null && c.SDT.Contains(keyword.Trim())))
                                    .Distinct()
                                    .ToList();
                var result = new List<KhachHang>();
                if (distinctResult.Count < 3)
                {
                    var additionalItems = distinctResult.Take(result.Count).ToList();
                    result.AddRange(additionalItems);
                }
                result = distinctResult.Take(3).ToList();
                return Json(new { data = result });
            }
            catch (Exception ex)
            {
                return RedirectToAction("BanHang", "BanHangTaiQuay");
            }
        }
        // AddHDCho
        [HttpGet]
        //public async Task<IActionResult> AddHDCho()
        //{
        //    try
        //    {
        //        var listhdcho = await _httpClient.GetFromJsonAsync<List<HoaDon>>("HoaDon/GetAllHDCho");

        //        if (listhdcho.Count < 15)
        //        {
        //            var loginInfor = new LoginViewModel();
        //            string? session = HttpContext.Session.GetString("LoginInfor");
        //            if (session != null)
        //            {
        //                loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
        //            }
        //            var request = await _httpClient.PostAsJsonAsync<HoaDon>($"HoaDon/Offline/{loginInfor.Id}", null);
        //            listhdcho = await _httpClient.GetFromJsonAsync<List<HoaDon>>("HoaDon/GetAllHDCho");
        //            return Json(new { success = true, data = listhdcho });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "Hệ thống giới hạn 15 tab cho màn hình bán hàng" });
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return RedirectToAction("BanHang", "BanHangTaiQuay");
        //    }
        //}
        [HttpGet]
        public async Task<IActionResult> AddHDCho()
        {
            try
            {
                // 1. Lấy thông tin user đang đăng nhập
                var loginInfor = new LoginViewModel();
                string? session = HttpContext.Session.GetString("LoginInfor");
                if (session == null)
                {
                    return Json(new { success = false, message = "Vui lòng đăng nhập để thực hiện chức năng này." });
                }
                loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                var currentUserId = loginInfor.Id;

                // 2. Lấy danh sách hóa đơn hiện tại để kiểm tra số lượng
                var listhdcho = await _httpClient.GetFromJsonAsync<List<HoaDon>>("HoaDon/GetAllHDCho");

                // Lọc hóa đơn của nhân viên này
                var hdChoCuaUser = listhdcho.Where(hd => hd.IDNhanVien == currentUserId).ToList();

                // 3. Kiểm tra giới hạn (Ví dụ: tối đa 5-10 hóa đơn chờ)
                int maxHdCho = 10;
                if (hdChoCuaUser.Count >= maxHdCho)
                {
                    return Json(new { success = false, message = $"Bạn chỉ được tạo tối đa {maxHdCho} hóa đơn chờ." });
                }

                // 4. Gọi API tạo hóa đơn mới
                // Sử dụng PostAsync thay vì PostAsJsonAsync vì body là null
                var response = await _httpClient.PostAsync($"HoaDon/Offline/{currentUserId}", null);

                if (response.IsSuccessStatusCode)
                {
                    // Đọc kết quả trả về từ API (API trả về true/false)
                    var resultString = await response.Content.ReadAsStringAsync();
                    bool isCreated = false;
                    bool parseSuccess = bool.TryParse(resultString, out isCreated);

                    // Nếu API trả về true (hoặc "true")
                    if (parseSuccess && isCreated)
                    {
                        // 5. Lấy lại danh sách mới nhất sau khi tạo thành công
                        listhdcho = await _httpClient.GetFromJsonAsync<List<HoaDon>>("HoaDon/GetAllHDCho");

                        // Lọc lại danh sách của user
                        var newList = listhdcho.Where(hd => hd.IDNhanVien == currentUserId)
                                               .OrderByDescending(c => c.NgayTao) // Sắp xếp mới nhất lên đầu
                                               .ToList();

                        // Lấy ID của hóa đơn vừa tạo (thằng đầu tiên trong danh sách giảm dần)
                        var newBillId = newList.FirstOrDefault()?.ID;

                        return Json(new
                        {
                            success = true,
                            message = "Tạo hóa đơn thành công",
                            data = newList,
                            newId = newBillId // Trả về ID mới để JS select luôn
                        });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Lỗi từ Server: Không thể tạo hóa đơn." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = $"Lỗi kết nối API: {response.StatusCode}" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi hệ thống: " + ex.Message });
            }
        }

        //
        [HttpGet]
        public async Task<IActionResult> GetHdChoCuaUser()
        {
            try
            {
                // 1. Lấy thông tin user
                var loginInfor = new LoginViewModel();
                string? session = HttpContext.Session.GetString("LoginInfor");
                if (session == null)
                {
                    return Json(new List<HoaDon>()); // Trả về danh sách rỗng nếu chưa login
                }
                loginInfor = JsonConvert.DeserializeObject<LoginViewModel>(session);
                var currentUserId = loginInfor.Id;

                // 2. Lấy tất cả HD Chờ
                var listhdcho = await _httpClient.GetFromJsonAsync<List<HoaDon>>("HoaDon/GetAllHDCho");

                // 3. Lọc theo user
                // *** NHỚ SỬA hd.IDNhanVien cho đúng với model HoaDon của bạn ***
                var hdChoCuaUser = listhdcho.Where(hd => hd.IDNhanVien == currentUserId).ToList();

                // 4. Trả về JSON
                return Json(hdChoCuaUser);
            }
            catch (Exception)
            {
                return Json(new List<HoaDon>()); // Trả về rỗng nếu lỗi
            }
        }

        ////HÓA ĐƠN
        //Chuyển view hóa đơn
        [HttpGet("/BanHangTaiQuay/QuanLyHD")]
        public IActionResult QuanLyHD()
        {
            return PartialView("_QuanLyHoaDon");
        }
        //Tam
        public IActionResult ScanQRCode()
        {
            return PartialView("ScanQRCode");
        }
        //End
    }
}