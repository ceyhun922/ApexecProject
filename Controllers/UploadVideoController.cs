using Microsoft.AspNetCore.Mvc;

namespace ApexWebAPI.Controllers
{
    public class UploadVideoRequest
{
    public IFormFile File { get; set; }
}

    [ApiController]
    [Route("api/[controller]")]

    public class UploadVideoController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public UploadVideoController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost("upload-video")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadVideo([FromForm] UploadVideoRequest request)
        {
            var file = request.File;
            if (file == null || file.Length == 0)
                return BadRequest("File yok");

            var allowedExtensions = new[] { ".mp4", ".webm", ".mov", ".avi" };
            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                return BadRequest("Yalnız video yüklenir");

            var videoFolder = Path.Combine(_env.WebRootPath, "videos");
            if (!Directory.Exists(videoFolder))
                Directory.CreateDirectory(videoFolder);

            var fileName = Guid.NewGuid() + extension;
            var path = Path.Combine(videoFolder, fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            var fileUrl = $"https://api.apexec.az/videos/{fileName}";
            return Ok(new { fileUrl });
        }
    }
}