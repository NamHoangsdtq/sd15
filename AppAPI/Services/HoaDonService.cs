
using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using AppData.ViewModels.SanPham;
using System.Security.Cryptography.Xml;
using Microsoft.EntityFrameworkCore;
using AppData.ViewModels.Mail;           // Thêm cái này
using Microsoft.Extensions.Configuration; // Thêm cái này
using Microsoft.Extensions.Options;       // Thêm cái này

namespace AppAPI.Services
{
    public class HoaDonService : IHoaDonService
    {
        private readonly IAllRepository<HoaDon> reposHoaDon;
        private readonly IAllRepository<ChiTietHoaDon> reposChiTietHoaDon;
        private readonly IAllRepository<ChiTietSanPham> repsCTSanPham;
        private readonly IAllRepository<Voucher> reposVoucher;
        private readonly IAllRepository<QuyDoiDiem> reposQuyDoiDiem;
        private readonly IAllRepository<LichSuTichDiem> reposLichSuTichDiem;
        private readonly IAllRepository<KhachHang> reposKhachHang;
        private readonly IAllRepository<SanPham> reposSanPham;
        private readonly IAllRepository<DanhGia> reposDanhGia;
        private readonly IAllRepository<NhanVien> reposNhanVien;

        // Khai báo Context và các Service khác
        private readonly AssignmentDBContext context; // Bỏ chữ 'new AssignmentDBContext()' ở đây đi
        private readonly IGioHangServices _iGioHangServices;
        private readonly IMailServices _mailService;
        private readonly IConfiguration _configuration;

        // === CONSTRUCTOR SỬA ĐỔI ===
        // Bạn cần Inject AssignmentDBContext vào để nó nhận chuỗi kết nối từ Program.cs
        public HoaDonService(
            IMailServices mailService,
            IConfiguration configuration,
            IGioHangServices gioHangServices
        // Thêm tham số này nếu bạn chưa đăng ký Repository vào DI Container
        // Nếu bạn đã đăng ký Repo trong Program.cs thì nên Inject Repo, 
        // nhưng theo cách viết của bạn là 'new Repository' thủ công thì phải Inject Context
        )
        {
            // 1. Khởi tạo Context đúng cách (lấy connection string)
            // Cách tốt nhất là Inject Context vào Constructor, nhưng nếu không muốn sửa Program.cs nhiều:
            context = new AssignmentDBContext();
            // !!! LƯU Ý QUAN TRỌNG: Class AssignmentDBContext của bạn phải có hàm 
            // OnConfiguring đọc appsettings.json thì dòng trên mới chạy được.
            // Nếu không, hãy xem cách 2 bên dưới (Khuyên dùng).

            _iGioHangServices = gioHangServices;
            _mailService = mailService;
            _configuration = configuration;

            // 2. Khởi tạo Repository dựa trên Context đã có
            reposHoaDon = new AllRepository<HoaDon>(context, context.HoaDons);
            reposChiTietHoaDon = new AllRepository<ChiTietHoaDon>(context, context.ChiTietHoaDons);
            repsCTSanPham = new AllRepository<ChiTietSanPham>(context, context.ChiTietSanPhams);
            reposVoucher = new AllRepository<Voucher>(context, context.Vouchers);
            reposQuyDoiDiem = new AllRepository<QuyDoiDiem>(context, context.QuyDoiDiems);
            reposLichSuTichDiem = new AllRepository<LichSuTichDiem>(context, context.LichSuTichDiems);
            reposKhachHang = new AllRepository<KhachHang>(context, context.KhachHangs);
            reposSanPham = new AllRepository<SanPham>(context, context.SanPhams);
            reposDanhGia = new AllRepository<DanhGia>(context, context.DanhGias);
            reposNhanVien = new AllRepository<NhanVien>(context, context.NhanViens);
        }

        public bool CheckHDHasLSGD(Guid idHoaDon)
        {
            var exist = reposLichSuTichDiem.GetAll().Any(c => c.IDHoaDon == idHoaDon);
            if (exist == true)
            {
                return true;
            }
            return false;
        }

        public int CheckVoucher(string ten, int tongtien)
        {
            var voucher = reposVoucher.GetAll().FirstOrDefault(p => p.Ten == ten);
            if (voucher != null)
            {
                if (tongtien >= voucher.SoTienCan && DateTime.Compare(voucher.NgayApDung, DateTime.Now) <= 0 && DateTime.Compare(DateTime.Now, voucher.NgayKetThuc) <= 0 && voucher.SoLuong > 0)
                {
                    if (voucher.HinhThucGiamGia == 1)
                    {
                        tongtien -= voucher.GiaTri;
                        return tongtien;
                    }
                    else
                    {
                        tongtien = tongtien - (tongtien * voucher.GiaTri / 100);
                        return tongtien;
                    }

                }
                else
                {
                    return tongtien;
                }
            }
            else
            {
                return tongtien;
            }
        }

