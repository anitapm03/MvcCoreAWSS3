using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using MvcCoreAWSS3.Services;

namespace MvcCoreAWSS3.Controllers
{
    public class AWSFilesController : Controller
    {
        private ServiceStorageS3 service;
        private readonly IConfiguration config;

        public AWSFilesController(ServiceStorageS3 service,
            IConfiguration config)
        {
            this.service = service;
            this.config = config;
        }

        public async Task<IActionResult> Index()
        {
            List<string> files = await
                this.service.GetVersionesFileAsync();
            return View(files);
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile
            (IFormFile file)
        {
            string fileName = file.FileName;

            using (Stream stream = file.OpenReadStream())
            {
                await this.service.UploadFileAsync(fileName, stream);
            }
            return View();
        }

        public async Task<IActionResult> DeleteFile
           (string fileName)
        {
            await this.service.DeleteFileAsync(fileName);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GetImagen(string key)
        {
            try
            {
                var imageBytes = await this.service.GetFileAsync(key);
                return File(imageBytes, "image/jpeg"); // Puedes cambiar el tipo MIME según el tipo de imagen
            }
            catch
            {
                return NotFound(); // Maneja el error como desees
            }
        }
        /*
         ??????????
        public async Task<IActionResult> GetFile(string key)
    {
        
        var fileStream = await this.service.GetFileAsync(key);
        return File(fileStream, "application/octet-stream", key);
        
    }
         */
    }
}
