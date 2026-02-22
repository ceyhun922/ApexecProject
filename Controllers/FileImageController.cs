using Microsoft.AspNetCore.Mvc;

namespace ApexWebAPI.Controllers
{
    public class UploadImageRequest
    {
        public IFormFile File { get; set; }
    }
    
    [ApiController]
    [Route("api/[controller]")]

    
    public class FileImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public FileImagesController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile([FromForm] UploadImageRequest request)
        {
            var file = request.File;
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine(_env.WebRootPath, "images", fileName);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            var fileUrl = $"https://api.apexec.az/images/{fileName}";


            return Ok(new { fileUrl });
        }
    }

}