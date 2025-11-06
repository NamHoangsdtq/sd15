using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;

namespace AppAPI.Services
{
    public class ChiTietGioHangServices : IChiTietGioHangServices
    {
        private readonly IAllRepository<ChiTietGioHang> repos;
        private readonly IAllRepository<ChiTietSanPham> chitietsanphams;
        AssignmentDBContext context = new AssignmentDBContext();
        public ChiTietGioHangServices()
        {
            repos = new AllRepository<ChiTietGioHang>(context, context.ChiTietGioHangs);
            chitietsanphams = new AllRepository<ChiTietSanPham>(context, context.ChiTietSanPhams);
        }
        public string Add(Guid idChiTietSanPham, Guid IdKhachHang, int soluong)
        {
            ChiTietGioHang chiTietGioHang = new ChiTietGioHang();
            chiTietGioHang.ID = Guid.NewGuid();
            chiTietGioHang.IDCTSP = idChiTietSanPham;
            chiTietGioHang.IDNguoiDung = IdKhachHang;
            chiTietGioHang.SoLuong = soluong;
            if (repos.GetAll().Exists(p => p.IDCTSP == idChiTietSanPham && p.IDNguoiDung == IdKhachHang))
            {
                Guid id = repos.GetAll().Find(p => p.IDCTSP == idChiTietSanPham && p.IDNguoiDung == IdKhachHang).ID;
                ChiTietGioHang chiTietGioHang1 = repos.GetAll().Find(p => p.IDCTSP == idChiTietSanPham && p.IDNguoiDung == IdKhachHang);
                if (chiTietGioHang.SoLuong + soluong > chitietsanphams.GetAll().Find(p => p.ID == idChiTietSanPham).SoLuong)
                {
                    return "so luong trong kho khong du";
                }
                else
                {
                    chiTietGioHang1.SoLuong = chiTietGioHang1.SoLuong + soluong;
                    return repos.Update(chiTietGioHang1).ToString();
                }
            }
            else
            {
                return repos.Add(chiTietGioHang).ToString();
            }
        }

        public bool Delete(Guid Id)
        {
            var chiTietGioHang = repos.GetAll().FirstOrDefault(x => x.ID == Id);
            if (chiTietGioHang != null)
            {

                return repos.Delete(chiTietGioHang);
            }
            else
            {
                return false;
            }
        }

        public List<ChiTietGioHang> GetAll()
        {
            return repos.GetAll().ToList();
        }

        public ChiTietGioHang GetById(Guid Id)
        {
            return repos.GetAll().FirstOrDefault(x => x.ID == Id);
        }

        public string Update(Guid id, Guid idChiTietSanPham, Guid IdKhachHang, int soluong)
        {
            var chiTietGioHang = repos.GetAll().FirstOrDefault(x => x.ID == id);
            if (chiTietGioHang != null)
            {
                chiTietGioHang.IDCTSP = idChiTietSanPham;
                chiTietGioHang.IDNguoiDung = IdKhachHang;
                chiTietGioHang.SoLuong = soluong;
                if (repos.GetAll().Exists(x => x.IDCTSP == idChiTietSanPham && x.IDNguoiDung == IdKhachHang))
                {

                    ChiTietGioHang chiTietGioHang1 = repos.GetAll().Find(p => p.IDCTSP == idChiTietSanPham && p.IDNguoiDung == IdKhachHang);
                    if (chiTietGioHang1.SoLuong > chitietsanphams.GetAll().Find(x => x.ID == idChiTietSanPham).SoLuong)
                    {
                        return "so luong trong kho khong du";
                    }
                    else
                    {
                        chiTietGioHang1.SoLuong = soluong;
                        return repos.Update(chiTietGioHang1).ToString();
                    }
                }
                else
                {
                    return repos.Update(chiTietGioHang).ToString();
                }
            }
            else
            {
                return "";
            }
        }
    }
}
