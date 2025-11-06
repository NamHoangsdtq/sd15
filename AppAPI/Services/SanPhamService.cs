using AppAPI.IServices;
using AppData.Models;
using AppData.ViewModels;
using AppData.ViewModels.BanOffline;
using AppData.ViewModels.SanPham;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class SanPhamService : ISanPhamService
    {
        private readonly AssignmentDBContext _context;
        public SanPhamService()
        {
            _context = new AssignmentDBContext();
        }

           #region SanPham
        public async Task<bool> UpdateSanPham(SanPhamUpdateRequest request)
        {
            try
            {
                var sanpham = await _context.SanPhams.FirstAsync(x => x.ID == request.ID);
                LoaiSP? loaiSPCon = _context.LoaiSPs.Where(x => x.IDLoaiSPCha != null).FirstOrDefault(x => x.Ten == request.TenLoaiSPCon);
                ChatLieu? chatLieu = _context.ChatLieus.FirstOrDefault(x => x.Ten == request.TenChatLieu);
                if (loaiSPCon == null)
                {
                    LoaiSP? loaiSPCha = _context.LoaiSPs.Where(x => x.IDLoaiSPCha == null).FirstOrDefault(x => x.Ten == request.TenLoaiSPCha);
                    if (loaiSPCha == null)
                    {
                        loaiSPCha = new LoaiSP() { ID = Guid.NewGuid(), Ten = request.TenLoaiSPCha, TrangThai = 1 };
                        _context.LoaiSPs.AddAsync(loaiSPCha);
                    }
                    loaiSPCon = new LoaiSP() { ID = Guid.NewGuid(), Ten = request.TenLoaiSPCon, IDLoaiSPCha = loaiSPCha.ID, TrangThai = 1 };
                    await _context.LoaiSPs.AddAsync(loaiSPCon);
                }
                if (chatLieu == null)
                {
                    chatLieu = new ChatLieu() { ID = Guid.NewGuid(), Ten = request.TenChatLieu, TrangThai = 1 };
                    await _context.AddAsync(chatLieu);
                }
                sanpham.Ten = request.Ten;
                sanpham.MoTa = request.MoTa;
                sanpham.IDChatLieu = chatLieu.ID;
                sanpham.IDLoaiSP = loaiSPCon.ID;
                _context.SanPhams.Update(sanpham);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Task<List<SanPhamViewModel>> TimKiemSanPham(SanPhamTimKiemNangCao sp)
        {
            throw new NotImplementedException();
        }
        public Task<List<SanPhamViewModel>> GetSanPhamByIdDanhMuc(Guid idloaisp)
        {
            throw new NotImplementedException();
        }
        public async Task<SanPhamUpdateRequest> GetSanPhamById(Guid id)
        {
            try
            {
                var sanPham = await _context.SanPhams.FirstAsync(x => x.ID == id);
                var loaiSP = await _context.LoaiSPs.FirstAsync(x => x.ID == sanPham.IDLoaiSP);
                var response = new SanPhamUpdateRequest()
                {
                    ID = sanPham.ID,
                    Ten = sanPham.Ten,
                    MoTa = sanPham.MoTa,
                    TenChatLieu = _context.ChatLieus.First(x => x.ID == sanPham.IDChatLieu).Ten,
                    TenLoaiSPCha = _context.LoaiSPs.First(x => x.ID == loaiSP.IDLoaiSPCha).Ten,
                    TenLoaiSPCon = loaiSP.Ten
                };
                return response;
            }
            catch
            {
                return new SanPhamUpdateRequest();
            }
        }
        public List<SanPhamViewModelAdmin> GetAllSanPhamAdmin()
        {
            try
            {
                KhuyenMai? khuyenMai;
                List<KhuyenMai> khuyenMais = _context.KhuyenMais.Where(x => x.NgayKetThuc > DateTime.Now && x.NgayApDung < DateTime.Now).ToList();
                //var lstSanPham = (from a in _context.SanPhams
                //                  join b in _context.ChiTietSanPhams.Where(x => x.TrangThai == 1) on a.ID equals b.IDSanPham into sanpham
                //                  from sp in sanpham.DefaultIfEmpty()
                //                  join e in _context.LoaiSPs.Where(x => x.LoaiSPCha != null) on a.IDLoaiSP equals e.ID
                //                  select new SanPhamViewModelAdmin()
                //                  {
                //                      ID = a.ID,
                //                      Ten = a.Ten,
                //                      Ma = a.Ma,
                //                      TrangThai = a.TrangThai,
                //                      LoaiSPCha = _context.LoaiSPs.First(x => x.ID == e.IDLoaiSPCha).Ten,
                //                      LoaiSPCon = e.Ten,
                //                      Anh = sp == null ? "" : _context.Anhs.First(x => x.IDMauSac == sp.IDMauSac && x.IDSanPham == a.ID).DuongDan,
                //                      ChatLieu = _context.ChatLieus.First(x => x.ID == a.IDChatLieu).Ten,
                //                      GiaGoc = sp == null ? -999 : sp.GiaBan,
                //                      SoLuong = _context.ChiTietSanPhams
                //               .Where(x => x.IDSanPham == a.ID && (x.TrangThai == 1 || x.TrangThai == 2))
                //               .Sum(x => (int?)x.SoLuong) ?? 0, // Tính tổng số lượng
                //                      IDKhuyenMai = sp == null ? null : sp.IDKhuyenMai,
                //                  }).ToList();
                var lstSanPham = (from a in _context.SanPhams
                                  join b in _context.ChiTietSanPhams.Where(x => x.TrangThai == 1) on a.ID equals b.IDSanPham into sanpham
                                  from sp in sanpham.DefaultIfEmpty()
                                  join e in _context.LoaiSPs.Where(x => x.LoaiSPCha != null) on a.IDLoaiSP equals e.ID
                                  select new SanPhamViewModelAdmin()
                                  {
                                      ID = a.ID,
                                      Ten = a.Ten,
                                      Ma = a.Ma,
                                      TrangThai = a.TrangThai,
                                      LoaiSPCha = _context.LoaiSPs.First(x => x.ID == e.IDLoaiSPCha).Ten,
                                      LoaiSPCon = e.Ten,
                                      // Dùng FirstOrDefault để an toàn hơn
                                      Anh = sp == null ? "" : _context.Anhs.FirstOrDefault(x => x.IDMauSac == sp.IDMauSac && x.IDSanPham == a.ID).DuongDan,
                                      ChatLieu = _context.ChatLieus.First(x => x.ID == a.IDChatLieu).Ten,

                                      // SỬA LỖI Ở ĐÂY: Thay -999 bằng 0
                                      GiaGoc = sp == null ? 0 : sp.GiaBan,

                                      SoLuong = _context.ChiTietSanPhams
                             .Where(x => x.IDSanPham == a.ID && (x.TrangThai == 1 || x.TrangThai == 2))
                             .Sum(x => (int?)x.SoLuong) ?? 0, // Tính tổng số lượng
                                      IDKhuyenMai = sp == null ? null : sp.IDKhuyenMai,
                                  }).ToList();
                foreach (var item in lstSanPham)
                {
                    if (item.IDKhuyenMai != null)
                    {
                        khuyenMai = khuyenMais.FirstOrDefault(x => x.ID == item.IDKhuyenMai);
                        if (khuyenMai != null)
                        {
                            item.GiaBan = GetKhuyenMai(khuyenMai.GiaTri, item.GiaGoc, khuyenMai.TrangThai);

                        }
                        else
                        {
                            item.GiaBan = item.GiaGoc;
                        }
                    }
                    else
                    {
                        item.GiaBan = item.GiaGoc;
                    }
                }
                return lstSanPham;
            }
            catch
            {
                return new List<SanPhamViewModelAdmin>();
            }
        }
        public async Task<List<SanPhamViewModel>> GetAllSanPham()
        {
            try
            {
                KhuyenMai? khuyenMai;
                List<KhuyenMai> khuyenMais = _context.KhuyenMais.Where(x => x.NgayKetThuc > DateTime.Now).ToList();
                var lstSanPham = await (from a in _context.SanPhams.Where(x => x.TrangThai == 1)
                                        join b in _context.ChiTietSanPhams.Where(x => x.TrangThai != 0) on a.ID equals b.IDSanPham
                                        join e in _context.LoaiSPs.Where(x => x.LoaiSPCha != null) on a.IDLoaiSP equals e.ID
                                        select new SanPhamViewModel()
                                        {
                                            ID = a.ID,
                                            Ten = a.Ten,
                                            TrangThai = a.TrangThai,
                                            TrangThaiCTSP = b.TrangThai,
                                            LoaiSP = e.Ten,
                                            IdChiTietSanPham = b.ID,
                                            Image = _context.Anhs.First(x => x.IDMauSac == b.IDMauSac && x.IDSanPham == a.ID).DuongDan,
                                            IDMauSac = b.IDMauSac,
                                            IDKichCo = b.IDKichCo,
                                            IDChatLieu = a.IDChatLieu,
                                            GiaGoc = b.GiaBan,
                                            SoLuong = b.SoLuong,
                                            soSao = (from cthd in _context.ChiTietHoaDons.AsNoTracking()
                                                     join ctsp in _context.ChiTietSanPhams.AsNoTracking()
                                                     on cthd.IDCTSP equals ctsp.ID
                                                     join dg in _context.DanhGias.AsNoTracking()
                                                     on cthd.ID equals dg.ID
                                                     where ctsp.IDSanPham == a.ID
                                                     select dg).AsEnumerable().ToList().Average(c => c.Sao),
                                            IDKhuyenMai = b.IDKhuyenMai,
                                            NgayTao = b.NgayTao
                                        }).ToListAsync();
                foreach (var item in lstSanPham)
                {
                    if (item.IDKhuyenMai != null)
                    {
                        khuyenMai = khuyenMais.FirstOrDefault(x => x.ID == item.IDKhuyenMai);
                        if (khuyenMai != null)
                        {
                            item.GiaBan = GetKhuyenMai(khuyenMai.GiaTri, item.GiaGoc.Value, khuyenMai.TrangThai);
                            item.TrangThaiKM = khuyenMai.TrangThai;
                            item.GiaTriKM = khuyenMai.GiaTri;
                        }
                        else
                        {
                            item.GiaBan = item.GiaGoc.Value;
                        }
                    }
                    else
                    {
                        item.GiaBan = item.GiaGoc.Value;
                    }
                }
                return lstSanPham;
            }
            catch
            {
                return new List<SanPhamViewModel>();
            }
        }
        public bool CheckTrungTenSP(SanPhamRequest lsp)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> UpdateTrangThaiSanPham(Guid id, int trangThai)
        {
            try
            {
                var sanPham = await _context.SanPhams.FirstAsync(x => x.ID == id);
                sanPham.TrangThai = trangThai;
                var chitietsanpham = _context.ChiTietSanPhams.Where(x => x.IDSanPham == id).ToList();
                foreach (var item in chitietsanpham)
                {
                    item.SoLuong = 0;
                }
                _context.SanPhams.Update(sanPham);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<ChiTietSanPhamUpdateRequest> AddSanPham(SanPhamRequest request)
        {
            try
            {
                List<ChiTietSanPhamRequest> lst = new List<ChiTietSanPhamRequest>();
                LoaiSP? loaiSPCon = _context.LoaiSPs.Where(x => x.IDLoaiSPCha != null).FirstOrDefault(x => x.Ten.ToUpper() == request.TenLoaiSPCon.Trim().ToUpper());
                ChatLieu? chatLieu = _context.ChatLieus.FirstOrDefault(x => x.Ten.ToUpper() == request.TenChatLieu.Trim().ToUpper());
                if (loaiSPCon == null)
                {
                    LoaiSP? loaiSPCha = _context.LoaiSPs.Where(x => x.IDLoaiSPCha == null).FirstOrDefault(x => x.Ten.ToUpper() == request.TenLoaiSPCha.Trim().ToUpper());
                    if (loaiSPCha == null)
                    {
                        loaiSPCha = new LoaiSP() { ID = Guid.NewGuid(), Ten = request.TenLoaiSPCha.Trim(), TrangThai = 1 };
                        _context.LoaiSPs.AddAsync(loaiSPCha);
                    }
                    loaiSPCon = new LoaiSP() { ID = Guid.NewGuid(), Ten = request.TenLoaiSPCon.Trim(), IDLoaiSPCha = loaiSPCha.ID, TrangThai = 1 };
                    await _context.LoaiSPs.AddAsync(loaiSPCon);
                }
                if (chatLieu == null)
                {
                    chatLieu = new ChatLieu() { ID = Guid.NewGuid(), Ten = request.TenChatLieu.Trim(), TrangThai = 1 };
                    await _context.AddAsync(chatLieu);
                }
                var max = 0;
                if (_context.SanPhams.Any())
                {
                    max = _context.SanPhams.Max(x => Convert.ToInt32(x.Ma.Substring(2)));
                }
                SanPham sanPham = new SanPham() { ID = Guid.NewGuid(), Ten = request.Ten, Ma = "SP" + (max + 1), MoTa = request.MoTa, TrangThai = 1, IDLoaiSP = loaiSPCon.ID, IDChatLieu = chatLieu.ID };
                await _context.SanPhams.AddAsync(sanPham);
                await _context.SaveChangesAsync();
                foreach (var x in request.MauSacs)
                {
                    foreach (var y in request.KichCos)
                    {
                        lst.Add(CreateChiTietSanPhamFromSanPham(x, y, null).Result);
                    }
                }
                return new ChiTietSanPhamUpdateRequest() { IDSanPham = sanPham.ID, ChiTietSanPhams = lst.GroupBy(x => new { MauSac = x.IDMauSac, KichCo = x.IDKichCo }).Select(y => y.First()).ToList(), Ma = sanPham.Ma, Location = 0 };
            }
            catch { return new ChiTietSanPhamUpdateRequest(); }
        }
        public async Task<bool> AddAnhToSanPham(List<AnhRequest> request)
        {
            try
            {
                foreach (var x in request)
                {
                    Anh anh = new Anh() { ID = Guid.NewGuid(), DuongDan = x.DuongDan, IDSanPham = x.IDSanPham, IDMauSac = _context.MauSacs.First(y => y.Ma == x.MaMau).ID, TrangThai = 1 };
                    _context.Anhs.Add(anh);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<AnhViewModel> GetAllAnhSanPham(Guid idSanPham)
        {
            try
            {
                var lst = (from a in _context.Anhs.Where(x => x.IDSanPham == idSanPham)
                           join b in _context.MauSacs on a.IDMauSac equals b.ID
                           select new AnhViewModel()
                           {
                               ID = a.ID,
                               DuongDan = a.DuongDan,
                               TenMau = b.Ten,
                               MaMau = b.Ma
                           }).ToList();
                return lst;
            }
            catch
            {
                return new List<AnhViewModel>();
            }
        }

        public async Task<bool> AddImageNoColor(Anh anh)
        {
            try
            {
                _context.Anhs.Add(anh);
                await _context.SaveChangesAsync();
                return true;

            }
            catch { return false; }
        }
        public async Task<bool> UpdateImage(Anh anh)
        {
            try
            {
                var temp = _context.Anhs.First(x => x.ID == anh.ID);
                temp.DuongDan = anh.DuongDan;
                _context.Anhs.Update(temp);
                await _context.SaveChangesAsync();
                return true;

            }
            catch { return false; }
        }
        public async Task<bool> DeleteImage(Guid id)
        {
            try
            {
                var temp = _context.Anhs.First(x => x.ID == id);
                if (temp.IDMauSac != null)
                {
                    temp.DuongDan = "";
                    _context.Anhs.Update(temp);
                }
                else _context.Anhs.Remove(temp);
                await _context.SaveChangesAsync();
                return true;

            }
            catch { return false; }
        }
        #endregion

        #region ChiTietSanPham
        public async Task<ChiTietSanPhamUpdateRequest> AddChiTietSanPham(ChiTietSanPhamAddRequest request)
        {
            try
            {
                var lstChiTietSanPham = await _context.ChiTietSanPhams.Where(x => x.IDSanPham == request.IDSanPham).ToListAsync();
                List<ChiTietSanPhamRequest> lst = new List<ChiTietSanPhamRequest>();
                List<MauSac> mauSac = new List<MauSac>();
                ChiTietSanPhamRequest? chiTietSanPham;
                foreach (var x in request.MauSacs)
                {
                    foreach (var y in request.KichCos)
                    {
                        chiTietSanPham = CreateChiTietSanPhamFromSanPham(x, y, lstChiTietSanPham).Result;
                        if (chiTietSanPham != null)
                        {
                            lst.Add(chiTietSanPham);
                        }
                    }
                    if (_context.Anhs.FirstOrDefault(item => item.IDSanPham == request.IDSanPham && item.IDMauSac == _context.MauSacs.First(z => z.Ma == x.Ma).ID) == null)
                    {
                        mauSac.Add(x);
                    }
                }
                return new ChiTietSanPhamUpdateRequest() { IDSanPham = request.IDSanPham, ChiTietSanPhams = lst.GroupBy(x => new { MauSac = x.IDMauSac, KichCo = x.IDKichCo }).Select(y => y.First()).ToList(), Location = 1, MauSacs = mauSac, Ma = _context.SanPhams.First(x => x.ID == request.IDSanPham).Ma };
            }
            catch { return new ChiTietSanPhamUpdateRequest(); }
        }
        public ChiTietSanPhamViewModel? GetChiTietSanPhamByID(Guid id)
        {
            try
            {
                var temp = _context.ChiTietSanPhams.FirstOrDefault(x => x.ID == id);
                if (temp != null)
                {
                    var anh = _context.Anhs.FirstOrDefault(x => x.IDMauSac == temp.IDMauSac && x.IDSanPham == temp.IDSanPham);
                    ChiTietSanPhamViewModel chiTietSanPham = new ChiTietSanPhamViewModel()
                    {
                        ID = temp.ID,
                        Ten = _context.SanPhams.First(x => x.ID == temp.IDSanPham).Ten,
                        SoLuong = temp.SoLuong,
                        GiaGoc = temp.GiaBan,
                        TrangThai = _context.SanPhams.First(x => x.ID == temp.IDSanPham).TrangThai == 0 ? 0 :
                        temp.TrangThai,
                        Anh = anh != null ? anh.DuongDan : null,
                        MauSac = _context.MauSacs.First(x => x.ID == temp.IDMauSac).Ten,
                        KichCo = _context.KichCos.First(x => x.ID == temp.IDKichCo).Ten
                    };
                    var khuyenMai = _context.KhuyenMais.FirstOrDefault(x => x.ID == temp.IDKhuyenMai && x.NgayKetThuc > DateTime.Now);
                    chiTietSanPham.GiaBan = khuyenMai != null ? GetKhuyenMai(khuyenMai.GiaTri, chiTietSanPham.GiaGoc, khuyenMai.TrangThai) : chiTietSanPham.GiaGoc;
                    return chiTietSanPham;
                }
                else return null;
            }
            catch
            {
                return null;
            }
        }
        public async Task<ChiTietSanPhamViewModelHome> GetAllChiTietSanPhamHome(Guid idSanPham)
        {
            try
            {
                KhuyenMai? khuyenMai;
                List<KhuyenMai> khuyenMais = _context.KhuyenMais.Where(x => x.NgayKetThuc > DateTime.Now).ToList();
                int giaBan;
                var sanPham = await _context.SanPhams.FindAsync(idSanPham);
                List<ChiTietSanPham> lstChiTietSanPham = _context.ChiTietSanPhams.Where(x => x.IDSanPham == idSanPham).ToList();
                List<MauSac> mauSacs = new List<MauSac>();
                List<KichCo> kichCos = new List<KichCo>();
                foreach (var x in lstChiTietSanPham)
                {
                    mauSacs.Add(_context.MauSacs.FindAsync(x.IDMauSac).Result);
                    kichCos.Add(_context.KichCos.FindAsync(x.IDKichCo).Result);
                }
                ChiTietSanPhamViewModelHome chiTietSanPham = new ChiTietSanPhamViewModelHome();
                chiTietSanPham.IDSanPham = idSanPham;
                chiTietSanPham.Ten = sanPham.Ten;
                chiTietSanPham.Anhs = new List<AnhRequest>();
                chiTietSanPham.MauSacs = new List<GiaTriViewModel>();
                chiTietSanPham.KichCos = new List<GiaTriViewModel>();
                chiTietSanPham.ChiTietSanPhams = new List<ChiTietSanPhamViewModel>();
                foreach (var item in _context.Anhs.Where(x => x.IDSanPham == idSanPham).ToList())
                {
                    chiTietSanPham.Anhs.Add(new AnhRequest() { DuongDan = item.DuongDan, MaMau = item.IDMauSac.ToString() });
                }
                foreach (var item in mauSacs.Distinct().ToList())
                {
                    chiTietSanPham.MauSacs.Add(new GiaTriViewModel() { GiaTri = item.Ma, ID = item.ID.Value, TenMau = item.Ten });
                }
                foreach (var item in kichCos.Distinct().OrderByDescending(x => x.Ten).ToList())
                {
                    chiTietSanPham.KichCos.Add(new GiaTriViewModel() { GiaTri = item.Ten, ID = item.ID });
                }

                foreach (var item in lstChiTietSanPham)
                {
                    int? TrangThaiKM = null;
                    int? giaTriKM = null;
                    if (item.IDKhuyenMai != null)
                    {
                        khuyenMai = khuyenMais.FirstOrDefault(x => x.ID == item.IDKhuyenMai);
                        if (khuyenMai != null)
                        {
                            giaBan = GetKhuyenMai(khuyenMai.GiaTri, item.GiaBan, khuyenMai.TrangThai);
                            TrangThaiKM = khuyenMai.TrangThai;
                            giaTriKM = khuyenMai.GiaTri;
                        }
                        else
                        {
                            giaBan = item.GiaBan;
                            DeleteKhuyenMai(item.ID);
                        }
                    }
                    else
                    {
                        giaBan = item.GiaBan;
                    }
                    chiTietSanPham.ChiTietSanPhams.Add(new ChiTietSanPhamViewModel() { ID = item.ID, MaCTSP = item.Ma, Ten = sanPham.Ten, SoLuong = item.SoLuong, GiaBan = giaBan, GiaGoc = item.GiaBan, MauSac = item.IDMauSac.ToString(), KichCo = item.IDKichCo.ToString(), TrangThai = item.TrangThai, TrangThaiKM = TrangThaiKM != null ? TrangThaiKM : null, GiaTriKM = giaTriKM != null ? giaTriKM : null });
                }
                chiTietSanPham.MoTa = sanPham.MoTa;
                var query = await (from sp in _context.SanPhams.Where(p => p.ID == idSanPham)
                                   join ctsp in _context.ChiTietSanPhams on sp.ID equals ctsp.IDSanPham
                                   join cthd in _context.ChiTietHoaDons on ctsp.ID equals cthd.IDCTSP
                                   join dg in _context.DanhGias.Where(p => p.TrangThai == 1) on cthd.ID equals dg.ID
                                   select new DanhGiaViewModel()
                                   {
                                       Sao = dg.Sao,
                                   }).ToListAsync();

                foreach (var item in query)
                {
                    chiTietSanPham.SoSao += Convert.ToInt32(item.Sao);
                }
                var sptt = await (from sp in _context.SanPhams.Where(p => p.IDLoaiSP == sanPham.IDLoaiSP && p.ID != idSanPham)
                                  join ctsp in _context.ChiTietSanPhams.Where(p => p.TrangThai == 1) on sp.ID equals ctsp.IDSanPham
                                  join ms in _context.MauSacs on ctsp.IDMauSac equals ms.ID

                                  select new SanPhamTuongTuViewModel()
                                  {
                                      IDSP = sp.ID,
                                      TenSP = sp.Ten,
                                      GiaSPTT = ctsp.GiaBan,
                                      DuongDanSPTT = _context.Anhs.FirstOrDefault(x => x.IDMauSac == ms.ID && x.IDSanPham == sp.ID).DuongDan,
                                  }).Take(5).ToListAsync();
                chiTietSanPham.SoSao = chiTietSanPham.SoSao / query.Count();
                chiTietSanPham.sosaoPercent = float.IsNaN(chiTietSanPham.SoSao) ? 0 : Convert.ToInt32((chiTietSanPham.SoSao / 5) * 100);
                chiTietSanPham.SoDanhGia = query.Count();
                chiTietSanPham.LSTSPTuongTu = sptt;
                return chiTietSanPham;
            }
            catch
            {
                return new ChiTietSanPhamViewModelHome();
            }
        }
        public Task<bool> UpdateChiTietSanPham(ChiTietSanPham chiTietSanPham)
        {
            throw new NotImplementedException();
        }

        public async Task<ChiTietSanPhamRequest?> CreateChiTietSanPhamFromSanPham(MauSac mauSacRequest, string tenKichCo, List<ChiTietSanPham>? chiTietSanPhams)
        {
            try
            {
                var mauSac = _context.MauSacs.FirstOrDefault(x => x.Ma == mauSacRequest.Ma);
                if (mauSac == null)
                {
                    mauSac = new MauSac() { ID = Guid.NewGuid(), Ten = mauSacRequest.Ten, Ma = mauSacRequest.Ma.ToLower(), TrangThai = 1 };
                    _context.Add(mauSac);
                }
                var kichCo = _context.KichCos.FirstOrDefault(x => x.Ten.ToUpper() == tenKichCo.Trim().ToUpper());
                if (kichCo == null)
                {
                    kichCo = new KichCo() { ID = Guid.NewGuid(), Ten = tenKichCo.Trim(), TrangThai = 1 };
                    _context.Add(kichCo);

                }
                _context.SaveChanges();
                if (chiTietSanPhams == null)
                {
                    var chiTietSanPham = new ChiTietSanPhamRequest() { IDChiTietSanPham = Guid.NewGuid(), SoLuong = 0, GiaBan = 0, IDMauSac = mauSac.ID.Value, IDKichCo = kichCo.ID, MaMau = mauSac.Ma, TenMauSac = mauSac.Ten, TenKichCo = kichCo.Ten };
                    return chiTietSanPham;
                }
                else
                {
                    var item = chiTietSanPhams.FirstOrDefault(x => x.IDMauSac == mauSac.ID && x.IDKichCo == kichCo.ID);
                    if (item == null)
                    {
                        var chiTietSanPham = new ChiTietSanPhamRequest() { IDChiTietSanPham = Guid.NewGuid(), SoLuong = 0, GiaBan = 0, IDMauSac = mauSac.ID.Value, IDKichCo = kichCo.ID, MaMau = mauSac.Ma, TenMauSac = mauSac.Ten, TenKichCo = kichCo.Ten };
                        return chiTietSanPham;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> AddChiTietSanPhamFromSanPham(ChiTietSanPhamUpdateRequest request)
        {
            try
            {
                if (request.TrangThai != null)
                {
                    var tempTrangThai = new Guid(request.TrangThai);
                    foreach (var x in request.ChiTietSanPhams)
                    {
                        _context.ChiTietSanPhams.Add(new ChiTietSanPham() { ID = x.IDChiTietSanPham, SoLuong = x.SoLuong.Value, GiaBan = x.GiaBan.Value, NgayTao = DateTime.Now, TrangThai = x.IDChiTietSanPham == tempTrangThai ? 1 : 2, IDSanPham = request.IDSanPham, IDMauSac = x.IDMauSac.Value, IDKichCo = x.IDKichCo.Value, Ma = RemoveUnicode(request.Ma + x.TenMauSac.Trim().ToUpper() + x.TenKichCo.ToUpper()) });
                    }
                    var chiTietSanPhamMacDinh = _context.ChiTietSanPhams.FirstOrDefault(x => x.IDSanPham == request.IDSanPham && x.TrangThai == 1);
                    if (chiTietSanPhamMacDinh != null)
                    {
                        chiTietSanPhamMacDinh.TrangThai = 2;
                        _context.ChiTietSanPhams.Update(chiTietSanPhamMacDinh);
                    }
                }
                else
                {
                    foreach (var x in request.ChiTietSanPhams)
                    {
                        _context.ChiTietSanPhams.Add(new ChiTietSanPham() { ID = x.IDChiTietSanPham, SoLuong = x.SoLuong.Value, GiaBan = x.GiaBan.Value, NgayTao = DateTime.Now, TrangThai = 2, IDSanPham = request.IDSanPham, IDMauSac = x.IDMauSac.Value, IDKichCo = x.IDKichCo.Value, Ma = RemoveUnicode(request.Ma + x.TenMauSac.Replace(" ", "").ToUpper() + x.TenKichCo.ToUpper()) });
                    }
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<bool> UpdateSoluongChiTietSanPham(Guid id, int soLuong)
        {
            try
            {
                var chiTietSanPham = _context.ChiTietSanPhams.First(x => x.ID == id);
                chiTietSanPham.SoLuong = soLuong;
                _context.ChiTietSanPhams.Update(chiTietSanPham);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<int> UpdateGiaBanChiTietSanPham(Guid id, int giaBan)
        {
            try
            {
                var chiTietSanPham = _context.ChiTietSanPhams.First(x => x.ID == id);
                chiTietSanPham.GiaBan = giaBan;
                _context.ChiTietSanPhams.Update(chiTietSanPham);
                await _context.SaveChangesAsync();
                if (chiTietSanPham.IDKhuyenMai != null)
                {
                    var khuyenMai = await _context.KhuyenMais.FirstAsync(x => x.ID == chiTietSanPham.IDKhuyenMai);
                    if (khuyenMai.NgayKetThuc > DateTime.Now && khuyenMai.TrangThai == 1)
                    {
                        giaBan = GetKhuyenMai(khuyenMai.GiaTri, giaBan, khuyenMai.TrangThai);
                    }
                }
                return giaBan;
            }
            catch
            {
                return -1;
            }
        }
        public async Task<bool> UpdateTrangThaiChiTietSanPham(Guid id)
        {
            try
            {
                var chiTietSanPhamNew = _context.ChiTietSanPhams.FirstOrDefault(x => x.ID == id);
                var chiTietSanPhamOld = _context.ChiTietSanPhams.FirstOrDefault(x => (x.TrangThai == 1) && x.IDSanPham == chiTietSanPhamNew.IDSanPham);
                if (chiTietSanPhamOld != null)
                {
                    chiTietSanPhamOld.TrangThai = 2;
                    _context.ChiTietSanPhams.Update(chiTietSanPhamOld);
                }
                if (chiTietSanPhamNew != null)
                {
                    chiTietSanPhamNew.TrangThai = 1;
                    _context.ChiTietSanPhams.Update(chiTietSanPhamNew);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<ChiTietSanPhamViewModelAdmin>> GetAllChiTietSanPhamAdmin(Guid idSanPham)
        {
            try
            {
                KhuyenMai? khuyenMai;
                List<KhuyenMai> khuyenMais = _context.KhuyenMais.Where(x => x.NgayKetThuc > DateTime.Now).ToList();
                var lstChiTietSanPham = await (from a in _context.ChiTietSanPhams.Where(x => x.IDSanPham == idSanPham)
                                               join b in _context.MauSacs on a.IDMauSac equals b.ID
                                               join c in _context.KichCos on a.IDKichCo equals c.ID
                                               select new ChiTietSanPhamViewModelAdmin()
                                               {
                                                   ID = a.ID,
                                                   Ma = a.Ma,
                                                   TenMauSac = b.Ten,
                                                   MaMauSac = b.Ma,
                                                   TenKichCo = c.Ten,
                                                   SoLuong = a.SoLuong,
                                                   GiaGoc = a.GiaBan,
                                                   IDKhuyenMai = a.IDKhuyenMai,
                                                   TrangThai = a.TrangThai
                                               }).ToListAsync();
                foreach (var item in lstChiTietSanPham)
                {
                    if (item.IDKhuyenMai != null)
                    {
                        khuyenMai = khuyenMais.FirstOrDefault(x => x.ID == item.IDKhuyenMai);
                        if (khuyenMai != null)
                        {
                            item.GiaBan = GetKhuyenMai(khuyenMai.GiaTri, item.GiaGoc, khuyenMai.TrangThai);
                            if (khuyenMai.TrangThai == 1)
                            {
                                item.GiaTriKhuyenMai = "-" + khuyenMai.GiaTri + "%";
                            }
                            else if (khuyenMai.TrangThai == 0)
                            {
                                item.GiaTriKhuyenMai = "-" + khuyenMai.GiaTri;
                            }

                        }
                        else
                        {
                            item.GiaBan = item.GiaGoc;
                            DeleteKhuyenMai(item.ID);
                        }
                    }
                    else
                    {
                        item.GiaBan = item.GiaGoc;
                    }
                }
                return lstChiTietSanPham.OrderBy(x => x.Ma).ToList();
            }
            catch
            {
                return new List<ChiTietSanPhamViewModelAdmin>();
            }
        }
        public async Task<List<ChiTietSanPhamViewModel>> GetAllChiTietSanPham()
        {
            try
            {
                return await (from sp in _context.SanPhams.AsNoTracking()
                              join ctsp in _context.ChiTietSanPhams.AsNoTracking()
                              on sp.ID equals ctsp.IDSanPham
                              select new ChiTietSanPhamViewModel()
                              {
                                  ID = ctsp.ID,
                                  Ten = sp.Ten,
                                  GiaBan = ctsp.GiaBan,
                                  GiaGoc = ctsp.GiaBan,
                                  TrangThai = ctsp.TrangThai,
                                  SoLuong = ctsp.SoLuong,
                              }).ToListAsync();
            }
            catch
            {
                return new List<ChiTietSanPhamViewModel>();
            }
        }
        public async Task<bool> DeleteChiTietSanPham(Guid id)
        {
            try
            {
                var chiTietSanPham = await _context.ChiTietSanPhams.FindAsync(id);
                if (chiTietSanPham.TrangThai != 1)
                {
                    chiTietSanPham.TrangThai = 0;
                    _context.ChiTietSanPhams.Update(chiTietSanPham);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> UndoChiTietSanPham(Guid id)
        {
            try
            {
                var chiTietSanPham = await _context.ChiTietSanPhams.FindAsync(id);
                chiTietSanPham.TrangThai = 2;
                _context.ChiTietSanPhams.Update(chiTietSanPham);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region LoaiSP
        public Task<LoaiSP> SaveLoaiSP(LoaiSPRequest lsp)
        {
            throw new NotImplementedException();
        }
        public Task<LoaiSP> GetLoaiSPById(Guid id)
        {
            throw new NotImplementedException();
        }
        public bool CheckTrungLoaiSP(LoaiSPRequest lsp)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> DeleteLoaiSP(Guid id)
        {
            try
            {
                var loaiSP = await _context.LoaiSPs.FindAsync(id);
                loaiSP.TrangThai = 0;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<List<LoaiSP>> GetAllLoaiSPCha()
        {
            return await _context.LoaiSPs.Where(x => x.IDLoaiSPCha == null && x.TrangThai == 1).ToListAsync();
        }
        public async Task<List<LoaiSP>?> GetAllLoaiSPCon(string tenLoaiSPCha)
        {
            var loaiSPCha = await _context.LoaiSPs.Where(x => x.IDLoaiSPCha == null).FirstOrDefaultAsync(x => x.Ten == tenLoaiSPCha);
            if (loaiSPCha != null)
            {
                return await _context.LoaiSPs.Where(x => x.IDLoaiSPCha == loaiSPCha.ID && x.TrangThai == 1).ToListAsync();
            }
            else return null;
        }
        #endregion

        #region Other
        public string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ", "đ", "é", "è", "ẻ", "ẽ", "ẹ", "ê", "ế", "ề", "ể", "ễ", "ệ", "í", "ì", "ỉ", "ĩ", "ị", "ó", "ò", "ỏ", "õ", "ọ", "ô", "ố", "ồ", "ổ", "ỗ", "ộ", "ơ", "ớ", "ờ", "ở", "ỡ", "ợ", "ú", "ù", "ủ", "ũ", "ụ", "ư", "ứ", "ừ", "ử", "ữ", "ự", "ý", "ỳ", "ỷ", "ỹ", "ỵ", };
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "d", "e", "e", "e", "e", "e", "e", "e", "e", "e", "e", "e", "i", "i", "i", "i", "i", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "o", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "u", "y", "y", "y", "y", "y", };
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            text = text.Replace(" ", "");
            return text;
        }
        public async Task<List<MauSac>> GetAllMauSac()
        {
            return await _context.MauSacs.Where(x => x.TrangThai == 1).ToListAsync();
        }

        public async Task<List<KichCo>> GetAllKichCo()
        {
            return await _context.KichCos.Where(x => x.TrangThai == 1).ToListAsync();
        }
        public async Task<List<ChatLieu>> GetAllChatLieu()
        {
            return await _context.ChatLieus.Where(x => x.TrangThai == 1).ToListAsync();
        }

        public Task<List<ChiTietSanPham>> GetAllChiTietSanPham(Guid idSanPham)
        {
            throw new NotImplementedException();
        }
        public int GetKhuyenMai(int giaTri, int giaSP, int trangThai)
        {
            var tienKhuyenMai = giaSP;
            //var khuyenMai = _context.KhuyenMais.First(x => x.ID == idKhuyenMai);
            if (trangThai == 0)
            {
                tienKhuyenMai -= giaTri;
            }
            else if (trangThai == 1)
            {
                tienKhuyenMai -= (giaTri * giaSP) / 100;
            }
            return tienKhuyenMai < 0 ? 0 : tienKhuyenMai;
        }
        public void DeleteKhuyenMai(Guid id)
        {
            var item = _context.ChiTietSanPhams.First(x => x.ID == id);
            item.IDKhuyenMai = null;
            _context.ChiTietSanPhams.Update(item);
            _context.SaveChanges();
        }
        #endregion
        //Nhinh thêm
        #region SanPhamBanHang
        public async Task<List<SanPhamBanHang>> GetAllSanPhamTaiQuay()
        {
            var result = await (from sp in _context.SanPhams.AsNoTracking().Where(c => c.TrangThai != 0)
                                join ctsp in _context.ChiTietSanPhams.Where(c => c.TrangThai == 1) on sp.ID equals ctsp.IDSanPham into ctspGroup
                                from ctsp in ctspGroup.DefaultIfEmpty()

                                join km in _context.KhuyenMais.Where(c => c.NgayKetThuc > DateTime.Now && c.TrangThai != 2) on ctsp.IDKhuyenMai equals km.ID into kmGroup
                                from km in kmGroup.DefaultIfEmpty()
                                select new SanPhamBanHang()
                                {
                                    Id = sp.ID,
                                    Ten = sp.Ten,
                                    MaSP = sp.Ma,
                                    Anh = (from ms in _context.MauSacs.AsNoTracking()
                                           join a in _context.Anhs.Where(c => c.IDSanPham == sp.ID).AsNoTracking()
                                           on ms.ID equals a.IDMauSac
                                           where ctsp != null && ctsp.IDMauSac == ms.ID
                                           select a.DuongDan).FirstOrDefault(),
                                    GiaGoc = ctsp == null ? null : ctsp.GiaBan,
                                    GiaBan = km == null ? ctsp.GiaBan :
                    (km.TrangThai == 1 ? (int)(ctsp.GiaBan / 100 * (100 - km.GiaTri)) :
                    (km.GiaTri < ctsp.GiaBan ? (ctsp.GiaBan - (int)km.GiaTri) : 0)),
                                    IdLsp = sp.IDLoaiSP,
                                }).OrderBy(c => c.MaSP).ToListAsync();

            return result;
        }

        public async Task<ChiTietSanPhamBanHang> GetChiTietSPBHById(Guid idsp)
        {
            var lstMS = (from ctsp in _context.ChiTietSanPhams.AsNoTracking()
                         join ms in _context.MauSacs.AsNoTracking() on ctsp.IDMauSac equals ms.ID
                         where ctsp.IDSanPham == idsp && ctsp.TrangThai != 0
                         select new MauSac
                         {
                             ID = ms.ID,
                             Ma = ms.Ma,
                             Ten = ms.Ten,
                         }).Distinct().ToList();

            var lstKC = (from ctsp in _context.ChiTietSanPhams
                         join kc in _context.KichCos on ctsp.IDKichCo equals kc.ID
                         where ctsp.IDSanPham == idsp && ctsp.TrangThai != 0
                         select new KichCo
                         {
                             ID = kc.ID,
                             Ten = kc.Ten,
                         }).Distinct().ToList();

            var result = await (from sp in _context.SanPhams.AsNoTracking()
                                where sp.ID == idsp && sp.TrangThai != 0
                                select new ChiTietSanPhamBanHang()
                                {
                                    Id = sp.ID,
                                    Ten = sp.Ten,
                                    lstMau = lstMS,
                                    lstKC = lstKC,
                                }).FirstOrDefaultAsync();
            return result;
        }

        public async Task<List<ChiTietCTSPBanHang>> GetChiTietCTSPBanHang(Guid idsp)
        {
            return await (from ctsp in _context.ChiTietSanPhams
                          join ms in _context.MauSacs on ctsp.IDMauSac equals ms.ID
                          join kc in _context.KichCos on ctsp.IDKichCo equals kc.ID
                          join sp in _context.SanPhams on ctsp.IDSanPham equals sp.ID
                          join km in _context.KhuyenMais.Where(c => c.NgayKetThuc > DateTime.Now && c.TrangThai != 2) on ctsp.IDKhuyenMai equals km.ID
                          into kmGroup
                          from km in kmGroup.DefaultIfEmpty()
                          where ctsp.IDSanPham == idsp && ctsp.TrangThai != 0
                          select new ChiTietCTSPBanHang()
                          {
                              Id = ctsp.ID,
                              Ten = sp.Ten,
                              ChiTiet = ms.Ten + " - " + kc.Ten,
                              idMauSac = ctsp.IDMauSac,
                              idKichCo = ctsp.IDKichCo,
                              SoLuong = ctsp.SoLuong,
                              Anh = (from ms in _context.MauSacs
                                     join a in _context.Anhs on ms.ID equals a.IDMauSac
                                     where ms.ID == ctsp.IDMauSac && a.IDSanPham == ctsp.IDSanPham
                                     select a).FirstOrDefault().DuongDan,
                              GiaGoc = ctsp.GiaBan,
                              GiaBan = km == null ? ctsp.GiaBan :
                    (km.TrangThai == 1 ? (int)(ctsp.GiaBan / 100 * (100 - km.GiaTri)) :
                    (km.GiaTri < ctsp.GiaBan ? (ctsp.GiaBan - (int)km.GiaTri) : 0)),
                          }).OrderByDescending(c => c.ChiTiet).ToListAsync();
        }

        public Guid GetIDsanPhamByIdCTSP(Guid idctsp)
        {
            var ctsp = _context.ChiTietSanPhams.FirstOrDefault(p => p.ID == idctsp);
            return ctsp.IDSanPham;
        }

        public async Task<List<HomeProductViewModel>> GetAllSanPhamTrangChu()
        {
            var result = await (from sp in _context.SanPhams.AsNoTracking().Where(c => c.TrangThai != 0)
                                join ctsp in _context.ChiTietSanPhams.Where(c => c.TrangThai == 1) on sp.ID equals ctsp.IDSanPham into ctspGroup
                                from ctsp in ctspGroup.DefaultIfEmpty()

                                join km in _context.KhuyenMais.Where(c => c.NgayKetThuc > DateTime.Now && c.TrangThai != 2) on ctsp.IDKhuyenMai equals km.ID into kmGroup
                                from km in kmGroup.DefaultIfEmpty()
                                select new HomeProductViewModel()
                                {
                                    Id = sp.ID,
                                    Ten = sp.Ten,
                                    IdCTSP = ctsp == null ? null : ctsp.ID,
                                    Anh = (from ms in _context.MauSacs.AsNoTracking()
                                           join a in _context.Anhs.Where(c => c.IDSanPham == sp.ID).AsNoTracking()
                                           on ms.ID equals a.IDMauSac
                                           where ms.ID == ctsp.IDMauSac
                                           select a).FirstOrDefault().DuongDan,
                                    SLBan = (from hd in _context.HoaDons.AsNoTracking().Where(c => c.TrangThaiGiaoHang == 6 && c.LoaiHD == 0)
                                             join cthd in _context.ChiTietHoaDons.AsNoTracking()
                                             on hd.ID equals cthd.IDHoaDon
                                             join ctsp in _context.ChiTietSanPhams.AsNoTracking()
                                             on cthd.IDCTSP equals ctsp.ID
                                             into ctspGroup
                                             from ctsp in ctspGroup.DefaultIfEmpty()
                                             where ctsp.IDSanPham == sp.ID
                                             select cthd).AsEnumerable().ToList().Sum(c => c.SoLuong),
                                    SoSao = (from cthd in _context.ChiTietHoaDons.AsNoTracking()
                                             join ctsp in _context.ChiTietSanPhams.AsNoTracking()
                                             on cthd.IDCTSP equals ctsp.ID
                                             into ctspGroup
                                             from ctsp in ctspGroup.DefaultIfEmpty()
                                             join dg in _context.DanhGias.AsNoTracking()
                                             on cthd.ID equals dg.ID
                                             where ctsp.IDSanPham == sp.ID
                                             select dg).AsEnumerable().ToList().Average(c => c.Sao),
                                    NgayTao = ctsp == null ? null : ctsp.NgayTao,
                                    GiaGoc = ctsp == null ? 0 : ctsp.GiaBan,
                                    GiaBan = km == null ? ctsp.GiaBan :
                    (km.TrangThai == 1 ? (int)(ctsp.GiaBan / 100 * (100 - km.GiaTri)) :
                    (km.GiaTri < ctsp.GiaBan ? (ctsp.GiaBan - (int)km.GiaTri) : 0)),
                                    KhuyenMai = (km == null ? null : km.GiaTri)
                                }).ToListAsync();
            return result;
        }
        #endregion
    }
}
