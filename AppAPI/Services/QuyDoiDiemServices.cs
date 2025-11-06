using AppAPI.IServices;
using AppData.IRepositories;
using AppData.Models;
using AppData.Repositories;

namespace AppAPI.Services
{
    public class QuyDoiDiemServices : IQuyDoiDiemServices
    {
        private readonly IAllRepository<QuyDoiDiem> _allRepository;
        AssignmentDBContext context= new AssignmentDBContext();
        public QuyDoiDiemServices()
        {
            _allRepository=new AllRepository<QuyDoiDiem>(context,context.QuyDoiDiems);
        }
        public bool Add(/*int sodiem,*/ int TiLeTichDiem, int TiLeTieuDiem, int TrangThai)
        {
            var quydoidiem = new QuyDoiDiem();
            quydoidiem.ID=Guid.NewGuid();
            //quydoidiem.SoDiem = sodiem;
            quydoidiem.TiLeTichDiem = TiLeTichDiem;
            quydoidiem.TiLeTieuDiem = TiLeTieuDiem;
            quydoidiem.TrangThai = TrangThai;
            return _allRepository.Add(quydoidiem);
        }

        public bool Delete(Guid Id)
        {
            var quydoidiem = _allRepository.GetAll().FirstOrDefault(x => x.ID == Id);
            if (quydoidiem != null)
            {
               
                return _allRepository.Delete(quydoidiem);
            }
            else
            {
                return false;
            }
        }

        public List<QuyDoiDiem> GetAll()
        {
           return _allRepository.GetAll();
        }

        public QuyDoiDiem GetById(Guid Id)
        {
            return _allRepository.GetAll().FirstOrDefault(x => x.ID == Id);
        }

        public bool Update(Guid Id, int TrangThai)
        {
            var quydoidiem= _allRepository.GetAll().FirstOrDefault(x => x.ID == Id);
            if(quydoidiem != null)
            {
                //quydoidiem.SoDiem = sodiem;
                //quydoidiem.TiLeTichDiem = TiLeTichDiem;
                //quydoidiem.TiLeTieuDiem = TiLeTieuDiem;
                quydoidiem.TrangThai = TrangThai;
                return _allRepository.Update(quydoidiem);
            }
            else
            {
                return false;
            }
        }
    }
}