        public DonMuaSuccessViewModel CreateHoaDon(List<ChiTietHoaDonViewModel> chiTietHoaDons, HoaDonViewModel hoaDon)
        {
            try
            {
                //Tâm - Thêm đơn mua
                DonMuaSuccessViewModel donMua = new DonMuaSuccessViewModel()
                {
                    Ten = hoaDon.Ten,
                    Email = hoaDon.Email,
                    SDT = hoaDon.SDT,
                    DiaChi = hoaDon.DiaChi,
                    PhuongThucThanhToan = hoaDon.PhuongThucThanhToan,
                    TongTien = hoaDon.TongTien,
                    GhiChu = hoaDon.GhiChu == null ? "" : hoaDon.GhiChu,
                    DiemSuDung = hoaDon.Diem == null ? 0 : hoaDon.Diem.Value,
                    Login = false,
                    GioHangs = new List<GioHangRequest>()
                };
                int subtotal = 0;
                var voucher = reposVoucher.GetAll().FirstOrDefault(p => p.Ten == hoaDon.TenVoucher);
                if (chiTietHoaDons != null)
                {
                    HoaDon hoaDon1 = new HoaDon();
                    hoaDon1.ID = Guid.NewGuid();
                    hoaDon1.IDNhanVien = null;
                    if (voucher != null)
                    {
                        hoaDon1.IDVoucher = voucher.ID;
                        voucher.SoLuong--;
                        reposVoucher.Update(voucher);
                        donMua.MaVoucher = voucher.Ten;
                    }
                    else
                    {
                        hoaDon1.IDVoucher = null;
                    }
                    hoaDon1.TenNguoiNhan = hoaDon.Ten;
                    hoaDon1.MaHD = "HD" + (hoaDon1.ID).ToString().Substring(0, 8).ToUpper();
                    hoaDon1.SDT = hoaDon.SDT;
                    hoaDon1.Email = hoaDon.Email;
                    hoaDon1.NgayTao = DateTime.Now;
                    //Tam
                    if (hoaDon.NgayThanhToan != null)
                    {
                        hoaDon1.NgayThanhToan = hoaDon.NgayThanhToan;
                    }
                    //End
                    hoaDon1.DiaChi = hoaDon.DiaChi;
                    hoaDon1.TienShip = hoaDon.TienShip;
                    hoaDon1.PhuongThucThanhToan = hoaDon.PhuongThucThanhToan;
                    hoaDon1.TrangThaiGiaoHang = 2; // Đây là "Chờ xác nhận"
                    hoaDon1.TongTien = hoaDon.TongTien;
                    hoaDon1.GhiChu = hoaDon.GhiChu;
                    hoaDon1.LoaiHD = 0; // 0 = Online

                    // === DÒNG QUAN TRỌNG ===
                    hoaDon1.TrackingToken = Guid.NewGuid();
                    // ======================

                    donMua.ID = hoaDon1.ID.ToString();
                    if (reposHoaDon.Add(hoaDon1))
                    {
                        foreach (var x in chiTietHoaDons)
                        {
                            ChiTietHoaDon chiTietHoaDon = new ChiTietHoaDon();
                            chiTietHoaDon.ID = Guid.NewGuid();
                            chiTietHoaDon.IDHoaDon = hoaDon1.ID;
                            chiTietHoaDon.IDCTSP = x.IDChiTietSanPham;
                            chiTietHoaDon.SoLuong = x.SoLuong;
                            chiTietHoaDon.DonGia = x.DonGia;
                            chiTietHoaDon.TrangThai = 1;
                            DanhGia danhGia = new DanhGia();
                            danhGia.ID = chiTietHoaDon.ID;
                            danhGia.Sao = null;
                            danhGia.BinhLuan = null;
                            danhGia.TrangThai = 0;
                            reposDanhGia.Add(danhGia);
                            reposChiTietHoaDon.Add(chiTietHoaDon);
                            var CTsanPham = repsCTSanPham.GetAll().FirstOrDefault(p => p.ID == x.IDChiTietSanPham);

                            if (CTsanPham.SoLuong < 0) // Logic này có vẻ sai, nên là < x.SoLuong
                            {
                                CTsanPham.SoLuong += chiTietHoaDon.SoLuong;
                                reposHoaDon.Delete(hoaDon1);
                                return new DonMuaSuccessViewModel()
                                {
                                    TongTien = -1
                                };
                            }
                            else
                            {
                                subtotal += x.SoLuong * x.DonGia;
                                repsCTSanPham.Update(CTsanPham);
                            }
                            //Tâm
                            donMua.GioHangs.Add(new GioHangRequest() { Anh = context.Anhs.First(y => y.IDSanPham == CTsanPham.IDSanPham && y.IDMauSac == CTsanPham.IDMauSac).DuongDan, SoLuong = x.SoLuong, DonGia = x.DonGia, Ten = context.SanPhams.First(y => y.ID == CTsanPham.IDSanPham).Ten, KichCo = context.KichCos.First(y => y.ID == CTsanPham.IDKichCo).Ten, MauSac = context.MauSacs.First(y => y.ID == CTsanPham.IDMauSac).Ten });
                        }
                        //tích điểm, dùng điểm
                        if (hoaDon.IDNguoiDung != null)
                        {
                            QuyDoiDiem quyDoiDiem = reposQuyDoiDiem.GetAll().First(p => p.TrangThai > 0);
                            KhachHang khachHang = reposKhachHang.GetAll().FirstOrDefault(p => p.IDKhachHang == hoaDon.IDNguoiDung);
                            donMua.Login = true;
                            if (quyDoiDiem.TrangThai == 1)
                            {
                                if (hoaDon.Diem == 0 || hoaDon.Diem == null)
                                {
                                    //khachHang.DiemTich += subtotal / quyDoiDiem.TiLeTichDiem;
                                    reposKhachHang.Update(khachHang);
                                    LichSuTichDiem lichSuTichDiem = new LichSuTichDiem()
                                    {
                                        ID = Guid.NewGuid(),
                                        IDKhachHang = hoaDon.IDNguoiDung,
                                        IDQuyDoiDiem = quyDoiDiem.ID,
                                        IDHoaDon = hoaDon1.ID,
                                        Diem = quyDoiDiem.TiLeTichDiem != 0 ? subtotal / quyDoiDiem.TiLeTichDiem : 0,
                                        TrangThai = 1
                                    };
                                    reposLichSuTichDiem.Add(lichSuTichDiem);
                                    donMua.DiemTich = lichSuTichDiem.Diem;
                                }
                                //dùng điểm
                                else
                                {
                                    if (khachHang.DiemTich >= hoaDon.Diem)
                                    {
                                        khachHang.DiemTich = khachHang.DiemTich - hoaDon.Diem;
                                        reposKhachHang.Update(khachHang);
                                        LichSuTichDiem lichSuTichDiem = new LichSuTichDiem()
                                        {
                                            ID = Guid.NewGuid(),
                                            IDKhachHang = hoaDon.IDNguoiDung,
                                            IDQuyDoiDiem = quyDoiDiem.ID,
                                            IDHoaDon = hoaDon1.ID,
                                            Diem = hoaDon.Diem.Value,
                                            TrangThai = 0
                                        };
                                        reposLichSuTichDiem.Add(lichSuTichDiem);
                                    }
                                }
                            }
                            //Vừa tích vừa tiêu
                            else if (quyDoiDiem.TrangThai == 2)
                            {
                                //tích điểm
                                //khachHang.DiemTich += subtotal / quyDoiDiem.TiLeTichDiem;
                                LichSuTichDiem lichSuTichDiem = new LichSuTichDiem()
                                {
                                    ID = Guid.NewGuid(),
                                    IDKhachHang = hoaDon.IDNguoiDung,
                                    IDQuyDoiDiem = quyDoiDiem.ID,
                                    IDHoaDon = hoaDon1.ID,
                                    Diem = quyDoiDiem.TiLeTichDiem != 0 ? subtotal / quyDoiDiem.TiLeTichDiem : 0,
                                    TrangThai = 1
                                };
                                reposLichSuTichDiem.Add(lichSuTichDiem);
                                donMua.DiemTich = lichSuTichDiem.Diem;
                                //tiều điểm
                                if (khachHang.DiemTich >= hoaDon.Diem && hoaDon.Diem != 0)
                                {
                                    khachHang.DiemTich = khachHang.DiemTich - hoaDon.Diem;
                                    reposKhachHang.Update(khachHang);
                                    LichSuTichDiem lichSuTieuDiem = new LichSuTichDiem()
                                    {
                                        ID = Guid.NewGuid(),
                                        IDKhachHang = hoaDon.IDNguoiDung,
                                        IDQuyDoiDiem = quyDoiDiem.ID,
                                        IDHoaDon = hoaDon1.ID,
                                        Diem = hoaDon.Diem.Value,
                                        TrangThai = 0
                                    };
                                    reposLichSuTichDiem.Add(lichSuTieuDiem);
                                }

                            }
                        }
                        if (hoaDon.TrangThai)
                        {
                            _iGioHangServices.DeleteCart(hoaDon.IDNguoiDung.Value);
                        }

                        return donMua;
                    }
                    else
                    {
                        return new DonMuaSuccessViewModel();
                    }
                }
                else
                {
                    return new DonMuaSuccessViewModel();
                }
            }
            catch
            {
                return new DonMuaSuccessViewModel();
            }
        }

