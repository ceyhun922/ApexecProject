using Microsoft.AspNetCore.Mvc;

namespace ApexWebAPI.Controllers
{
    public class UploadFileRequest
    {
        public IFormFile? File { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class FileImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;

        public FileImagesController(IWebHostEnvironment env, IConfiguration configuration)
        {
            _env = env;
            _configuration = configuration;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] UploadFileRequest request)
        {
            if (!Request.HasFormContentType)
                return BadRequest($"Expected multipart/form-data, got: {Request.ContentType}");

            var file = request.File;
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(ext))
                return BadRequest("File extension missing");

            ext = ext.ToLowerInvariant();

            var imageExt = new HashSet<string> { ".jpg", ".jpeg", ".png", ".webp" };
            var videoExt = new HashSet<string> { ".mp4", ".webm", ".mov", ".avi" };

            string folder;
            string publicPath;

            if (imageExt.Contains(ext))
            {
                folder = "images";
                publicPath = "/images";
            }
            else if (videoExt.Contains(ext))
            {
                folder = "videos";
                publicPath = "/videos";
            }
            else
            {
                return BadRequest("Invalid file type");
            }

            var webRoot = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var targetDir = Path.Combine(webRoot, folder);
            Directory.CreateDirectory(targetDir);

            var fileName = $"{Guid.NewGuid():N}{ext}";
            var fullPath = Path.Combine(targetDir, fileName);

            await using var stream = System.IO.File.Create(fullPath);
            await file.CopyToAsync(stream);

            var baseUrl = _configuration["App:BaseUrl"]?.TrimEnd('/') ?? string.Empty;
            var url = $"{baseUrl}{publicPath}/{fileName}";
            return Ok(url);
        }
    }
}