using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;

namespace AppAPI.Services
{
    public class VoucherServices : IVoucherServices
    {
        private readonly IAllRepository<Voucher> _allRepository;
        AssignmentDBContext context= new AssignmentDBContext();
        public VoucherServices()
        {
            _allRepository= new AllRepository<Voucher>(context,context.Vouchers);
        }
        public bool Add(VoucherView voucherview)
        {
            voucherview.Id=Guid.NewGuid();
            var voucher= new Voucher();
            voucher.ID=voucherview.Id;
            voucher.Ten=voucherview.Ten?.Trim();
            voucher.HinhThucGiamGia=voucherview.HinhThucGiamGia;
            voucher.SoTienCan=voucherview.SoTienCan;
            voucher.GiaTri = voucherview.GiaTri;
            voucher.NgayApDung=voucherview.NgayApDung;
            voucher.NgayKetThuc=voucherview.NgayKetThuc;
            if (voucher.NgayApDung > voucher.NgayKetThuc)
            {
                return false;
            }
            voucher.SoLuong=voucherview.SoLuong;
            voucher.MoTa = voucherview.MoTa?.Trim();
            voucher.TrangThai=voucherview.TrangThai;
            return _allRepository.Add(voucher);
        }

        public bool Delete(Guid Id)
        {
            var voucher = _allRepository.GetAll().FirstOrDefault(x => x.ID == Id);
            if (voucher != null)
            {
               
                return _allRepository.Delete(voucher);
            }
            else
            {
                return false;
            }
        }

        public List<Voucher> GetAll()
        {
            return _allRepository.GetAll();
        }

        public Voucher GetById(Guid Id)
        {
            return _allRepository.GetAll().FirstOrDefault(x => x.ID == Id);
        }

        public bool Update(Guid id,VoucherView voucherview)
        {
            var voucher= _allRepository.GetAll().FirstOrDefault(x => x.ID == id);
            if (voucher != null)
            {
              
                //voucher.Ten = voucherview.Ten;
                //voucher.HinhThucGiamGia = voucherview.HinhThucGiamGia;
                voucher.SoTienCan = voucherview.SoTienCan;
                //voucher.GiaTri = voucherview.GiaTri;
                voucher.NgayApDung = voucherview.NgayApDung;
                voucher.NgayKetThuc = voucherview.NgayKetThuc;
                if (voucher.NgayApDung > voucher.NgayKetThuc)
                {
                    return false;
                }
                voucher.SoLuong = voucherview.SoLuong;
                voucher.MoTa = voucherview.MoTa?.Trim();
              
                return _allRepository.Update(voucher);
            }
            else
            {
                return false;
            }
        }
        public Voucher? GetVoucherByMa(string ma)
        {
            return _allRepository.GetAll().FirstOrDefault(x => x.Ten.ToUpper() == ma.ToUpper());
        }
        public List<Voucher> GetAllVoucherByTien(int tongTien) 
        {
            return _allRepository.GetAll().Where(x=>x.NgayApDung<DateTime.Now && x.NgayKetThuc>DateTime.Now && x.SoTienCan<=tongTien && x.TrangThai>0 && x.SoLuong>0).ToList();
        }
    }
}
