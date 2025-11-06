using AppAPI.IServices;
using AppData.Models;
using AppData.ViewModels.SanPham;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppAPI.Services
{
    public class LoaiSPService : ILoaiSPService
    {
        private readonly AssignmentDBContext _context;
        public LoaiSPService()
        {
            _context = new AssignmentDBContext();
        }
        #region LoaiSP
        public async Task<bool> DeleteLoaiSP(Guid id)
        {

            try
            {
                var lsp = await _context.LoaiSPs.FindAsync(id);
                if (lsp == null) throw new Exception($"Không tìm thấy Loại sản phẩm: {id}");
                // Check LoaiSP đag đc sử dụng k
                if (_context.SanPhams.Any(c => c.IDLoaiSP == id)) return false;
                _context.LoaiSPs.Remove(lsp);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<LoaiSP>> GetAllLoaiSP()
        {
            try
            {
                return await _context.LoaiSPs.AsNoTracking().OrderByDescending(x => x.TrangThai).ToListAsync();
            }
            catch (Exception) { throw; }
        }

        public async Task<LoaiSP> GetLoaiSPById(Guid id)
        {
            try
            {
                return await _context.LoaiSPs.FindAsync(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<LoaiSP> SaveLoaiSP(LoaiSPRequest lsp)
        {

            try
            {
                var Lsp = await _context.LoaiSPs.FindAsync(lsp.ID);
                //Check tồn tại
                var existingLoaiSP = await _context.LoaiSPs
            .Where(x => x.Ten.ToUpper().Trim() == lsp.Ten.ToUpper().Trim() && x.ID != lsp.ID)
            .FirstOrDefaultAsync();

                // Throw exception if duplicate name found
                if (existingLoaiSP != null)
                {
                    return null;
                }
                if (Lsp != null) //Update
                {
                    Lsp.Ten = lsp.Ten;
                    Lsp.TrangThai = 1;
                    Lsp.IDLoaiSPCha = lsp.IDLoaiSPCha;
                    _context.LoaiSPs.Update(Lsp);
                    await _context.SaveChangesAsync();
                    return Lsp;
                }
                else // Tạo mới
                {
                    LoaiSP loaiSP = new LoaiSP()
                    {
                        ID = new Guid(),
                        Ten = lsp.Ten,
                        IDLoaiSPCha = lsp.IDLoaiSPCha,
                        TrangThai = 1,
                    };
                    await _context.AddAsync(loaiSP);
                    await _context.SaveChangesAsync();
                    return loaiSP;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckTrungLoaiSP(LoaiSPRequest lsp)
        {
            try
            {
                var existingColor = _context.LoaiSPs.FirstOrDefaultAsync(x => x.Ten.Trim().ToUpper() == lsp.Ten.Trim().ToUpper() && x.ID != lsp.ID);

                if (existingColor != null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<LoaiSP> AddSpCha(Guid idLoaiSPCha, string ten, int trangthai)
        {
            try
            {
                var check = _context.LoaiSPs.FirstOrDefaultAsync(x => x.Ten == ten && x.IDLoaiSPCha != idLoaiSPCha);
                if (check != null)
                {
                    return null;
                }
                LoaiSP kc = new LoaiSP()
                {
                    IDLoaiSPCha = Guid.NewGuid(),
                    Ten = ten,
                    TrangThai = 1
                };
                _context.LoaiSPs.Add(kc);
                _context.SaveChanges();
                return kc;
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion
    }
}
