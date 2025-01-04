using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureSpookyLogic.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace AzureSpookyLogic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        static readonly HttpClient client = new HttpClient();

        private readonly BlobServiceClient _blobServiceClient;
        public HomeController(ILogger<HomeController> logger, BlobServiceClient blobServiceClient )
        {
            _blobServiceClient= blobServiceClient;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(Spookyrequest spookyrequest,IFormFile file)
        {
            spookyrequest.Id=Guid.NewGuid().ToString();
            var jsonContent=JsonConvert.SerializeObject(spookyrequest);
            using(var content=new StringContent(jsonContent, Encoding.UTF8, "application/json"))
            {
               HttpResponseMessage httpresponse=await client.PostAsync("https://prod-09.northcentralus.logic.azure.com:443/workflows/a205180aa8f84d8c8b7afa8b8cd06c46/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=dQj_f65-Obag_K2oi_9edY70PLXmC1oPm4N4XwFbHI8", content);
            }
            if (file != null)
            {
                var fileName = spookyrequest.Id + Path.GetExtension(file.FileName);
                BlobContainerClient blobContainerClient = _blobServiceClient.GetBlobContainerClient("logicappholder");

                var blobclient=blobContainerClient.GetBlobClient(fileName);
                var httpHeaders = new BlobHttpHeaders()
                {
                    ContentType = file.ContentType
                };

                await blobclient.UploadAsync(file.OpenReadStream(),httpHeaders);


            }
            return RedirectToAction(nameof(Index));
            //return View();
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