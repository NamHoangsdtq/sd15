using AppAPI.IServices;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class VaiTroSevice : IVaiTroService
    {
        private readonly AssignmentDBContext dBContext;

        public VaiTroSevice()
        {
            this.dBContext = new AssignmentDBContext();
        }

        public bool CreateVaiTro(string ten, int trangthai)
        {
            try
            {
                var vaitro = new VaiTro();
                vaitro.ID = Guid.NewGuid();
                vaitro.Ten = ten;
                vaitro.TrangThai = trangthai;
                dBContext.VaiTros.Add(vaitro);
                dBContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool DeleteVaiTro(Guid id)
        {
            try
            {
                var vt = dBContext.VaiTros.FirstOrDefault(a => a.ID == id);
                if (vt != null)
                {
                    dBContext.VaiTros.Remove(vt);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<VaiTro> GetAllVaiTro()
        {
            try
            {
                return dBContext.VaiTros.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public VaiTro GetVaiTroById(Guid id)
        {
            try
            {
                return dBContext.VaiTros.FirstOrDefault(x => x.ID == id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool UpdateVaiTro(Guid id, string ten, int trangthai)
        {
            try
            {
                var vaitro = dBContext.VaiTros.FirstOrDefault(a => a.ID == id);
                if (vaitro == null)
                {
                    return false;
                }
                else
                {
                    vaitro.Ten = ten;
                    vaitro.TrangThai = trangthai;
                    dBContext.VaiTros.Update(vaitro);
                    dBContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        
    }
}
