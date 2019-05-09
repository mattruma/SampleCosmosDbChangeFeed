using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp5
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([CosmosDBTrigger(
            databaseName: "ToDoList",
            collectionName: "Items",
            StartFromBeginning = true,
            ConnectionStringSetting = "CosmosDB:ConnectionString",
            LeaseCollectionName = "FunctionApp5-leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> documents, ILogger log)
        {
            if (documents != null)
            {
                log.LogInformation($"{nameof(Function1)} received {documents.Count} document(s).");

                foreach (var document in documents)
                {
                    log.LogDebug(JsonConvert.SerializeObject(document, Formatting.Indented));
                }
            }
            else
            {
                log.LogInformation($"{nameof(Function1)} received 0 document(s).");
            }
        }
    }
}
