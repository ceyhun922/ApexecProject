using ApexWebAPI.Services.Interfaces;

namespace ApexWebAPI.Services.Concrete
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        private static readonly HashSet<string> ImageExtensions = new(StringComparer.OrdinalIgnoreCase)
            { ".jpg", ".jpeg", ".png", ".webp", ".gif" };

        private static readonly HashSet<string> VideoExtensions = new(StringComparer.OrdinalIgnoreCase)
            { ".mp4", ".webm", ".mov", ".avi" };

        public FileUploadService(IWebHostEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;
        }

        public async Task<string> UploadAsync(IFormFile file, string subfolder)
        {
            var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var targetDir = Path.Combine(webRoot, subfolder);
            Directory.CreateDirectory(targetDir);

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(targetDir, fileName);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return $"/{subfolder}/{fileName}";
        }

        public async Task<string> UploadAutoDetectAsync(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

            string subfolder = IsImageFile(ext) ? "images"
                : IsVideoFile(ext) ? "videos"
                : throw new ArgumentException($"Unsupported file type: {ext}");

            var relativePath = await UploadAsync(file, subfolder);

            var baseUrl = _config["App:BaseUrl"] ?? string.Empty;
            return $"{baseUrl}{relativePath}";
        }

        public void DeleteFile(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath)) return;

            var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var fullPath = Path.Combine(webRoot, relativePath.TrimStart('/'));

            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }

        public bool IsImageFile(string extension) =>
            ImageExtensions.Contains(extension);

        public bool IsVideoFile(string extension) =>
            VideoExtensions.Contains(extension);
    }
}
