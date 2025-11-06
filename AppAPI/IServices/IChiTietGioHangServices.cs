using AppData.Models;

namespace AppAPI.IServices
{
    public interface IChiTietGioHangServices
    {
        public string Add(Guid IdBienThe, Guid IdKhachHang, int soluong);
        public ChiTietGioHang GetById(Guid Id);
        public string Update(Guid id, Guid IdBienThe, Guid IdKhachHang, int soluong);
        public List<ChiTietGioHang> GetAll();
        public bool Delete(Guid Id);
    }
}
