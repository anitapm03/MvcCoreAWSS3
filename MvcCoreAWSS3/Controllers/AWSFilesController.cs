using Microsoft.AspNetCore.Mvc;
using MvcCoreAWSS3.Services;

namespace MvcCoreAWSS3.Controllers
{
    public class AWSFilesController : Controller
    {
        private ServiceStorageS3 service;

        public AWSFilesController (ServiceStorageS3 service)
        {
            this.service = service;
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

            using(Stream stream = file.OpenReadStream())
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
    }
}
