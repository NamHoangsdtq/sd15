using AppData.ViewModels.SanPham;
using AppView.IServices;
using Microsoft.Extensions.Hosting;

namespace AppView.Services
{
    public class FileService : IFileService
    {
        public async Task<string> AddFile(IFormFile file,string wwwRootPath)
        {
            //string wwwrootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
            string extension = Path.GetExtension(file.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath + "/img/product/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }

        public bool DeleteFile(string fileName,string wwwRootPath)
        {
            try
            {
                string path = Path.Combine(wwwRootPath + "/img/product/", fileName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                    
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
