using System;
using System.Collections.Generic;
using AzureFunc.Data;
using AzureFunc.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunc
{
    public class OnQueueTriggerUpdateDatabase
    {
        private readonly AzureDbContext _db;
        public OnQueueTriggerUpdateDatabase(AzureDbContext db)
        {
            _db = db;
        }

        [FunctionName("OnQueueTriggerUpdateDatabase")]
        public void Run([QueueTrigger("SalesRequetInbound", Connection = "AzureWebJobsStorage")] SalesRequest myQueueItems, ILogger log)
        {

            log.LogInformation($"C# Queue trigger function processed: {myQueueItems}");

            //foreach (var myQueueItem in myQueueItems)
            //{
            //    log.LogInformation($"Processing item with ID: {myQueueItem.Id}");
            //    myQueueItem.Status = "Submitted";
            //    _db.SalesRequests.Add(myQueueItem);
            //}

            //log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            myQueueItems.Status = "Submitted";
            _db.SalesRequests.Add(myQueueItems);
            _db.SaveChanges();
        }
    }
}