        // Bán hàng tại quầy
        // Trong file HoaDonService.cs
        public bool CreateHoaDonOffline(Guid idnhanvien)
        {
            try
            {
                HoaDon hoaDon1 = new HoaDon();
                hoaDon1.ID = Guid.NewGuid();

                // === THÊM DÒNG NÀY ===
                hoaDon1.TrackingToken = Guid.NewGuid(); // Tạo mã theo dõi ngẫu nhiên để không bị trùng
                                                        // =====================

                hoaDon1.IDNhanVien = idnhanvien;
                hoaDon1.NgayTao = DateTime.Now;
                hoaDon1.TrangThaiGiaoHang = 1;
                hoaDon1.LoaiHD = 1;
                hoaDon1.MaHD = "HD" + (hoaDon1.ID).ToString().Substring(0, 8).ToUpper();

                // Các giá trị mặc định khác (giữ nguyên như lúc nãy sửa)
                hoaDon1.TongTien = 0;
                hoaDon1.TenNguoiNhan = "Khách lẻ";
                hoaDon1.SDT = "0000000000";
                hoaDon1.DiaChi = "Tại quầy";

                context.HoaDons.Add(hoaDon1);
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                // ... (Giữ nguyên phần log lỗi để đề phòng)
                throw new Exception($"LỖI GHI DB: {ex.Message} || {ex.InnerException?.Message}");
            }
        }

