using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunc.Models;

namespace AzureFunc
{
    public static class onSalesUploadwriteToQueues
    {
        [FunctionName("onSalesUploadwriteToQueues")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Queue("SalesRequestInbound",Connection ="AzureWebJobsStorage")]
            IAsyncCollector<SalesRequest>salesRequestQueue,
            ILogger log)
        {
            log.LogInformation("Sales Request received by OnsalesUploadwritetoQueue function.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            SalesRequest data = JsonConvert.DeserializeObject<SalesRequest>(requestBody);
            await salesRequestQueue.AddAsync(data);
            string responseMessage = "Sales Request has been recorded for" + data.Name;
            return new OkObjectResult(responseMessage);
        }
    }
}
