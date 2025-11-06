namespace AppView.IServices
{
    public interface IFileService
    {
        Task<string> AddFile(IFormFile file,string wwwRootPath);
        bool DeleteFile(string fileName,string wwwRootPath);
    }
}
