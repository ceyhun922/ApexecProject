using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<FileImagesController> _logger;

        public FileImagesController(IWebHostEnvironment env, IConfiguration configuration, ILogger<FileImagesController> logger)
        {
            _env = env;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(52_428_800)] // 50 MB
        public async Task<IActionResult> Upload([FromForm] UploadFileRequest request)
        {
            try
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
                    return BadRequest("Invalid file type. Allowed: jpg, jpeg, png, webp, mp4, webm, mov, avi");
                }

                // WebRootPath is always set to {ContentRoot}/wwwroot in .NET 6 regardless of directory existence
                var webRoot = _env.WebRootPath
                    ?? Path.Combine(_env.ContentRootPath, "wwwroot");

                var targetDir = Path.Combine(webRoot, folder);

                _logger.LogInformation("Upload target directory: {TargetDir}", targetDir);

                Directory.CreateDirectory(targetDir);

                var fileName = $"{Guid.NewGuid():N}{ext}";
                var fullPath = Path.Combine(targetDir, fileName);

                await using (var stream = System.IO.File.Create(fullPath))
                {
                    await file.CopyToAsync(stream);
                }

                var baseUrl = _configuration["App:BaseUrl"]?.TrimEnd('/') ?? string.Empty;
                var url = $"{baseUrl}{publicPath}/{fileName}";

                _logger.LogInformation("File uploaded successfully: {Url}", url);

                return Ok(url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "File upload failed. ContentRoot: {Root}", _env.ContentRootPath);
                return StatusCode(500, new { message = "File upload failed", detail = ex.Message });
            }
        }
    }
}