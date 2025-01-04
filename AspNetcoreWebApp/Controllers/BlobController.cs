using AspNetcoreWebApp.Models;
using AspNetcoreWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetcoreWebApp.Controllers
{
    public class BlobController : Controller
    {
        private readonly IBlobService _blobService;
        public BlobController(IBlobService blobService)
        {
            _blobService = blobService;
        }

        public IActionResult Addfile(string containerName)
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Manage(string containerName)
        {
            var blobsobj = await _blobService.GetAllBlobs(containerName);
            return View(blobsobj);
        }
        [HttpPost]
        public async Task<IActionResult> Addfile(string containerName, IFormFile file, Blob blob)
        {
            if (file == null || file.Length < 1) return View();
            var fileName = Path.GetFileNameWithoutExtension(file.Name) + " _ " + Guid.NewGuid() + Path.GetExtension(file.Name);

            var result = await _blobService.UploadBlob(fileName, file, containerName, blob);
            if (result)
                return RedirectToAction("Index", "Container");
            return View();
        }
        public async Task<IActionResult> Deletefile(string name, string containerName)
        {
            await _blobService.DeleteBlob(name, containerName);
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Viewfile(string name, string containerName)
        {
            return Redirect(await _blobService.GetBlob(name, containerName));
        }

    }
}
