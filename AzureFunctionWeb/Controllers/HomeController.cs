using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureFunctionWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace AzureFunctionWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
       static readonly HttpClient client = new HttpClient();
       private readonly BlobServiceClient _blobServiceClient;

        public HomeController(ILogger<HomeController> logger,BlobServiceClient blobServiceClient)
        {

           _blobServiceClient = blobServiceClient;
            _logger = logger;
           
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(SalesRequest salesRequest, IFormFile file)
        {
            salesRequest.Id = Guid.NewGuid().ToString();
            using (var content = new StringContent(JsonConvert.SerializeObject(salesRequest), System.Text.Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage response = await client.PostAsync(" http://localhost:7252/api/onSalesUploadwriteToQueues", content);
                string returnvalue = response.Content.ReadAsStringAsync().Result;
            }
            if (file != null)
            {
                var filename = salesRequest.Id + Path.GetExtension(file.FileName);
                BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient("functionsalesapp");
                var blobclient = blobContainerClient.GetBlobClient(filename);

                var httpheaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                };

                await blobclient.UploadAsync(file.OpenReadStream(), httpheaders);
                return View();

            }

            return RedirectToAction(nameof(Index));
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