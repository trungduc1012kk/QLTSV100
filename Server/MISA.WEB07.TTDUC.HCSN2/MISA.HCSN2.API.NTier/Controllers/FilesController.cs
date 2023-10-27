using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace MISA.HCSN2.API.NTier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;

        public FilesController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> Upload(IFormFile upload)
        {
            if (upload == null || upload.Length == 0)
                return BadRequest("Invalid file");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(upload.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension))
                return BadRequest("Invalid file extension");

            var uploadsFolderPath = Path.Combine(_environment.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            string fileName = "Image_" + DateTime.Now.Ticks + fileExtension;
            var filePath = Path.Combine(uploadsFolderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await upload.CopyToAsync(stream);
            }

            var urlGetImage = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/Files/get-image/" + fileName;

            return Ok(new
            {
                url = urlGetImage,
            });
        }


        [HttpGet("get-image/{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            var uploadsFolderPath = Path.Combine(_environment.WebRootPath, "images");
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var fileExtension = Path.GetExtension(filePath);
            var mimeType = GetMimeType(fileExtension);

            return File(fileStream, mimeType);
        }

        private string GetMimeType(string fileExtension)
        {
            var provider = new FileExtensionContentTypeProvider(); 
            if (!provider.TryGetContentType(fileExtension, out var mimeType))
            {
                // Set a default mime type if the provider doesn't recognize the extension
                mimeType = "application/octet-stream";
            }
            return mimeType;
        }
    }
}
