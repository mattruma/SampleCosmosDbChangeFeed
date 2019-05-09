using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp3
{
    public static class Function2
    {
        [FunctionName("Function2")]
        public static void Run([CosmosDBTrigger(
            databaseName: "ToDoList",
            collectionName: "Items",
            StartFromBeginning = true,
            ConnectionStringSetting = "CosmosDB:ConnectionString",
            LeaseCollectionName = "FunctionApp3-leases",
            LeaseCollectionPrefix = "Function2",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents, ILogger log)
        {
            if (documents != null)
            {
                log.LogInformation($"{nameof(Function2)} received {documents.Count} document(s).");

                foreach (var document in documents)
                {
                    log.LogDebug(JsonConvert.SerializeObject(document, Formatting.Indented));
                }
            }
            else
            {
                log.LogInformation($"{nameof(Function2)} received 0 document(s).");
            }
        }
    }
}