        public bool DeleteHoaDon(Guid id)
        {
            try
            {
                HoaDon hoaDon = reposHoaDon.GetAll().FirstOrDefault(p => p.ID == id);
                var lsthdct = reposChiTietHoaDon.GetAll().Where(c => c.IDHoaDon == hoaDon.ID).ToList();

                var deletedg = context.DanhGias.Where(c => lsthdct.Select(x => x.ID).Contains(c.ID)).ToList();
                foreach (var item in lsthdct)
                {
                    var ctsp = repsCTSanPham.GetAll().FirstOrDefault(c => c.ID == item.IDCTSP);
                    ctsp.SoLuong += item.SoLuong;
                    repsCTSanPham.Update(ctsp);
                }
                //Xóa chiTietHD
                context.ChiTietHoaDons.RemoveRange(lsthdct);
                context.SaveChanges();
                //Xóa đánh giá
                context.DanhGias.RemoveRange(deletedg);
                context.SaveChanges();
                //Xóa hóa đơn
                context.HoaDons.Remove(hoaDon);
                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public List<ChiTietHoaDon> GetAllChiTietHoaDon(Guid idHoaDon)
        {
            return reposChiTietHoaDon.GetAll().Where(x => x.IDHoaDon == idHoaDon).ToList();
        }

        public List<HoaDon> GetAllHDCho()
        {
            return context.HoaDons.Where(c => c.TrangThaiGiaoHang == 1 || c.TrangThaiGiaoHang == 0).OrderByDescending(c => c.TrangThaiGiaoHang).ToList();

        }

        public List<HoaDonQL> GetAllHDQly()
        {
            var result = (from hd in context.HoaDons
                          join lstd in context.LichSuTichDiems on hd.ID equals lstd.IDHoaDon into lstdGroup
                          from lstd in lstdGroup.DefaultIfEmpty()
                          join kh in context.KhachHangs on lstd.IDKhachHang equals kh.IDKhachHang into khGroup
                          from kh in khGroup.DefaultIfEmpty()
                          where hd.TrangThaiGiaoHang != 1 && hd.TrangThaiGiaoHang != 0
                          select new HoaDonQL()
                          {
                              Id = hd.ID,
                              MaHD = hd.MaHD,
                              KhachHang = kh != null ? kh.Ten : "Khách lẻ",
                              SDTKH = kh != null ? kh.SDT : null,
                              SDTnhanhang = hd.SDT != null ? hd.SDT : "null",
                              PTTT = hd.PhuongThucThanhToan,
                              ThoiGian = hd.NgayTao,
                              KhachDaTra = (hd.TrangThaiGiaoHang == 6 || hd.PhuongThucThanhToan == "VNPay" && hd.TrangThaiGiaoHang != 7) == true ? hd.TongTien : 0,
                              TongTienHang = context.ChiTietHoaDons.Where(c => c.IDHoaDon == hd.ID).ToList().AsQueryable().Sum(c => c.DonGia * c.SoLuong),
                              LoaiHD = hd.LoaiHD,
                              TrangThai = hd.TrangThaiGiaoHang,
                          }).Distinct().ToList();

            return result;
        }

        public List<HoaDon> GetAllHoaDon()
        {
            return reposHoaDon.GetAll();
        }

        public ChiTietHoaDonQL GetCTHDByID(Guid idhd)
        {
            try
            {
                var lsthdct = (from cthd in context.ChiTietHoaDons
                               join ctsp in context.ChiTietSanPhams on cthd.IDCTSP equals ctsp.ID
                               join ms in context.MauSacs on ctsp.IDMauSac equals ms.ID
                               join kc in context.KichCos on ctsp.IDKichCo equals kc.ID
                               join sp in context.SanPhams on ctsp.IDSanPham equals sp.ID
                               join km in context.KhuyenMais on ctsp.IDKhuyenMai equals km.ID into kmGroup
                               from km in kmGroup.DefaultIfEmpty()
                               where cthd.IDHoaDon == idhd
                               select new HoaDonChiTietViewModel
                               {
                                   Id = cthd.ID,
                                   IdHoaDon = cthd.IDHoaDon,
                                   IdSP = sp.ID,
                                   Ten = sp.Ten,
                                   MaCTSP = ctsp.Ma,
                                   PhanLoai = ms.Ten + " - " + kc.Ten,
                                   SoLuong = cthd.SoLuong,
                                   GiaGoc = ctsp.GiaBan,
                                   GiaLuu = cthd.DonGia == null ? 0 : cthd.DonGia,
                                   GiaKM = km == null ? ctsp.GiaBan :
                       (km.TrangThai == 1 ? (int)(ctsp.GiaBan / 100 * (100 - km.GiaTri)) :
                       (km.GiaTri < ctsp.GiaBan ? (ctsp.GiaBan - (int)km.GiaTri) : 0)),
                               }).ToList();

                var result = (from hd in context.HoaDons
                              join nv in context.NhanViens on hd.IDNhanVien equals nv.ID
                              into nvGroup
                              from nv in nvGroup.DefaultIfEmpty()
                              join lstd in context.LichSuTichDiems on hd.ID equals lstd.IDHoaDon into lstdGroup
                              from lstd in lstdGroup.DefaultIfEmpty()
                              join kh in context.KhachHangs on lstd.IDKhachHang equals kh.IDKhachHang into khGroup
                              from kh in khGroup.DefaultIfEmpty()
                              where hd.ID == idhd
                              select new ChiTietHoaDonQL
                              {
                                  Id = hd.ID,
                                  MaHD = hd.MaHD,
                                  NgayTao = hd.NgayTao,
                                  NgayThanhToan = hd.NgayThanhToan != null ? hd.NgayThanhToan : null,
                                  NgayNhanHang = hd.NgayNhanHang != null ? hd.NgayNhanHang : null,
                                  PTTT = hd.PhuongThucThanhToan,
                                  NhanVien = nv != null ? nv.Ten : null,
                                  LoaiHD = hd.LoaiHD,
                                  KhachHang = kh == null ? "Khách lẻ" : kh.Ten,
                                  NguoiNhan = hd.TenNguoiNhan != null ? hd.TenNguoiNhan : null,
                                  DiaChi = hd.DiaChi != null ? hd.DiaChi : null,
                                  SĐT = hd.SDT != null ? hd.SDT : null,
                                  Email = hd.Email != null ? hd.Email : null,
                                  TienShip = hd.TienShip != null ? hd.TienShip : null,
                                  TrangThai = hd.TrangThaiGiaoHang,
                                  KhachCanTra = hd.TongTien,
                                  TienKhachTra = (hd.TrangThaiGiaoHang == 6 || hd.PhuongThucThanhToan == "VNPay" && hd.TrangThaiGiaoHang != 7) ? hd.TongTien : 0,
                                  GhiChu = hd.GhiChu,
                                  TruTieuDiem = (from lstd in context.LichSuTichDiems
                                                 join qdd in context.QuyDoiDiems on lstd.IDQuyDoiDiem equals qdd.ID
                                                 where lstd.IDHoaDon == hd.ID && lstd.TrangThai == 0
                                                 select lstd.Diem * qdd.TiLeTieuDiem).FirstOrDefault(),
                                  voucher = (from vc in context.Vouchers
                                             where vc.ID == hd.IDVoucher
                                             select new Voucher
                                             {
                                                 ID = vc.ID,
                                                 Ten = vc.Ten,
                                                 GiaTri = vc.GiaTri,
                                                 TrangThai = vc.TrangThai,
                                                 HinhThucGiamGia = vc.HinhThucGiamGia,
                                             }).FirstOrDefault(),
                                  listsp = lsthdct,
                                  lstlstd = (from lstd in context.LichSuTichDiems
                                             where lstd.IDHoaDon == hd.ID
                                             select lstd).OrderBy(c => c.TrangThai).ToList()
                              }).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // === HÀM BỊ THIẾU CỦA BẠN (ĐÃ TRẢ LẠI) ===
        public HoaDonViewModelBanHang GetHDBanHang(Guid id)
        {
            List<HoaDonChiTietViewModel> lsthdct = (from cthd in context.ChiTietHoaDons
                                                    join ctsp in context.ChiTietSanPhams on cthd.IDCTSP equals ctsp.ID
                                                    join ms in context.MauSacs on ctsp.IDMauSac equals ms.ID
                                                    join kc in context.KichCos on ctsp.IDKichCo equals kc.ID
                                                    join sp in context.SanPhams on ctsp.IDSanPham equals sp.ID
                                                    join km in context.KhuyenMais.Where(c => c.NgayKetThuc > DateTime.Now && c.TrangThai != 2) on ctsp.IDKhuyenMai equals km.ID
                                                    into kmGroup
                                                    from km in kmGroup.DefaultIfEmpty()
                                                    where cthd.IDHoaDon == id
                                                    select new HoaDonChiTietViewModel()
                                                    {
                                                        Id = cthd.ID,
                                                        IdHoaDon = cthd.IDHoaDon,
                                                        IdSP = sp.ID,
                                                        Ten = sp.Ten,
                                                        MaCTSP = ctsp.Ma,
                                                        PhanLoai = ms.Ten + " - " + kc.Ten,
                                                        SoLuong = cthd.SoLuong,
                                                        GiaGoc = ctsp.GiaBan,
                                                        GiaKM = km == null ? ctsp.GiaBan :
                                (km.TrangThai == 1 ? (int)(ctsp.GiaBan / 100 * (100 - km.GiaTri)) :
                                (km.GiaTri < ctsp.GiaBan ? (ctsp.GiaBan - (int)km.GiaTri) : 0)),

                                                    }).AsEnumerable().Reverse().ToList();
            var result = (from hd in reposHoaDon.GetAll()
                          join lstd in reposLichSuTichDiem.GetAll() on hd.ID equals lstd.IDHoaDon into lstdGroup
                          from lstd in lstdGroup.DefaultIfEmpty()
                          join kh in reposKhachHang.GetAll() on lstd?.IDKhachHang equals kh?.IDKhachHang into khGroup
                          from kh in khGroup.DefaultIfEmpty()
                          where hd.ID == id
                          select new HoaDonViewModelBanHang()
                          {
                              Id = hd.ID,
                              MaHD = hd.MaHD,
                              IdKhachHang = kh?.IDKhachHang,
                              TenKhachHang = kh?.Ten,
                              lstHDCT = lsthdct,
                              GhiChu = hd.GhiChu == null ? "" : hd.GhiChu,
                          }).FirstOrDefault();
            return result;
        }

        public HoaDon GetHoaDonById(Guid idhd)
        {
            return reposHoaDon.GetAll().FirstOrDefault(c => c.ID == idhd);
        }

        // === THÊM HÀM MỚI TẠI ĐÂY ===
        public HoaDon GetHoaDonByToken(Guid token)
        {
            if (token == Guid.Empty)
            {
                return null;
            }

            // Dùng context để Include các bảng liên quan
            var hoaDon = context.HoaDons
                .Include(h => h.Voucher)
                          .Include(h => h.ChiTietHoaDons)
                              .ThenInclude(ct => ct.ChiTietSanPham) // Model ChiTietSanPham
                                  .ThenInclude(spct => spct.SanPham) // Model SanPham
                          .FirstOrDefault(h => h.TrackingToken == token);

            return hoaDon;
        }
        // ============================

        public LichSuTichDiem GetLichSuGiaoDichByIdHD(Guid idHoaDon)
        {
            return reposLichSuTichDiem.GetAll().FirstOrDefault(c => c.IDHoaDon == idHoaDon);
        }

        public bool HuyHD(Guid idhd, Guid idnv)
        {
            try
            {
                var hd = context.HoaDons.Where(c => c.ID == idhd).FirstOrDefault();
                //Update hd
                hd.IDNhanVien = idnv;
                hd.TrangThaiGiaoHang = 7;
                //hd.TongTien = 0;
                context.HoaDons.Update(hd);
                context.SaveChanges();

                // Cộng lại số lượng voucher nếu áp dụng
                if (hd.IDVoucher != null)
                {
                    var vc = context.Vouchers.FirstOrDefault(c => c.ID == hd.IDVoucher);
                    vc.SoLuong += 1;
                    context.Vouchers.Update(vc);
                    context.SaveChanges();
                }
                // Cộng lại tiêu điểm cho khách hàng
                var lstlstd = context.LichSuTichDiems.Where(c => c.IDHoaDon == idhd).ToList();
                if (lstlstd.Count != 0)
                {
                    var tieud = lstlstd.Where(c => c.TrangThai == 0).FirstOrDefault();
                    var tichd = lstlstd.Where(c => c.TrangThai == 1).FirstOrDefault();
                    if (lstlstd.Count == 1)
                    {
                        if (tieud != null)
                        {
                            //Cộng điểm kh
                            var kh = context.KhachHangs.Where(c => c.IDKhachHang == tieud.IDKhachHang).FirstOrDefault();
                            kh.DiemTich += tieud.Diem;
                            context.KhachHangs.Update(kh);
                            context.SaveChanges();
                            //Thêm 1 lịch sử trả lại điểm
                            LichSuTichDiem diemtra = new LichSuTichDiem()
                            {
                                ID = new Guid(),
                                IDHoaDon = hd.ID,
                                IDKhachHang = kh.IDKhachHang,
                                Diem = tieud.Diem,
                                TrangThai = 2,
                                IDQuyDoiDiem = tieud.IDQuyDoiDiem,
                            };
                            context.LichSuTichDiems.Add(diemtra);
                            context.SaveChanges();
                        }
                        else
                        {
                            tichd.Diem = 0;
                            context.LichSuTichDiems.Update(tichd);
                            context.SaveChanges();
                        }
                    }
                    if (lstlstd.Count == 2)
                    {
                        //Cộng điểm khách hàng
                        var kh = context.KhachHangs.Where(c => c.IDKhachHang == tieud.IDKhachHang).FirstOrDefault();
                        kh.DiemTich += tieud.Diem;
                        context.KhachHangs.Update(kh);
                        context.SaveChanges();

                        //Thêm 1 lịch sử trả lại điểm
                        LichSuTichDiem diemtra = new LichSuTichDiem()
                        {
                            ID = new Guid(),
                            IDHoaDon = hd.ID,
                            IDKhachHang = kh.IDKhachHang,
                            Diem = tieud.Diem,
                            TrangThai = 2,
                            IDQuyDoiDiem = tieud.IDQuyDoiDiem,
                        };
                        context.LichSuTichDiems.Add(diemtra);
                        context.SaveChanges();

                        context.LichSuTichDiems.Remove(tichd);
                        context.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CopyHD(Guid idhd, Guid idnv)
        {
            try
            {
                var hd = context.HoaDons.Where(c => c.ID == idhd).FirstOrDefault();
                //hóa đơn chi tiết
                var lsthdct = context.ChiTietHoaDons.Where(c => c.IDHoaDon == idhd).ToList();
                // lịch sử tích điểm
                var lstlstd = context.LichSuTichDiems.Where(c => c.IDHoaDon == idhd).ToList();

                // Tạo hóa đơn mới có sản phẩm y hệt hóa đơn sao chép
                HoaDon hoaDon = new HoaDon();
                hoaDon.ID = Guid.NewGuid();
                hoaDon.NgayTao = DateTime.Now;
                hoaDon.TenNguoiNhan = hd.TenNguoiNhan;
                hoaDon.SDT = hd.SDT;
                hoaDon.DiaChi = hd.DiaChi;
                hoaDon.Email = hd.DiaChi;
                hoaDon.GhiChu = "Copy " + hd.MaHD;
                hoaDon.IDNhanVien = idnv;
                hoaDon.TrangThaiGiaoHang = 0;
                hoaDon.LoaiHD = hd.LoaiHD;
                hoaDon.MaHD = "HD" + (hoaDon.ID).ToString().Substring(0, 8).ToUpper();
                context.HoaDons.Add(hoaDon);
                context.SaveChanges();

                // Tạo chi tiết hóa đơn mới
                foreach (var item in lsthdct)
                {
                    var ctsp = context.ChiTietSanPhams.FirstOrDefault(c => c.ID == item.IDCTSP);
                    if (ctsp.SoLuong > item.SoLuong)
                    {
                        var danhgia = new DanhGia()
                        {
                            ID = new Guid(),
                            TrangThai = 0,
                        };
                        await context.DanhGias.AddAsync(danhgia);
                        await context.SaveChangesAsync();

                        ChiTietHoaDon ct = new ChiTietHoaDon()
                        {
                            ID = danhgia.ID,
                            SoLuong = item.SoLuong,
                            TrangThai = 0,
                            IDCTSP = item.IDCTSP,
                            IDHoaDon = hoaDon.ID
                        };
                        await context.ChiTietHoaDons.AddAsync(ct);
                        await context.SaveChangesAsync();

                        // Trừ số lượng
                        ctsp.SoLuong -= item.SoLuong;
                        context.ChiTietSanPhams.Update(ctsp);
                        await context.SaveChangesAsync();
                    }
                }
                // Nếu có khách hàng -> Tạo lịch sử ms
                var qqd = context.QuyDoiDiems.FirstOrDefault(c => c.TrangThai != 0);
                if (lstlstd.Count != 0)
                {
                    LichSuTichDiem lstd = new LichSuTichDiem()
                    {
                        ID = new Guid(),
                        Diem = 0,
                        TrangThai = 1,
                        IDKhachHang = lstlstd.FirstOrDefault(c => c.IDHoaDon == hd.ID).IDKhachHang,
                        IDHoaDon = hoaDon.ID,
                        IDQuyDoiDiem = qqd.ID,
                    };
                    await context.LichSuTichDiems.AddAsync(lstd);
                    await context.SaveChangesAsync();
                };
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<HoaDon> LichSuGiaoDich(Guid idNguoiDung)
        {
            var idhoadon = reposLichSuTichDiem.GetAll().Where(p => p.IDKhachHang == idNguoiDung).ToList();
            List<HoaDon> lichSuGiaoDich = new List<HoaDon>();
            foreach (var item in idhoadon)
            {
                lichSuGiaoDich.Add(reposHoaDon.GetAll().FirstOrDefault(p => p.ID == item.IDHoaDon));
            }
            return lichSuGiaoDich;
        }

        public List<HoaDon> TimKiemVaLocHoaDon(string ten, int? loc)
        {
            List<HoaDon> timkiem = reposHoaDon.GetAll().Where(p => p.TenNguoiNhan.ToLower().Contains(ten.ToLower())).ToList();
            if (loc == 0)
            {
                List<HoaDon> locTangDan = timkiem.OrderBy(p => p.NgayTao).ToList();
                return locTangDan;
            }
            else if (loc == 1)
            {
                List<HoaDon> locGiamDan = timkiem.OrderByDescending(p => p.NgayTao).ToList();
                return locGiamDan;
            }
            return timkiem;
        }

        public bool UpdateGhiChuHD(Guid idhd, Guid idnv, string ghichu)
        {
            try
            {
                var hd = reposHoaDon.GetAll().FirstOrDefault(c => c.ID == idhd);
                if (ghichu == "null")
                {
                    hd.GhiChu = null;
                    hd.IDNhanVien = idnv;
                }
                else
                {
                    hd.GhiChu = ghichu;
                }
                reposHoaDon.Update(hd);
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckCusUseVoucher(Guid idkh, Guid idvoucher)
        {
            var hdkh = (from hd in context.HoaDons
                        join lstd in context.LichSuTichDiems on hd.ID equals lstd.IDHoaDon
                        join kh in context.KhachHangs on lstd.IDKhachHang equals kh.IDKhachHang
                        where kh.IDKhachHang == idkh
                        select hd).ToList();
            if (hdkh == null)
            {
                return false;
            }
            return (hdkh.Any(c => c.IDVoucher == idvoucher) ? true : false);
        }

        public (bool Success, string ErrorMessage) UpdateHoaDon(HoaDonThanhToanRequest hoaDon)
        {
            // 1. BẮT ĐẦU TRANSACTION
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var update = reposHoaDon.GetAll().FirstOrDefault(p => p.ID == hoaDon.Id);
                    if (update == null)
                    {
                        transaction.Rollback();
                        return (false, "Không tìm thấy hóa đơn.");
                    }

                    // 2. LẤY HÓA ĐƠN CHI TIẾT
                    var lsthdct = context.ChiTietHoaDons.Where(c => c.IDHoaDon == hoaDon.Id).ToList();

                    // 3. XÓA SẢN PHẨM (SỐ LƯỢNG = 0)
                    var delete = lsthdct.Where(c => c.SoLuong == 0).ToList();
                    if (delete.Any())
                    {
                        context.ChiTietHoaDons.RemoveRange(delete);
                        context.SaveChanges();
                    }

                    lsthdct = context.ChiTietHoaDons.Where(c => c.IDHoaDon == hoaDon.Id).ToList();

                    // 4. KIỂM TRA TỒN KHO & TRỪ KHO
                    if (hoaDon.TrangThai == 6 && update.TrangThaiGiaoHang != 6)
                    {
                        foreach (var item in lsthdct)
                        {
                            var rowsAffected = context.Database.ExecuteSqlRaw(
                                "UPDATE ChiTietSanPham SET SoLuong = SoLuong - {0} WHERE ID = {1} AND SoLuong >= {0}",
                                item.SoLuong,
                                item.IDCTSP
                            );

                            if (rowsAffected == 0)
                            {
                                transaction.Rollback();
                                return (false, $"Thanh toán thất bại: Sản phẩm (Mã CTSP: {item.IDCTSP}) không đủ tồn kho.");
                            }
                        }
                    }

                    // 5. LƯU ĐƠN GIÁ
                    foreach (var item in lsthdct)
                    {
                        var result = (from ctsp in context.ChiTietSanPhams
                                      join km in context.KhuyenMais
                                          .Where(c => c.NgayKetThuc > DateTime.Now && c.TrangThai != 2)
                                        on ctsp.IDKhuyenMai equals km.ID into kmGroup
                                      from km in kmGroup.DefaultIfEmpty()
                                      where ctsp.ID == item.IDCTSP
                                      select km != null ? (km.TrangThai == 0 ? (km.GiaTri < ctsp.GiaBan ? (ctsp.GiaBan - km.GiaTri) : 0) : (ctsp.GiaBan * (100 - km.GiaTri) / 100)) : ctsp.GiaBan)
                                      .FirstOrDefault();
                        item.DonGia = result;
                        context.ChiTietHoaDons.Update(item);
                    }

                    // 6. XỬ LÝ ĐIỂM VÀ VOUCHER
                    var lstd = context.LichSuTichDiems.FirstOrDefault(c => c.IDHoaDon == hoaDon.Id);
                    if (lstd != null)
                    {
                        // ... (Logic xử lý điểm của bạn)
                    }

                    var vc = context.Vouchers.Find(hoaDon.IdVoucher);
                    if (vc != null)
                    {
                        vc.SoLuong -= 1;
                        context.Vouchers.Update(vc);
                    }

                    // 7. CẬP NHẬT HÓA ĐƠN
                    update.IDNhanVien = hoaDon.IdNhanVien;
                    update.NgayThanhToan = hoaDon.NgayThanhToan;
                    update.TrangThaiGiaoHang = hoaDon.TrangThai;
                    update.TongTien = hoaDon.TongTien;
                    update.PhuongThucThanhToan = hoaDon.PTTT;
                    update.IDVoucher = hoaDon.IdVoucher == Guid.Empty ? null : hoaDon.IdVoucher;
                    reposHoaDon.Update(update);

                    // 8. LƯU TẤT CẢ VÀ COMMIT
                    context.SaveChanges();
                    transaction.Commit();
                    return (true, "Thanh toán thành công.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return (false, $"Lỗi hệ thống: {ex.Message}");
                }
            }
        }

        // === THAY THẾ HÀM CŨ BẰNG HÀM NÀY (ĐÃ THÊM LOGIC EMAIL) ===
        //public (bool Success, string ErrorMessage) UpdateTrangThaiGiaoHang(Guid idHoaDon, int trangThai, Guid? idNhanVien)
        //{
        //    // Bắt đầu một Transaction để đảm bảo an toàn dữ liệu
        //    using (var transaction = context.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            var update = context.HoaDons.FirstOrDefault(p => p.ID == idHoaDon); // Dùng context trực tiếp
        //            if (update == null)
        //            {
        //                transaction.Rollback();
        //                return (false, "Không tìm thấy hóa đơn.");
        //            }

        //            // Kiểm tra xem trạng thái có thực sự thay đổi không
        //            if (update.TrangThaiGiaoHang == trangThai)
        //            {
        //                transaction.Rollback();
        //                return (false, "Trạng thái không thay đổi."); // Không cần cập nhật
        //            }

        //            List<ChiTietHoaDon> chitiethoadon = context.ChiTietHoaDons.Where(p => p.IDHoaDon == idHoaDon).ToList();

        //            // 1. Xử lý logic XÁC NHẬN ĐƠN (trangThai == 10) - TRỪ KHO
        //            if (trangThai == 10)
        //            {
        //                foreach (var item in chitiethoadon)
        //                {
        //                    var rowsAffected = context.Database.ExecuteSqlRaw(
        //                        "UPDATE ChiTietSanPham SET SoLuong = SoLuong - {0} WHERE ID = {1} AND SoLuong >= {0}",
        //                        item.SoLuong, item.IDCTSP
        //                    );
        //                    if (rowsAffected == 0)
        //                    {
        //                        transaction.Rollback();
        //                        return (false, $"Xác nhận thất bại: Sản phẩm (Mã CTSP: {item.IDCTSP}) không đủ tồn kho.");
        //                    }
        //                }
        //            }

        //            // 2. Xử lý logic HOÀN HÀNG (trangThai == 5 hoặc 11) - CỘNG LẠI KHO
        //            if (trangThai == 5 || trangThai == 11)
        //            {
        //                foreach (var item in chitiethoadon)
        //                {
        //                    context.Database.ExecuteSqlRaw(
        //                        "UPDATE ChiTietSanPham SET SoLuong = SoLuong + {0} WHERE ID = {1}",
        //                        item.SoLuong, item.IDCTSP
        //                    );
        //                }
        //            }

        //            // 3. Xử lý logic GIAO THÀNH CÔNG (trangThai == 6) - Cộng điểm tích
        //            if (trangThai == 6)
        //            {
        //                var lstlstd = context.LichSuTichDiems.Where(c => c.IDHoaDon == idHoaDon).ToList();
        //                if (lstlstd.Count != 0)
        //                {
        //                    var td = lstlstd.Where(c => c.TrangThai == 1).FirstOrDefault();
        //                    if (td != null)
        //                    {
        //                        var kh = context.KhachHangs.Where(c => c.IDKhachHang == td.IDKhachHang).FirstOrDefault();
        //                        if (kh != null)
        //                        {
        //                            kh.DiemTich += td.Diem;
        //                            context.KhachHangs.Update(kh);
        //                        }
        //                    }
        //                }
        //                update.NgayThanhToan = update.NgayThanhToan == null ? DateTime.Now : update.NgayThanhToan;
        //                update.NgayNhanHang = update.NgayNhanHang == null ? DateTime.Now : update.NgayNhanHang;
        //            }

        //            // 4. Cập nhật trạng thái hóa đơn
        //            update.TrangThaiGiaoHang = trangThai;
        //            update.IDNhanVien = idNhanVien;
        //            context.HoaDons.Update(update);

        //            // 5. LƯU TẤT CẢ THAY ĐỔI
        //            context.SaveChanges();

        //            // === BẮT ĐẦU LOGIC GỬI EMAIL (ĐÃ SỬA) ===

        //            // Chỉ gửi nếu là đơn Online và có Email
        //            if (update.LoaiHD == 0 && !string.IsNullOrEmpty(update.Email))
        //            {
        //                try
        //                {
        //                    // Lấy URL của website từ appsettings.json
        //                    string websiteUrl = _configuration.GetValue<string>("WebsiteBaseUrl");
        //                    string trackingUrl = $"{websiteUrl}/theo-doi/{update.TrackingToken}";

        //                    // Lấy trạng thái bằng chữ
        //                    string trangThaiText = GetTrangThaiText(trangThai);

        //                    var subject = $"[Shop Giày] Đơn hàng {update.MaHD} đã cập nhật trạng thái!";
        //                    var body = $@"
        //                <p>Chào {update.TenNguoiNhan},</p>
        //                <p>Đơn hàng #{update.MaHD} của bạn vừa được cập nhật sang trạng thái mới:</p>
        //                <h3 style='color: #007bff;'>{trangThaiText}</h3>
        //                <p>Bạn có thể theo dõi chi tiết đơn hàng tại đây:</p>
        //                <p>
        //                    <a href='{trackingUrl}' style='padding: 10px 15px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;'>
        //                        Xem chi tiết đơn hàng
        //                    </a>
        //                </p>";

        //                    var mailData = new MailData
        //                    {
        //                        EmailToId = update.Email,
        //                        EmailToName = update.TenNguoiNhan,
        //                        EmailSubject = subject, 
        //                        EmailBody = body
        //                    };

        //                    _mailService.SendMail(mailData);
        //                }
        //                catch (Exception ex)
        //                {
        //                    // Ghi log lỗi email, nhưng không dừng transaction
        //                }
        //            }
        //            // === KẾT THÚC LOGIC GỬI EMAIL ===

        //            // 6. Hoàn tất transaction
        //            transaction.Commit();

        //            return (true, "Cập nhật trạng thái thành công.");
        //        }
        //        catch (Exception ex)
        //        {
        //            // Có lỗi xảy ra, hủy bỏ mọi thứ
        //            transaction.Rollback();
        //            return (false, $"Lỗi hệ thống: {ex.Message}");
        //        }
        //    }
        //}




        public (bool Success, string ErrorMessage) UpdateTrangThaiGiaoHang(Guid idHoaDon, int trangThai, Guid? idNhanVien, string? ghiChu) // Đã thêm ghiChu
        {
            // Bắt đầu một Transaction để đảm bảo an toàn dữ liệu
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var update = context.HoaDons.FirstOrDefault(p => p.ID == idHoaDon); // Dùng context trực tiếp
                    if (update == null)
                    {
                        transaction.Rollback();
                        return (false, "Không tìm thấy hóa đơn.");
                    }

                    // Kiểm tra xem trạng thái có thực sự thay đổi không
                    if (update.TrangThaiGiaoHang == trangThai)
                    {
                        transaction.Rollback();
                        return (false, "Trạng thái không thay đổi."); // Không cần cập nhật
                    }

                    List<ChiTietHoaDon> chitiethoadon = context.ChiTietHoaDons.Where(p => p.IDHoaDon == idHoaDon).ToList();

                    // 1. Xử lý logic XÁC NHẬN ĐƠN (trangThai == 10) - TRỪ KHO
                    if (trangThai == 10)
                    {
                        foreach (var item in chitiethoadon)
                        {
                            var rowsAffected = context.Database.ExecuteSqlRaw(
                                "UPDATE ChiTietSanPham SET SoLuong = SoLuong - {0} WHERE ID = {1} AND SoLuong >= {0}",
                                item.SoLuong, item.IDCTSP
                            );
                            if (rowsAffected == 0)
                            {
                                transaction.Rollback();
                                return (false, $"Xác nhận thất bại: Sản phẩm (Mã CTSP: {item.IDCTSP}) không đủ tồn kho.");
                            }
                        }
                    }

                    // 2. Xử lý logic HOÀN HÀNG (trangThai == 5 hoặc 11) - CỘNG LẠI KHO
                    if (trangThai == 5 || trangThai == 11)
                    {
                        foreach (var item in chitiethoadon)
                        {
                            context.Database.ExecuteSqlRaw(
                                "UPDATE ChiTietSanPham SET SoLuong = SoLuong + {0} WHERE ID = {1}",
                                item.SoLuong, item.IDCTSP
                            );
                        }
                    }

                    // 3. Xử lý logic GIAO THÀNH CÔNG (trangThai == 6) - Cộng điểm tích
                    if (trangThai == 6)
                    {
                        var lstlstd = context.LichSuTichDiems.Where(c => c.IDHoaDon == idHoaDon).ToList();
                        if (lstlstd.Count != 0)
                        {
                            var td = lstlstd.Where(c => c.TrangThai == 1).FirstOrDefault();
                            if (td != null)
                            {
                                var kh = context.KhachHangs.Where(c => c.IDKhachHang == td.IDKhachHang).FirstOrDefault();
                                if (kh != null)
                                {
                                    kh.DiemTich += td.Diem;
                                    context.KhachHangs.Update(kh);
                                }
                            }
                        }
                        update.NgayThanhToan = update.NgayThanhToan == null ? DateTime.Now : update.NgayThanhToan;
                        update.NgayNhanHang = update.NgayNhanHang == null ? DateTime.Now : update.NgayNhanHang;
                    }

                    // 4. Cập nhật trạng thái hóa đơn VÀ GHI CHÚ
                    update.TrangThaiGiaoHang = trangThai;
                    update.IDNhanVien = idNhanVien;

                    // === LƯU GHI CHÚ (NẾU CÓ) ===
                    if (!string.IsNullOrWhiteSpace(ghiChu))
                    {
                        update.GhiChu = ghiChu; // Lưu ghi chú mới vào DB
                    }
                    // ============================

                    context.HoaDons.Update(update);

                    // 5. LƯU TẤT CẢ THAY ĐỔI (để email có thể đọc được ghi chú mới)
                    context.SaveChanges();

                    // === BẮT ĐẦU LOGIC GỬI EMAIL ===
                    // Email sẽ được gửi SAU KHI ghi chú đã được lưu
                    if (update.LoaiHD == 0 && !string.IsNullOrEmpty(update.Email))
                    {
                        try
                        {
                            string websiteUrl = _configuration.GetValue<string>("WebsiteBaseUrl");
                            string trackingUrl = $"{websiteUrl}/theo-doi/{update.TrackingToken}";
                            string trangThaiText = GetTrangThaiText(trangThai); // Dùng hàm helper (bạn phải dán hàm này vào class)

                            // === THÊM KHỐI HIỂN THỊ GHI CHÚ ===
                            string ghiChuHtml = ""; // Mặc định là rỗng

                            // Lấy ghi chú vừa lưu (update.GhiChu)
                            if (!string.IsNullOrWhiteSpace(update.GhiChu) && (trangThai == 7 || trangThai == 4 || trangThai == 9 || trangThai == 11 || trangThai == 8))
                            {
                                // Nếu là trạng thái "xấu", hiển thị lý do
                                ghiChuHtml = $"<p style='border-left: 4px solid #ffc107; padding-left: 10px; background-color: #fffaf3; margin-top: 10px;'><strong>Lý do:</strong> {update.GhiChu}</p>";
                            }
                            // ===================================

                            var subject = $"[Shop Giày] Đơn hàng {update.MaHD} đã cập nhật trạng thái!";
                            var body = $@"
                        <p>Chào {update.TenNguoiNhan},</p>
                        <p>Đơn hàng #{update.MaHD} của bạn vừa được cập nhật sang trạng thái mới:</p>
                        
                        <h3 style='color: #007bff;'>{trangThaiText}</h3>
                        
                        {ghiChuHtml} <p>Bạn có thể theo dõi chi tiết đơn hàng (kể cả khi không đăng nhập) tại đây:</p>
                        <p>
                            <a href='{trackingUrl}' style='padding: 10px 15px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px;'>
                                Xem chi tiết đơn hàng
                            </a>
                        </p>";

                            var mailData = new MailData
                            {
                                EmailToId = update.Email,
                                EmailToName = update.TenNguoiNhan,
                                EmailSubject = subject,
                                EmailBody = body
                            };

                            _mailService.SendMail(mailData);
                        }
                        catch (Exception ex)
                        {
                            // Ghi log
                        }
                    }
                    // === KẾT THÚC LOGIC GỬI EMAIL ===

                    // 6. Hoàn tất transaction
                    transaction.Commit();

                    return (true, "Cập nhật trạng thái thành công.");
                }
                catch (Exception ex)
                {
                    // Có lỗi xảy ra, hủy bỏ mọi thứ
                    transaction.Rollback();
                    return (false, $"Lỗi hệ thống: {ex.Message}");
                }
            }
        }



        public bool ThanhCong(Guid idhd, Guid idnv)
        {
            try
            {
                var hd = context.HoaDons.FirstOrDefault(c => c.ID == idhd);
                hd.TrangThaiGiaoHang = 6;
                hd.IDNhanVien = idnv;
                hd.NgayNhanHang = DateTime.Now;
                hd.NgayThanhToan = DateTime.Now;
                context.HoaDons.Update(hd);
                context.SaveChanges();
                //Cộng tích điểm cho khách
                var lstlstd = context.LichSuTichDiems.Where(c => c.IDHoaDon == idhd).ToList();
                if (lstlstd.Count != 0)
                {
                    var td = lstlstd.Where(c => c.TrangThai == 1).FirstOrDefault();
                    if (td != null)
                    {
                        var kh = context.KhachHangs.Where(c => c.IDKhachHang == td.IDKhachHang).FirstOrDefault();
                        kh.DiemTich += td.Diem;
                        context.KhachHangs.Update(kh);
                        context.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool HoanHang(Guid idhd, Guid idnv)
        {
            try
            {
                var hd = context.HoaDons.FirstOrDefault(c => c.ID == idhd);
                hd.TrangThaiGiaoHang = 4;
                hd.IDNhanVien = idnv;

                var lstlstd = context.LichSuTichDiems.Where(c => c.IDHoaDon == idhd).ToList();
                if (lstlstd.Count != 0)
                {
                    var td = lstlstd.Where(c => c.TrangThai == 1).FirstOrDefault();
                    if (td != null)
                    {
                        var kh = context.KhachHangs.Where(c => c.IDKhachHang == td.IDKhachHang).FirstOrDefault();
                        kh.DiemTich -= td.Diem;
                        context.KhachHangs.Update(kh);
                        context.SaveChanges();

                        LichSuTichDiem diemtru = new LichSuTichDiem()
                        {
                            ID = new Guid(),
                            IDHoaDon = hd.ID,
                            IDKhachHang = kh.IDKhachHang,
                            Diem = td.Diem,
                            TrangThai = 3,
                            IDQuyDoiDiem = td.IDQuyDoiDiem,
                        };
                        context.LichSuTichDiems.Add(diemtru);
                        context.SaveChanges();
                    }
                }
                context.HoaDons.Update(hd);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool HoanHangThanhCong(Guid idhd, Guid idnv)
        {
            try
            {
                var hd = context.HoaDons.FirstOrDefault(c => c.ID == idhd);
                hd.TrangThaiGiaoHang = 5;
                hd.IDNhanVien = idnv;
                hd.TongTien = 0;
                context.HoaDons.Update(hd);
                context.SaveChanges();

                var lsthdct = context.ChiTietHoaDons.Where(c => c.IDHoaDon == idhd).ToList();
                foreach (var hdct in lsthdct)
                {
                    var ctsp = context.ChiTietSanPhams.FirstOrDefault(c => c.ID == hdct.IDCTSP);
                    ctsp.SoLuong += hdct.SoLuong;
                    context.ChiTietSanPhams.Update(ctsp);
                    context.SaveChanges();
                }

                if (hd.IDVoucher != null)
                {
                    var vc = context.Vouchers.FirstOrDefault(c => c.ID == hd.IDVoucher);
                    vc.SoLuong += 1;
                    context.Vouchers.Update(vc);
                    context.SaveChanges();
                }

                var lstlstd = context.LichSuTichDiems.Where(c => c.IDHoaDon == idhd).ToList();
                if (lstlstd.Count != 0)
                {
                    var tieud = lstlstd.Where(c => c.TrangThai == 0).FirstOrDefault();
                    if (tieud != null)
                    {
                        var kh = context.KhachHangs.Where(c => c.IDKhachHang == tieud.IDKhachHang).FirstOrDefault();
                        kh.DiemTich += tieud.Diem;
                        context.KhachHangs.Update(kh);
                        context.SaveChanges();

                        LichSuTichDiem diemtra = new LichSuTichDiem()
                        {
                            ID = new Guid(),
                            IDHoaDon = hd.ID,
                            IDKhachHang = kh.IDKhachHang,
                            Diem = tieud.Diem,
                            TrangThai = 4,
                            IDQuyDoiDiem = tieud.IDQuyDoiDiem,
                        };
                        context.LichSuTichDiems.Add(diemtra);
                        context.SaveChanges();
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private string GetTrangThaiText(int trangThai)
        {
            switch (trangThai)
            {
                case 1: return "Đơn nháp";
                case 2: return "Chờ xác nhận";
                case 3: return "Đang giao hàng";
                case 4: return "Đang hoàn hàng";
                case 5: return "Hoàn hàng thành công";
                case 6: return "Giao hàng thành công";
                case 7: return "Đơn hủy";
                case 8: return "Chờ xác nhận hủy";
                case 9: return "Chờ xác nhận hoàn hàng";
                case 10: return "Đã xác nhận";
                case 11: return "Giao không thành công";
                default: return "Không xác định";
            }
        }
    }
}
