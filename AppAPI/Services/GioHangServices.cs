using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class GioHangServices : IGioHangServices
    {
        private readonly IAllRepository<GioHang> repos;
        AssignmentDBContext context = new AssignmentDBContext();
        private readonly ISanPhamService _iSanPhamService;
        public GioHangServices()
        {
            repos = new AllRepository<GioHang>(context, context.GioHangs);
            _iSanPhamService = new SanPhamService();
        }
        public bool Add(Guid IdKhachHang, DateTime ngaytao)
        {
            GioHang gioHang = new GioHang();
            gioHang.IDKhachHang = IdKhachHang;
            gioHang.NgayTao = ngaytao;
            return repos.Add(gioHang);
        }

        public bool Delete(Guid Id)
        {
            var gioHang = repos.GetAll().FirstOrDefault(x => x.IDKhachHang == Id);
            if (gioHang != null)
            {
                return repos.Delete(gioHang);
            }
            else
            {
                return false;
            }
        }

        public List<GioHang> GetAll()
        {
            return repos.GetAll().ToList();
        }

        public GioHang GetById(Guid Id)
        {
            return repos.GetAll().FirstOrDefault(x => x.IDKhachHang == Id);
        }

        public bool Update(Guid Id, DateTime ngaytao)
        {
            var gioHang = repos.GetAll().FirstOrDefault(x => x.IDKhachHang == Id);
            if (gioHang != null)
            {
                gioHang.NgayTao = ngaytao;
                return repos.Update(gioHang);
            }
            else
            {
                return false;
            }
        }
        public GioHangViewModel GetCart(List<GioHangRequest> request)
        {
            var response = new GioHangViewModel();
            long tongTien = 0;
            ChiTietSanPhamViewModel chiTietSanPham;
            foreach (var item in request)
            {
                chiTietSanPham = _iSanPhamService.GetChiTietSanPhamByID(item.IDChiTietSanPham);
                item.DonGia = chiTietSanPham.GiaBan;
                item.Ten = chiTietSanPham.Ten;
                item.MauSac = chiTietSanPham.MauSac;
                item.KichCo = chiTietSanPham.KichCo;
                item.Anh = chiTietSanPham.Anh;
                item.HetHang = chiTietSanPham.SoLuong < item.SoLuong ? false : chiTietSanPham.TrangThai < 1 ? false : true;
                item.SoLuongConLai = chiTietSanPham.SoLuong;
                tongTien += item.DonGia.Value * item.SoLuong;
            }
            response.GioHangs = request;
            response.TongTien = tongTien;
            return response;
        }
        public GioHangViewModel GetCartLogin(string idNguoiDung)
        {
            var lstChiTietGioHang = context.ChiTietGioHangs.Where(x => x.IDNguoiDung == new Guid(idNguoiDung)).ToList();
            ChiTietSanPhamViewModel chiTietSanPham;
            List<GioHangRequest> lst = new List<GioHangRequest>();
            var response = new GioHangViewModel();
            long tongTien = 0;
            foreach (var item in lstChiTietGioHang)
            {
                chiTietSanPham = _iSanPhamService.GetChiTietSanPhamByID(item.IDCTSP);
                lst.Add(new GioHangRequest() { IDChiTietSanPham = chiTietSanPham.ID, SoLuong = item.SoLuong, SoLuongConLai = chiTietSanPham.SoLuong, DonGia = chiTietSanPham.GiaBan, Ten = chiTietSanPham.Ten, MauSac = chiTietSanPham.MauSac, KichCo = chiTietSanPham.KichCo, Anh = chiTietSanPham.Anh, HetHang = chiTietSanPham.SoLuong < item.SoLuong ? false : true });
                tongTien += chiTietSanPham.GiaBan * item.SoLuong;
            }
            response.GioHangs = lst;
            response.TongTien = tongTien;
            return response;
        }
        public async Task<IActionResult> AddCart(ChiTietGioHang chiTietGioHang) //backup nếu có lỗi
        {
            try
            {
                var temp = context.ChiTietGioHangs.FirstOrDefault(x => x.IDNguoiDung == chiTietGioHang.IDNguoiDung && x.IDCTSP == chiTietGioHang.IDCTSP);
                if (temp != null)
                {
                    var ctsp = context.ChiTietSanPhams.FirstOrDefault(x => x.ID == temp.IDCTSP).SoLuong;
                    var idSP = context.ChiTietSanPhams.FirstOrDefault(x => x.ID == temp.IDCTSP).IDSanPham;
                    var sp = context.SanPhams.FirstOrDefault(x => x.ID == idSP).Ten;
                    if ((temp.SoLuong + chiTietGioHang.SoLuong) > ctsp)
                    {
                        return new BadRequestObjectResult(new { success = false, message = "Bạn đã có " + temp.SoLuong + " sản phẩm trong giỏ hàng. Không thể thêm số lượng đã chọn vào giỏ hàng vì sẽ vượt quá giới hạn mua hàng của bạn." });
                    }
                    //if ((temp.SoLuong + chiTietGioHang.SoLuong) > 15)
                    //{
                    //    return new BadRequestObjectResult(new { success = false, message = "Bạn chỉ có thể thêm tối đa 15 chiếc " + sp + " vào giỏ hàng" });
                    //}
                    else
                    {
                        temp.SoLuong += chiTietGioHang.SoLuong;
                        context.ChiTietGioHangs.Update(temp);

                    }
                }
                else
                {
                    context.ChiTietGioHangs.Add(chiTietGioHang);
                }
                await context.SaveChangesAsync();
                return new OkObjectResult(new { success = true, message = "Thêm vào giỏ hàng thành công" });
            }
            catch (Exception)
            {
                return new BadRequestObjectResult(new { success = false, message = "Đã xảy ra lỗi trong quá trình thêm sản phẩm vào giỏ hàng." });
            }
        }
        public async Task<bool> DeleteCart(Guid idNguoiDung)
        {
            try
            {
                var lstChiTietGioHang = context.ChiTietGioHangs.Where(x => x.IDNguoiDung == idNguoiDung).ToList();
                context.ChiTietGioHangs.RemoveRange(lstChiTietGioHang);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCartbyIDCTSP(Guid idctsp, Guid idNguoiDung)
        {
            try
            {
                var lstChiTietGioHang = context.ChiTietGioHangs.Where(x => x.IDNguoiDung == idNguoiDung && x.IDCTSP == idctsp);
                context.ChiTietGioHangs.RemoveRange(lstChiTietGioHang);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> UpdateCart(Guid idctsp, int soluong, Guid idNguoiDung)
        {
            try
            {
                ChiTietGioHang lstChiTietGioHang = context.ChiTietGioHangs.First(x => x.IDNguoiDung == idNguoiDung && x.IDCTSP == idctsp);
                lstChiTietGioHang.SoLuong = soluong;
                context.ChiTietGioHangs.Update(lstChiTietGioHang);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool ClearCartLogin(Guid idNguoiDung)
        {
            try
            {
                // 1. Tìm TẤT CẢ các mục ChiTietGioHang của người dùng này
                // (Code này giả định ChiTietGioHang có thuộc tính IDNguoiDung, 
                // dựa trên code GioHangController bạn gửi)
                var allItems = context.ChiTietGioHangs.Where(ct => ct.IDNguoiDung == idNguoiDung);

                // 2. Nếu không có gì để xóa, trả về thành công
                if (allItems == null || !allItems.Any())
                {
                    return true;
                }

                // 3. Xóa tất cả các mục đã tìm thấy
                context.ChiTietGioHangs.RemoveRange(allItems);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                // Nếu có lỗi trong quá trình xóa
                return false;
            }
        }
    }
}
