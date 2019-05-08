using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp4
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static void Run([CosmosDBTrigger(
            databaseName: "ToDoList",
            collectionName: "Items",
            StartFromBeginning = true,
            ConnectionStringSetting = "CosmosDB:ConnectionString",
            LeaseCollectionName = "FunctionApp4-Function2-leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents, ILogger log)
        {
            log.LogInformation("Function2 function processed a request.");

            if (documents != null)
            {
                foreach (var document in documents)
                {
                    log.LogInformation(JsonConvert.SerializeObject(document, Formatting.Indented));
                }
            }
        }
    }
}
