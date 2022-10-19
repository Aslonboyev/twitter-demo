using BlogApp.WebApi.Helpers;
using BlogApp.WebApi.Interfaces.Services;

namespace BlogApp.WebApi.Services
{
    public class FileService : IFileService
    {
        private readonly string _imageFolderName = "images";
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            string fileName = ImageHelper.MakeImageName(file.FileName);
            string partPath = Path.Combine(_imageFolderName, fileName);
            string path = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", partPath);

            var stream = File.Create(path);
            await file.CopyToAsync(stream);
            stream.Close();

            return partPath;
        }

        public async Task DeleteImageAsync(string imagePath)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;

            var fullPath = webRootPath + "\\" + imagePath;

            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }
    }
}
