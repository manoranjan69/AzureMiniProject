using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunc.Data;
using AzureFunc.Model;
using System.Linq;

namespace AzureFunc
{
    public  class GroceryAPI
    {
        private readonly AzureDbContext _db;
        public GroceryAPI(AzureDbContext db)
        {
            _db = db;
        }

        [FunctionName("CreateGrocery")]
        public  async Task<IActionResult> CreateGrocery(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "GroceryList")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Creating Grocery List item");

           

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            GroceryItem_Upsert data = JsonConvert.DeserializeObject<GroceryItem_Upsert>(requestBody);
            

            var groceryitem = new GroceryItem
            {
                Name = data.Name,

            };
            _db.GroceryItems.Add(groceryitem);
            _db.SaveChanges();
            return new OkObjectResult(groceryitem);
           
        }
        [FunctionName("GetGrocery")]
        public async Task<IActionResult> GetGrocery(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GroceryList")] HttpRequest req,
           ILogger log)
        {

            log.LogInformation("Getting Grocery List item");

            return new OkObjectResult(_db.GroceryItems.ToList());
        }

        [FunctionName("GetGroceryById")]
        public async Task<IActionResult> GetGroceryById(
           [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GroceryList/{id}")] HttpRequest req,
           ILogger log,string id)
        {
            log.LogInformation("Getting Grocery List item By Id");

            var item=_db.GroceryItems.FirstOrDefault(x => x.Id == id);
            if (item==null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(item);

        }
        [FunctionName("UpdateGrocery")]
        public async Task<IActionResult> UpdateGrocery(
           [HttpTrigger(AuthorizationLevel.Function, "put", Route = "GroceryList/{id}")] HttpRequest req,
           ILogger log, string id)
        {
            log.LogInformation("Updated Grocery List item.");
            var item = _db.GroceryItems.FirstOrDefault(x => x.Id == id);
            if (item==null)
            {
                return new NotFoundResult();
            }

            string requestbody=await new StreamReader(req.Body).ReadToEndAsync();
           
            GroceryItem_Upsert updatedData=JsonConvert.DeserializeObject<GroceryItem_Upsert>(requestbody);

            if (!string.IsNullOrEmpty(updatedData.Name))
            {
                item.Name = updatedData.Name;
                _db.GroceryItems.Update(item);
                _db.SaveChanges();
            }
           
            return new OkObjectResult(item);


        }



        [FunctionName("DeleteGrocery")]
        public async Task<IActionResult> DeleteGrocery(
         [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "GroceryList/{id}")] HttpRequest req,
         ILogger log, string id)
        {
            log.LogInformation("Delete Grocery List item.");
            var item = _db.GroceryItems.FirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return new NotFoundResult();
            }

          _db.GroceryItems.Remove(item);
            _db.SaveChanges();
            return new OkResult();
          
         


        }

    }
}
