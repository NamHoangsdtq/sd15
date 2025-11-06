using AppData.Models;
using AppData.ViewModels.SanPham;
using System;
using System.Security.Cryptography.X509Certificates;

namespace AppAPI.IServices
{
    public interface IVaiTroService
    {
        public List<VaiTro> GetAllVaiTro();
        public VaiTro GetVaiTroById(Guid id);
        public bool CreateVaiTro(string ten, int trangthai);
        public bool DeleteVaiTro(Guid id);
        public bool UpdateVaiTro(Guid id, string ten, int trangthai);
    }
}
