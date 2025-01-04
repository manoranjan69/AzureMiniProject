using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RediesNewCache.Data;
using RediesNewCache.Models;
using System.Diagnostics;

namespace RediesNewCache.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _db;

        private readonly IDistributedCache _cache;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext db,IDistributedCache cache)
        {
            _db = db;
            _logger = logger;
            _cache = cache; 
        }

        public IActionResult Index()
        {
            //_cache.Remove("catagoryList");
            List<Catagory> catagoryList = new();
            var cachedcatagory = _cache.GetString("catagoryList");
            if (!string.IsNullOrEmpty(cachedcatagory))
            {
                catagoryList=JsonConvert.DeserializeObject<List<Catagory>>(cachedcatagory);

                //cache

            }
            else
            {
                catagoryList = _db.Category.ToList();
                DistributedCacheEntryOptions options = new();
                options.SetAbsoluteExpiration(new TimeSpan(0, 0, 30));

                _cache.SetString("catagoryList", JsonConvert.SerializeObject(catagoryList), options);
            }
            return View(catagoryList);
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