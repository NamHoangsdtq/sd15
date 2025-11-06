using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class LishSuTichDiemServices : ILishSuTichDiemServices
    {
        private readonly IAllRepository<LichSuTichDiem> _allRepository;
        AssignmentDBContext context= new AssignmentDBContext();
        public LishSuTichDiemServices()
        {
            _allRepository= new AllRepository<LichSuTichDiem>(context,context.LichSuTichDiems);
        }
        public bool Add(int diem, int trangthai, Guid IdKhachHang, Guid IdQuyDoiDiem, Guid IdHoaDon)
        {
            var lichsu = new LichSuTichDiem();
            lichsu.ID=Guid.NewGuid();
            lichsu.Diem=diem;
            lichsu.TrangThai=trangthai;
            lichsu.IDKhachHang=IdKhachHang;
            lichsu.IDQuyDoiDiem = IdQuyDoiDiem;
            lichsu.IDHoaDon = IdHoaDon;
            return _allRepository.Add(lichsu);
        }

        public bool Delete(Guid Id)
        {
            var lichsu = _allRepository.GetAll().First(x => x.ID == Id);
            if (lichsu != null)
            {
               
                return _allRepository.Delete(lichsu);
            }
            else
            {
                return false;
            }
        }

        public List<LichSuTichDiem> GetAll()
        {
            return _allRepository.GetAll(); 
        }
       

        public LichSuTichDiem GetById(Guid Id)
        {
            return _allRepository.GetAll().First(x => x.ID==Id);
        }

        public bool Update(Guid Id, int diem, int trangthai, Guid IdKhachHang, Guid IdQuyDoiDiem, Guid IdHoaDon)
        {
            var lichsu= _allRepository.GetAll().First(x => x.ID == Id);
            if (lichsu != null)
            {
                lichsu.Diem = diem;
                lichsu.TrangThai = trangthai;
                lichsu.IDKhachHang = IdKhachHang;
                lichsu.IDQuyDoiDiem = IdQuyDoiDiem;
                lichsu.IDHoaDon = IdHoaDon;
                return _allRepository.Update(lichsu);
            }
            else
            {
                return false;
            }
        }
        //dũng
        public async Task<List<DonMuaViewModel>> getAllDonMua(Guid idKhachHang)
        {
            try
            {
                var lstDonMua = await (from b in context.HoaDons

                                       select new DonMuaViewModel()
                                       {
                                           IDHoaDon = b.ID,
                                           NgayTao = b.NgayTao,
                                           NgayThanhToan = b.NgayThanhToan,
                                           NgayNhanHang = b.NgayNhanHang,
                                           Ngaytao1 = null,
                                           Ngaynhanhang1 = null,
                                           Ngaythanhtoan1 = null,
                                           TenNguoiNhan = b.TenNguoiNhan,
                                           SDT = b.SDT,
                                           Email = b.Email,
                                           DiaChi = b.DiaChi,
                                           TienShip = b.TienShip,
                                           TrangThaiGiaoHang = b.TrangThaiGiaoHang,
                                           IdNguoiDung = context.LichSuTichDiems.FirstOrDefault(p => p.IDHoaDon == b.ID) != null ? context.LichSuTichDiems.FirstOrDefault(p => p.IDHoaDon == b.ID).IDKhachHang.Value : null,
                                           MaHD = b.MaHD,
                                           TongTien = b.TongTien,
                                           LoaiHoaDon = b.LoaiHD
                                       }).ToListAsync();

                lstDonMua = lstDonMua.Where(P => P.IdNguoiDung == idKhachHang).ToList();
                return lstDonMua;
            }
            catch
            {
                return new List<DonMuaViewModel>();
            }

        }


        public async Task<List<DonMuaChiTietViewModel>> getAllDonMuaChiTiet(Guid idHoaDon)
        {
            try
            {
                var lstDonMuaCT = await(from c in context.ChiTietHoaDons.Where(p => p.IDHoaDon == idHoaDon)
                                      join a in context.HoaDons on c.IDHoaDon equals a.ID
                                      //join b in context.LichSuTichDiems on a.ID equals b.IDHoaDon
                                      join d in context.DanhGias on c.ID equals d.ID
                                      join e in context.ChiTietSanPhams on c.IDCTSP equals e.ID
                                      join f in context.KichCos on e.IDKichCo equals f.ID
                                      join g in context.MauSacs on e.IDMauSac equals g.ID
                                      join i in context.SanPhams.Where(x => x.TrangThai == 1) on e.IDSanPham equals i.ID
                                      //join l in context.QuyDoiDiems on b.IDQuyDoiDiem equals l.ID
                                      select new DonMuaChiTietViewModel()
                                      {
                                         ID = a.ID,
                                         NgayTao = a.NgayTao,
                                         NgayThanhToan = a.NgayThanhToan,
                                         TenNguoiNhan = a.TenNguoiNhan,
                                         SDT = a.SDT,
                                         Email = a.Email,
                                         DiaChi = a.DiaChi,
                                         TienShip = a.TienShip,
                                         PhuongThucThanhToan = a.PhuongThucThanhToan,
                                         TrangThaiGiaoHang = a.TrangThaiGiaoHang,
                                         //Diem = b.Diem,
                                         //TrangThaiLichSuTichDiem = b.TrangThai,
                                         IDCTHD = c.ID,
                                         DonGia = c.DonGia,
                                         SoLuong = c.SoLuong,
                                         TenKichCo = f.Ten,
                                         TenMau = g.Ten,
                                         TenSanPham = i.Ten,
                                         DuongDan = context.Anhs.First(x => x.IDMauSac == g.ID && x.IDSanPham == i.ID).DuongDan,
                                         HinhThucGiamGia = a.IDVoucher == null ? null : (context.Vouchers.First(x=>x.ID == a.IDVoucher)).HinhThucGiamGia,
                                         GiaTri = a.IDVoucher == null ? null : (context.Vouchers.First(x => x.ID == a.IDVoucher)).GiaTri,
                                         TrangThaiDanhGia = d.TrangThai,
                                         LoaiHoaDon = a.LoaiHD,
                                         lichSuTichDiems = context.LichSuTichDiems.Where(p=>p.IDHoaDon == a.ID).ToList(),
                                         TiLeTieuDiem = context.QuyDoiDiems.FirstOrDefault(p=>p.ID == context.LichSuTichDiems.FirstOrDefault(p => p.IDHoaDon == a.ID).IDQuyDoiDiem).TiLeTieuDiem,
                                         TiLeTichDiem = context.QuyDoiDiems.FirstOrDefault(p => p.ID == context.LichSuTichDiems.FirstOrDefault(p => p.IDHoaDon == a.ID).IDQuyDoiDiem).TiLeTichDiem,
                                      }).ToListAsync();
                return lstDonMuaCT;
            }
            catch
            {
                return new List<DonMuaChiTietViewModel>();
            }
        }

        public async Task<ChiTietHoaDonDanhGiaViewModel> getCTHDDanhGia(Guid idcthd)
        {
            try
            {
                var getCTHDDanhGia = (from a in context.ChiTietHoaDons.Where(p=>p.ID == idcthd)
                                        join b in context.ChiTietSanPhams on a.IDCTSP equals b.ID
                                        join c in context.KichCos on b.IDKichCo equals c.ID
                                        join d in context.MauSacs on b.IDMauSac equals d.ID
                                        join e in context.SanPhams on b.IDSanPham equals e.ID
                                        join g in context.HoaDons on a.IDHoaDon equals g.ID
                                       
                                        select new ChiTietHoaDonDanhGiaViewModel()
                                        {
                                            ID = a.ID,
                                            TenSanPham = e.Ten,
                                            TenMau = d.Ten,
                                            TenKichThuoc = c.Ten,
                                            DuongDan = context.Anhs.First(x => x.IDMauSac == b.IDMauSac && x.IDSanPham == e.ID).DuongDan,
                                            IDHoaDon = g.ID
                                            
                                        }).FirstOrDefault();
                return getCTHDDanhGia;
            }
            catch
            {
                return new ChiTietHoaDonDanhGiaViewModel();
            }
        }

        public async Task<List<LichSuTichDiemTieuDiemViewModel>> GetALLLichSuTichDiembyIdUser(Guid idKhachHang)
        {
            try
            {
                var lstLichSuTichDiem = await(from a in context.LichSuTichDiems
                                      join b in context.HoaDons on a.IDHoaDon equals b.ID
                                      select new LichSuTichDiemTieuDiemViewModel()
                                      {
                                          IDHoaDon = b.ID,
                                          NgayTao = b.NgayTao,
                                          NgayThanhToan = b.NgayThanhToan,
                                          NgayNhanHang = b.NgayNhanHang,
                                          Ngaytao1 = null,
                                          Ngaynhanhang1 = null,
                                          Ngaythanhtoan1 = null,
                                          TenNguoiNhan = b.TenNguoiNhan,
                                          TrangThaiGiaoHang = b.TrangThaiGiaoHang,
                                          IdNguoiDung = context.LichSuTichDiems.FirstOrDefault(p => p.IDHoaDon == b.ID) != null ? context.LichSuTichDiems.FirstOrDefault(p => p.IDHoaDon == b.ID).IDKhachHang.Value : null,
                                          MaHD = b.MaHD,
                                          TrangThaiLSTD = a.TrangThai,
                                          Diem = a.Diem,
                                          LoaiHoaDon = b.LoaiHD
                                      }).ToListAsync();

                lstLichSuTichDiem = lstLichSuTichDiem.Where(P => P.IdNguoiDung == idKhachHang).ToList();
                return lstLichSuTichDiem;
            }
            catch
            {
                return new List<LichSuTichDiemTieuDiemViewModel>();
            }
        }

        private object await(IQueryable<DonMuaViewModel> donMuaViewModels)
        {
            throw new NotImplementedException();
        }
    }
}
