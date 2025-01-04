using AspNetcoreWebApp.Models;
using AspNetcoreWebApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspNetcoreWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IContainerService _containerservice;

        private readonly IBlobService _blobService;


        public HomeController(IContainerService containerservice, IBlobService blobService)
        {
            _containerservice = containerservice;
            _blobService = blobService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _containerservice.GetAllContainerAndBlobs());
        }
        public async Task<IActionResult> Images()
        {
            return View(await _blobService.GetAllBlobwithUri("private"));

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}