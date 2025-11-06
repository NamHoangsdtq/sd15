using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;
using AppData.ViewModels;
using AppData.ViewModels.SanPham;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks.Dataflow;

namespace AppAPI.Services
{
    public class KhuyenMaiServices : IKhuyenMaiServices
    {
        private readonly IAllRepository<KhuyenMai> _repos;
        private readonly IAllRepository<ChiTietSanPham> _reposCTSP;
        private readonly IAllRepository<SanPham> _reposSP;
        private readonly IAllRepository<MauSac> _reposMS;
        private readonly IAllRepository<KichCo> _reposSize;
        AssignmentDBContext context = new AssignmentDBContext();
        public KhuyenMaiServices()
        {
            _repos = new AllRepository<KhuyenMai>(context, context.KhuyenMais);
            _reposCTSP = new AllRepository<ChiTietSanPham>(context, context.ChiTietSanPhams);
            _reposSP = new AllRepository<SanPham>(context, context.SanPhams);
            _reposMS = new AllRepository<MauSac>(context, context.MauSacs);
        }

       

        public bool Add(KhuyenMaiView kmv)
        {
            kmv.ID = Guid.NewGuid();
            var khuyenmai = new KhuyenMai();
            khuyenmai.ID = kmv.ID;
            khuyenmai.Ten = kmv.Ten?.Trim();
            khuyenmai.GiaTri = kmv.GiaTri;
            khuyenmai.MoTa = kmv.MoTa?.Trim();
            khuyenmai.NgayApDung = kmv.NgayApDung;
            khuyenmai.NgayKetThuc = kmv.NgayKetThuc;
            if (khuyenmai.NgayApDung > khuyenmai.NgayKetThuc)
            {
                return false;
            }
            khuyenmai.TrangThai = kmv.TrangThai;
            return _repos.Add(khuyenmai);
        }

        public bool AdKMVoBT(List<Guid> btrequest, Guid IdKhuyenMai)
        {

            foreach (var km in btrequest)
            {
                var timidkm = _repos.GetAll().FirstOrDefault(x => x.ID == IdKhuyenMai);
                if (timidkm.NgayKetThuc < DateTime.Now)
                {
                    return false;
                }
                else
                {
                    var tim = _reposCTSP.GetAll().FirstOrDefault(x => x.ID == km);
                    if (tim != null)
                    {
                        tim.IDKhuyenMai = IdKhuyenMai;
                        _reposCTSP.Update(tim);
                    }
                }

            }
            return true;
        }
        public bool XoaAllKMRaBT(List<Guid> bienthes)
        {
            foreach (var km in bienthes)
            {

                var tim = _reposCTSP.GetAll().FirstOrDefault(x => x.ID == km);
                if (tim != null)
                {
                    tim.IDKhuyenMai = null;
                    _reposCTSP.Update(tim);
                }


            }
            return true;
        }

        public bool Delete(Guid Id)
        {
            var khuyenmai = _repos.GetAll().FirstOrDefault(x => x.ID == Id);
            if (khuyenmai != null)
            {

                return _repos.Delete(khuyenmai);
            }
            else
            {
                return false;
            }
        }

        public List<KhuyenMai> GetAll()
        {
            return _repos.GetAll();
        }

       

        public KhuyenMai GetById(Guid Id)
        {
            return _repos.GetAll().FirstOrDefault(x => x.ID == Id);
        }

        public List<KhuyenMai> GetKMByName(string Ten)
        {
            return _repos.GetAll().Where(x => x.Ten.Contains(Ten)).ToList();
        }

        public bool Update(KhuyenMaiView kmv)
        {
            var khuyenmai = _repos.GetAll().FirstOrDefault(x => x.ID == kmv.ID);
            if (khuyenmai != null)
            {
                //khuyenmai.TrangThai = kmv.TrangThai;
                //khuyenmai.Ten = kmv.Ten;
                //khuyenmai.GiaTri = kmv.GiaTri;
                khuyenmai.MoTa = kmv.MoTa?.Trim();
                khuyenmai.NgayApDung = kmv.NgayApDung;
                khuyenmai.NgayKetThuc = kmv.NgayKetThuc;
                if (khuyenmai.NgayApDung > khuyenmai.NgayKetThuc)
                {
                    return false;
                }
             
                return _repos.Update(khuyenmai);
            }
            else
            {
                return false;
            }
        }


    }
}
