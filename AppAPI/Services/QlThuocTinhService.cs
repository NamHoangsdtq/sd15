using AppAPI.IServices;
using AppData.Models;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class QlThuocTinhService : IQlThuocTinhService
    {
        private readonly
        AssignmentDBContext _dbContext;
        public QlThuocTinhService()
        {
            _dbContext = new AssignmentDBContext();
        }
        #region KichCo
        public async Task<KichCo> AddKichCo(string ten, int trangthai)
        {
            try
            {
                //string[] commonSizes = new string[] { "XS", "S", "M", "L", "XL", "XXL", "XXXL", "4XL", "5XL", "6XL" };
                //if (!commonSizes.Contains(ten.Trim().ToUpper()))
                //{
                //    return null; 
                //}
                var existingColor = await _dbContext.KichCos.FirstOrDefaultAsync(x => x.Ten.Trim().ToUpper() == ten.Trim().ToUpper());
                if (existingColor != null)
                {
                    return null;
                }
                KichCo kc = new KichCo()
                {
                    ID = Guid.NewGuid(),
                    Ten = ten,
                    TrangThai = 1
                };
                _dbContext.KichCos.Add(kc);
                _dbContext.SaveChanges();
                return kc;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<bool> DeleteKichCo(Guid id)
        {
            try
            {
                var nv = await _dbContext.KichCos.FirstOrDefaultAsync(nv => nv.ID == id);
                if (nv != null)
                {
                    _dbContext.KichCos.Remove(nv);
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception) { throw; }
        }
        public async Task<KichCo> UpdateKichCo(Guid id, string ten, int trangthai)
        {
            try
            {
                var nv = await _dbContext.KichCos.FirstOrDefaultAsync(x => x.ID == id);
                if (nv != null)
                {
                    var existingColor = await _dbContext.KichCos.FirstOrDefaultAsync(x => x.Ten.Trim().ToUpper() == ten.Trim().ToUpper() && x.ID != id);
                    if (existingColor != null)
                    {
                        return null; // Trả về null để báo hiệu tên trùng
                    }
                    nv.Ten = ten;
                    nv.TrangThai = 1;
                    _dbContext.KichCos.Update(nv);
                    _dbContext.SaveChanges();
                    return nv;
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<KichCo>> GetAllKichCo()
        {
            try
            {
                return await _dbContext.KichCos.OrderByDescending(x => x.TrangThai).ToListAsync();
            }
            catch (Exception) { throw; }
        }
        public async Task<KichCo> GetKickCoById(Guid id)
        {
            var nv = await _dbContext.KichCos.FirstOrDefaultAsync(nv => nv.ID == id);
            return nv;
        }

        #endregion MauSac
        public async Task<MauSac> AddMauSac(string ten, string ma, int trangthai)
        {
            try
            {
                var existingColor = await _dbContext.MauSacs.FirstOrDefaultAsync(x => x.Ten.Trim().ToUpper() == ten.Trim().ToUpper());
                if (existingColor != null)
                {
                    return null;
                }
                bool isHasHash = ma.StartsWith("#");
                MauSac kc = new MauSac()
                {
                    ID = Guid.NewGuid(),
                    Ten = ten,
                    Ma = isHasHash ? ma : $"#{Uri.EscapeDataString(ma)}",
                    TrangThai = 1
                };
                _dbContext.MauSacs.Add(kc);
                _dbContext.SaveChanges();
                return kc;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeleteMauSac(Guid id)
        {
            try
            {
                var nv = await _dbContext.MauSacs.FirstOrDefaultAsync(nv => nv.ID == id);
                if (nv != null)
                {
                    _dbContext.MauSacs.Remove(nv);
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;

            }
        }

        public async Task<List<MauSac>> GetAllMauSac()
        {
            try
            {
                return await _dbContext.MauSacs.OrderByDescending(x => x.TrangThai).ToListAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<MauSac> GetMauSacById(Guid id)
        {
            try
            {
                var nv = await _dbContext.MauSacs.FirstOrDefaultAsync(nv => nv.ID == id);
                return nv;
            }
            catch (Exception) { throw; }
        }

        public async Task<MauSac> UpdateMauSac(Guid id, string ten, string ma, int trangthai)
        {
            try
            {
                var nv = await _dbContext.MauSacs.FirstOrDefaultAsync(x => x.ID == id);
                if (nv != null)
                {
                    var existingColor = await _dbContext.MauSacs.FirstOrDefaultAsync(x => x.Ten.Trim().ToUpper() == ten.Trim().ToUpper() && x.ID != id);
                    if (existingColor != null)
                    {
                        return null;
                    }
                    bool isHasHash = ma.StartsWith("#");
                    nv.Ten = ten;
                    nv.Ma = isHasHash ? ma : $"#{Uri.EscapeDataString(ma)}";
                    nv.TrangThai = 1;
                    _dbContext.MauSacs.Update(nv);
                    _dbContext.SaveChanges();
                    return nv;
                }

                return null;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #region chat lieu
        public async Task<ChatLieu> AddChatLieu(string ten, int trangthai)
        {
            try
            {
                var existingColor = await _dbContext.ChatLieus.FirstOrDefaultAsync(x => x.Ten.Trim().ToUpper() == ten.Trim().ToUpper());
                if (existingColor != null)
                {
                    return null;
                }
                ChatLieu kc = new ChatLieu()
                {
                    ID = Guid.NewGuid(),
                    Ten = ten,
                    TrangThai = 1
                };
                _dbContext.ChatLieus.Add(kc);
                _dbContext.SaveChanges();
                return kc;

            }
            catch (Exception)
            {

                throw;

            }
        }

        public async Task<ChatLieu> GetChatLieuById(Guid id)
        {
            try
            {
                var nv = await _dbContext.ChatLieus.FirstOrDefaultAsync(nv => nv.ID == id);
                return nv;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> DeleteChatLieu(Guid id)
        {

            try
            {
                var nv = await _dbContext.ChatLieus.FirstOrDefaultAsync(nv => nv.ID == id);
                if (nv != null)
                {
                    _dbContext.ChatLieus.Remove(nv);
                    _dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ChatLieu> UpdateChatLieu(Guid id, string ten, int trangthai)
        {

            try
            {
                var nv = await _dbContext.ChatLieus.FirstOrDefaultAsync(x => x.ID == id);
                if (nv != null)
                {
                    var existingColor = await _dbContext.ChatLieus.FirstOrDefaultAsync(x => x.Ten.Trim().ToUpper() == ten.Trim().ToUpper());
                    if (existingColor != null)
                    {
                        return null; // Trả về null để báo hiệu tên trùng
                    }
                    nv.Ten = ten;
                    nv.TrangThai = 1;
                    _dbContext.ChatLieus.Update(nv);
                    _dbContext.SaveChanges();
                    return nv;
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<ChatLieu>> GetAllChatLieu()
        {
            try
            {
                return await _dbContext.ChatLieus.OrderByDescending(x => x.TrangThai).ToListAsync();
            }
            catch (Exception)
            {

                throw;

            }
        }
        #endregion
    }
}
